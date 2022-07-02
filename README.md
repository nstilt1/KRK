# KRK
# Kerbal Rocket Kalculator*
\**Konstructor coming soon (maybe)*

This runs in `Visual Studio Code` when you have C# and dotnet installed. The GUI that we attempted to create in Unity did not function properly, thus we had to resort to running the code through console.
The algorithm is quite efficient and only take a few seconds to compute. When the payload is 0.94 tonnes, with 4000 m/s delta-v, and 1.4 as the minimum thrust to weight ratio (TWR) are inputted, the recommended assembly is only off by 0.7%.
The commands to run it, once everything is installed (and after restarting Visual Studio Code), are as follows:
```
dotnet restore
dotnet run
```
Then, there will be an on-screen prompt for further input.

Maybe, if I have nothing better to do, I might work on making it a mod; however, it is pretty far down my todo list. For it to function with less hard-coding, we would need to access the game's files to fill an array or two of rocket parts.
