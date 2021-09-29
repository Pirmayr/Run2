# Predefined Run2-Commands

##### -

subtracts second number from first number

* a: first number
* b: second number

Tests:

* performtest (- 47 11) 36

##### !=

tests two values for unequality

* value1: first value
* value2: second value

Tests:

* performtest (!= 47 11) true

##### *

multiplies two numbers

* a: first number
* b: second number

Tests:

* performtest (* 47 11) 517
* performtest (local product 1 map (array 3 1 4 1) (local product (* product item))) 12

##### /

divides first number by second number

* a: first number
* b: second number

Tests:

* performtest (/ 47 11) 4

##### +

adds two values

* value1: first value
* value2: second value

Tests:

* performtest (+ 47 11) 58
* performtest (local result 0 for i 1 10 1 (local result (+ result i))) 55
* performtest (local result 0 foreach i (array 3 1 4 1) (local result (+ result i))) 9

##### <

Tests:

* performtest (< 47 11) false

##### <=

Tests:

* performtest (<= 47 11) false

##### ==

tests two values for equality

* value1: first value
* value2: second value

Tests:

* performtest (== 47 11) false
* performtest (== (array 1 2 3) (array 1 2 3)) true
* performtest (== (array 1 2 3) (array 1 2 4)) false
* performtest (== (array 1 2 3 4) (array 1 2 3)) false

##### >

Tests:

* performtest (> 47 11) true

##### >=

Tests:

* performtest (>= 47 11) true

##### add

calls the "Add"-method of an object with a key and a value

* object
* key
* value

##### Add

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Add

##### array

converts the arguments to an array

Tests:

* performtest (== (array 1 2 3) (array 1 2 3)) true
* performtest (== (array 1 2 3) (array 1 2 4)) false
* performtest (== (array 1 2 3 4) (array 1 2 3)) false
* performtest (sum (array 1 2 3)) 6
* performtest (average (array 1 2 3 4)) 2.5
* performtest (local result 0 foreach i (array 3 1 4 1) (local result (+ result i))) 9
* performtest (local product 1 map (array 3 1 4 1) (local product (* product item))) 12
* performtest (upperbound (array 1 2 3)) 2
* performtest (size (array 1 2 3 4 5 6)) 6
* performtest (sort (split 3\n1\n4\n1\n5\n9)) (array 1 1 3 4 5 9)

##### Array.AsReadOnly

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.AsReadOnly

##### Array.BinarySearch

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.BinarySearch

##### Array.Clear

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Clear

##### Array.ConstrainedCopy

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.ConstrainedCopy

##### Array.ConvertAll

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.ConvertAll

##### Array.Copy

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Copy

##### Array.CreateInstance

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.CreateInstance

##### Array.Empty

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Empty

##### Array.Exists

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Exists

##### Array.Fill

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Fill

##### Array.Find

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Find

##### Array.FindAll

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.FindAll

##### Array.FindIndex

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.FindIndex

##### Array.FindLast

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.FindLast

##### Array.FindLastIndex

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.FindLastIndex

##### Array.ForEach

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.ForEach

##### Array.IndexOf

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.IndexOf

##### Array.LastIndexOf

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.LastIndexOf

##### Array.Resize

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Resize

##### Array.Reverse

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Reverse

##### Array.Sort

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Sort

##### Array.TrueForAll

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.TrueForAll

##### at

returns the element of an array, a list, or a string at the specified index

* object: array, list, or string
* index: index of the element

Tests:

* performtest (ToString (at Hello 1)) e
* performtest (at (split Hello\nworld!) 1) world!

##### average

computes the average of the given values

* values

Tests:

* performtest (average (array 1 2 3 4)) 2.5
* performtest (writefile c:\testdirectory\values.txt 6\n1\n7\n8\n5\n9 average (split (readfile c:\testdirectory\values.txt))) 6

##### averageof

computes the average of the arguments

Tests:

* performtest (averageof 1 2 3 4) 2.5

##### Chars

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Chars

##### Clear

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Clear

##### Clone

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Clone

##### CompareTo

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.CompareTo

##### Console.Write

