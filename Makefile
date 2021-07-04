NAME = asciilaser
CC = g++

SOURCES = $(wildcard src/*.cpp)

CC_FULL = -Wall -Wextra -Werror

.PHONY: build

build:
	$(CC) $(SOURCES) $(CC_FULL) -o $(NAME)
	@echo Build succeeded

clean:
	rm $(NAME)
