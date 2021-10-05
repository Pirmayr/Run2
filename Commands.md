# Predefined Run2-Commands

#### -

subtracts second number from first number

* a: first number
* b: second number

Examples:

~~~
performtest ( - 47 11 ) 36
~~~
~~~
performtest ( < ( - ( / 22.0 7.0 ) 3.142857 ) 0.000001 ) True
~~~
~~~
performtest ( < ( - ( Math.Sin 1.0 ) 0.84147 ) 0.000001 ) True
~~~

---

#### !=

tests two values for unequality

* value1: first value
* value2: second value

Examples:

~~~
performtest ( != 47 11 ) True
~~~

---

#### *

multiplies two numbers

* a: first number
* b: second number

Examples:

~~~
performtest ( * 47 11 ) 517
~~~
~~~
performtest
  (
    local product 1
    map ( array 3 1 4 1 ) ( local product ( * product item ) )
  )
  12
~~~

---

#### /

divides first number by second number

* a: first number
* b: second number

Examples:

~~~
performtest ( / 47 11 ) 4
~~~
~~~
performtest ( < ( - ( / 22.0 7.0 ) 3.142857 ) 0.000001 ) True
~~~

---

#### %

return the remainder when dividing the first number by the second number

* a: first number
* b: second number

---

#### +

adds two values

* value1: first value
* value2: second value

Examples:

~~~
performtest ( + 47 11 ) 58
~~~
~~~
performtest
  (
    local result 0
    for i 1 10 1 ( local result ( + result i ) )
  )
  55
~~~
~~~
performtest
  (
    local result 0
    foreach i ( array 3 1 4 1 ) ( local result ( + result i ) )
  )
  9
~~~

---

#### <

tests if value1 is less than value2

* value1: first value
* value2: second value

Examples:

~~~
performtest ( < ( - ( / 22.0 7.0 ) 3.142857 ) 0.000001 ) True
~~~
~~~
performtest ( < 47 11 ) False
~~~
~~~
performtest
  (
    switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 )
  )
  2
~~~
~~~
performtest ( if ( < 1 1 ) ( return 0 ) ( return 1 ) ) 1
~~~
~~~
performtest ( < ( - ( Math.Sin 1.0 ) 0.84147 ) 0.000001 ) True
~~~

---

#### <=

tests if value1 is less than or equal to value2

* value1: first value
* value2: second value

Examples:

~~~
performtest ( <= 47 11 ) False
~~~

---

#### ==

tests two values for equality

* value1: first value
* value2: second value

Examples:

~~~
performtest ( == 47 11 ) False
~~~
~~~
performtest ( == ( array 1 2 3 ) ( array 1 2 3 ) ) True
~~~
~~~
performtest ( == ( array 1 2 3 ) ( array 1 2 4 ) ) False
~~~
~~~
performtest ( == ( array 1 2 3 4 ) ( array 1 2 3 ) ) False
~~~
~~~
performtest
  (
    switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 )
  )
  2
~~~
~~~
performtest ( for i 1 10 1 ( if ( == i 5 ) ( break i ) i ) ) 5
~~~

---

#### >

tests if value1 is greater than value2

* value1: first value
* value2: second value

Examples:

~~~
performtest ( > 47 11 ) True
~~~
~~~
performtest
  (
    switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 )
  )
  2
~~~
~~~
performtest ( > ( size ( dir SystemRoot ) ) 0 ) True
~~~
~~~
performtest ( > ( size ( getmember 'Hello' 'Split' ) ) 0 ) True
~~~

---

#### >=

tests if value1 is greater than or equal to value2

* value1: first value
* value2: second value

Examples:

~~~
performtest ( >= 47 11 ) True
~~~

---

#### Add

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Add
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Add

---

#### addentry

adds an entry to a dictionary

* object: object
* key: key
* value: value

Examples:

~~~
performtest
  (
    global dictionary ( newdictionary )
    addentry dictionary 'foo' 'bar'
    addentry dictionary 'hello' 'world'
    size dictionary
  )
  2
~~~

---

#### additem

adds an item to a list

* object: list
* value: value

Examples:

~~~
performtest ( size ( additem ( list 1 2 3 ) 4 ) ) 4
~~~

---

#### AddRange

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.AddRange

---

#### array

converts the arguments to an array

Examples:

~~~
performtest ( == ( array 1 2 3 ) ( array 1 2 3 ) ) True
~~~
~~~
performtest ( == ( array 1 2 3 ) ( array 1 2 4 ) ) False
~~~
~~~
performtest ( == ( array 1 2 3 4 ) ( array 1 2 3 ) ) False
~~~
~~~
performtest ( sum ( array 1 2 3 ) ) 6
~~~
~~~
performtest ( average ( array 1 2 3 4 ) ) 2.5
~~~
~~~
performtest
  (
    local result 0
    foreach i ( array 3 1 4 1 ) ( local result ( + result i ) )
  )
  9
~~~
~~~
performtest
  (
    local product 1
    map ( array 3 1 4 1 ) ( local product ( * product item ) )
  )
  12
~~~
~~~
performtest ( upperbound ( array 1 2 3 ) ) 2
~~~
~~~
performtest ( size ( array 1 2 3 4 5 6 ) ) 6
~~~
~~~
performtest ( sort ( split '3~n1~n4~n1~n5~n9' ) ) ( array '1' '1' '3' '4' '5' '9' )
~~~
~~~
performtest ( join ( array 'h' 'e' 'l' 'l' 'o' ) ) 'h e l l o'
~~~
~~~
performtest ( concatenation ( array foo bar ) ) foobar
~~~
~~~
performtest ( isarray ( array 1 2 3 ) ) True
~~~
~~~
performtest ( sortarray ( array 3 1 4 1 5 9 2 6 0 ) ) ( array 0 1 1 2 3 4 5 6 9 )
~~~
~~~
performtest ( head ( array 1 2 3 4 ) ) 1
~~~
~~~
performtest ( arraytail ( array 4 7 1 1 ) ) ( array 7 1 1 )
~~~
~~~
performtest ( tail ( array 1 2 3 4 ) ) ( array 2 3 4 )
~~~
~~~
performtest ( median ( array 5 3 2 4 1 ) ) 3
~~~
~~~
performtest ( islist ( array ) ) False
~~~
~~~
performtest ( isarray ( array ) ) True
~~~
~~~
performtest ( notempty ( array ) ) False
~~~
~~~
performtest ( notempty ( array 1 2 3 ) ) True
~~~

