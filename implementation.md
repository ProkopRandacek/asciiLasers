# Implementation notes
Board is parsed line by line. Every object is saved in a list.
Board compiling means calculating what objects can send lasers between each other. This information is cached and a graph representation of the board is generated. This representation is then used at runtime.
