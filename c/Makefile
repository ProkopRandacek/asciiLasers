CC = gcc

BIN_PATH = bin
DBG_PATH = debug
OBJ_PATH = obj
SRC_PATH = src

SRC = $(foreach x, $(SRC_PATH), $(wildcard $(addprefix $(x)/*,.c*)))
OBJ = $(addprefix $(OBJ_PATH)/, $(addsuffix .o, $(notdir $(basename $(SRC)))))
OBJ_DEBUG = $(addprefix $(DBG_PATH)/, $(addsuffix .o, $(notdir $(basename $(SRC)))))

CC_WFLG = -Wall -Wextra -Wfloat-equal -Wundef -Wshadow -Wstrict-prototypes -Wswitch-default -Wswitch-enum -Wunreachable-code -Winit-self -Wuninitialized -Waggregate-return -Wno-misleading-indentation
CC_FLGS = $(GCC_WFLAGS) -std=gnu11 -Ofast $(shell pkg-config --cflags libucw)
CC_DFLG = -g -O0
CC_LIBS = -lm $(shell pkg-config --libs libucw)

CC_FULL = $(CC_FLGS) $(CC_LIBS)

CC_FOBJ = $(CC_FULL) -c

TARGET_NAME = al
TARGET = $(BIN_PATH)/$(TARGET_NAME)
TARGET_DEBUG = $(DBG_PATH)/$(TARGET_NAME)

CLEAN_LIST = $(TARGET) $(TARGET_DEBUG) $(OBJ) $(OBJ_DEBUG)


default: makedir all


$(TARGET): $(OBJ)
	$(CC) $(CC_FULL) -o $@ $(OBJ)

$(OBJ_PATH)/%.o: $(SRC_PATH)/%.c*
	$(CC) $(CC_FOBJ) -o $@ $<

$(DBG_PATH)/%.o: $(SRC_PATH)/%.c*
	$(CC) $(CC_FOBJ) $(CC_DFLG) -o $@ $<

$(TARGET_DEBUG): $(OBJ_DEBUG)
	$(CC) $(CC_FULL) $(CC_DFLG) $(OBJ_DEBUG) -o $@


.PHONY: makedir all debug clean

makedir:
	@mkdir -p $(BIN_PATH) $(OBJ_PATH) $(DBG_PATH)

all: $(TARGET)

debug: $(TARGET_DEBUG)

clean:
	@echo CLEAN $(CLEAN_LIST)
	@rm -f $(CLEAN_LIST)