---

#### Array.AsReadOnly

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.AsReadOnly

---

#### Array.BinarySearch

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.BinarySearch

---

#### Array.Clear

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Clear

---

#### Array.ConstrainedCopy

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.ConstrainedCopy

---

#### Array.ConvertAll

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.ConvertAll

---

#### Array.Copy

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Copy

---

#### Array.CreateInstance

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.CreateInstance

---

#### Array.Empty

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Empty

---

#### Array.Exists

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Exists

---

#### Array.Fill

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Fill

---

#### Array.Find

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Find

---

#### Array.FindAll

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.FindAll

---

#### Array.FindIndex

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.FindIndex

---

#### Array.FindLast

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.FindLast

---

#### Array.FindLastIndex

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.FindLastIndex

---

#### Array.ForEach

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.ForEach

---

#### Array.IndexOf

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.IndexOf

---

#### Array.LastIndexOf

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.LastIndexOf

---

#### Array.Resize

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Resize

---

#### Array.Reverse

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Reverse

---

#### Array.Sort

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Sort

---

#### Array.TrueForAll

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.TrueForAll

---

#### ArrayList.Adapter

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Adapter

---

#### ArrayList.FixedSize

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.FixedSize

---

#### ArrayList.ReadOnly

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.ReadOnly

---

#### ArrayList.Repeat

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Repeat

---

#### ArrayList.Synchronized

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Synchronized

---

#### arraytail

returns an array without its first element

* elements: array

Examples:

~~~
performtest ( arraytail ( array 4 7 1 1 ) ) ( array 7 1 1 )
~~~

---

#### at

returns the element of an array, a list, or a string at the specified index

* object: array, list, or string
* index: index of the element

Examples:

~~~
performtest ( ToString ( at 'Hello' 1 ) ) 'e'
~~~
~~~
performtest ( at ( split 'Hello~nworld!' ) 1 ) 'world!'
~~~
~~~
performtest (     local foobar 4711
    at ( quote foobar ) 0 ) foobar
~~~
~~~
performtest
  (
    local values ( newarray 1000 0 )
    put values 100 'foobar'
    at values 100
  )
  'foobar'
~~~

---

#### average

computes the average of the given values

* values: values

Examples:

~~~
performtest ( average ( array 1 2 3 4 ) ) 2.5
~~~
~~~
performtest
  (
    writefile c:\testdirectory\values.txt '6~n1~n7~n8~n5~n9'
    average ( split ( readfile c:\testdirectory\values.txt ) )
  )
  6
~~~

---

#### averageof

computes the average of the arguments

Examples:

~~~
performtest ( averageof 1 2 3 4 ) 2.5
~~~

---

#### besttype

tries to find the best type for the given object

* object: object

Examples:

~~~
performtest ( besttype '4711' ) 4711
~~~

---

#### BinarySearch

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.BinarySearch

---

#### break

breaks the innermost loop and returns a value

* value: the value to be returned

Examples:

~~~
performtest ( for i 1 10 1 ( if ( == i 5 ) ( break i ) i ) ) 5
~~~

---

#### canparseint32

tests if the given string can be parsed to a int32

* string: string to be parsed

Examples:

~~~
performtest ( canparseint32 'foobar' ) False
~~~
~~~
performtest ( canparseint32 '1234' ) True
~~~

---

#### Capacity

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Capacity

---

#### Char.ConvertFromUtf32

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.ConvertFromUtf32

---

#### Char.ConvertToUtf32

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.ConvertToUtf32

---

#### Char.GetNumericValue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.GetNumericValue

---

#### Char.GetUnicodeCategory

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.GetUnicodeCategory

---

#### Char.IsControl

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsControl

---

#### Char.IsDigit

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsDigit

---

#### Char.IsHighSurrogate

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsHighSurrogate

---

#### Char.IsLetter

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsLetter

---

#### Char.IsLetterOrDigit

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsLetterOrDigit

---

#### Char.IsLower

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsLower

---

#### Char.IsLowSurrogate

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsLowSurrogate

---

#### Char.IsNumber

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsNumber

---

#### Char.IsPunctuation

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsPunctuation

---

#### Char.IsSeparator

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsSeparator

---

#### Char.IsSurrogate

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsSurrogate

---

#### Char.IsSurrogatePair

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsSurrogatePair

---

#### Char.IsSymbol

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsSymbol

---

#### Char.IsUpper

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsUpper

---

#### Char.IsWhiteSpace

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.IsWhiteSpace

---

#### Char.Parse

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.Parse

---

#### Char.ToLower

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.ToLower

---

#### Char.ToLowerInvariant

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.ToLowerInvariant

---

#### Char.ToString

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.ToString

---

#### Char.ToUpper

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.ToUpper

---

#### Char.ToUpperInvariant

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.ToUpperInvariant

---

#### Char.TryParse

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.TryParse

---

#### Chars

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Chars

---

#### Clear

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Clear
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Clear
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Clear
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Clear

---

#### Clone

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Clone
* https://docs.microsoft.com/en-us/dotnet/api/System.String.Clone
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Clone
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Clone
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Clone
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Clone

---

#### CompareTo

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.CompareTo
* https://docs.microsoft.com/en-us/dotnet/api/System.Char.CompareTo
* https://docs.microsoft.com/en-us/dotnet/api/System.Int32.CompareTo

---

#### concatenation

concatenates the given strings

* strings: strings to be concatenated

Examples:

~~~
performtest ( concatenation ( array foo bar ) ) foobar
~~~

---

#### concatenationof

concatenate the arguments, which are assumed to be strings

Examples:

~~~
performtest ( concatenationof Hello ' ' World! ) 'Hello World!'
~~~

---

#### Console.Write

See:

* https://docs.microsoft.com/en-us/dotnet/api/Internal.Console.Write

---

#### Console.WriteLine

See:

* https://docs.microsoft.com/en-us/dotnet/api/Internal.Console.WriteLine

---

#### Contains

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Contains
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Contains
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Contains
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Contains
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Contains

---

#### ContainsKey

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.ContainsKey

---

#### ContainsValue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.ContainsValue

---

#### Convert.ChangeType

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ChangeType

