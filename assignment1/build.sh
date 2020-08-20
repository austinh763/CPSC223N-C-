/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 1
Program name: Flashing Red Light
*/
echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile ui.cs to create the file: ui.dll
mcs -target:library -r:System -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:ui.dll ui.cs

echo Compile intro.cs to create the file: intro.dll
mcs -r:System -r:System.Windows.Forms.dll -r:ui.dll -out:Redlight.exe intro.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 1 program.
./Redlight.exe

echo The script has terminated.
