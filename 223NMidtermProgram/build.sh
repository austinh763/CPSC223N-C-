Austin Hoang
CPSC 223N
C Sharp Midterm Test
Oct 14, 2019


echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile CSharpMidtermUI.cs to create the file: CSharpMidtermUI.dll
mcs -target:library -r:System -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:CSharpMidtermUI.dll CSharpMidtermUI.cs

echo Compile CSharpMidtermMain.cs to create the file: CSharpMidtermMain.dll
mcs -r:System -r:System.Windows.Forms.dll -r:CSharpMidtermUI.dll -out:CSharpMidterm.exe CSharpMidtermMain.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 3 program.
./CSharpMidterm.exe

echo The script has terminated.