---

#### Convert.FromBase64CharArray

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.FromBase64CharArray

---

#### Convert.FromBase64String

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.FromBase64String

---

#### Convert.FromHexString

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.FromHexString

---

#### Convert.GetTypeCode

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.GetTypeCode

---

#### Convert.IsDBNull

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.IsDBNull

---

#### Convert.ToBase64CharArray

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToBase64CharArray

---

#### Convert.ToBase64String

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToBase64String

---

#### Convert.ToBoolean

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToBoolean

---

#### Convert.ToByte

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToByte

---

#### Convert.ToChar

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToChar

---

#### Convert.ToDateTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToDateTime

---

#### Convert.ToDecimal

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToDecimal

---

#### Convert.ToDouble

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToDouble

---

#### Convert.ToHexString

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToHexString

---

#### Convert.ToInt16

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToInt16

---

#### Convert.ToInt32

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToInt32

---

#### Convert.ToInt64

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToInt64

---

#### Convert.ToSByte

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToSByte

---

#### Convert.ToSingle

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToSingle

---

#### Convert.ToString

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToString

---

#### Convert.ToUInt16

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToUInt16

---

#### Convert.ToUInt32

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToUInt32

---

#### Convert.ToUInt64

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.ToUInt64

---

#### Convert.TryFromBase64Chars

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.TryFromBase64Chars

---

#### Convert.TryFromBase64String

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.TryFromBase64String

---

#### Convert.TryToBase64Chars

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.TryToBase64Chars

---

#### CopyTo

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.CopyTo
* https://docs.microsoft.com/en-us/dotnet/api/System.String.CopyTo
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.CopyTo
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.CopyTo
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.CopyTo
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.CopyTo

---

#### Count

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Count
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Count
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Count
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Count

Examples:

~~~
performtest ( Count ( list 1 2 3 4 5 ) ) 5
~~~

---

#### createdirectory

creates specified directory

* directory: directory to be created

Examples:

~~~
performtest
  (
    createdirectory c:\testdirectory
    directoryexists c:\testdirectory
  )
  True
~~~

---

#### Deconstruct

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.DictionaryEntry.Deconstruct

---

#### decrement

decrements a variable

* "name": name of the variable
* amount: decrement

Examples:

~~~
performtest (     local a 4711
    decrement a 11
    return a ) 4700
~~~

---

#### deletedirectory

deletes specified directory

* directory: directory to be deleted

Examples:

~~~
performtest
  (
    deletedirectory c:\testdirectory
    directoryexists c:\testdirectory
  )
  False
~~~

---

#### dequeue

dequeues an element

* queue: queue

Examples:

~~~
performtest
  (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    dequeue queue
  )
  47
~~~
~~~
performtest ( dequeue ( newqueue ( list 1 2 3 ) ) ) 1
~~~

---

#### Dequeue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Dequeue

---

#### dir

executes the Windows command "dir" and returns the result

Examples:

~~~
performtest ( > ( size ( dir SystemRoot ) ) 0 ) True
~~~

---

#### Directory.CreateDirectory

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.CreateDirectory

---

#### Directory.Delete

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.Delete

---

#### Directory.EnumerateDirectories

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.EnumerateDirectories

---

#### Directory.EnumerateFiles

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.EnumerateFiles

---

#### Directory.EnumerateFileSystemEntries

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.EnumerateFileSystemEntries

---

#### Directory.Exists

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.Exists

---

#### Directory.GetCreationTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetCreationTime

---

#### Directory.GetCreationTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetCreationTimeUtc

---

#### Directory.GetCurrentDirectory

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetCurrentDirectory

---

#### Directory.GetDirectories

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetDirectories

---

#### Directory.GetDirectoryRoot

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetDirectoryRoot

---

#### Directory.GetFiles

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetFiles

---

#### Directory.GetFileSystemEntries

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetFileSystemEntries

---

#### Directory.GetLastAccessTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLastAccessTime

---

#### Directory.GetLastAccessTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLastAccessTimeUtc

---

#### Directory.GetLastWriteTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLastWriteTime

---

#### Directory.GetLastWriteTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLastWriteTimeUtc

---

#### Directory.GetLogicalDrives

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetLogicalDrives

---

#### Directory.GetParent

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.GetParent

---

#### Directory.Move

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.Move

---

#### Directory.SetCreationTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetCreationTime

---

#### Directory.SetCreationTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetCreationTimeUtc

---

#### Directory.SetCurrentDirectory

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetCurrentDirectory

---

#### Directory.SetLastAccessTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetLastAccessTime

---

#### Directory.SetLastAccessTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetLastAccessTimeUtc

---

#### Directory.SetLastWriteTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetLastWriteTime

---

#### Directory.SetLastWriteTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Directory.SetLastWriteTimeUtc

---

#### directoryexists

tests if the specified directory exists

* directory: directory to be tested for existence

Examples:

~~~
performtest
  (
    createdirectory c:\testdirectory
    directoryexists c:\testdirectory
  )
  True
~~~
~~~
performtest
  (
    deletedirectory c:\testdirectory
    directoryexists c:\testdirectory
  )
  False
~~~

---

#### dos

executes specified dos-command and returns the output

Examples:

~~~
performtest ( dos echo 'hello world' ) 'hello world'
~~~

---

#### empty

checks if an object is not empty

* object: object

Examples:

~~~
performtest ( empty ( list ) ) True
~~~
~~~
performtest ( empty ( list 1 2 3 ) ) False
~~~

---

#### endswith

tests if a strings ends with an other string

* string: string to be tested
* value: string to be tested for being the ending

Examples:

~~~
performtest ( endswith 'Hello World!' 'World!' ) True
~~~

---

#### EndsWith

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.EndsWith

---

#### enqueue

enqueues an element

* queue: queue
* value: element to be enqueued

Examples:

~~~
performtest
  (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    dequeue queue
  )
  47
~~~

---

#### Enqueue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Enqueue

---

#### EnumerateRunes

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.EnumerateRunes

---

#### Equals

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Object.Equals
* https://docs.microsoft.com/en-us/dotnet/api/System.ValueType.Equals

---

#### evaluate

evaluates an object

* object: object to be evaluated

---

#### evaluatevalues

evaluates an array or a list

* values: values to be evaluated

---

