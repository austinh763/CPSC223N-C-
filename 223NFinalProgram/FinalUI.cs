/*
Austin Hoang
CPSC 223N
C Sharp Final Test
Dec 18, 2019
*/
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class FinalUI : Form
{  private const int form_width = 1920;  //Horizontal width measured in pixels..
   private const int form_height = 1080;  //Vertical height measured in pixels

   //Declare constants
   private const double scale_factor = 100.0;
   private const double delta = 0.015;
   private const double refresh_rate = 60.0; //Hertz: How many times per second the bitmap is updated and copied to the visible surface.
   private const double dot_update_rate = 25.0;  //Hertz: How many times per second the coordinates of the dot are updated.

   //Declare max and min sizes
   private Size maxframesize = new Size(form_width,form_height);
   private Size minframesize = new Size(form_width,form_height);

   private double amplitude = 5;
   private double coefficient;

   private Button start = new Button();
   private Button pause = new Button();
   private Button exit = new Button();
   private TextBox x_outputBox = new TextBox();
   private TextBox y_outputBox = new TextBox();


   private const double t_initial_value = 0.0;
   private double t = t_initial_value;
   private double x;
   private double y;



   private int mouse_x = 0;
   private int mouse_y = 0;
   private int clickCounter = 0;

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
   Flower curve_algorithms;

   //Define the constructor of this class.
   public FinalUI()
   {   //Set the initial size of this form.
       Size = new Size(form_width,form_height);
       MaximumSize = maxframesize;
       MinimumSize = minframesize;
       //Set the title of this user interface.
       Text = "Final";
       start.Text = "Start";
       pause.Text = "Pause";
       exit.Text = "Exit";
       x_outputBox.Text = "x: ";
       y_outputBox.Text = "y: ";

       start.Size = new Size(50, 20);
       pause.Size = new Size(50, 20);
       exit.Size = new Size(50, 20);
       x_outputBox.Size = new Size(70, 20);
       y_outputBox.Size = new Size(70, 20);

       start.Location = new Point(100, 970);
       pause.Location = new Point(150, 970);
       exit.Location = new Point(200, 970);
       x_outputBox.Location = new Point(800, 970);
       y_outputBox.Location = new Point(800, 1000);

       Controls.Add(start);
       Controls.Add(exit);
       Controls.Add(pause);
       Controls.Add(x_outputBox);
       Controls.Add(y_outputBox);

       start.Click += new EventHandler(startFn);
       exit.Click += new EventHandler(quitFn);
       pause.Click += new EventHandler(pauseFn);



       //Give feedback to the programmer.
       System.Console.WriteLine("The size of this form is {0}x{1} pixels.",form_width,form_height);

       //Set the initial background color of this form.
       BackColor = Color.Pink;

       //Compute the coefficient of t in the function: y = amplitude * sin(coefficient * t).
       coefficient = 2.0*System.Math.PI/6;

       //Instantiate the collection of supporting algorithms
       curve_algorithms = new Flower();

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


       //Initialize the pointer used to write onto the bitmap stored in memory.
       pointer_to_graphic_surface = Graphics.FromImage(pointer_to_bitmap_in_memory);
       initialize_bitmap();

   }//End of constructor of FinalUI class.

   public class Flower
{   private double absolute_value_of_derivative_squared;
    private double absolute_value_of_derivative;

    public void get_next_coordinates(double amp, double coef, double delta, ref double dot, out double x, out double y)
       {absolute_value_of_derivative_squared = 1.0 + amp*amp * coef*coef * System.Math.Cos(coef*dot)*System.Math.Cos(coef*dot);
        absolute_value_of_derivative = System.Math.Sqrt(absolute_value_of_derivative_squared);
        dot = dot + delta/absolute_value_of_derivative;
        x = 5*System.Math.Cos(2*dot) * System.Math.Cos(dot);
        y = 5*System.Math.Cos(2*dot) * System.Math.Sin(dot);
       }//End of get_next_coordinates

}//End  of class Flower

   protected void initialize_bitmap()
   {Font labelfont = new System.Drawing.Font("Arial",8,FontStyle.Regular);
    Brush labelbrush = new SolidBrush(System.Drawing.Color.Black);
    pointer_to_graphic_surface.Clear(System.Drawing.Color.White);
    pointer_to_graphic_surface.FillRectangle(Brushes.Yellow,0,950,form_width,100);
    //Draw the vertical Y-axis.
    bic.Width = 2;
    pointer_to_graphic_surface.DrawLine(bic,form_width/2,0,form_width/2,form_height);
    //Draw the horizontal X-axis.
    bic.Width = 2;
    pointer_to_graphic_surface.DrawLine(bic,0,form_height/2,form_width,form_height/2);
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
   {   Graphics finalgraph = eee.Graphics;
       finalgraph.FillRectangle(Brushes.Yellow, 0, 900, form_width, 300);
       //Copy the contents of the bitmap to the graphic surface area.
       finalgraph.DrawImage(pointer_to_bitmap_in_memory,0,0,form_width,form_height);
       base.OnPaint(eee);
   }
   protected override void OnMouseDown(MouseEventArgs e) {
     mouse_x = e.X;
     mouse_y = e.Y;
     double distsq = Math.Pow(mouse_x - (x_scaled_int + 6), 2) + Math.Pow(mouse_y - (y_scaled_int +
     6), 2);
     if(distsq <= 36) {
       clickCounter++;
     }
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
   }//End of method Update_the_position_of_the_dot


   protected void Update_the_graphic_area(System.Object sender, ElapsedEventArgs even)
   {   x_scaled_int = (int)System.Math.Round(x_scaled_double);
       y_scaled_int = (int)System.Math.Round(y_scaled_double);
       double currentX = x_scaled_int;
       double currentY = y_scaled_int;
       x_outputBox.Text = "X: " + currentX.ToString();
       y_outputBox.Text = "Y: " + currentY.ToString();
       pointer_to_graphic_surface.FillEllipse(Brushes.DarkRed,x_scaled_int,y_scaled_int,6,6);
       if(clickCounter == 1) {
         pointer_to_graphic_surface.FillEllipse(Brushes.Black,x_scaled_int,y_scaled_int,6,6);
       }
       else if(clickCounter == 2) {
         pointer_to_graphic_surface.FillEllipse(Brushes.Red,x_scaled_int,y_scaled_int,6,6);
       }
       else if(clickCounter == 3) {
         pointer_to_graphic_surface.FillEllipse(Brushes.Blue,x_scaled_int,y_scaled_int,6,6);
       }
       else if(clickCounter == 4) {
         pointer_to_graphic_surface.FillEllipse(Brushes.Green,x_scaled_int,y_scaled_int,6,6);
       }
       else if(clickCounter == 5) {
         pointer_to_graphic_surface.FillEllipse(Brushes.Black,x_scaled_int,y_scaled_int,6,6);
         clickCounter = 0;
       }
       Invalidate();
   }//End of Update_the_graphic_area
   protected void startFn(Object Sender, EventArgs events) {
       Start_graphic_clock(refresh_rate);
       Start_dot_clock(dot_update_rate);
       graphic_area_refresh_clock.Enabled = true;
       dot_refresh_clock.Enabled = true;
   }
   protected void pauseFn(Object sender, EventArgs events) {
     dot_refresh_clock.Enabled = false;
   }
   protected void quitFn(Object sender, EventArgs events) {
     Close();
   }


}//End of FinalUI class