See: https://docs.microsoft.com/en-us/dotnet/api/Internal.Console.Write

##### Console.WriteLine

See: https://docs.microsoft.com/en-us/dotnet/api/Internal.Console.WriteLine

##### Contains

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Contains

##### ContainsKey

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.ContainsKey

##### ContainsValue

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.ContainsValue

##### CopyTo

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.CopyTo

##### Count

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Count

Tests:

* performtest (Count (list 1 2 3 4 5)) 5

##### createdirectory

creates specified directory

* directory

Tests:

* performtest (createdirectory c:\testdirectory directoryexists c:\testdirectory) true

##### Deconstruct

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.DictionaryEntry.Deconstruct

##### deletedirectory

deletes specified directory

* directory

Tests:

* performtest (deletedirectory c:\testdirectory directoryexists c:\testdirectory) false

##### dir

executes the Windows command "dir" and returns the result

* directory

##### Directory.CreateDirectory

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.CreateDirectory

##### Directory.Delete

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.Delete

##### Directory.EnumerateDirectories

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.EnumerateDirectories

##### Directory.EnumerateFiles

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.EnumerateFiles

##### Directory.EnumerateFileSystemEntries

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.EnumerateFileSystemEntries

##### Directory.Exists

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.Exists

##### Directory.GetCreationTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetCreationTime

##### Directory.GetCreationTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetCreationTimeUtc

##### Directory.GetCurrentDirectory

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetCurrentDirectory

##### Directory.GetDirectories

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetDirectories

##### Directory.GetDirectoryRoot

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetDirectoryRoot

##### Directory.GetFiles

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetFiles

##### Directory.GetFileSystemEntries

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetFileSystemEntries

##### Directory.GetLastAccessTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLastAccessTime

##### Directory.GetLastAccessTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLastAccessTimeUtc

##### Directory.GetLastWriteTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLastWriteTime

##### Directory.GetLastWriteTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLastWriteTimeUtc

##### Directory.GetLogicalDrives

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLogicalDrives

##### Directory.GetParent

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetParent

##### Directory.Move

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.Move

##### Directory.SetCreationTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetCreationTime

##### Directory.SetCreationTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetCreationTimeUtc

##### Directory.SetCurrentDirectory

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetCurrentDirectory

##### Directory.SetLastAccessTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetLastAccessTime

##### Directory.SetLastAccessTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetLastAccessTimeUtc

##### Directory.SetLastWriteTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetLastWriteTime

##### Directory.SetLastWriteTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetLastWriteTimeUtc

##### directoryexists

tests if the specified directory exists

* directory

Tests:

* performtest (createdirectory c:\testdirectory directoryexists c:\testdirectory) true
* performtest (deletedirectory c:\testdirectory directoryexists c:\testdirectory) false

##### EndsWith

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.EndsWith

##### EnumerateRunes

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.EnumerateRunes

##### Equals

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Equals

##### evaluate

evaluates an object

* object: object to be evaluated

##### factorial

computes the factorial of a number

* number

Tests:

* performtest (factorial 5) 120

##### File.AppendAllLines

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendAllLines

##### File.AppendAllLinesAsync

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendAllLinesAsync

##### File.AppendAllText

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendAllText

##### File.AppendAllTextAsync

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendAllTextAsync

##### File.AppendText

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendText

##### File.Copy

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Copy

##### File.Create

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Create

##### File.CreateText

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.CreateText

##### File.Decrypt

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Decrypt

##### File.Delete

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Delete

##### File.Encrypt

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Encrypt

##### File.Exists

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Exists

##### File.GetAttributes

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetAttributes

##### File.GetCreationTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetCreationTime

##### File.GetCreationTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetCreationTimeUtc

##### File.GetLastAccessTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetLastAccessTime

##### File.GetLastAccessTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetLastAccessTimeUtc

##### File.GetLastWriteTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetLastWriteTime

##### File.GetLastWriteTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetLastWriteTimeUtc

##### File.Move

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Move

##### File.Open

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Open

##### File.OpenRead

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.OpenRead

##### File.OpenText

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.OpenText

##### File.OpenWrite

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.OpenWrite