#### factorial

computes the factorial of a number

* number: number

Examples:

~~~
performtest ( factorial 50 ) 30414093201713378043612608166064768844377641568960512000000000000
~~~

---

#### File.AppendAllLines

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendAllLines

---

#### File.AppendAllLinesAsync

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendAllLinesAsync

---

#### File.AppendAllText

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendAllText

---

#### File.AppendAllTextAsync

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendAllTextAsync

---

#### File.AppendText

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.AppendText

---

#### File.Copy

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Copy

---

#### File.Create

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Create

---

#### File.CreateText

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.CreateText

---

#### File.Decrypt

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Decrypt

---

#### File.Delete

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Delete

---

#### File.Encrypt

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Encrypt

---

#### File.Exists

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Exists

---

#### File.GetAttributes

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetAttributes

---

#### File.GetCreationTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetCreationTime

---

#### File.GetCreationTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetCreationTimeUtc

---

#### File.GetLastAccessTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetLastAccessTime

---

#### File.GetLastAccessTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetLastAccessTimeUtc

---

#### File.GetLastWriteTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetLastWriteTime

---

#### File.GetLastWriteTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.GetLastWriteTimeUtc

---

#### File.Move

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Move

---

#### File.Open

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Open

---

#### File.OpenRead

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.OpenRead

---

#### File.OpenText

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.OpenText

---

#### File.OpenWrite

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.OpenWrite

---

#### File.ReadAllBytes

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllBytes

---

#### File.ReadAllBytesAsync

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllBytesAsync

---

#### File.ReadAllLines

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllLines

---

#### File.ReadAllLinesAsync

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllLinesAsync

---

#### File.ReadAllText

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllText

---

#### File.ReadAllTextAsync

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadAllTextAsync

---

#### File.ReadLines

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.ReadLines

---

#### File.Replace

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.Replace

---

#### File.SetAttributes

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetAttributes

---

#### File.SetCreationTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetCreationTime

---

#### File.SetCreationTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetCreationTimeUtc

---

#### File.SetLastAccessTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetLastAccessTime

---

#### File.SetLastAccessTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetLastAccessTimeUtc

---

#### File.SetLastWriteTime

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetLastWriteTime

---

#### File.SetLastWriteTimeUtc

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.SetLastWriteTimeUtc

---

#### File.WriteAllBytes

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllBytes

---

#### File.WriteAllBytesAsync

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllBytesAsync

---

#### File.WriteAllLines

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllLines

---

#### File.WriteAllLinesAsync

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllLinesAsync

---

#### File.WriteAllText

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllText

---

#### File.WriteAllTextAsync

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.File.WriteAllTextAsync

---

#### fileexists

checks if a file exists

* path: path of the file

Examples:

~~~
performtest ( fileexists ( findfile basedirectory LICENSE ) ) True
~~~
~~~
performtest ( fileexists ( findfile basedirectory DOESNOTEXIST ) ) False
~~~

---

#### finddirectory

searches for a directory

* basedirectory: directory in which the search should begin
* pattern: pattern to be searched for

Examples:

~~~
performtest ( finddirectory SystemRoot System32 ) '[SystemRoot]\System32'
~~~

---

#### findfile

searches for a file

* basedirectory: directory in which the search should begin
* pattern: pattern to be searched for

Examples:

~~~
performtest ( findfile SystemRoot notepad.exe ) '[SystemRoot]\notepad.exe'
~~~
~~~
performtest ( fileexists ( findfile basedirectory LICENSE ) ) True
~~~
~~~
performtest ( fileexists ( findfile basedirectory DOESNOTEXIST ) ) False
~~~
~~~
performtest ( size ( gettokens ( findfile basedirectory LICENSE ) ) ) 168
~~~

---

#### for

performs a for-loop

* name: name of the variable which holds the counter
* from: start value of the counter
* to: end value of the counter
* step: increment for the counter
* code: body of the for-loop

Examples:

~~~
performtest
  (
    local result 0
    for i 1 10 1 ( local result ( + result i ) )
  )
  55
~~~
~~~
performtest ( for i 1 10 1 ( if ( == i 5 ) ( break i ) i ) ) 5
~~~

---

#### foreach

performs a foreach-loop

* name: name of the variable which holds the current iteration-value
* values: values which are to be iterated through
* code: body of the foreach-loop

Examples:

~~~
performtest
  (
    local result 0
    foreach i ( array 3 1 4 1 ) ( local result ( + result i ) )
  )
  9
~~~

---

#### get

return the value of a variable; the variable can exist in any active scope

* name: name of the variable

---

#### getcode

returns the formatted script

---

#### getcommands

returns the list of commands

---

#### GetEnumerator

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetEnumerator
* https://docs.microsoft.com/en-us/dotnet/api/System.String.GetEnumerator
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.GetEnumerator
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.GetEnumerator
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.GetEnumerator
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.GetEnumerator

---

#### getfiles

returns an array of paths of the files in a directory

* directory: directory

Examples:

~~~
performtest ( size ( getfiles c:\testdirectory ) ) 3
~~~

---

#### GetHashCode

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Object.GetHashCode
* https://docs.microsoft.com/en-us/dotnet/api/System.ValueType.GetHashCode

---

#### gethelp

returns help-information (formatted as markdown)

---

#### GetLength

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetLength

---

#### GetLongLength

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetLongLength

---

#### GetLowerBound

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetLowerBound

---

#### getmember

get the array of member-information for the specified member in the given object

* object: object
* name: name of the member

Examples:

~~~
performtest ( > ( size ( getmember 'Hello' 'Split' ) ) 0 ) True
~~~

---

#### GetObjectData

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.GetObjectData

---

#### GetPinnableReference

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.GetPinnableReference

---

#### GetRange

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.GetRange

---

#### gettokens

returns the tokenized contents of a text or a text file

* textorpath: text or path to a file

Examples:

~~~
performtest ( size ( gettokens 'the big brown fox' ) ) 4
~~~
~~~
performtest ( size ( gettokens ( findfile basedirectory LICENSE ) ) ) 168
~~~

---

#### GetType

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Object.GetType
* https://docs.microsoft.com/en-us/dotnet/api/System.ValueType.GetType

---

#### GetTypeCode

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.GetTypeCode
* https://docs.microsoft.com/en-us/dotnet/api/System.Char.GetTypeCode
* https://docs.microsoft.com/en-us/dotnet/api/System.Int32.GetTypeCode

