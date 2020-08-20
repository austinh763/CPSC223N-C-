/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 3
Program name: Travelling Ball
*/
echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile TrafficLight.cs to create the file: TravellingBallUI.dll
mcs -target:library -r:System -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:TravellingBallUI.dll TravellingBallUI.cs

echo Compile TrafficLightMain.cs to create the file: TravellingBallMain.dll
mcs -r:System -r:System.Windows.Forms.dll -r:TravellingBallUI.dll -out:TravellingBall.exe TravellingBallMain.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 3 program.
./TravellingBall.exe

echo The script has terminated.
