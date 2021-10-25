using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;

namespace Run2
{
  internal delegate void UserCallBack(string data);
  public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

  [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
  [SuppressMessage("ReSharper", "UnusedType.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public class FixedProcess : Process
  {
    private AsyncStreamReader error;
    private AsyncStreamReader output;

    public new event DataReceivedEventHandler ErrorDataReceived;

    public new event DataReceivedEventHandler OutputDataReceived;

    public new void BeginErrorReadLine()
    {
      var baseStream = StandardError.BaseStream;
      error = new AsyncStreamReader(baseStream, FixedErrorReadNotifyUser, StandardError.CurrentEncoding);
      error.BeginReadLine();
    }

    public new void BeginOutputReadLine()
    {
      var baseStream = StandardOutput.BaseStream;
      output = new AsyncStreamReader(baseStream, FixedOutputReadNotifyUser, StandardOutput.CurrentEncoding);
      output.BeginReadLine();
    }

    private void FixedErrorReadNotifyUser(string data)
    {
      var errorDataReceived = ErrorDataReceived;
      if (errorDataReceived != null)
      {
        var dataReceivedEventArgs = new DataReceivedEventArgs(data);
        if (SynchronizingObject is { InvokeRequired: true })
        {
          SynchronizingObject.Invoke(errorDataReceived, new object[] { this, dataReceivedEventArgs });
          return;
        }
        errorDataReceived(this, dataReceivedEventArgs);
      }
    }

    private void FixedOutputReadNotifyUser(string data)
    {
      var outputDataReceived = OutputDataReceived;
      if (outputDataReceived != null)
      {
        var dataReceivedEventArgs = new DataReceivedEventArgs(data);
        if (SynchronizingObject is { InvokeRequired: true })
        {
          SynchronizingObject.Invoke(outputDataReceived, new object[] { this, dataReceivedEventArgs });
          return;
        }
        outputDataReceived(this, dataReceivedEventArgs);
      }
    }
  }

  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  internal sealed class AsyncStreamReader : IDisposable
  {
    internal const int DefaultBufferSize = 1024;
    private readonly Queue messageQueue;
    private bool bLastCarriageReturn;
    private byte[] byteBuffer;
    private bool cancelOperation;
    private char[] charBuffer;
    private Decoder decoder;
    private ManualResetEvent eofEvent;
    private int maxCharsPerBuffer;
    private StringBuilder sb;
    private UserCallBack userCallBack;

    private Stream BaseStream { get; set; }

    internal AsyncStreamReader(Stream stream, UserCallBack callback, Encoding encoding) : this(stream, callback, encoding, 1024)
    {
    }

    private AsyncStreamReader(Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
    {
      Init(stream, callback, encoding, bufferSize);
      messageQueue = new Queue();
    }

    public void Close()
    {
      Dispose(true);
    }

    [SuppressMessage("ReSharper", "GCSuppressFinalizeForTypeWithoutDestructor")]
    void IDisposable.Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    internal void BeginReadLine()
    {
      if (cancelOperation)
      {
        cancelOperation = false;
      }
      if (sb == null)
      {
        sb = new StringBuilder(1024);
        BaseStream.BeginRead(byteBuffer, 0, byteBuffer.Length, ReadBuffer, null);
        return;
      }
      FlushMessageQueue();
    }

    internal void CancelOperation()
    {
      cancelOperation = true;
    }

    internal void WaitUtilEOF()
    {
      if (eofEvent != null)
      {
        eofEvent.WaitOne();
        eofEvent.Close();
        eofEvent = null;
      }
    }

    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        BaseStream?.Close();
      }
      if (BaseStream != null)
      {
        BaseStream = null;
        decoder = null;
        byteBuffer = null;
        charBuffer = null;
      }
      if (eofEvent != null)
      {
        eofEvent.Close();
        eofEvent = null;
      }
    }

    private void FlushMessageQueue()
    {
      lock (messageQueue)
      {
        while (messageQueue.Count > 0)
        {
          lock (messageQueue)
          {
            if (messageQueue.Count > 0)
            {
              var data = (string) messageQueue.Dequeue();
              if (!cancelOperation)
              {
                userCallBack(data);
              }
            }
          }
        }
      }
    }

    private void GetLinesFromStringBuilder()
    {
      var i = 0;
      var num = 0;
      var length = sb.Length;
      if (bLastCarriageReturn && length > 0 && sb[0] == '\n')
      {
        i = 1;
        num = 1;
        bLastCarriageReturn = false;
      }
      while (i < length)
      {
        var c = sb[i];
        if (c is '\r' or '\n')
        {
          if (c == '\r' && i + 1 < length && sb[i + 1] == '\n')
          {
            i++;
          }
          var obj = sb.ToString(num, i + 1 - num);
          num = i + 1;
          lock (messageQueue)
          {
            messageQueue.Enqueue(obj);
          }
        }
        i++;
      }

      // Flush Fix: Send Whatever is left in the buffer
      var endOfBuffer = sb.ToString(num, length - num);
      lock (messageQueue)
      {
        messageQueue.Enqueue(endOfBuffer);
        num = length;
      }
      // End Flush Fix
      if (sb[length - 1] == '\r')
      {
        bLastCarriageReturn = true;
      }
      if (num < length)
      {
        sb.Remove(0, num);
      }
      else
      {
        sb.Length = 0;
      }
      FlushMessageQueue();
    }

    private void Init(Stream aStream, UserCallBack callback, Encoding anEncoding, int bufferSize)
    {
      BaseStream = aStream;
      userCallBack = callback;
      decoder = anEncoding.GetDecoder();
      if (bufferSize < 128)
      {
        bufferSize = 128;
      }
      byteBuffer = new byte[bufferSize];
      maxCharsPerBuffer = anEncoding.GetMaxCharCount(bufferSize);
      charBuffer = new char[maxCharsPerBuffer];
      cancelOperation = false;
      eofEvent = new ManualResetEvent(false);
      sb = null;
      bLastCarriageReturn = false;
    }

    private void ReadBuffer(IAsyncResult ar)
    {
      int num;
      try
      {
        num = BaseStream.EndRead(ar);
      }
      catch (IOException)
      {
        num = 0;
      }
      catch (OperationCanceledException)
      {
        num = 0;
      }
      if (num == 0)
      {
        lock (messageQueue)
        {
          if (sb.Length != 0)
          {
            messageQueue.Enqueue(sb.ToString());
            sb.Length = 0;
          }
          messageQueue.Enqueue(null);
        }
        try
        {
          FlushMessageQueue();
          return;
        }
        finally
        {
          eofEvent.Set();
        }
      }
      var chars = decoder.GetChars(byteBuffer, 0, num, charBuffer, 0);
      sb.Append(charBuffer, 0, chars);
      GetLinesFromStringBuilder();
      BaseStream.BeginRead(byteBuffer, 0, byteBuffer.Length, ReadBuffer, null);
    }
  }

  public sealed class DataReceivedEventArgs : EventArgs
  {
    /// <summary>
    ///   Gets the line of characters that was written to a redirected <see cref="T:System.Diagnostics.Process" />
    ///   output stream.
    /// </summary>
    /// <returns>
    ///   The line that was written by an associated <see cref="T:System.Diagnostics.Process" /> to its redirected
    ///   <see cref="P:System.Diagnostics.Process.StandardOutput" /> or
    ///   <see cref="P:System.Diagnostics.Process.StandardError" /> stream.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public string Data { get; }

    internal DataReceivedEventArgs(string data)
    {
      Data = data;
    }
  }
}