---

#### GetUpperBound

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetUpperBound

---

#### GetValue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.GetValue

---

#### global

creates or sets a global variable

* name: name of the variable
* value: value to be assigned to the variable

Examples:

~~~
performtest
  (
    global value 4711
    increment value 1
    return value
  )
  4712
~~~
~~~
performtest
  (
    global dictionary ( newdictionary )
    addentry dictionary 'foo' 'bar'
    addentry dictionary 'hello' 'world'
    size dictionary
  )
  2
~~~

---

#### Hashtable.Synchronized

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Synchronized

---

#### hasmember

tests if the specified member exists in the given object

* object: object
* name: name of the member

Examples:

~~~
performtest ( hasmember 'hello' 'Length' ) True
~~~
~~~
performtest ( hasmember 'hello' 'Count' ) False
~~~

---

#### hastype

tests if the specified object has the given type

* object: object
* name: name of the type

Examples:

~~~
performtest ( hastype 'hello' String ) True
~~~

---

#### head

returns the first element of an indexed object

* object: indexed object

Examples:

~~~
performtest ( head ( array 1 2 3 4 ) ) 1
~~~
~~~
performtest ( head ( list 4 7 1 1 ) ) 4
~~~

---

#### if

performs the if-statement

* condition: condition
* true-block: command to be executed if the condition is 'true'
* false-block: (optional) command to be executed if the condition is 'false'

Examples:

~~~
performtest ( if ( < 1 1 ) ( return 0 ) ( return 1 ) ) 1
~~~
~~~
performtest ( for i 1 10 1 ( if ( == i 5 ) ( break i ) i ) ) 5
~~~

---

#### increment

increments a variable

* "name": name of the variable
* amount: increment

Examples:

~~~
performtest
  (
    global value 4711
    increment value 1
    return value
  )
  4712
~~~
~~~
performtest
  (
    local value 1234
    increment value 2
    return value
  )
  1236
~~~

---

#### IndexOf

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.IndexOf
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.IndexOf

---

#### IndexOfAny

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.IndexOfAny

---

#### initialize

performs various inititalizations

Examples:

~~~
performtest ( initialize ) True
~~~

---

#### Initialize

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Initialize

---

#### Insert

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Insert
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Insert

---

#### InsertRange

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.InsertRange

---

#### Int32.Parse

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Int32.Parse

---

#### Int32.TryParse

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Int32.TryParse

---

#### invokeinstancemember

calls the 'Invoke'-method of the type of the specified object

* name: name of the object-member
* object: target of the invokation

---

#### invoketests

invokes the tests

Examples:

~~~
performtest ( invoketests ) True
~~~

---

#### isarray

tests if an object is an array

* object: object to be tested

Examples:

~~~
performtest ( isarray ( array 1 2 3 ) ) True
~~~
~~~
performtest ( isarray ( array ) ) True
~~~
~~~
performtest ( isarray ( newarray 100 0 ) ) True
~~~

---

#### isdictionary

tests if an object is a dictionary

* object: object to be tested

Examples:

~~~
performtest ( isdictionary ( newdictionary ) ) True
~~~

---

#### IsFixedSize

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.IsFixedSize
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.IsFixedSize
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.IsFixedSize

---

#### islist

tests if an object is a list

* object: object to be tested

Examples:

~~~
performtest ( islist ( list ) ) True
~~~
~~~
performtest ( islist ( array ) ) False
~~~
~~~
performtest ( islist ( newlist ) ) True
~~~

---

#### IsNormalized

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.IsNormalized

---

#### IsReadOnly

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.IsReadOnly
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.IsReadOnly
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.IsReadOnly

---

#### isstring

tests if an object is a string

* object: object to be tested

Examples:

~~~
performtest ( isstring 'Hello' ) True
~~~

---

#### IsSynchronized

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.IsSynchronized
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.IsSynchronized
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.IsSynchronized
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.IsSynchronized
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.IsSynchronized

---

#### iswhitespace

checks if the given character is white space

* character

Examples:

~~~
performtest ( iswhitespace ( tochar 'a' ) ) False
~~~
~~~
performtest ( iswhitespace ( tochar ' ' ) ) True
~~~

---

#### Item

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Item
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Item

---

#### join

joins the elements of an array or a list of strings to a string separated by blanks

* strings: array or list of strings

Examples:

~~~
performtest ( join ( array 'h' 'e' 'l' 'l' 'o' ) ) 'h e l l o'
~~~

---

#### joinfrom

joins the given arguments to a string separated by blanks

Examples:

~~~
performtest ( joinfrom 'h' 'e' 'l' 'l' 'o' ) 'h e l l o'
~~~
~~~
performtest ( joinfrom ( typename 1 ) ( typename ( todouble 1 ) ) ) 'Int32 Double'
~~~

---

#### Key

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.DictionaryEntry.Key

---

#### Keys

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Keys

---

#### LastIndexOf

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.LastIndexOf
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.LastIndexOf

---

#### LastIndexOfAny

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.LastIndexOfAny

---

#### Length

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Length
* https://docs.microsoft.com/en-us/dotnet/api/System.String.Length

Examples:

~~~
performtest ( Length 'Hello World!' ) 12
~~~

---

#### list

returns the arguments as a list

Examples:

~~~
performtest ( Count ( list 1 2 3 4 5 ) ) 5
~~~
~~~
performtest ( size ( list 1 2 3 4 5 6 7 ) ) 7
~~~
~~~
performtest ( head ( list 4 7 1 1 ) ) 4
~~~
~~~
performtest ( listtail ( list 4 7 1 1 ) ) ( list 7 1 1 )
~~~
~~~
performtest ( size ( additem ( list 1 2 3 ) 4 ) ) 4
~~~
~~~
performtest ( islist ( list ) ) True
~~~
~~~
performtest ( dequeue ( newqueue ( list 1 2 3 ) ) ) 1
~~~
~~~
performtest ( size ( newqueue ( list 1 2 3 ) ) ) 3
~~~
~~~
performtest ( empty ( list ) ) True
~~~
~~~
performtest ( empty ( list 1 2 3 ) ) False
~~~

---

#### listtail

returns a list without its first element

* items: list

Examples:

