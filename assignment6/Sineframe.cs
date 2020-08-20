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
//  Program long name: Dot Drawing a Sine Curve
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
//  This technique does perform correctly within some reasonable limits, for example, the constant d cannot be huge

//This file
//  File name: Sineframe.cs
//  Language: C#
//  Purpose: This file defines the one user window of this program.
//  Max page width: 132
//  Optimal print specifications: Landscape, 6 point font, monospace, 132 columns, 8½x11 paper
//  Date this file last modified: 2018-Nov-11
//  Compile this file and thereby create a binary file with extension dll:
//           mcs -target:library Sineframe.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:Sinecurve.dll -out:Sineframe.dll





using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class Sineframe : Form            //Sineframe inherits from Form class
{  private const int form_width = 1920;  //Horizontal width measured in pixels..
   private const int form_height = 1080;  //Vertical height measured in pixels
   //In 223n you should use the largest possible graphical area supported by your monitor and still maintain the 16:9 ratio.

   //Declare constants
   private const double scale_factor = 100.0;
   //private const double offset = 40.0;  //The origin is located this many pixels to the right of the left boundary.
   private const double delta = 0.015;
   private const double refresh_rate = 90.0; //Hertz: How many times per second the bitmap is updated and copied to the visible surface.
   private const double dot_update_rate = 70.0;  //Hertz: How many times per second the coordinates of the dot are updated.

   //Declare max and min sizes
   private Size maxframesize = new Size(form_width,form_height);
   private Size minframesize = new Size(form_width,form_height);

   //Declare variables for the sine function: y = amplitude * sin (coefficient * t).
   private double amplitude = 5;
   private double coefficient;

   //Variables for the Cartesian description of γ(t) = (t,Asin(Bt)), where A = amplitude, B = coefficient, x(t) = t, y(t) = A*sin(B*t)
   private const double t_initial_value = 0.0;  //Start curve drawing at γ(0.0) = (0.0,0.0) = Origin
   private double t = t_initial_value;
   private double x;
   private double y;

   //Variables for the scaled description of the sine curve
   private double x_scaled_double;
   private double y_scaled_double;

   //Variable for drawing on the bitmap
   private int x_scaled_int;
   private int y_scaled_int;

   //Declare clocks
   private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
   private static System.Timers.Timer dot_refresh_clock = new System.Timers.Timer();

   //Instruments
   Pen bic = new Pen(Color.Black,1);

   //Declare visible graphical area and bitmap graphical area
   private System.Drawing.Graphics pointer_to_graphic_surface;
   private System.Drawing.Bitmap pointer_to_bitmap_in_memory =
                                         new Bitmap(form_width,form_height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);

   //Declare a tool kit of algorithms
   Sinecurve curve_algorithms;

   //Define the constructor of this class.
   public Sineframe()
   {   //Set the initial size of this form.
       Size = new Size(form_width,form_height);
       MaximumSize = maxframesize;
       MinimumSize = minframesize;
       //Set the title of this user interface.
       Text = "Sine Curve Traced by Moving Particle";
       //Give feedback to the programmer.
       System.Console.WriteLine("The size of this form is {0}x{1} pixels.",form_width,form_height);
////////       System.Console.WriteLine("form_width = {0}, form_height = {1}.", form_width, form_height);

       //Set the initial background color of this form.
       BackColor = Color.Pink;

       //Compute the coefficient of t in the function: y = amplitude * sin(coefficient * t).
       coefficient = 2.0*System.Math.PI/6;

       //Instantiate the collection of supporting algorithms
       curve_algorithms = new Sinecurve();

       //Set initial values for the sine curve in a standard mathematical cartesian coordinate system
       t = 0.0;
       x = form_width/2;
       y = form_height/2;

       //Prepare the refresh clock
       graphic_area_refresh_clock.Enabled = false;
       graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Update_the_graphic_area);

       //Prepare the dot clock
       dot_refresh_clock.Enabled = false;
       dot_refresh_clock.Elapsed += new ElapsedEventHandler(Update_the_position_of_the_dot);

       //Start both clocks running
       Start_graphic_clock(refresh_rate);
       Start_dot_clock(dot_update_rate);

       //Use extra memory to make a smooth animation.
       DoubleBuffered = true;

       //Initialize the pointer used to write onto the bitmap stored in memory.
       pointer_to_graphic_surface = Graphics.FromImage(pointer_to_bitmap_in_memory);
       initialize_bitmap();

   }//End of constructor of Sineframe class.

   public class Sinecurve
{   private double absolute_value_of_derivative_squared;
    private double absolute_value_of_derivative;

