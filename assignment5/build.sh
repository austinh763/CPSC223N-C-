
echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile FallingAppleUI.cs to create the file: FallingAppleUI.dll
mcs -target:library -r:System -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:FallingAppleUI.dll FallingAppleUI.cs

echo Compile FallingAppleMain.cs to create the file: FallingAppleMain.dll
mcs -r:System -r:System.Windows.Forms.dll -r:FallingAppleUI.dll -out:FallingApple.exe FallingAppleMain.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 3 program.
./FallingApple.exe

echo The script has terminated.
