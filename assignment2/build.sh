/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 2
Program name: Traffic Light
*/
echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile TrafficLight.cs to create the file: TrafficLight.dll
mcs -target:library -r:System -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:TrafficLight.dll TrafficLight.cs

echo Compile TrafficLightMain.cs to create the file: TrafficLightMain.dll
mcs -r:System -r:System.Windows.Forms.dll -r:TrafficLight.dll -out:Trafficlight.exe TrafficLightMain.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 2 program.
./Trafficlight.exe

echo The script has terminated.