##### File.ReadAllBytes

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllBytes

##### File.ReadAllBytesAsync

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllBytesAsync

##### File.ReadAllLines

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllLines

##### File.ReadAllLinesAsync

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllLinesAsync

##### File.ReadAllText

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllText

##### File.ReadAllTextAsync

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllTextAsync

##### File.ReadLines

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadLines

##### File.Replace

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Replace

##### File.SetAttributes

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetAttributes

##### File.SetCreationTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetCreationTime

##### File.SetCreationTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetCreationTimeUtc

##### File.SetLastAccessTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetLastAccessTime

##### File.SetLastAccessTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetLastAccessTimeUtc

##### File.SetLastWriteTime

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetLastWriteTime

##### File.SetLastWriteTimeUtc

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetLastWriteTimeUtc

##### File.WriteAllBytes

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllBytes

##### File.WriteAllBytesAsync

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllBytesAsync

##### File.WriteAllLines

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllLines

##### File.WriteAllLinesAsync

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllLinesAsync

##### File.WriteAllText

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllText

##### File.WriteAllTextAsync

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllTextAsync

##### finddirectory

searches for a directory

* basedirectory: directory in which the search should begin
* pattern: pattern to be searched for

##### findfile

searches for a file

* basedirectory: directory in which the search should begin
* pattern: pattern to be searched for

##### for

performs a for-loop

* name: name of the variable which holds the counter
* from: start value of the counter
* to: end value of the counter
* step: increment for the counter
* code: body of the for-loop

Tests:

* performtest (local result 0 for i 1 10 1 (local result (+ result i))) 55

##### foreach

performs a foreach-loop

* name: name of the variable which holds the current iteration-value
* values: values which are to be iterated through
* code: body of the foreach-loop

Tests:

* performtest (local result 0 foreach i (array 3 1 4 1) (local result (+ result i))) 9

##### get

return the value of a variable

* name: name of the variable

##### GetEnumerator

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetEnumerator

##### getfiles

returns an array of paths of the files in a directory

* directory

Tests:

* performtest (size (getfiles c:\testdirectory)) 3

##### GetHashCode

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetHashCode

##### gethelp

returns help-information (formatted as markdown)

##### GetLength

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetLength

##### GetLongLength

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetLongLength

##### GetLowerBound

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetLowerBound

##### getmember

* instance
* name

##### GetObjectData

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.GetObjectData

##### GetPinnableReference

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.GetPinnableReference

##### GetType

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetType

##### GetTypeCode

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.GetTypeCode

##### GetUpperBound

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetUpperBound

##### GetValue

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetValue

##### global

creates or sets a global variable

* name: name of the variable
* value: value which should be assigned to the variable

Tests:

* performtest (global value 4711 increment value 1 return value) 4712

##### Hashtable.Synchronized

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Synchronized

##### hasmember

* instance
* name

Tests:

* performtest (hasmember hugo Length) true
* performtest (hasmember hugo Count) false

##### hastype

* instance
* type

##### if

##### increment

increments a variable

* name: name of the variable
* _increment: increment

Tests:

* performtest (global value 4711 increment value 1 return value) 4712
* performtest (local value 1234 increment value 2 return value) 1236

##### IndexOf

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.IndexOf

##### IndexOfAny

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.IndexOfAny

##### initialize

##### Initialize

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Initialize

##### Insert

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Insert

##### invokeinstancemember

##### invoketests

##### isarray

tests if an object is an array

* object

##### IsFixedSize

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.IsFixedSize

##### IsNormalized

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.IsNormalized

##### IsReadOnly

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.IsReadOnly

##### isstring

tests if an object is a string

* object

##### IsSynchronized

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.IsSynchronized

##### Item

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Item

##### Key

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.DictionaryEntry.Key

##### Keys

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Keys

##### LastIndexOf

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.LastIndexOf

##### LastIndexOfAny

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.LastIndexOfAny

##### Length

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Length

Tests:

* performtest (Length Hello World!) 12

##### list

returns the arguments as a list

Tests:

* performtest (Count (list 1 2 3 4 5)) 5
* performtest (size (list 1 2 3 4 5 6 7)) 7

