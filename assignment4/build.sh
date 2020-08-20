echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile RicochetBallUI.cs to create the file: RicochetBallUI.dll
mcs -target:library -r:System -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:RicochetBallUI.dll RicochetBallUI.cs

echo Compile RicochetBallMain.cs to create the file: RicochetBallMain.dll
mcs -r:System -r:System.Windows.Forms.dll -r:RicochetBallUI.dll -out:RicochetBall.exe RicochetBallMain.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 3 program.
./RicochetBall.exe

echo The script has terminated.
