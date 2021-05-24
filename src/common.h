//vim:filetype=c
#include <stdint.h>

#define u64 uint64_t
#define u32 uint32_t
#define u8  uint8_t

typedef enum {
	SLASH_MIRROR, // `\` `/`
	ONE_DIR_MIRROR, // `>` `<` `^` `v`
	FOWARD_MIRROR, // `=` `H`
	CONST, // `0` - `9`, `A` - `F`
	MOD, // `*`, `i`, `d`, `m`, `#`, `m`, `s`, `l`, `n`, `a`
	START, // `{`
	END, // `}`
	PRINT, // `&` `$`
	INPUT, // `_`
} OType;

typedef struct {
	Object* up, down, left, right; // output connection to other objects
	OType type; // object type
	u64 input[2]; // objects should only ever need 2 inputs max
	u8 filled; // how many of the inputs are filled (atm only 0 or 1 since no object needs more than 2 inputs)
	u8 state; // object specific; mirror rotation, mod type, ...
} Object;
