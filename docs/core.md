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

### Any of `0123456789ABCDEF`
Sets the laser frequency to that value (hexadecimal)  

## IO

### `$`
Prints the frequency of the incoming laser to stdout (in base 10), followed by newline.

### `&`
Same as `$` but prints value as ascii character and no newline is inserted.  
Only 8 least significant bits are used for the value.

### `{`
The start point of the program. Sends a laser to the right with frequency of 1 through all of its output mirrors.

### `}`
The end point of the program. When any laser hits this blocks, the program ends execution and that laser's value gets used as exit code.

### `_`
When laser hits this blocks, a single byte is read from stdin. If there is no byte on stdin, the evaluation of this block waits until there is one.

