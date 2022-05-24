# AsciiLaser
AsciiLaser is text based visual programming language.
The code is being executed on a board and terminates when the `}` is hit or when there are not lasers on the board
AL code file extension is `.al`.
All block on a border of a region are replaced by `#`. The above example is interpreted the same way as this:

## Lasers
Lasers have a frequency between 0 and 2^64 - 1.
Lasers travel instantly on a straight line between 2 blocks (for example between 2 mirrors).
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

Module | Description
-------|------------
[`main`](./main.md) | Elementar language structure
[`core`](./core.md) | Core blocks
[`wire`](./wire.md) | Wire and current constructs
[`list`](./list.md) | Sequences in AL

## Used symbols
[Map of used symbols that I try to keep updated](./used.md)

