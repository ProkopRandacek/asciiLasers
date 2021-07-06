# AL Wire
The wire module adds the Current data type and wires that can modify the behavior of blocks at runtime.

### Current
Current can only hold 1 (on) or 0 (off).  
Current moves instantly along the entire wire.  
When blocks get powered by current (rising edge), it alters its state either from A to B or B to A:

A   | B
----|----
`^` | `v`
`<` | `>`
`#` | `*`
`i` | `d`
`a` | `s`
`m` | `n`

Wire can transport current.  
There are 4 wire blocks:

#### `@`
Laser detector. Interacts with Laser the same exact way as a `*` block does. But unlike `*`, `@` sends signal over any connected wire when it gets evaluated.
  
Example:  
```
         O----------^
{> > v   |
     @---+--O
	 v   |  |
   ^ <   O--O
```
A detector that gets activated every 5 ticks. It rotates a connected mirror.

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

