#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char* argv[]) {
	if (argc != 2) {
		printf("wrong number of arguments\n");
		exit(1);
	}
	FILE *fp;
	fp = fopen(argv[1], "r");

	if (!fp) {
		printf("error opening file\n");
		exit(1);
	}

	fseek(fp, 0L, SEEK_END);
	long int fileSize = ftell(fp);

	char file[fileSize + 1];
	memset(file, 0, fileSize + 1);

	fseek(fp, 0L, SEEK_SET);

	fread(file, fileSize, 1, fp);

	printf("%s", file);

	return 0;
}
