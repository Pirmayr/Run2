# Run2

Run2 is a simple scripting language. It is intended to be a robust tool for running tasks. The original motivation was to be able to build and test software reliably.

## Properties

* Very simple syntax. Scripts consist entirely of commands. Commands can be nested and can return values. 
* It is easy to add new commands. In fact, the majority of predefined commands is defined in Run2.
* Run2 is built on top on .NET, so every functionality in .NET is available in Run2.
* Run2 needs no setup. "Run2.exe" is everything needed.

## Examples

### Simple Commands

```
! Lists the files in "c:\windows" by executing the dir-command of windows: 
dir c:\windows

! Runs "notepad.exe":
run notepad.exe ! Runs the notepad

! Write the text "Hello World" into the file "c:\hello.txt":
writefile c:\hello.txt 'Hello World!' ! 
```

### Composed Commands

```
! Displays the number of files in "c:\windows":
write (size (getfiles c:\windows)) ! 

! Read "c:\somefile.txt", split the contents into separate lines, and display the number of lines:
write (size (split (readfile c:\somefile.txt))) 
```

### Define Commands

```
! Compute the square of a number:
command square number
  * number number
  
! Compute the factorial of a number:
command factorial number 
  if (== number 1) 
    1 
    (* (factorial (- number 1)) number)
```

These new commands can be used as follows:

```
write (square 11)
write (factorial 10)
```

There is no difference between predefined commands and commands defined by the user.