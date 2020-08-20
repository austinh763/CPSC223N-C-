//****************************************************************************************************************************
//Program name: "Dot Drawing a Sine Curve".  This programs shows an invisible pencil drawing an ordinary sine function graph *
//on lined graph paper.   Copyright (C) 2017  Floyd Holliday.                                                                *
//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License  *
//version 3 as published by the Free Software Foundation.                                                                    *
//This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied         *
//warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.     *
//A copy of the GNU General Public License v3 is available here:  <https://www.gnu.org/licenses/>.                           *
//****************************************************************************************************************************



﻿//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**
//Author: Floyd Holliday
//Mail: holliday@fullerton.edu

//Program information
//  Program long name: Dot Drawing a Since Curve
//  Program short name: Sine Curve
//  Programming language: C Sharp
//  Date development of program began: 2016-Dec-18
//  Date of last update: 2018-Nov-30
//  Date of re-organized comments: 2018-Nov-11.
//  Files in project: Sinewave,cs, Sineframe.cs, Sinecurve.cs, run.sh
//  Status: Done; no more updates will be released -- not even bug fixes.
//  Options: Since this is a 'free' program anyone may continue to enhance and expand this program.  ['free' means 'freedom']  You
//  may post this program at your own website or distribute copies on alternative media. 

//Purpose of program:  
//  This program shows a particle (tiny dot) moving and tracing the path of a sine curve.  It is significant that the dot 
//  travels at constant linear speed.  This is somewhat difficult to attain.  It means that the speed is a fixed number of pixels
//  per second no matter where the dot may be.

//Achieve constant speed
//  If the program were to simply plug in successive values of x, compute y=sin(x), and put a dot at the point (x,y) then constant
//  speed is NOT realized.  This program employs this technique: given the current location of the dot at say (x,y) compute the 
//  next location of the dot at (x',y') such that the euclidean distance between (x,y) and (x',y') equals a designated constant d.
//  This technique does perform correctly within some reasonable limits, for example, the constant d cannot be huge.

//Files in this program.  The set of these files together equal one program called Dot Drawing a Sine Curve.
//  1. Sinecurve.cs
//  2. Sineframe.cs
//  3. Sinewave.cs
//  4. run.sh

//This file
//  File name: Sinewave.cs
//  Language: C#
//  Purpose: This file contains Main which launches the one user interface.
//  Max page width: 132
//  Optimal print specifications: Landscape, 6 point font, monospace, 132 columns, 8½x11 paper
//  Date this file last modified: 2018-Nov-11
//  Compile this file for syntax checking -- no executable created: mcs -target:library -out:Sinewave.dll Sinewave.cs
//  Compile this file and link it to other dll files: 
//             mcs -target:exe Sinewave.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:Sineframe.dll -out:Sine.exe



using System;
using System.Windows.Forms;            //Needed for "Application" near the end of Main function.
public class Sinewave
{  public static void Main()
   {  System.Console.WriteLine("The Sine Wave program has begun.");
      Sineframe sineapp = new Sineframe();
      Application.Run(sineapp);
      System.Console.WriteLine("This Sine Wave program has ended.  Bye.");
   }//End of Main function
}//End of Sinewave class




//Mathematics for the sine function.
//The general sine function is y = A*sin(B*x).  In that case A = amplitude and P = 2π/B is the period.

//The domain of y = A*sin(B*x) is taken to be the non-negative real numbers.

//Some scaling is necessary.  We map 1 mathematical unit to 100 pixels.  The result is that whereas the mathematical function 
//y = sin(x) has an amplitude of 1.0 the graphical image will have an amplitude of 100 pixels.  Likewise, the mathematical
//function y = sin(x) has a natural period of 2π = 6.2832, but the graphical image of that function will have a period of 
//2π*100 = 628.32.  Simply stated: the graphic image has been expanded by a factor of 100 in both vertical and horizontal 
//directions.

//True constant linear speed is difficult to attain.  Let γ(t) be a function defined on [a;b] and mapping into RxR.  One can impose
//restrictions that γ(t) be continuous, differentiable, smooth, etc.  Let C>0 be a positive constant.  Let t be in the domain of
//γ(t).  Then it is computationally expensive to find another value s in the domain of γ(t), s>t, such that the arc length distance
//from γ(t) to γ(s) equals C.  At this point we abandon the attempt to attain true constant linear speed.

//We can program a close approximation to constant linear speed as described here.  Let γ(t) be function defined on [a;b] and 
//mapping into RxR, and assume that the function is differentiable everywhere with non-zero derivative everywhere.  Think of 
//tracing the sine curve in real time.  We want that particle to move a fixed distance in any two time periods of equal duration.
//Here is how to accomplish this.  Select a very small positive number δ.  Assume that the tracing particle is now at a pixel at
//point γ(t) = (x,y).  The job is to find a parameter u>t such that the arc distance from γ(t) to γ(u) is δ.  As mentioned in the
//previous paragraph that calculation is not feasible.  However, we can find a u that approximates the required arc distance.  
//Specifically, let u = t + δ/|γ'(t)|.  Then the cord from γ(t) to γ(u), i.e. γ(u)-γ(t), is exactly δ.  Since the length of the
//chord from γ(t) to γ(u) is approximately equal to the length of the curve from γ(t) to γ(u) when u is close to t, we accept this
//strategy of "equal chord lengths" as a good enough approximation to "equal arc lengths".

//This program uses this technique of equal chord lengths to approximate equal arc lengths.  You, the reader of this description,
//are invited to run this program.  Try to detect any noticeable changes in speed.  Human eyes cannot detect any changes in speed.

//In this program there are three quantities each playing an important role.
//V = Linear velocity (pixels per second)
//C = Length of the chord = distance traveled between successive tics of the clock (pixels)
//U = Update rate of the coordinates of the particle (Hz)

//These values must satisfy the equation V = C*U.  Thus, if any two of the three are known then the other can be easily computed.
//Example.  Suppose V = 90 pixels be second, C = 0.8 pixels.  Then U can be computed: U = V/C = 112.5 Hz.
//We can then find the update interval = 1000ms/112.5Hz = 8.8888ms.  Since the clocks of C# can accept only a whole number of 
//milliseconds, that number must be rounded to update interval = 9ms.