~~~
performtest ( listtail ( list 4 7 1 1 ) ) ( list 7 1 1 )
~~~

---

#### local

creates or sets a local variable

* name: name of the variable
* value: value to be assigned to the variable

Examples:

~~~
performtest
  (
    local result 0
    for i 1 10 1 ( local result ( + result i ) )
  )
  55
~~~
~~~
performtest
  (
    local result 0
    foreach i ( array 3 1 4 1 ) ( local result ( + result i ) )
  )
  9
~~~
~~~
performtest
  (
    local product 1
    map ( array 3 1 4 1 ) ( local product ( * product item ) )
  )
  12
~~~
~~~
performtest
  (
    local value 1234
    increment value 2
    return value
  )
  1236
~~~
~~~
performtest (     local a 4711
    set a 1234 ) 1234
~~~
~~~
performtest (     local foobar 4711
    at ( quote foobar ) 0 ) foobar
~~~
~~~
performtest
  (
    local values ( newarray 1000 0 )
    put values 100 'foobar'
    at values 100
  )
  'foobar'
~~~
~~~
performtest
  (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    dequeue queue
  )
  47
~~~
~~~
performtest
  (
    local stack ( newstack )
    push stack 47
    push stack 11
    pop stack
  )
  11
~~~
~~~
performtest (     local a 4711
    decrement a 11
    return a ) 4700
~~~

---

#### LongLength

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.LongLength

---

#### map

executes a command with all elements of an array or listf; the variable 'item' holds the current element

* arrayOrList: array or list
* command: command

Examples:

~~~
performtest
  (
    local product 1
    map ( array 3 1 4 1 ) ( local product ( * product item ) )
  )
  12
~~~

---

#### Math.Abs

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Abs

---

#### Math.Acos

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Acos

---

#### Math.Acosh

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Acosh

---

#### Math.Asin

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Asin

---

#### Math.Asinh

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Asinh

---

#### Math.Atan

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Atan

---

#### Math.Atan2

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Atan2

---

#### Math.Atanh

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Atanh

---

#### Math.BigMul

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.BigMul

---

#### Math.BitDecrement

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.BitDecrement

---

#### Math.BitIncrement

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.BitIncrement

---

#### Math.Cbrt

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Cbrt

---

#### Math.Ceiling

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Ceiling

---

#### Math.Clamp

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Clamp

---

#### Math.CopySign

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.CopySign

---

#### Math.Cos

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Cos

---

#### Math.Cosh

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Cosh

---

#### Math.DivRem

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.DivRem

---

#### Math.Exp

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Exp

---

#### Math.Floor

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Floor

---

#### Math.FusedMultiplyAdd

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.FusedMultiplyAdd

---

#### Math.IEEERemainder

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.IEEERemainder

---

#### Math.ILogB

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.ILogB

---

#### Math.Log

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Log

---

#### Math.Log10

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Log10

---

#### Math.Log2

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Log2

---

#### Math.Max

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Max

---

#### Math.MaxMagnitude

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.MaxMagnitude

---

#### Math.Min

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Min

---

#### Math.MinMagnitude

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.MinMagnitude

---

#### Math.Pow

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Pow

---

#### Math.Round

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Round

---

#### Math.ScaleB

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.ScaleB

---

#### Math.Sign

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Sign

---

#### Math.Sin

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Sin

Examples:

~~~
performtest ( < ( - ( Math.Sin 1.0 ) 0.84147 ) 0.000001 ) True
~~~

---

#### Math.Sinh

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Sinh

---

#### Math.Sqrt

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Sqrt

---

#### Math.Tan

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Tan

---

#### Math.Tanh

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Tanh

---

#### Math.Truncate

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Truncate

---

#### median

returns the median of the given numbers

* values: numbers

Examples:

~~~
performtest ( median ( array 5 3 2 4 1 ) ) 3
~~~

---

#### medianof

returns the median of the arguments

Examples:

~~~
performtest ( medianof 5 3 2 4 1 ) 3
~~~

---

#### newarray

creates an array

* length: size of the array
* initialvalue

Examples:

~~~
performtest ( size ( newarray 1000 0 ) ) 1000
~~~
~~~
performtest
  (
    local values ( newarray 1000 0 )
    put values 100 'foobar'
    at values 100
  )
  'foobar'
~~~
~~~
performtest ( isarray ( newarray 100 0 ) ) True
~~~

---

#### newdictionary

creates a new dictionary (actually a new hashtable)

Examples:

~~~
performtest
  (
    global dictionary ( newdictionary )
    addentry dictionary 'foo' 'bar'
    addentry dictionary 'hello' 'world'
    size dictionary
  )
  2
~~~
~~~
performtest ( isdictionary ( newdictionary ) ) True
~~~

---

#### newlist

creates a list

* values: (optional) values to initialize the list

Examples:

~~~
performtest ( islist ( newlist ) ) True
~~~

---

#### newqueue

creates a new queue

* values: (optional) values to initialize the queue

Examples:

~~~
performtest
  (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    dequeue queue
  )
  47
~~~
~~~
performtest ( dequeue ( newqueue ( list 1 2 3 ) ) ) 1
~~~
~~~
performtest ( size ( newqueue ( list 1 2 3 ) ) ) 3
~~~

---

#### newstack

creates a new stack

Examples:

~~~
performtest
  (
    local stack ( newstack )
    push stack 47
    push stack 11
    pop stack
  )
  11
~~~

---

#### Normalize

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Normalize

---

#### not

negates a truth value

* value: truth value

Examples:

~~~
performtest ( not True ) False
~~~

---

#### notempty

checks if an object is not empty

* object: object

Examples:

~~~
performtest ( notempty ( array ) ) False
~~~
~~~
performtest ( notempty ( array 1 2 3 ) ) True
~~~

---

#### null

the value 'null'

---

#### OnDeserialization

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.OnDeserialization

---

#### PadLeft

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.PadLeft

---

#### PadRight

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.PadRight

---

#### parseint32

parses the given string to an int32

* string: string to be parsed

Examples:

~~~
performtest ( parseint32 '4321' ) 4321
~~~

---

#### Path.ChangeExtension

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.ChangeExtension

---

#### Path.Combine

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.Combine

---

#### Path.EndsInDirectorySeparator

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.EndsInDirectorySeparator

---

#### Path.GetDirectoryName

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetDirectoryName

