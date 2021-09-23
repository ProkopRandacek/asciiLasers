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
The start point of a region.  
If in global scope, emits a value of `1` and then is inactive for the rest of the execution.  
If inside a function region, acts as a input for the function. When any laser hits the function call, this block emits a laser with the same value.

### `}`
The end point of a region.  
If in global scope, terminates the program on any laser interaction and uses the laser value as a exit code.  
If inside a function, transports incoming lasers into the function call block where they are emitted.

### `_`
When laser hits this blocks, a single byte is read from stdin. If there is no byte on stdin, the evaluation of this block waits until there is one.

