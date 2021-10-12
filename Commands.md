# Predefined Run2-Commands

#### -

subtracts second number from first number

* a: first number
* b: second number

**Examples**

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

**Examples**

~~~
performtest ( != 47 11 ) True
~~~

---

#### *

multiplies two numbers

* a: first number
* b: second number

**Examples**

~~~
performtest ( * 47 11 ) 517
~~~
~~~
performtest (
  local product 1
  map ( arrayof 3 1 4 1 ) ( local product ( * product item ) )
  )
  12
~~~

---

#### /

divides first number by second number

* a: first number
* b: second number

**Examples**

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

**Examples**

~~~
performtest ( + 47 11 ) 58
~~~
~~~
performtest (
  local result 0
  for i 1 10 1 ( local result ( + result i ) )
  )
  55
~~~
~~~
performtest (
  local result 0
  foreach i ( arrayof 3 1 4 1 ) ( local result ( + result i ) )
  )
  9
~~~

---

#### <

tests if value1 is less than value2

* value1: first value
* value2: second value

**Examples**

~~~
performtest ( < ( - ( / 22.0 7.0 ) 3.142857 ) 0.000001 ) True
~~~
~~~
performtest ( < 47 11 ) False
~~~
~~~
performtest ( switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 ) ) 2
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

**Examples**

~~~
performtest ( <= 47 11 ) False
~~~

---

#### ==

tests two values for equality

* value1: first value
* value2: second value

**Examples**

~~~
performtest ( == 47 11 ) False
~~~
~~~
performtest ( == ( arrayof 1 2 3 ) ( arrayof 1 2 3 ) ) True
~~~
~~~
performtest ( == ( arrayof 1 2 3 ) ( arrayof 1 2 4 ) ) False
~~~
~~~
performtest ( == ( arrayof 1 2 3 4 ) ( arrayof 1 2 3 ) ) False
~~~
~~~
performtest ( switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 ) ) 2
~~~
~~~
performtest ( for i 1 10 1 ( if ( == i 5 ) ( break i ) i ) ) 5
~~~

---

#### >

tests if value1 is greater than value2

* value1: first value
* value2: second value

**Examples**

~~~
performtest ( > 47 11 ) True
~~~
~~~
performtest ( switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 ) ) 2
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

**Examples**

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

* dictionary: dictionary
* key: key
* value: value

**Returns**


the dictionary

**Examples**