##### local

creates or sets a local variable

* name: name of the variable
* value: value which should be assigned to the variable

Tests:

* performtest (local result 0 for i 1 10 1 (local result (+ result i))) 55
* performtest (local result 0 foreach i (array 3 1 4 1) (local result (+ result i))) 9
* performtest (local product 1 map (array 3 1 4 1) (local product (* product item))) 12
* performtest (local value 1234 increment value 2 return value) 1236

##### LongLength

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.LongLength

##### map

Tests:

* performtest (local product 1 map (array 3 1 4 1) (local product (* product item))) 12

##### Math.Abs

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Abs

##### Math.Acos

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Acos

##### Math.Acosh

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Acosh

##### Math.Asin

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Asin

##### Math.Asinh

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Asinh

##### Math.Atan

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Atan

##### Math.Atan2

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Atan2

##### Math.Atanh

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Atanh

##### Math.BigMul

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.BigMul

##### Math.BitDecrement

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.BitDecrement

##### Math.BitIncrement

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.BitIncrement

##### Math.Cbrt

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Cbrt

##### Math.Ceiling

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Ceiling

##### Math.Clamp

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Clamp

##### Math.CopySign

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.CopySign

##### Math.Cos

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Cos

##### Math.Cosh

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Cosh

##### Math.DivRem

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.DivRem

##### Math.Exp

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Exp

##### Math.Floor

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Floor

##### Math.FusedMultiplyAdd

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.FusedMultiplyAdd

##### Math.IEEERemainder

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.IEEERemainder

##### Math.ILogB

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.ILogB

##### Math.Log

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Log

##### Math.Log10

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Log10

##### Math.Log2

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Log2

##### Math.Max

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Max

##### Math.MaxMagnitude

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.MaxMagnitude

##### Math.Min

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Min

##### Math.MinMagnitude

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.MinMagnitude

##### Math.Pow

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Pow

##### Math.Round

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Round

##### Math.ScaleB

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.ScaleB

##### Math.Sign

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Sign

##### Math.Sin

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Sin

##### Math.Sinh

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Sinh

##### Math.Sqrt

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Sqrt

##### Math.Tan

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Tan

##### Math.Tanh

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Tanh

##### Math.Truncate

See: https://docs.microsoft.com/en-us/dotnet/api/System.Math.Truncate

##### Normalize

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Normalize

##### OnDeserialization

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.OnDeserialization

##### PadLeft

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.PadLeft

##### PadRight

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.PadRight

##### Path.ChangeExtension

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.ChangeExtension

##### Path.Combine

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.Combine

##### Path.EndsInDirectorySeparator

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.EndsInDirectorySeparator

##### Path.GetDirectoryName

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetDirectoryName

##### Path.GetExtension

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetExtension

##### Path.GetFileName

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetFileName

##### Path.GetFileNameWithoutExtension

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetFileNameWithoutExtension

##### Path.GetFullPath

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetFullPath

##### Path.GetInvalidFileNameChars

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetInvalidFileNameChars

##### Path.GetInvalidPathChars

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetInvalidPathChars

##### Path.GetPathRoot

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetPathRoot

##### Path.GetRandomFileName

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetRandomFileName

##### Path.GetRelativePath

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetRelativePath

##### Path.GetTempFileName

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetTempFileName

##### Path.GetTempPath

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetTempPath

##### Path.HasExtension

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.HasExtension

##### Path.IsPathFullyQualified

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.IsPathFullyQualified

##### Path.IsPathRooted

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.IsPathRooted

##### Path.Join

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.Join

##### Path.TrimEndingDirectorySeparator

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.TrimEndingDirectorySeparator

##### Path.TryJoin

See: https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.TryJoin

##### performtest

* tokens
* expected

##### performtests

##### power

computes the power of two numbers

* a: base
* b: exponent

Tests:

* performtest (power 3 4) 81

##### Rank

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.Rank

##### readfile

returns the text contained in a file

* path: path of the file

Tests:

