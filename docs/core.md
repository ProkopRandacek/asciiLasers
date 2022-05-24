# AL Core
The core module implements basic laser modifiers, mirrors, stdout and stdin.

## Mirrors

There are 4 mirror blocks:  
Symbol | Description
-------|-------------
`^`    | Reflects all lasers up
`v`    | Reflects all lasers down
`>`    | Reflects all lasers right
`<`    | Reflects all lasers left

## Laser modifiers

### Regular modifiers:
Symbol | Description               | Number of inputs
-------|---------------------------|------------------
`*`    | Outputs unmodified input  | 1
`i`    | Increments the input by 1 | 1
`d`    | Decrements the input by 1 | 1
`#`    | Never outputs anything    | 1
`m`    | Multiplication            | 2
`n`    | Division                  | 2
`a`    | Addition                  | 2
`s`    | Subtraction               | 2
`l`    | Modulo                    | 2
`:` | Duplicator Reemits laser twice in the upcoming two ticks, buffered | 1
`?` | Compare the two laster and outputs 0 if they are equal, 1 if the first laser is bigger and 2 if the seconds lasers is bigger | 2

### Any of `0123456789ABCDEF`
Sets the laser frequency to that value (hexadecimal)  
TODO: could be better

## IO

### `$`
Prints the frequency of the incoming laser to stdout (in base 10), followed by newline.

### `&`
Same as `$` but prints value as utf32 character and no newline is inserted.  

### `{`
The start point of a program.  
Emits a value of `1` and then is inactive for the rest of the execution.  

### `}`
The end point of a program.  
Terminates the program on any laser interaction and uses the laser value as a exit code.  

### `_`
When laser hits this blocks, a single byte is read from stdin. If there is no byte on stdin, the evaluation of this block waits until there is one.

## Functions

Functions bodies are surrouded by a frame marked by the `/` and `\` characters in its corners.

There can be no blocks at the function frame, except for function name which follows immediately after the top left `/` and bottom left `\`.
  
Example:
```
Outside
/name       \
 inside
 also inside
  unsafe -->x
\name       /
```

Becomes

```
Outside
/iiii#######\
#inside     #
#also inside#
# unsafe -->#
\oooo#######/
```

The `i` and `o` blocks on the border here represent input and output slots reprectively.

```
/abcd  \
 vvvv
 $&id
   vv

\abcd  /

{ ****>
  vvvv

 !abcd
    vv
    $
     >  }
```
Here, the `abcd` function prints the first two slots, the last two slots are modified and return throught the output slots.
The function is then called with some values demonstrating the function calling syntax.

Function calls have to start with `!` and end with white space.

