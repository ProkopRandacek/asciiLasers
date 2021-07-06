# AL Core
The core module implements basic laser modifiers, mirrors, stdout and stdin.

## IO

### `$`
Prints the value of any laser that hits it. Laser's value gets printed as number to the stdout followed by newline. Laser gets deleted when it hits it.

### `&`
Same as `$` but prints value as ascii character and no newline is inserted.  
Only first 8 bits are used.  
Example:
```
13633 = 11010101000001
              01000001 = 64 = 'A'
```

### `{`
The start point of the program. Sends a laser to the right with frequency of 1 through all of its output mirrors.

### `}`
The end point of the program. When any laser hits this blocks, the program ends execution and that laser's value gets used as exit code.

### `_`
When laser hits this blocks, user is prompted for a number. All code execution is paused until user types in valid laser value that is then applied to the input laser.

## Mirrors

There are 4 mirror blocks:  
Symbol | Description
-------|-------------
`^` | Reflects all lasers up
`v` | Reflects all lasers down
`>` | Reflects all lasers right
`<` | Reflects all lasers left

On the rising edge of received current, mirrors rotate.
When any mirror is triggered by a wire current, the mirror rotates according to this table:


## Laser modifiers

### Regular modifiers:
Symbol | Description | Number of inputs
-------|-------------|------------------
`*` | Reflects laser into all connected outputs. | 1
`i` | Increments the laser frequency by 1. | 1
`d` | Decrements the laser frequency by 1. | 1
`#` | Never outputs anything | 1
`m` | Multiplication | 2
`n` | Division | 2
`a` | Addition | 2
`s` | Subtraction | 2
`l` | Modulo | 2

### Any of `0123456789ABCDEF`
Sets the laser frequency to the value (hexadecimal)  

