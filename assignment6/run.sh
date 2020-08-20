#!/bin/bash

#Author: Floyd Holliday
#Mail:  holliday@fullerton.edu
#Program long name: Dot Drawing a Sine Curve
#Program short name: Sine Curve

echo "Program: Sine Curve"
echo "Draw a sine curve in real time"


echo "Compile Sineframe.cs"
mcs -target:library Sineframe.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:Sineframe.dll

echo "Compile Sinewave.cs"
mcs -target:exe Sinewave.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:Sineframe.dll -out:Sine.exe

echo "Run the program"
./Sine.exe

echo "The bash script will finish now"
