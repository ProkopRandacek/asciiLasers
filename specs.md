# AsciiLaser
AsciiLaser is interpreted, AsciiDots inspired, text based, case sensitive, visual programming language.  
The code is being executed on a board of unlimited size. Tick time is not constant, they are executed as fast as possible unless in debug mode. Program is terminated if there are no more lasers traveling or any laser hits the `}` object.
AL code file extension is `.al`.

## Data types

### Lasers
Lasers have a frequency (value) between 0 and 2^64 - 1. (unsigned 64 bit int)  
Lasers frequencies overflow.  
Lasers travel instantly on a straight line between 2 objects (for example bethween 2 mirrors)  
Lasers travel over wire objects without influencing each other.  
Laser that hit the board edge get teleported to the other end. (The board is a 3D torus)  

### Current
Current can only hold 0 or 1.
Current moves instantly along the entire wire.  
When objects get powered by current, they alter their state (for example mirrors rotate)

## Objects
Objects is a single ascii character that has some functionality in the language.

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
The start point of the program. Sends a laser to the right with frequency of 1 when the program starts.

#### `}`
The end point of the program. When laser hits this object, the program ends execution and that laser's value gets used as exit code.

#### `_`
When laser hits this object, user is prompted for a number. All code execution is paused until user types in valid laser value that is then applied to the input laser.

### Mirrors
Lasers take 1 tick to go through a mirror
On the rising edge of recieved current, mirrors rotate.
Rotation is done clockwise. (\ -> /, ^ -> >, > -> v, v -> <, < -> ^)
There are 8 mirror objects:

#### `\`
Reflects lasers coming from up to right, right to up, down to left and left to down.

#### `/`
Reflects lasers coming from up to left, left to up, down to right and right to down.

#### `^`
Reflects all incoming lasers up (including lasers from up)

#### `>`
Reflects all incoming lasers right (including lasers from right)

#### `v`
Reflects all incoming lasers down (including lasers from down)

#### `<`
Reflects all incoming lasers left (including lasers from left)

#### `=`
Reflects laser from left to right and from right to left.
Used when you need to slow down the laser 1 tick.
Lasers from up and down are deleted.

#### `H`
Reflects laser from up to down and from down to up.
Used when you need to slow down the laser 1 tick.
Lasers from left and right are deleted.

### Laser modifiers
the laser modifier character needs to be connected to a output mirror that sets the output direction. all unconnected sides are considered to be equaly viable inputs.  
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

#### `*`
Reflects laser into all connected outputs.

#### `i`
Increments the laser frequency by 1.

#### `d`
Decrements the laser frequency by 1.

#### `m`
Multiplies the laser frequency by 1.

#### Any of `0123456789ABCDEF`
Sets the laser frequency to the value (hexadecimal)  
There is no built in way of setting laser frequency higher

#### `#`
Any laser that hits `#` from any direction gets deleted.

#### `m`
Multiplication

#### `d`
Division

#### `a`
Addition

#### `s`
Subtraction

#### `d`
Modulo

### Wire
Wire can transport current.  
There are 5 wire objects:

#### `-`
Vertical wire.

#### `|`
Horizontal wire.

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

