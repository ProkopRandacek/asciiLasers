# AsciiLaser
AsciiLaser is interpreted, AsciiDots inspired, text based, case sensitive, visual programming language.  
The code is being executed on a board of implementation specific size. Tick time is implementation specific. Program is terminated if no block was evaluated last tick (in which case the exit code is 0) or if any laser hit the `}` block in which case the exit code it that lasers value.  
AL code file extension is `.al`.  

## Comments
Comments start with `[` and end with `]` every character from `[` to `]` including `[` and `]` are treated as spaces when compiling.  
Example:
```
this is not comment [ but this is
this is still comment this too and this [ this too
this still is ] this is not
```

## Data types

### Lasers
Lasers have a frequency (value) between 0 and 2^64 - 1. (unsigned 64 bit int)  
Lasers frequencies overflow.  
Lasers travel instantly on a straight line between 2 blocks (for example between 2 mirrors)  
Lasers travel over wire blocks without influencing each other.  

### Current
Current can only hold 0 or 1.
Current moves instantly along the entire wire.  
When blocks get powered by current, they alter their state (for example mirrors rotate)

## Blocks
Block is a single ascii character that has some functionality in the language.  
All blocks have output mirrors except for mirror blocks  
Examples:
```
m
v
```
Multiplier with inputs from up, left and right. Output is send down.
```
i>
v
```
Incrementer with input up and left, Output is send down and right.  
All unconnected sides are equally viable inputs. If a block would get more inputs in one tick, the inputs are processed in this order: Top, Left, Right, Bottom.  
That mean that if for example `i` block would get 3 input from left, right and bottom in a single tick, only the input from left would be registered since `i` takes only 1 input and left has the highest priority of all those inputs.  
  
Blocks don't interact with wire unless explicitly specified.

### IO

#### `$`
Prints the value of any laser that hits it. Laser's value gets printed as number to the stdout followed by newline. Laser gets deleted when it hits it.

#### `&`
Same as `$` but prints value as ascii character and no newline is inserted.  
Only first 8 bits are used.  
Example:
```
13633 = 11010101000001
              01000001 = 64 = 'A'
```

#### `{`
The start point of the program. Sends a laser to the right with frequency of 1 through all of its output mirrors.

#### `}`
The end point of the program. When any laser hits this blocks, the program ends execution and that laser's value gets used as exit code.

#### `_`
When laser hits this blocks, user is prompted for a number. All code execution is paused until user types in valid laser value that is then applied to the input laser.

### Mirrors

There are 4 mirror blocks:  
Symbol | Description
-------|-------------
`^` | Reflects all lasers up
`v` | Reflects all lasers down
`>` | Reflects all lasers right
`<` | Reflects all lasers left

On the rising edge of received current, mirrors rotate.
When any mirror is triggered by a wire current (rising edge) the mirror rotate according to this table:

From | to
-----|----
`^` | `v`
`v` | `^`
`<` | `>`
`>` | `<`

### Laser modifiers

#### Regular modifiers:
Symbol | Description | Number of inputs
-------|-------------|------------------
`*` | Reflects laser into all connected outputs. | 1
`i` | Increments the laser frequency by 1. | 1
`d` | Decrements the laser frequency by 1. | 1
`#` | Any laser that hits `#` from any direction gets deleted. | 1
`m` | Multiplication | 2
`n` | Division | 2
`a` | Addition | 2
`s` | Subtraction | 2
`l` | Modulo | 2

#### Any of `0123456789ABCDEF`
Sets the laser frequency to the value (hexadecimal)  
There is no built in way of setting laser frequency higher

### Wire
Wire can transport current.  
There are 4 wire blocks:

#### `-`, `|`
Vertical and horizontal wire respectively.

#### `+`
Wire crossing. Wires are not connected.  
For example:  
3 unconnected wires crossing each other.
```
 | |
-+-+-
 | |
-+-O
 |
```
The same code but smaller:
```
 ||
-++-
-+O
 |
```

#### `O`
Wire crossing. Wires are connected.
Only horizontal wires above and below, vertical wires to the left and right and other wire crossing are connected.  
For example:  
Two unconnected wires turning right
```
O--
|O-
||
```