---

#### Path.GetExtension

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetExtension

---

#### Path.GetFileName

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetFileName

---

#### Path.GetFileNameWithoutExtension

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetFileNameWithoutExtension

---

#### Path.GetFullPath

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetFullPath

---

#### Path.GetInvalidFileNameChars

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetInvalidFileNameChars

---

#### Path.GetInvalidPathChars

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetInvalidPathChars

---

#### Path.GetPathRoot

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetPathRoot

---

#### Path.GetRandomFileName

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetRandomFileName

---

#### Path.GetRelativePath

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetRelativePath

---

#### Path.GetTempFileName

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetTempFileName

---

#### Path.GetTempPath

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.GetTempPath

---

#### Path.HasExtension

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.HasExtension

---

#### Path.IsPathFullyQualified

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.IsPathFullyQualified

---

#### Path.IsPathRooted

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.IsPathRooted

---

#### Path.Join

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.Join

---

#### Path.TrimEndingDirectorySeparator

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.TrimEndingDirectorySeparator

---

#### Path.TryJoin

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.TryJoin

---

#### Peek

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Peek
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Peek

---

#### performtest

tests if a code-block yields the expected result

* "tokens": code to be tested
* expected: expected value

---

#### performtests

performs various tests

Examples:

~~~
performtest ( performtests ) True
~~~

---

#### pop

pops an element

* stack: stack

Examples:

~~~
performtest
  (
    local stack ( newstack )
    push stack 47
    push stack 11
    pop stack
  )
  11
~~~

---

#### Pop

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Pop

---

#### power

computes the power of two numbers

* a: base
* b: exponent

Examples:

~~~
performtest ( power 3 4 ) 81
~~~

---

#### primes

computes the primes up to n

* n: upper limit

Examples:

~~~
performtest ( size ( primes 1000 ) ) 167
~~~

---

#### push

pushes an element

* stack: stack
* value: element

Examples:

~~~
performtest
  (
    local stack ( newstack )
    push stack 47
    push stack 11
    pop stack
  )
  11
~~~

---

#### Push

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Push

---

#### put

assigns a new value to the element of an array, a list, or a string at the specified index

* object: array, list, or string
* index: index of the element
* value: value to be set

Examples:

~~~
performtest
  (
    local values ( newarray 1000 0 )
    put values 100 'foobar'
    at values 100
  )
  'foobar'
~~~

---

#### Queue.Synchronized

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Synchronized

---

#### quote

returns the unevaluated argument

Examples:

~~~
performtest (     local foobar 4711
    at ( quote foobar ) 0 ) foobar
~~~

---

#### Rank

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Rank

---

#### readfile

returns the text contained in a file

* path: path of the file

Examples:

~~~
performtest
  (
    writefile c:\testdirectory\hello.txt hello
    readfile c:\testdirectory\hello.txt
  )
  'hello'
~~~
~~~
performtest
  (
    writefile c:\testdirectory\world.txt world
    readfile c:\testdirectory\world.txt
  )
  'world'
~~~
~~~
performtest
  (
    writefile c:\testdirectory\values.txt '6~n1~n7~n8~n5~n9'
    average ( split ( readfile c:\testdirectory\values.txt ) )
  )
  6
~~~

---

#### Remove

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Remove
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Remove
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Remove

---

#### RemoveAt

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.RemoveAt

---

#### RemoveRange

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.RemoveRange

---

#### Replace

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Replace

---

#### return

returns a value

* value: the value to be returned

Examples:

~~~
performtest
  (
    global value 4711
    increment value 1
    return value
  )
  4712
~~~
~~~
performtest
  (
    local value 1234
    increment value 2
    return value
  )
  1236
~~~
~~~
performtest
  (
    switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 )
  )
  2
~~~
~~~
performtest ( if ( < 1 1 ) ( return 0 ) ( return 1 ) ) 1
~~~
~~~
performtest (     local a 4711
    decrement a 11
    return a ) 4700
~~~

---

#### Reverse

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Reverse

---

#### run

runs an external program with the arguments given

* path: path of the external program
* arguments: the values ​​to be passed to the external program

---

#### set

assigns a new value to an existing variable; the variable can exist in any active scope

* value: the value to be assigned to the variable

Examples:

~~~
performtest (     local a 4711
    set a 1234 ) 1234
~~~

---

#### SetRange

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.SetRange

---

#### SetValue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.SetValue

---

#### size

returns the size of an array or a list

* object: array or list

Examples:

~~~
performtest ( size 'hello' ) 5
~~~
~~~
performtest ( size ( array 1 2 3 4 5 6 ) ) 6
~~~
~~~
performtest ( size ( list 1 2 3 4 5 6 7 ) ) 7
~~~
~~~
performtest ( size ( getfiles c:\testdirectory ) ) 3
~~~
~~~
performtest
  (
    global dictionary ( newdictionary )
    addentry dictionary 'foo' 'bar'
    addentry dictionary 'hello' 'world'
    size dictionary
  )
  2
~~~
~~~
performtest ( > ( size ( dir SystemRoot ) ) 0 ) True
~~~
~~~
performtest ( > ( size ( getmember 'Hello' 'Split' ) ) 0 ) True
~~~
~~~
performtest ( size ( newarray 1000 0 ) ) 1000
~~~
~~~
performtest ( size ( additem ( list 1 2 3 ) 4 ) ) 4
~~~
~~~
performtest ( size ( primes 1000 ) ) 167
~~~
~~~
performtest ( size ( newqueue ( list 1 2 3 ) ) ) 3
~~~
~~~
performtest ( size ( gettokens 'the big brown fox' ) ) 4
~~~
~~~
performtest ( size ( gettokens ( findfile basedirectory LICENSE ) ) ) 168
~~~

---

#### sort

sorts an object (e.g. an array, a list, or a string)

* object: object to be sorted

Examples:

~~~
performtest ( sort 'hello' ) 'ehllo'
~~~
~~~
performtest ( sort ( split '3~n1~n4~n1~n5~n9' ) ) ( array '1' '1' '3' '4' '5' '9' )
~~~

---

#### Sort

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Sort

---

#### sortarray

sorts an array

* object: array

Examples:

~~~
performtest ( sortarray ( array 3 1 4 1 5 9 2 6 0 ) ) ( array 0 1 1 2 3 4 5 6 9 )
~~~