~~~
performtest (
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

* list: listof
* value: value

**Returns**


the list

**Examples**

~~~
performtest ( size ( additem ( listof 1 2 3 ) 4 ) ) 4
~~~

---

#### AddRange

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.AddRange

---

#### and

performs the 'and'-operation

* value1: first value
* value2: second value

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

#### arrayof

converts the arguments to an array

**Returns**


an array

**Examples**

~~~
performtest ( == ( arrayof 1 2 3 ) ( arrayof 1 2 3 ) ) True
~~~
~~~
performtest ( == ( arrayof 1 2 3 ) ( arrayof 1 2 4 ) ) False
~~~
~~~
performtest ( == ( arrayof 1 2 3 4 ) ( arrayof 1 2 3 ) ) False
~~~
~~~
performtest ( sum ( arrayof 1 2 3 ) ) 6
~~~
~~~
performtest ( average ( arrayof 1 2 3 4 ) ) 2.5
~~~
~~~
performtest (
  local result 0
  foreach i ( arrayof 3 1 4 1 ) ( local result ( + result i ) )
  )
  9
~~~
~~~
performtest (
  local product 1
  map ( arrayof 3 1 4 1 ) ( local product ( * product item ) )
  )
  12
~~~
~~~
performtest ( upperbound ( arrayof 1 2 3 ) ) 2
~~~
~~~
performtest ( size ( arrayof 1 2 3 4 5 6 ) ) 6
~~~
~~~
performtest ( sort ( split '3~n1~n4~n1~n5~n9' ) ) ( arrayof '1' '1' '3' '4' '5' '9' )
~~~
~~~
performtest ( sort ( split '3;1;4;1;5;9' ';' ) ) ( arrayof '1' '1' '3' '4' '5' '9' )
~~~
~~~
performtest ( join ( arrayof 'h' 'e' 'l' 'l' 'o' ) ) 'h e l l o'
~~~
~~~
performtest ( concatenation ( arrayof foo bar ) ) foobar
~~~
~~~
performtest ( isarray ( arrayof 1 2 3 ) ) True
~~~
~~~
performtest ( sortarray ( arrayof 3 1 4 1 5 9 2 6 0 ) ) ( arrayof 0 1 1 2 3 4 5 6 9 )
~~~
~~~
performtest ( head ( arrayof 1 2 3 4 ) ) 1
~~~
~~~
performtest ( arraytail ( arrayof 4 7 1 1 ) ) ( arrayof 7 1 1 )
~~~
~~~
performtest ( tail ( arrayof 1 2 3 4 ) ) ( arrayof 2 3 4 )
~~~
~~~
performtest ( median ( arrayof 5 3 2 4 1 ) ) 3
~~~
~~~
performtest ( islist ( arrayof ) ) False
~~~
~~~
performtest ( isarray ( arrayof ) ) True
~~~
~~~
performtest ( notempty ( arrayof ) ) False
~~~
~~~
performtest ( notempty ( arrayof 1 2 3 ) ) True
~~~
~~~
performtest (
  tail (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    return queue
    )
  )
  ( arrayof 11 )
~~~

---

#### arraytail

returns an array without its first element

* array: array

**Examples**

~~~
performtest ( arraytail ( arrayof 4 7 1 1 ) ) ( arrayof 7 1 1 )
~~~

---

#### at

returns the element of an array, a list, or a string at the specified index

* object: array, list, or string
* index: index of the element

**Examples**

~~~
performtest ( ToString ( at 'Hello' 1 ) ) 'e'
~~~
~~~
performtest ( at ( split 'Hello~nworld!' ) 1 ) 'world!'
~~~
~~~
performtest (
  local foobar 4711
  at ( quote foobar ) 0
  )
  foobar
~~~
~~~
performtest (
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

**Returns**


the average

**Examples**

~~~
performtest ( average ( arrayof 1 2 3 4 ) ) 2.5
~~~
~~~
performtest (
  writefile c:\testdirectory\values.txt '6~n1~n7~n8~n5~n9'
  average ( split ( readfile c:\testdirectory\values.txt ) )
  )
  6
~~~

---

#### averageof

computes the average of the arguments

**Returns**


the average

**Examples**

~~~
performtest ( averageof 1 2 3 4 ) 2.5
~~~

---

#### besttype

tries to find the best type for the given object

* object: object

**Examples**

~~~
performtest ( besttype '4711' ) 4711
~~~

---

#### BigInteger.Abs

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Abs

---

#### BigInteger.Add

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Add

---

#### BigInteger.Compare

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Compare

---

#### BigInteger.Divide

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Divide

---

#### BigInteger.DivRem

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.DivRem

---

#### BigInteger.GreatestCommonDivisor

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.GreatestCommonDivisor

---

#### BigInteger.Log

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Log

---

#### BigInteger.Log10

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Log10

---

#### BigInteger.Max

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Max

---

#### BigInteger.Min

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Min

---

#### BigInteger.MinusOne

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.MinusOne

---

#### BigInteger.ModPow

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.ModPow

---

#### BigInteger.Multiply

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Multiply

---

#### BigInteger.Negate

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Negate

---

#### BigInteger.One

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.One

---

#### BigInteger.Parse

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Parse

---

#### BigInteger.Pow

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Pow

---

#### BigInteger.Remainder

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Remainder

---

#### BigInteger.Subtract

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Subtract

---

#### BigInteger.TryParse

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.TryParse

---

#### BigInteger.Zero

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Zero

---

#### BinarySearch

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.BinarySearch

---

#### break

breaks the innermost loop and returns a value

* value: the value to be returned

**Examples**

~~~
performtest ( for i 1 10 1 ( if ( == i 5 ) ( break i ) i ) ) 5
~~~

---

#### canparseint32

tests whether the specified string can be parsed to an int32

* string: string

**Examples**

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

#### Char.MaxValue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.MaxValue

---

#### Char.MinValue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Char.MinValue

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
* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.CompareTo

---

#### concatenation

concatenates the given strings

* strings: strings

**Examples**

~~~
performtest ( concatenation ( arrayof foo bar ) ) foobar
~~~

---

#### concatenationof

concatenates the arguments

**Returns**


the concatenated arguments

**Remarks**


the arguments must be strings

**Examples**

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

#### Convert.DBNull

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Convert.DBNull

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

#### copyfiles

copies files from one location to another

* sourcedirectory: directory to copy from
* sourcespecification: name of the file to copy or a pattern (e.g. "*.run2")
* destinationdirectory: directory to copy to
* destinationfilename: name of the file after beeing copied; if "null" or empty, the filename remains unchanged
* expandincludes: if true, includes are expanded
* lineaction: action to be performed on every line

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

**Examples**

~~~
performtest ( Count ( listof 1 2 3 4 5 ) ) 5
~~~

---

#### createdirectory

creates specified directory

* directory: directory to be created

**Examples**

~~~
performtest (
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

**Examples**

~~~
performtest (
  local a 4711
  decrement a 11
  return a
  )
  4700
~~~

---

#### deletedirectory

deletes specified directory

* directory: directory to be deleted

**Examples**

~~~
performtest (
  deletedirectory c:\testdirectory
  directoryexists c:\testdirectory
  )
  False
~~~

---

#### dequeue

dequeues an element

* queue: queue

**Examples**

~~~
performtest (
  local queue ( newqueue )
  enqueue queue 47
  enqueue queue 11
  dequeue queue
  )
  47
~~~
~~~
performtest ( dequeue ( newqueue ( listof 1 2 3 ) ) ) 1
~~~

---

#### Dequeue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Dequeue

---

#### dir

executes the Windows command "dir" and returns the result

**Examples**

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

tests wether the specified directory exists

* directory: directory to be tested for existence

**Examples**

~~~
performtest (
  createdirectory c:\testdirectory
  directoryexists c:\testdirectory
  )
  True
~~~
~~~
performtest (
  deletedirectory c:\testdirectory
  directoryexists c:\testdirectory
  )
  False
~~~

---

#### directoryseparator

---

#### dos

executes specified dos-command and returns the output

**Examples**

~~~
performtest ( dos echo 'hello world' ) 'hello world'
~~~

---

#### endswith

tests if a string ends with an other string

* string: string
* value: string to be tested for being the ending

**Examples**

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

**Examples**

~~~
performtest (
  local queue ( newqueue )
  enqueue queue 47
  enqueue queue 11
  dequeue queue
  )
  47
~~~
~~~
performtest (
  tail (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    return queue
    )
  )
  ( arrayof 11 )
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

**Examples**

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

**Examples**

~~~
performtest ( fileexists ( locatefile basedirectory LICENSE ) ) True
~~~
~~~
performtest ( fileexists ( locatefile basedirectory DOESNOTEXIST ) ) False
~~~

---

#### finddirectory

gets the directory with the given name

* directory: directory from where to search upwards
* name: name

**Examples**

~~~
performtest ( finddirectory 'C:\Windows\System32\drivers' 'Windows' ) 'C:\Windows'
~~~

---

#### finddirectorywithparent

finds the directory whose parent has the given name

* directory: directory from where to search upwards
* parent: name of the parent

**Examples**

~~~
performtest ( finddirectorywithparent 'C:\Windows\System32\drivers' 'Windows' ) 'C:\Windows\System32'
~~~

---

#### for

performs a for-loop

* name: name of the variable which holds the counter
* from: start value of the counter
* to: end value of the counter
* step: increment for the counter
* code: body of the for-loop

**Examples**

~~~
performtest (
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

**Examples**

~~~
performtest (
  local result 0
  foreach i ( arrayof 3 1 4 1 ) ( local result ( + result i ) )
  )
  9
~~~

---

#### get

return the value of a variable; the variable can exist in any active scope

* name: name of the variable

---

#### GetBitLength

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.GetBitLength

---

#### GetByteCount

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.GetByteCount

---

#### getcode

returns the formatted script

---

#### getcommands

returns the list of commands

---

#### getdirectory

gets the directory from a path

* path: path

**Examples**

~~~
performtest ( getdirectory 'C:\Windows\notepad.exe' ) 'C:\Windows'
~~~

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

#### getextension

gets the extension from a path

* path: path

**Examples**

~~~
performtest ( getextension 'c:\windows\notepad.exe' ) '.exe'
~~~

---

#### getfilename

gets the filename from a path

* path: path

**Examples**

~~~
performtest ( getfilename 'C:\Windows\notepad.exe' ) 'notepad.exe'
~~~
~~~
performtest ( getfilename ( removetrailingdirectoryseparator 'c:\windows\system32\' ) ) 'system32'
~~~

---

#### getfiles

returns an array of paths of the files in a directory

* directory: directory

**Examples**

~~~
performtest ( size ( getfiles c:\testdirectory ) ) 3
~~~

---

#### getfname

gets the filename without extension from a path

* path: path

**Examples**

~~~
performtest ( getfname 'C:\Windows\System32' ) 'System32'
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

get the array of member-information for the specified member in an object

* object: object
* name: name of the member

**Returns**


an array of "MemberInfo"-values

**Examples**

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

returns the tokenized contents of a string

* string: string

**Returns**


a list with tokens

**Examples**

~~~
performtest ( size ( gettokens 'the big brown fox' ) ) 4
~~~
~~~
performtest ( size ( gettokens ( readfile ( locatefile basedirectory LICENSE ) ) ) ) 168
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

**Examples**

~~~
performtest (
  global value 4711
  increment value 1
  return value
  )
  4712
~~~
~~~
performtest (
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

tests if the specified member exists in an object

* object: object
* name: name of the member

**Returns**


if the object has the specified member, "true" is returned, otherwise "false"

**Examples**

~~~
performtest ( hasmember 'hello' 'Length' ) True
~~~
~~~
performtest ( hasmember 'hello' 'Count' ) False
~~~

---

#### hastype

tests if an object has the given type

* object: object
* name: name of the type

**Returns**


if the object has the given type, "true" is returned, otherwise "false"

**Examples**

~~~
performtest ( hastype 'hello' String ) True
~~~

---

#### head

returns the first element of an object

* object: object

**Returns**


if a first element can be obtained, it is returned, otherwise an empty list

**Examples**

~~~
performtest ( head ( arrayof 1 2 3 4 ) ) 1
~~~
~~~
performtest ( head ( listof 4 7 1 1 ) ) 4
~~~

---

#### if

performs the if-statement

* condition: condition
* true-block: command to be executed if the condition is 'true'
* false-block: (optional) command to be executed if the condition is 'false'

**Examples**

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

**Returns**


the incremented value of the variable

**Remarks**


incrementing a string by a string means appending one string to the other

**Examples**

~~~
performtest (
  global value 4711
  increment value 1
  return value
  )
  4712
~~~
~~~
performtest (
  local value 1234
  increment value 2
  return value
  )
  1236
~~~
~~~
performtest (
  local value 'foo'
  increment value 'bar'
  return value
  )
  'foobar'
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

performs various initializations

**Returns**


the command always returns "true"

**Examples**

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

#### Int32.MaxValue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Int32.MaxValue

---

#### Int32.MinValue

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Int32.MinValue

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

**Returns**


if all tests succeeded, true is returned, otherwise false

**Examples**

~~~
performtest ( invoketests ) True
~~~

---

#### isarray

tests if an object is an array

* object: object

**Returns**


if the object is an array, "true" is returned, otherwise "false"

**Remarks**


the test returns the value of the "IsArray"-property of the type of the object

**Examples**

~~~
performtest ( isarray ( arrayof 1 2 3 ) ) True
~~~
~~~
performtest ( isarray ( arrayof ) ) True
~~~
~~~
performtest ( isarray ( newarray 100 0 ) ) True
~~~

---

#### ischar

tests if an object is a "char"-value

* object: object

**Returns**


a "bool"-value

**Remarks**


if the object is a "char"-value, "true" is returned, otherwise "false"

**Examples**

~~~
performtest ( ischar ( tochar 'hello' ) ) True
~~~
~~~
performtest ( ischar ( tochar 4711 ) ) True
~~~

---

#### isconvertabletoarray

test, if an object can be converted to an array

* object: object

**Returns**


if the object can be converted to an array, "true" is returned, otherwise "false"

**Remarks**


the test checks if the object already is an array, or has the method "ToArray"

---

#### isdictionary

tests if an object is a dictionary

* object: object

**Returns**


if the object in a dictionary, "true" is returned, otherwise "false"

**Remarks**


the test checks, if the object if of the Hashtable-type

**Examples**

~~~
performtest ( isdictionary ( newdictionary ) ) True
~~~

---

#### isempty

checks wether an object is isempty

* object: object

**Returns**


if the object is empty, "true" is returned, otherwise "false"

**Examples**

~~~
performtest ( isempty ( listof ) ) True
~~~
~~~
performtest ( isempty ( listof 1 2 3 ) ) False
~~~

---

#### IsEven

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.IsEven

---

#### IsFixedSize

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.IsFixedSize
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.IsFixedSize
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.IsFixedSize

---

#### islist

tests if an object is a list

* object: object

**Returns**


if the object is a list, "true" is returned, otherwise "false"

**Remarks**


the object can be of the "ArrayList"-type or the generic "List"-type

**Examples**

~~~
performtest ( islist ( listof ) ) True
~~~
~~~
performtest ( islist ( arrayof ) ) False
~~~
~~~
performtest ( islist ( newlist ) ) True
~~~

---

#### IsNormalized

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.IsNormalized

---

#### isnull

tests, if an object has the value "null"

* object: object

**Returns**


if the object has the value "null", "true" is returned, otherwise "false"

---

#### IsOne

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.IsOne

---

#### IsPowerOfTwo

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.IsPowerOfTwo

---

#### IsReadOnly

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.IsReadOnly
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.IsReadOnly
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.IsReadOnly

---

#### isstring

tests if an object is a "string"-value

* object: object

**Returns**


a "bool"-value

**Remarks**


if the object is a "string"-value, "true" is returned, otherwise "false"

**Examples**

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

checks if a value is white space

* value: value to be checked

**Returns**


if the character is white space, "true" is returned, otherwise "false"

**Examples**

~~~
performtest ( iswhitespace ( tochar 'a' ) ) False
~~~
~~~
performtest ( iswhitespace ( tochar ' ' ) ) True
~~~
~~~
performtest ( iswhitespace ( tochar 4711 ) ) False
~~~

---

#### IsZero

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.IsZero

---

#### Item

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Item
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.Item

---

#### join

joins strings to a string separated by blanks

* strings: strings

**Returns**


a string

**Remarks**


the command applies to arrays of strings or any object that can be converted to an array of strings

**Examples**

~~~
performtest ( join ( arrayof 'h' 'e' 'l' 'l' 'o' ) ) 'h e l l o'
~~~

---

#### joinfrom

joins the arguments to a string separated by blanks

**Returns**


a string

**Examples**

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

**Examples**

~~~
performtest ( Length 'Hello World!' ) 12
~~~

---

#### listof

returns the arguments as a list

**Returns**


a list

**Examples**

~~~
performtest ( Count ( listof 1 2 3 4 5 ) ) 5
~~~
~~~
performtest ( size ( listof 1 2 3 4 5 6 7 ) ) 7
~~~
~~~
performtest ( head ( listof 4 7 1 1 ) ) 4
~~~
~~~
performtest ( listtail ( listof 4 7 1 1 ) ) ( listof 7 1 1 )
~~~
~~~
performtest ( size ( additem ( listof 1 2 3 ) 4 ) ) 4
~~~
~~~
performtest ( islist ( listof ) ) True
~~~
~~~
performtest ( dequeue ( newqueue ( listof 1 2 3 ) ) ) 1
~~~
~~~
performtest ( size ( newqueue ( listof 1 2 3 ) ) ) 3
~~~
~~~
performtest ( isempty ( listof ) ) True
~~~
~~~
performtest ( isempty ( listof 1 2 3 ) ) False
~~~

---

#### listtail

returns a list without its first item

* items: listof

**Returns**


the list without the first item

**Examples**

~~~
performtest ( listtail ( listof 4 7 1 1 ) ) ( listof 7 1 1 )
~~~

---

#### local

creates or sets a local variable

* name: name of the variable
* value: value to be assigned to the variable

**Examples**

~~~
performtest (
  local result 0
  for i 1 10 1 ( local result ( + result i ) )
  )
  55
~~~
~~~
performtest (
  local result 0
  foreach i ( arrayof 3 1 4 1 ) ( local result ( + result i ) )
  )
  9
~~~
~~~
performtest (
  local product 1
  map ( arrayof 3 1 4 1 ) ( local product ( * product item ) )
  )
  12
~~~
~~~
performtest (
  local value 1234
  increment value 2
  return value
  )
  1236
~~~
~~~
performtest (
  local a 4711
  set a 1234
  )
  1234
~~~
~~~
performtest (
  local foobar 4711
  at ( quote foobar ) 0
  )
  foobar
~~~
~~~
performtest (
  local values ( newarray 1000 0 )
  put values 100 'foobar'
  at values 100
  )
  'foobar'
~~~
~~~
performtest (
  local queue ( newqueue )
  enqueue queue 47
  enqueue queue 11
  dequeue queue
  )
  47
~~~
~~~
performtest (
  local stack ( newstack )
  push stack 47
  push stack 11
  pop stack
  )
  11
~~~
~~~
performtest (
  local a 4711
  decrement a 11
  return a
  )
  4700
~~~
~~~
performtest (
  local value 'foo'
  increment value 'bar'
  return value
  )
  'foobar'
~~~
~~~
performtest (
  tail (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    return queue
    )
  )
  ( arrayof 11 )
~~~

---

#### locatedirectories

searches directories

* basedirectory: directory in which the search should begin
* pattern: pattern to be searched for

**Returns**


an array of all directories that could be found

---

#### locatedirectory

searches for a directory

* basedirectory: directory in which the search should begin
* pattern: pattern to be searched for

**Returns**


if the directory could be found, the directory is returned, otherwise an empty string

**Examples**

~~~
performtest ( locatedirectory SystemRoot System32 ) '[SystemRoot]\System32'
~~~

---

#### locatefile

searches for a file

* basedirectory: directory in which the search should begin
* pattern: pattern to be searched for

**Returns**


if the file could be found, the path of the file is returned, otherwise an empty string

**Examples**

~~~
performtest ( locatefile SystemRoot notepad.exe ) '[SystemRoot]\notepad.exe'
~~~
~~~
performtest ( fileexists ( locatefile basedirectory LICENSE ) ) True
~~~
~~~
performtest ( fileexists ( locatefile basedirectory DOESNOTEXIST ) ) False
~~~
~~~
performtest ( size ( gettokens ( readfile ( locatefile basedirectory LICENSE ) ) ) ) 168
~~~

---

#### locatefiles

searches files

* basedirectory: directory in which the search should begin
* pattern: pattern to be searched for

**Returns**


an array of all paths that could be found

---

#### LongLength

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.LongLength

---

#### map

executes a command with all elements of an array or list; the variable 'item' holds the current element

* arrayOrList: array or list
* command: command

**Examples**

~~~
performtest (
  local product 1
  map ( arrayof 3 1 4 1 ) ( local product ( * product item ) )
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

#### Math.E

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.E

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

#### Math.PI

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.PI

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

**Examples**

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

#### Math.Tau

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Tau

---

#### Math.Truncate

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Math.Truncate

---

#### median

returns the median of the given numbers

* values: numbers

**Returns**


the median

**Examples**

~~~
performtest ( median ( arrayof 5 3 2 4 1 ) ) 3
~~~

---

#### medianof

returns the median of the arguments

**Returns**


the median

**Examples**

~~~
performtest ( medianof 5 3 2 4 1 ) 3
~~~

---

#### multireplace

replaces by multiple pairs of searchstring/replacement

**Returns**


the modified string

**Examples**

~~~
performtest ( multireplace 'Hello World!' 'Hello' 'Good' 'World' 'Bye' ) 'Good Bye!'
~~~

---

#### newarray

creates an array

* length: size of the array
* initialvalue: (optional) value with which the array is to be initialized

**Returns**


the new array

**Examples**

~~~
performtest ( size ( newarray 1000 0 ) ) 1000
~~~
~~~
performtest (
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

creates a dictionary

**Returns**


the new dictionary

**Remarks**


the dictionary is of type "Hashtable"

**Examples**

~~~
performtest (
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

* values: (optional) values with which the list is to be initialized

**Returns**


the new list

**Remarks**


the type of the list is "ArrayList"

**Examples**

~~~
performtest ( islist ( newlist ) ) True
~~~

---

#### newqueue

creates a queue

* values: (optional) value with which the queue is to be initialized

**Returns**


the new queue

**Examples**

~~~
performtest (
  local queue ( newqueue )
  enqueue queue 47
  enqueue queue 11
  dequeue queue
  )
  47
~~~
~~~
performtest ( dequeue ( newqueue ( listof 1 2 3 ) ) ) 1
~~~
~~~
performtest ( size ( newqueue ( listof 1 2 3 ) ) ) 3
~~~
~~~
performtest (
  tail (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    return queue
    )
  )
  ( arrayof 11 )
~~~

---

#### newstack

creates a stack

**Returns**


a stack

**Examples**

~~~
performtest (
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

**Returns**


if the value is "false", "true" is returned, otherwise "false"

**Examples**

~~~
performtest ( not True ) False
~~~

---

#### notempty

checks if an object is not empty

* object: object

**Returns**


if the object is empty, "false" is returned, otherwise "true"

**Examples**

~~~
performtest ( notempty ( arrayof ) ) False
~~~
~~~
performtest ( notempty ( arrayof 1 2 3 ) ) True
~~~

---

#### null

the value 'null'

---

#### OnDeserialization

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Hashtable.OnDeserialization

---

#### or

performs the 'or'-operation

* value1: first value
* value2: second value

---

#### PadLeft

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.PadLeft

---

#### PadRight

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.PadRight

---

#### Path.AltDirectorySeparatorChar

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.AltDirectorySeparatorChar

---

#### Path.ChangeExtension

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.ChangeExtension

---

#### Path.Combine

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.Combine

---

#### Path.DirectorySeparatorChar

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.DirectorySeparatorChar

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

#### Path.InvalidPathChars

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.InvalidPathChars

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

#### Path.PathSeparator

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.PathSeparator

---

#### Path.TrimEndingDirectorySeparator

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.TrimEndingDirectorySeparator

---

#### Path.TryJoin

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.TryJoin

---

#### Path.VolumeSeparatorChar

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.IO.Path.VolumeSeparatorChar

---

#### Peek

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.Peek
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.Peek

---

#### performtest

tests if a code-block yields the expected result

* "code": code to be tested
* expected: expected value

**Returns**


if the test succeeds true ist returned, otherwise false

---

#### performtests

performs various tests

**Returns**


if all tests succeeded, true is returned, otherwise false

**Examples**

~~~
performtest ( performtests ) True
~~~

---

#### pop

pops an element

* stack: stack

**Returns**


the element popped

**Examples**

~~~
performtest (
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

**Returns**


the power

**Examples**

~~~
performtest ( power 3 4 ) 81
~~~

---

#### primes

computes the primes up to n

* n: upper limit

**Returns**


a list of primes up to n

**Examples**

~~~
performtest ( size ( primes 1000 ) ) 167
~~~

---

#### push

pushes an element

* stack: stack
* value: element

**Returns**


the element pushed

**Examples**

~~~
performtest (
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

**Examples**

~~~
performtest (
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

returns the unevaluated arguments

**Returns**


the unenvaluated arguments

**Examples**

~~~
performtest (
  local foobar 4711
  at ( quote foobar ) 0
  )
  foobar
~~~

---

#### Rank

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Array.Rank

---

#### readfile

returns the text contained in a file

* path: path of the file

**Returns**


the text contained in the file

**Examples**

~~~
performtest (
  writefile c:\testdirectory\hello.txt hello
  readfile c:\testdirectory\hello.txt
  )
  'hello'
~~~
~~~
performtest (
  writefile c:\testdirectory\world.txt world
  readfile c:\testdirectory\world.txt
  )
  'world'
~~~
~~~
performtest (
  writefile c:\testdirectory\values.txt '6~n1~n7~n8~n5~n9'
  average ( split ( readfile c:\testdirectory\values.txt ) )
  )
  6
~~~
~~~
performtest ( size ( gettokens ( readfile ( locatefile basedirectory LICENSE ) ) ) ) 168
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

#### removetrailingdirectoryseparator

removes all trailing occurrences of the directory-separator from a path

* path: path

**Examples**

~~~
performtest ( removetrailingdirectoryseparator 'c:\windows\' ) 'c:\windows'
~~~
~~~
performtest ( getfilename ( removetrailingdirectoryseparator 'c:\windows\system32\' ) ) 'system32'
~~~

---

#### replace

replaces a substring

* string: string to be searched for the search-string
* searchstring: string to be searched for
* replacement: replacement

**Returns**


the modified string

**Examples**

~~~
performtest ( replace 'Hello World!' 'Hello' 'Bye' ) 'Bye World!'
~~~

---

#### Replace

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Replace

---

#### return

returns a value

* value: the value to be returned

**Examples**

~~~
performtest (
  global value 4711
  increment value 1
  return value
  )
  4712
~~~
~~~
performtest (
  local value 1234
  increment value 2
  return value
  )
  1236
~~~
~~~
performtest ( switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 ) ) 2
~~~
~~~
performtest ( if ( < 1 1 ) ( return 0 ) ( return 1 ) ) 1
~~~
~~~
performtest (
  local a 4711
  decrement a 11
  return a
  )
  4700
~~~
~~~
performtest (
  local value 'foo'
  increment value 'bar'
  return value
  )
  'foobar'
~~~
~~~
performtest (
  tail (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    return queue
    )
  )
  ( arrayof 11 )
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

**Examples**

~~~
performtest (
  local a 4711
  set a 1234
  )
  1234
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

#### setvariable

* "name"
* value

---

#### Sign

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.Sign

---

#### size

returns the size of an object (e.g. an array, a list, or a string)

* object: object

**Returns**


the size of the object

**Examples**

~~~
performtest ( size 'hello' ) 5
~~~
~~~
performtest ( size ( arrayof 1 2 3 4 5 6 ) ) 6
~~~
~~~
performtest ( size ( listof 1 2 3 4 5 6 7 ) ) 7
~~~
~~~
performtest ( size ( getfiles c:\testdirectory ) ) 3
~~~
~~~
performtest (
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
performtest ( size ( additem ( listof 1 2 3 ) 4 ) ) 4
~~~
~~~
performtest ( size ( primes 1000 ) ) 167
~~~
~~~
performtest ( size ( newqueue ( listof 1 2 3 ) ) ) 3
~~~
~~~
performtest ( size ( gettokens 'the big brown fox' ) ) 4
~~~
~~~
performtest ( size ( gettokens ( readfile ( locatefile basedirectory LICENSE ) ) ) ) 168
~~~

---

#### sort

sorts an object (e.g. an array, a list, or a string)

* object: object to be sorted

**Returns**


the sorted object

**Examples**

~~~
performtest ( sort 'hello' ) 'ehllo'
~~~
~~~
performtest ( sort ( split '3~n1~n4~n1~n5~n9' ) ) ( arrayof '1' '1' '3' '4' '5' '9' )
~~~
~~~
performtest ( sort ( split '3;1;4;1;5;9' ';' ) ) ( arrayof '1' '1' '3' '4' '5' '9' )
~~~

---

#### Sort

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.Sort

---

#### sortarray

sorts an array

* array: array

**Returns**


the sorted array

**Examples**

~~~
performtest ( sortarray ( arrayof 3 1 4 1 5 9 2 6 0 ) ) ( arrayof 0 1 1 2 3 4 5 6 9 )
~~~

---

#### sortstring

sorts the characters of a string

* string: string

**Returns**


the sorted string

**Examples**

~~~
performtest ( sortstring 'foobar' ) 'abfoor'
~~~

---

#### split

creates an array by splitting a string at carriage-returns

* string: string to be splitted
* separator: (optional) separator

**Returns**


an array of strings

**Examples**

~~~
performtest ( at ( split 'Hello~nworld!' ) 1 ) 'world!'
~~~
~~~
performtest ( upperbound ( split 'Hello~nworld' ) ) 1
~~~
~~~
performtest (
  writefile c:\testdirectory\values.txt '6~n1~n7~n8~n5~n9'
  average ( split ( readfile c:\testdirectory\values.txt ) )
  )
  6
~~~
~~~
performtest ( sort ( split '3~n1~n4~n1~n5~n9' ) ) ( arrayof '1' '1' '3' '4' '5' '9' )
~~~
~~~
performtest ( sort ( split '3;1;4;1;5;9' ';' ) ) ( arrayof '1' '1' '3' '4' '5' '9' )
~~~

---

#### Split

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Split

---

#### square

computes the square of a number

* n: number

**Returns**


the square of the number

**Examples**

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

#### String.Empty

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Empty

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

**Returns**


the tail of the string

**Examples**

~~~
performtest ( stringtail 'Hello' ) 'ello'
~~~

---

#### substring

returns a substring

* string: string
* start: start-index of the substring
* length: length of the substring

**Returns**


the substring

**Examples**

~~~
performtest ( substring 'Hello World!' 1 4 ) 'ello'
~~~

---

#### Substring

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.Substring

---

#### sum

returns the sum of the given values

* values: values

**Returns**


the sum

**Remarks**


the values must be numbers

**Examples**

~~~
performtest ( sum ( arrayof 1 2 3 ) ) 6
~~~

---

#### sumof

returns the sum of the arguments

**Returns**


the sum

**Examples**

~~~
performtest ( sumof 1 2 3 ) 6
~~~

---

#### switch

calls the first command for which a condition holds true

* condition-command-pairs: pairs consisting of condition and command; the first command whose condition is 'true' is executed

**Examples**

~~~
performtest ( switch ( < 1 1 ) ( return 1 ) ( == 1 1 ) ( return 2 ) ( > 1 1 ) ( return 3 ) ) 2
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

#### system

various tests

**Returns**


if all tests succeeded, true is returned, otherwise false

**Examples**

~~~
performtest ( system ) True
~~~

---

#### tail

tail of an object (e.g. an array, a list, or a string)

* object: object

**Returns**


the tail of the object

**Remarks**


if the object is a list, an array, or can be converted to an array, the proper tail is returned, otherwise the object is converted to a string and its tail is returned

**Examples**

~~~
performtest ( tail ( arrayof 1 2 3 4 ) ) ( arrayof 2 3 4 )
~~~
~~~
performtest ( tail 4711 ) '711'
~~~
~~~
performtest (
  tail (
    local queue ( newqueue )
    enqueue queue 47
    enqueue queue 11
    return queue
    )
  )
  ( arrayof 11 )
~~~

---

#### tests

performs various tests

---

#### throw

throws an exception

* message: message

---

#### toarray

converts an object to an array

* object: object

**Returns**


an array

**Remarks**


the method can be used all objects, where "isconvertabletoarray" is "true"

---

#### ToArray

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.ArrayList.ToArray
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Queue.ToArray
* https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Stack.ToArray

---

#### tobiginteger

converts a number to a BigInteger

**Returns**


a "BigInteger"-value

**Examples**

~~~
performtest ( typename ( tobiginteger 4711 ) ) 'BigInteger'
~~~
~~~
performtest ( typename ( tobiginteger '4711' ) ) 'BigInteger'
~~~

---

#### ToByteArray

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.ToByteArray

---

#### tochar

converts the value to "char"-value

* value: value

**Returns**


a "char"-value

**Remarks**


if the value is already a "char"-value, it is returned, otherwise the value is converted to a string and the first character is returned

**Examples**

~~~
performtest ( String.new ( tochar 'a' ) 10 ) 'aaaaaaaaaa'
~~~
~~~
performtest ( iswhitespace ( tochar 'a' ) ) False
~~~
~~~
performtest ( iswhitespace ( tochar ' ' ) ) True
~~~
~~~
performtest ( iswhitespace ( tochar 4711 ) ) False
~~~
~~~
performtest ( ischar ( tochar 'hello' ) ) True
~~~
~~~
performtest ( ischar ( tochar 4711 ) ) True
~~~

---

#### tocharacters

converts a string to an array of characters

* string: string

**Returns**


an array of characters

**Examples**

~~~
performtest ( typename ( tocharacters 'Hello World!' ) ) 'Char[]'
~~~

---

#### ToCharArray

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.String.ToCharArray

---

#### todouble

converts value to a "double"-value

* value: number to be converted

**Returns**


a double-value

**Examples**

~~~
performtest ( joinfrom ( typename 1 ) ( typename ( todouble 1 ) ) ) 'Int32 Double'
~~~
~~~
performtest ( typename ( todouble 4711 ) ) 'Double'
~~~
~~~
performtest ( typename ( todouble '4711' ) ) 'Double'
~~~

---

#### toint32

converts a value to an "int32"-value

* value: value to be converted

**Returns**


an "int32"-value

**Remarks**


the value can be a number or a string

**Examples**

~~~
performtest ( toint32 '4321' ) 4321
~~~
~~~
performtest ( typename ( toint32 4711 ) ) 'Int32'
~~~
~~~
performtest ( typename ( toint32 '4711' ) ) 'Int32'
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

#### tostring

converts an object to a string

* object: object

**Returns**


a "string"-value

---

#### ToString

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Object.ToString
* https://docs.microsoft.com/en-us/dotnet/api/System.ValueType.ToString

**Examples**

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

#### trimend

removes all trailing occurrences of a character from a string

* string: string
* character: character

**Examples**

~~~
performtest ( trimend 'c:\windows\' '\' ) 'c:\windows'
~~~

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
* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.TryFormat

---

#### TryWriteBytes

See:

* https://docs.microsoft.com/en-us/dotnet/api/System.Numerics.BigInteger.TryWriteBytes

---

#### typename

returns the name of the type of an object

* object: object

**Returns**


the name of the type

**Examples**

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
~~~
performtest ( typename ( tobiginteger 4711 ) ) 'BigInteger'
~~~
~~~
performtest ( typename ( tobiginteger '4711' ) ) 'BigInteger'
~~~
~~~
performtest ( typename ( toint32 4711 ) ) 'Int32'
~~~
~~~
performtest ( typename ( toint32 '4711' ) ) 'Int32'
~~~
~~~
performtest ( typename ( todouble 4711 ) ) 'Double'
~~~
~~~
performtest ( typename ( todouble '4711' ) ) 'Double'
~~~

---

#### typeof

returns the type-object of an object

* object: object

**Returns**


the type-object

**Examples**

~~~
performtest ( typename ( typeof 'foobar' ) ) RuntimeType
~~~

---

#### upperbound

returns the upper-bound of an object (e.g. an array or a list)

* object: object

**Returns**


the upper-bound

**Remarks**


the upper-bound is the size of the object minus one

**Examples**

~~~
performtest ( upperbound ( arrayof 1 2 3 ) ) 2
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

**Returns**


the text written

**Examples**

~~~
performtest ( write 'foobar' ) 'foobar'
~~~

---

#### writefile

writes text to a file

* path: path of the file
* text: text to be written

**Returns**


the text written

**Examples**

~~~
performtest (
  writefile c:\testdirectory\hello.txt hello
  readfile c:\testdirectory\hello.txt
  )
  'hello'
~~~
~~~
performtest (
  writefile c:\testdirectory\world.txt world
  readfile c:\testdirectory\world.txt
  )
  'world'
~~~
~~~
performtest (
  writefile c:\testdirectory\values.txt '6~n1~n7~n8~n5~n9'
  average ( split ( readfile c:\testdirectory\values.txt ) )
  )
  6
~~~

#### Missing Documentation:


* directoryseparator

* setvariable
  - "name"
  - value

#### Missing Examples:

* copyfiles
* directoryseparator
* isconvertabletoarray
* isnull
* locatedirectories
* locatefiles
* setvariable
* toarray
* tostring