# languageforge
The shiny new Language Forge repository

## .NET external code debugging

You can view the implementation of external libs (e.g. the .NET SDK) directly in VS Code. There are two ways to do this and they compliment each other.

1) The simplest way is to turn on `omnisharp.enableDecompilationSupport` (only available in user settings, because a user agreement is required). When navigating to external code, libraries are then decompiled rather than only showing metadata. The decompiled code is, of course, different than the original source code.

2) The original source code can only be viewed while debugging by stepping into the code using the debugging tools (as far as I can figure out). Appropriate settings for this have been added to the [workspace settings](.vscode\settings.json) for debugging our unit tests and to the .NET [launch configuration](.vscode\launch.json) for debugging the application itself. In each case, a few lines need to be uncommented in order to tell the debugger to download the source code. This slows down the debugging process, so the lines should be commented out again once the libs have been downloaded.
