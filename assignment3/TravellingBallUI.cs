/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 3
Program name: Travelling Ball
*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class TravellingBallUI : Form {
  private Label title = new Label();
  private Label direction = new Label();
  private Button GoPauseButton = new Button();
  private Button exit = new Button();
  private Button reset = new Button();
  private Size maxInterface = new Size(1600, 1000);
  private Size minInterface = new Size(1600, 1000);
  private const String welcome_message = "Travelling Ball by Austin Hoang";
  private System.Drawing.Font welcome_style = new System.Drawing.Font("TimesNewRoman",24,FontStyle.Regular);
   private Brush welcome_paint_brush = new SolidBrush(System.Drawing.Color.Black);
   private Point welcome_location;   //Will be initialized in the constructor.
  private const int penwidth = 1;
  private const double ballRadius = 6.8;
  private const double delta = 10.5; //Animation Speed: distance travelled/tick
  private const double animationClockSpeed = 190.7;//Hz; times/sec coordinates of
  //of ball updated.
  private const double refreshClockSpeed = 30.0; //times/sec ui repainted.
  private const double millisecPerSec = 1000.0;
  private const double animationInterval = millisecPerSec/animationClockSpeed;
  private const double refreshClockInterval = millisecPerSec/refreshClockSpeed;
  private static System.Timers.Timer userInterfaceRefresh = new System.Timers.Timer();
  private static System.Timers.Timer ballUpdate = new System.Timers.Timer();
  private const double lineSegmentWidth = 2.0;
  private Pen bic = new Pen(Color.Red, (int)System.Math.Round(lineSegmentWidth));

  private TextBox x_Coordinate = new TextBox();
  private TextBox y_Coordinate = new TextBox();
  private bool clocksStopped = true;
  private double ballStartingX = 1495;
  private double ballStartingY = 195;
  private int p0x = 1495;
  private int p0y = 195;
  private int p1x;
  private int p1y;
  private int p2x;
  private int p2y;
  private int p3x;
  private int p3y;
  private int p4x;
  private int p4y;
  private double x;
  private double y;
  private Point startPoint;
  private Point upperleftCorner;
  private Point lowerLeftCorner;
  private Point lowerRightCorner;
  private Point end;


public TravellingBallUI() {
  MaximumSize = maxInterface;
  MinimumSize = minInterface;
  userInterfaceRefresh.Enabled = false;
  userInterfaceRefresh.Elapsed += new ElapsedEventHandler(refreshUserInterface);
  ballUpdate.Enabled = false;
  ballUpdate.Elapsed += new ElapsedEventHandler(updateBallCoords);

  direction.Text = "Down";
  direction.Size = new Size(50, 16);
  direction.Location = new Point(1100,800);


  x_Coordinate.Size = new Size(50,16);
  x_Coordinate.Location = new Point(1100, 850);

  y_Coordinate.Size = new Size(50, 16);
  y_Coordinate.Location = new Point(1100,875);

  startPoint = new Point(p0x, p0y);

  p1x = 90;  p1y = p0y;
  upperleftCorner = new Point(p1x, p1y);

  p2x = p1x;  p2y = 700;
  lowerLeftCorner = new Point(p2x, p2y);

  p3x = 1495; p3y = p2y;
  lowerRightCorner = new Point(p3x, p3y);

  p4x = p0x;  p4y = p0y;
  end = new Point(p4x, p4y);

  x = (double)ballStartingX - ballRadius;
  y = (double)ballStartingY - ballRadius;


  Text = "Travelling Ball";
  //title.BackColor = Color.Aquamarine;
  exit.Text = "Exit";
  reset.Text = "Reset";
  GoPauseButton.Text = "Go";

  Size = new Size(1600, 1000);
  title.Size = new Size(1600, 100);
  exit.Size = new Size(80, 80);
  GoPauseButton.Size = new Size(80, 80);
  reset.Size = new Size(80, 80);

  //title.Location = new Point(0, 15);
  exit.Location = new Point(1000, 800);
  GoPauseButton.Location = new Point(200, 800);
  reset.Location = new Point (300, 800);

  Controls.Add(exit);
  Controls.Add(GoPauseButton);
  //Controls.Add(title);
  Controls.Add(reset);
  Controls.Add(x_Coordinate);
  Controls.Add(y_Coordinate);
  Controls.Add(direction);

  GoPauseButton.Click += new EventHandler(goPause);
  exit.Click += new EventHandler(Exit);
  reset.Click += new EventHandler(resetClick);

  welcome_location = new Point(600,50);




  }

  protected override void OnPaint(PaintEventArgs ee){
    Graphics graph = ee.Graphics;
    graph.FillRectangle(Brushes.Yellow, 0, 790, 1600, 300);
    graph.FillRectangle(Brushes.Green, 0, 0, 1600, 150);
    graph.DrawLine(bic, startPoint, upperleftCorner);
    graph.DrawLine(bic, upperleftCorner, lowerLeftCorner);
    graph.DrawLine(bic, lowerLeftCorner, lowerRightCorner);
    graph.DrawLine(bic, lowerRightCorner, end);
    graph.DrawString(welcome_message,welcome_style,welcome_paint_brush,welcome_location);
    graph.FillEllipse(Brushes.Blue, (int)System.Math.Round(x),
    (int)System.Math.Round(y),(int)System.Math.Round(2.0*ballRadius),
    (int)System.Math.Round(2.0*ballRadius));
    if(GoPauseButton.Text == "Done") {
      graph.FillEllipse(Brushes.Gold, (int)System.Math.Round(x),
      (int)System.Math.Round(y),(int)System.Math.Round(2.0*ballRadius),
      (int)System.Math.Round(2.0*ballRadius));
    }
    base.OnPaint(ee);
  }

  protected void refreshUserInterface(System.Object sender, ElapsedEventArgs even) {
    Invalidate();
  }

    protected void updateBallCoords(System.Object sender, ElapsedEventArgs even) {
    x_Coordinate.Text = "X: " + x.ToString();
    y_Coordinate.Text = "Y: " + y.ToString();
    if(System.Math.Abs(y+ballRadius - p0y) < 0.5) {
      if(System.Math.Abs(x+ballRadius - (double)p1x) > delta) {
        x -= delta;
        direction.Text = "Direction: Left";
      }
      else {
        y = (double)p1y + (delta - (x+ballRadius - (double)p1x));
        x = (double)p1x - ballRadius;
        direction.Text = "Direction: Down";

      }
    }
    else if(System.Math.Abs(x+ballRadius - (double)p1x) < 0.5) {
      if(System.Math.Abs((double)p2y - (y + ballRadius)) > delta) {
        y = y + delta;
      }
      else {
        x = (double)p2x + (delta - ((double)p2y - (y+ballRadius)));
        y = (double)p2y - ballRadius;
        direction.Text = "Direction: Right";
      }
    }
    else if(System.Math.Abs(y + ballRadius - (double)p2y) < 0.5) {
      if(System.Math.Abs((double)p3x - (x+ballRadius)) > delta) {
        x = x + delta;
      }
      else {
        x = (double)p3x - ballRadius;
        y = (double)p3y - (delta - (x+ballRadius - (double)p3x));
        direction.Text = "Direction: Up";
      }
    }
    else if(System.Math.Abs(x + ballRadius - (double)p3x) < 0.5) {
      if(System.Math.Abs((double)p4y - (y + ballRadius)) > delta) {
        y = y - delta;
      }
      else {
        x = (double)ballStartingX - ballRadius;
        y = (double)ballStartingY - ballRadius;
        userInterfaceRefresh.Enabled = false;
        ballUpdate.Enabled = false;
        GoPauseButton.Text = "Done";
        GoPauseButton.Enabled = false;
      }
    }
  }


  protected void resetClick (Object sender, EventArgs events) {
    x = (double)ballStartingX - ballRadius;
    y = (double)ballStartingY - ballRadius;
  }

  protected void goPause(Object sender, EventArgs events) {
    if(clocksStopped){
      userInterfaceRefresh.Enabled = true;
      ballUpdate.Enabled = true;
      GoPauseButton.Text = "Pause";
    }
    else {
      userInterfaceRefresh.Enabled = false;
      ballUpdate.Enabled = false;
      GoPauseButton.Text = "Go";
    }
    clocksStopped = !clocksStopped;

}


  protected void Exit(Object sender, EventArgs events) {
    Close();
  }





}
