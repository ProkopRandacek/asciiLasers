# AsciiLaser
AsciiLaser is interpreted, AsciiDots inspired, text based, case sensitive, visual programming language.  
The code is being executed on a board of implementation specific size. Tick time is implementation specific. Program is terminated if no block was evaluated last tick (in which case the exit code is 0) or if any laser hit the `}` block in which case the exit code it that lasers value.  
AL code file extension is `.al`.

## Lasers
Laser is the main datatype used in AL.  
Lasers have a frequency (value) between 0 and 2^64 - 1. (unsigned 64 bit int)  
Laser overflow behavior is implementation specific.  
Lasers travel instantly on a straight line between 2 blocks (for example between 2 mirrors)  
Lasers travel over wire blocks without influencing each other.  

## Blocks
Block is a single ascii character that has some functionality in the language.  
  
All blocks have input and output sides. Output side is every side that has a output mirror on it. All other sides are inputs.
Example:
```
i>
v
```
`i` with inputs from **top** and **left**. **Right** and **bottom** are both outputs and both will output a value in their direciton if `i` is evaluated.  
```
 v
^i<
 >
```
`i` with no outputs. Output mirrors must be facing **from** the block they are connected to.
```
i
```
`i` with no outputs.  
  
If a block should receive multiple inputs in one tick. The order in which the inputs will be processed by the block is: Up, Left, Right, Bottom.  
All blocks have input queue of some length. Block is evaluated only on ticks, in which the block filled its queue. For example: `m` takes 2 inputs. It can receive the first input, then n ticks wait and then receive the second input. Only when the second (last) input arrives, is the block evaluated, and is it's output send further. If block receives multiple inputs in a single tick, total number of which would be higher that the number of inputs it requires, all extra inputs are discarded.  

## Comments
Comments start with `[` and end with `]` every character from `[` to `]` including `[` and `]` are treated as spaces when compiling.  
Example:
```
this is not comment [ but this is
this is still comment this too and this [ this too
this still is ] this is not
```

## Modules
The AL language documentation is split into modules:
[core](./core.md)  
[wire](./wire.md)  
[file](./file.md)  
[nets](./nets.md)  

## Used symbols
[Map of used symbols that I try to keep updated](./used.md)

