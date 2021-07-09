#include <ucw/lib.h>

int main(int argc, char *argv[]) {
  log_init(argv[0]);
  msg(L_INFO, "Hello world");
  return 0;
}