---

#### sortstring

sorts the characters of a string

* string: string

Examples:

~~~
performtest ( sortstring 'foobar' ) 'abfoor'
~~~

---

#### split

creates an array by splitting a string at carriage-returns

* string: string to be splitted

Examples:

~~~
performtest ( at ( split 'Hello~nworld!' ) 1 ) 'world!'
~~~
~~~
performtest ( upperbound ( split 'Hello~nworld' ) ) 1
~~~
~~~
performtest
  (
    writefile c:\testdirectory\values.txt '6~n1~n7~n8~n5~n9'
    average ( split ( readfile c:\testdirectory\values.txt ) )
  )
  6
~~~
~~~
performtest ( sort ( split '3~n1~n4~n1~n5~n9' ) ) ( array '1' '1' '3' '4' '5' '9' )
~~~

---

#### Split

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Split

---

#### square

computes the square of a number

* n: number

Examples:

~~~
performtest ( square 2 ) 4
~~~

---

#### Stack.Synchronized

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Synchronized

---

#### StartsWith

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.StartsWith

---

#### String.Compare

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Compare

---

#### String.CompareOrdinal

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.CompareOrdinal

---

#### String.Concat

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Concat

---

#### String.Copy

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Copy

---

#### String.Create

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Create

---

#### String.Equals

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Equals

---

#### String.Format

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Format

---

#### String.GetHashCode

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.GetHashCode

---

#### String.Intern

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Intern

---

#### String.IsInterned

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.IsInterned

---

#### String.IsNullOrEmpty

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.IsNullOrEmpty

---

#### String.IsNullOrWhiteSpace

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.IsNullOrWhiteSpace

---

#### String.Join

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Join

---

#### stringtail

returns a string without its first character

* string: string

Examples:

~~~
performtest ( stringtail 'Hello' ) 'ello'
~~~

---

#### Substring

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Substring

---

#### sum

returns the sum of the values of an enumeration

* values: values

Examples:

~~~
performtest ( sum ( array 1 2 3 ) ) 6
~~~

---

#### sumof

returns the sum of the arguments

Examples:

~~~
performtest ( sumof 1 2 3 ) 6
~~~

---

#### switch

calls the first command for which a condition holds true

* condition-command-pairs: pairs consisting of condition and command; the first command whose condition is 'true' is executed

Examples:

~~~
performtest
  (
    switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 )
  )
  2
~~~

---

#### SyncRoot

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.SyncRoot
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.SyncRoot
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.SyncRoot
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.SyncRoot
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.SyncRoot

---

#### System

various tests

---

#### tail

tail of an object (e.g. an array, a list, or a string)

* object: array, list, or string

Examples:

~~~
performtest ( tail ( array 1 2 3 4 ) ) ( array 2 3 4 )
~~~

---

#### ToArray

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.ToArray
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.ToArray
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.ToArray

---

#### tochar

converts the first character of a string to type "char"

* text: string

Examples:

~~~
performtest ( String.new ( tochar 'a' ) 10 ) 'aaaaaaaaaa'
~~~
~~~
performtest ( iswhitespace ( tochar 'a' ) ) False
~~~
~~~
performtest ( iswhitespace ( tochar ' ' ) ) True
~~~

---

#### tocharacters

converts a string to an array of characters

* string: string

Examples:

~~~
performtest ( typename ( tocharacters 'Hello World!' ) ) 'Char[]'
~~~

---

#### ToCharArray

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.ToCharArray

---

#### todouble

converts an integer value to a double value

* number: number to be converted

Examples:

~~~
performtest ( joinfrom ( typename 1 ) ( typename ( todouble 1 ) ) ) 'Int32 Double'
~~~

---

#### ToLower

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.ToLower

---

#### ToLowerInvariant

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.ToLowerInvariant

---

#### ToString

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Object.ToString
* https://docs.microsoft.com/en-us/dotnet/api/System.ValueType.ToString

Examples:

~~~
performtest ( ToString ( at 'Hello' 1 ) ) 'e'
~~~

---

#### ToUpper

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.ToUpper

---

#### ToUpperInvariant

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.ToUpperInvariant

---

#### Trim

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Trim

---

#### TrimEnd

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.TrimEnd

---

#### TrimStart

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.TrimStart

---

#### TrimToSize

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.TrimToSize
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.TrimToSize

---

#### TryFormat

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Int32.TryFormat

---

#### typename

returns the name of the type of an object

* object: object

Examples:

~~~
performtest ( typename 'Hello' ) 'String'
~~~
~~~
performtest ( joinfrom ( typename 1 ) ( typename ( todouble 1 ) ) ) 'Int32 Double'
~~~
~~~
performtest ( typename ( typeof 'foobar' ) ) RuntimeType
~~~
~~~
performtest ( typename ( tocharacters 'Hello World!' ) ) 'Char[]'
~~~

---

#### typeof

returns the type-object of an object

* object: object

Examples:

~~~
performtest ( typename ( typeof 'foobar' ) ) RuntimeType
~~~

---

#### upperbound

returns the upper-bound of an object (e.g. an array or a list)

* object: object

Examples:

~~~
performtest ( upperbound ( array 1 2 3 ) ) 2
~~~
~~~
performtest ( upperbound ( split 'Hello~nworld' ) ) 1
~~~

---

#### Value

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.DictionaryEntry.Value

---

#### Values

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Values

---

#### while

performs a while-loop

* condition: condition for continuing the loop
* code: body of the while-loop

---

#### write

writes text on the console

* text: text to be written

Examples:

~~~
performtest ( write 'foobar' ) 'foobar'
~~~

---

#### writefile

writes text to a file

* path: path of the file
* text: text to be written

Examples:

~~~
performtest
  (
    writefile c:\testdirectory\hello.txt hello
    readfile c:\testdirectory\hello.txt
  )
  'hello'
~~~
~~~
performtest
  (
    writefile c:\testdirectory\world.txt world
    readfile c:\testdirectory\world.txt
  )
  'world'
~~~
~~~
performtest
  (
    writefile c:\testdirectory\values.txt '6~n1~n7~n8~n5~n9'
    average ( split ( readfile c:\testdirectory\values.txt ) )
  )
  6
~~~

#### Missing Documentation:


* iswhitespace
  - character

* newarray
  - initialvalue