* performtest (writefile c:\testdirectory\hello.txt hello readfile c:\testdirectory\hello.txt) hello
* performtest (writefile c:\testdirectory\world.txt world readfile c:\testdirectory\world.txt) world
* performtest (writefile c:\testdirectory\values.txt 6\n1\n7\n8\n5\n9 average (split (readfile c:\testdirectory\values.txt))) 6

##### Remove

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Remove

##### Replace

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Replace

##### return

Tests:

* performtest (global value 4711 increment value 1 return value) 4712
* performtest (local value 1234 increment value 2 return value) 1236

##### run

##### set

##### setfailure

* code
* actual
* expected

##### SetValue

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.SetValue

##### size

returns the size of an array or a list

* object: array or list

Tests:

* performtest (size hugo) 4
* performtest (size (array 1 2 3 4 5 6)) 6
* performtest (size (list 1 2 3 4 5 6 7)) 7
* performtest (size (getfiles c:\testdirectory)) 3

##### sort

sorts an object (e.g. an array, a list, or a string)

* instance

Tests:

* performtest (sort hugo) ghou
* performtest (sort (split 3\n1\n4\n1\n5\n9)) (array 1 1 3 4 5 9)

##### sortarray

sorts an array

* object

##### sortstring

sorts the characters of a string

* value

##### split

* value

Tests:

* performtest (at (split Hello\nworld!) 1) world!
* performtest (upperbound (split Hello\nworld)) 1
* performtest (writefile c:\testdirectory\values.txt 6\n1\n7\n8\n5\n9 average (split (readfile c:\testdirectory\values.txt))) 6
* performtest (sort (split 3\n1\n4\n1\n5\n9)) (array 1 1 3 4 5 9)

##### Split

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Split

##### square

computes the square of a number

* n: number

Tests:

* performtest (square 2) 4

##### StartsWith

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.StartsWith

##### String.Compare

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Compare

##### String.CompareOrdinal

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.CompareOrdinal

##### String.Concat

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Concat

##### String.Copy

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Copy

##### String.Create

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Create

##### String.Equals

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Equals

##### String.Format

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Format

##### String.GetHashCode

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.GetHashCode

##### String.Intern

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Intern

##### String.IsInterned

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.IsInterned

##### String.IsNullOrEmpty

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.IsNullOrEmpty

##### String.IsNullOrWhiteSpace

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.IsNullOrWhiteSpace

##### String.Join

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Join

##### Substring

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Substring

##### sum

* values

Tests:

* performtest (sum (array 1 2 3)) 6

##### sumof

returns the sum of the arguments

Tests:

* performtest (sumof 1 2 3) 6

##### switch

##### SyncRoot

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.SyncRoot

##### test

##### ToCharArray

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.ToCharArray

##### todouble

* n

##### ToLower

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.ToLower

##### ToLowerInvariant

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.ToLowerInvariant

##### ToString

See: https://docs.microsoft.com/en-us/dotnet/api/System.Array.ToString

Tests:

* performtest (ToString (at Hello 1)) e

##### ToUpper

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.ToUpper

##### ToUpperInvariant

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.ToUpperInvariant

##### Trim

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.Trim

##### TrimEnd

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.TrimEnd

##### TrimStart

See: https://docs.microsoft.com/en-us/dotnet/api/System.String.TrimStart

##### typename

returns the name of the type of an object

* object

Tests:

* performtest (typename Hello) String

##### typeof

returns the type-object of an object

* object

##### upperbound

returns the upper-bound of an object (e.g. an array or a list)

* object

Tests:

* performtest (upperbound (array 1 2 3)) 2
* performtest (upperbound (split Hello\nworld)) 1

##### Value

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.DictionaryEntry.Value

##### Values

See: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Values

##### write

* message

##### writefile

writes text to a file

* path: path of the file
* text: text to be written

Tests:

* performtest (writefile c:\testdirectory\hello.txt hello readfile c:\testdirectory\hello.txt) hello
* performtest (writefile c:\testdirectory\world.txt world readfile c:\testdirectory\world.txt) world
* performtest (writefile c:\testdirectory\values.txt 6\n1\n7\n8\n5\n9 average (split (readfile c:\testdirectory\values.txt))) 6