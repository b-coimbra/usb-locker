#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>
#include <dirent.h>
#include <errno.h>

#define RED   "\x1b[31m"
#define GREEN "\x1b[32m"
#define BLUE  "\x1b[34m"
#define COLOR_RESET "\x1b[0m"
#define PASS(str) #str
#define say(...) fprintf(stderr, __VA_ARGS__)

int32_t system(const char *command);

void cmd(const char *c, const char *s)
{
    char buff[258];
    snprintf(buff, sizeof(buff), "%s %s", c, s);
    system(buff);
}

void hide(const char *f) { cmd("attrib +h +s", f); }
void show(const char *f) { cmd("attrib -h -s", f); }

void lock(void)   { system("mkdir locked && attrib +h +s locked"); }
void unlock(void) { system("rmdir locked"); }

void create_folder(const char *folder) { cmd("mkdir", folder); }

void help(void)
{
    say("[Flash Drive Locker]");
    say("Set your password, compile it, and use it.");
}

int main(int argc, char *argv[])
{
    const char *PASSWORD = PASS(12345);
    const char *FOLDER = "Private";
    int locked = 0;
    char *input = (char*) malloc(100 * sizeof(char));

    DIR *dir = opendir(FOLDER);
    DIR *state = opendir("locked");

    if (dir)
        for (size_t attempts = 1; attempts <= 5; attempts++) {
            say("PASSWORD: ");
            scanf("%s", input);

            if (strcmp(input, PASSWORD) == 0) {
                if (state) {
                    unlock(); show(FOLDER); cmd("explorer", FOLDER); locked = 0;
                } else if (ENOENT == errno) {
                    lock(); hide(FOLDER); locked = 1;
                } else abort();

                free(input);
                say(locked ? GREEN "LOCKED" COLOR_RESET "\n" : BLUE "UNLOCKED" COLOR_RESET "\n");
                break;
            } else if (attempts == 5) say("Out of tries :(\n"); else say(RED "[%d/5] Try again!" COLOR_RESET "\n", attempts);
        }
    else if (ENOENT == errno)
        create_folder(FOLDER);
    else abort();

    if (argc > 1)
        if (argv[1] == "help" || argv[1] == "--help")
            help();

    return 0;
}