    public void get_next_coordinates(double amp, double coef, double delta, ref double dot, out double x, out double y)
       {absolute_value_of_derivative_squared = 1.0 + amp*amp * coef*coef * System.Math.Cos(coef*dot)*System.Math.Cos(coef*dot);
        absolute_value_of_derivative = System.Math.Sqrt(absolute_value_of_derivative_squared);
        dot = dot + delta/absolute_value_of_derivative;
        x = 5*System.Math.Cos(2*dot) * System.Math.Cos(dot);
        y = 5*System.Math.Cos(2*dot) * System.Math.Sin(dot);
       }//End of get_next_coordinates

}//End  of class Sinecurve

   protected void initialize_bitmap()
   {Font labelfont = new System.Drawing.Font("Arial",8,FontStyle.Regular);
    Brush labelbrush = new SolidBrush(System.Drawing.Color.Black);
    pointer_to_graphic_surface.Clear(System.Drawing.Color.White);
    //Draw the vertical Y-axis.
    bic.Width = 2;
    pointer_to_graphic_surface.DrawLine(bic,form_width/2,0,form_width/2,form_height);
    //Draw the horizontal X-axis.
    bic.Width = 2;
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2,form_width,form_height/2);
    //Draw horizontal guide lines.
    /*
    bic.DashStyle = DashStyle.Dash;
    bic.Width = 1;
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2+1*(int)scale_factor,form_width,form_height/2+1*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2-1*(int)scale_factor,form_width,form_height/2-1*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2+2*(int)scale_factor,form_width,form_height/2+2*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2-2*(int)scale_factor,form_width,form_height/2-2*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2+3*(int)scale_factor,form_width,form_height/2+3*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2-3*(int)scale_factor,form_width,form_height/2-3*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2+4*(int)scale_factor,form_width,form_height/2+4*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2-4*(int)scale_factor,form_width,form_height/2-4*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2+5*(int)scale_factor,form_width,form_height/2+5*(int)scale_factor);
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2-5*(int)scale_factor,form_width,form_height/2-5*(int)scale_factor);
    */
    //Add labels to the Y-axis.
    pointer_to_graphic_surface.DrawString("+1",labelfont,labelbrush,form_width/2,form_height/2-100-4);
    pointer_to_graphic_surface.DrawString("+2",labelfont,labelbrush,form_width/2,form_height/2-200-4);
    pointer_to_graphic_surface.DrawString("+3",labelfont,labelbrush,form_width/2,form_height/2-300-4);
    pointer_to_graphic_surface.DrawString("+4",labelfont,labelbrush,form_width/2,form_height/2-400-4);
    pointer_to_graphic_surface.DrawString("+5",labelfont,labelbrush,form_width/2,form_height/2-500-4);
    pointer_to_graphic_surface.DrawString("-1",labelfont,labelbrush,form_width/2,form_height/2+100-4);
    pointer_to_graphic_surface.DrawString("-2",labelfont,labelbrush,form_width/2,form_height/2+200-4);
    pointer_to_graphic_surface.DrawString("-3",labelfont,labelbrush,form_width/2,form_height/2+300-4);
    pointer_to_graphic_surface.DrawString("-4",labelfont,labelbrush,form_width/2,form_height/2+400-4);
    pointer_to_graphic_surface.DrawString("-5",labelfont,labelbrush,form_width/2,form_height/2+500-4);
    //Add labels to the X-axis.
    pointer_to_graphic_surface.DrawString("+1",labelfont,labelbrush,form_width/2-100-4,form_height/2);
    pointer_to_graphic_surface.DrawString("+2",labelfont,labelbrush,form_width/2-200-4,form_height/2);
    pointer_to_graphic_surface.DrawString("+3",labelfont,labelbrush,form_width/2-300-4,form_height/2);
    pointer_to_graphic_surface.DrawString("+4",labelfont,labelbrush,form_width/2-400-4,form_height/2);
    pointer_to_graphic_surface.DrawString("+5",labelfont,labelbrush,form_width/2-500-4,form_height/2);
    pointer_to_graphic_surface.DrawString("-1",labelfont,labelbrush,form_width/2+100-4,form_height/2);
    pointer_to_graphic_surface.DrawString("-2",labelfont,labelbrush,form_width/2+200-4,form_height/2);
    pointer_to_graphic_surface.DrawString("-3",labelfont,labelbrush,form_width/2+300-4,form_height/2);
    pointer_to_graphic_surface.DrawString("-4",labelfont,labelbrush,form_width/2+400-4,form_height/2);
    pointer_to_graphic_surface.DrawString("-5",labelfont,labelbrush,form_width/2+500-4,form_height/2);
   }

   protected override void OnPaint(PaintEventArgs eee)
   {   Graphics sinegraph = eee.Graphics;
       //Copy the contents of the bitmap to the graphic surface area.
       sinegraph.DrawImage(pointer_to_bitmap_in_memory,0,0,form_width,form_height);
       base.OnPaint(eee);
   }

   protected void Start_graphic_clock(double refreshrate)
   {double elapsedtimebetweentics;
    if(refreshrate < 1.0) refreshrate = 1.0;  //Do not allow updates slower than 1 hertz.
    elapsedtimebetweentics = 1000.0/refreshrate;  //elapsed time between tics has units milliseconds
    graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
    graphic_area_refresh_clock.Enabled = true;  //Start this clock ticking.
   }//End of method Start_graphic_clock

   protected void Start_dot_clock(double dot_parameter_update_rate)
   {double elapsedtimebetweenchangestodotcoordinates;
    //This program does not allow a clock speed slower than one hertz.
    if(dot_parameter_update_rate < 1.0) dot_parameter_update_rate = 1.0;
    //Compute the interval in millisec between each tick of the clock.
    elapsedtimebetweenchangestodotcoordinates = 1000.0/dot_parameter_update_rate;
    dot_refresh_clock.Interval = (int)System.Math.Round(elapsedtimebetweenchangestodotcoordinates);
    dot_refresh_clock.Enabled = true;  //Start this clock ticking
   }//End of method Start_dot_clock

   //The next method updates the x and y coordinates of the dot that is tracing the sine curve.
   protected void Update_the_position_of_the_dot(System.Object sender,ElapsedEventArgs an_event)
   {   //Call a method to compute the next pair of Cartesian coordinates for the moving particle.
       curve_algorithms.get_next_coordinates(amplitude,coefficient,delta,ref t,out x,out y);
       //Convert the Cartesian coordinates to scaled coordinates for viewing on a monitor
       x_scaled_double = (double)form_width/2.0 - scale_factor * x; //+ offset;
       y_scaled_double = (double)form_height/2.0 - scale_factor * y;
       if(x_scaled_double > (double)(form_width-1))
          {dot_refresh_clock.Enabled = false;
           graphic_area_refresh_clock.Enabled = false;
           System.Console.WriteLine("Both clocks have stopped.  You may close the window.");
          }
   }//End of method Update_the_position_of_the_dot

   //The next method places the dot into the bitmapped region of memory according to its own coordinates, and then calls method
   //OnPaint to copy the bitmapped image to the graphical surface for viewing.  This occurs each time the graphic_area_refresh_clock
   //makes a tic.
   protected void Update_the_graphic_area(System.Object sender, ElapsedEventArgs even)
   {   x_scaled_int = (int)System.Math.Round(x_scaled_double);
       y_scaled_int = (int)System.Math.Round(y_scaled_double);
       pointer_to_graphic_surface.FillEllipse(Brushes.DarkRed,x_scaled_int,y_scaled_int,5,5);
       Invalidate();  //This function actually calls OnPaint.  Yes, that is true.
       if(x_scaled_int >= form_width)  //dot has reach the right edge of the frame
          {graphic_area_refresh_clock.Enabled = false;  //Stop refreshing the graphic area
           System.Console.WriteLine("The graphical area is no longer refreshing.  You may close the window.");
          }
   }//End of Update_the_graphic_area


}//End of Sineframe class
