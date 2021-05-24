NAME = asciilaser
CC = gcc

SOURCES = $(wildcard src/*.c)

CC_WFLAGS = -Wall -Wextra -Wuninitialized -Wunreachable-code -Wfloat-equal -Wundef -Wshadow -Winit-self -Wno-misleading-indentation
CC_FFLAGS = -fshort-enums
CC_FLAGS  = $(CC_WFLAGS) $(CC_FFLAGS) -std=c99 -march=native -Ofast #-g -pg
CC_LIB    = -lm

BUILD_NAME = $(shell git rev-parse --short HEAD)

CC_FULL = $(CC_INCLUDES) $(CC_FLAGS) $(CC_LIB) -DBUILD_NAME=\"$(BUILD_NAME)\"

.PHONY: build

build:
	$(CC) $(SOURCES) $(CC_FULL) -o $(NAME)
	@echo Build succeeded

clean:
	rm $(NAME)
