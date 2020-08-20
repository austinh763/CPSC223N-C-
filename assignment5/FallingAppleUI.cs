using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class FallingAppleUI : Form {
  private const int formWidth = 1280;
  private const int formHeight = 720;
  private const int horizontalAdjustment = 8;
  private const int penWidth = 3;

  private Label title = new Label();
  private int mouse_x = 0;
  private int mouse_y = 0;

  private const double ballRadius = 20;
  private double ballLinearSpeedSec;
  private double ballLinearSpeedTic;
  private double ballDirectionY;
  private double ballDeltaY;
  private const double ballCenterInitialCoordY = 0;

  private TextBox applesCaught = new TextBox();
  private int applesCaughtNum;
  private bool caught = false;
  private double distance;
  private int appleX;
  private int appleY;
  private int p0x = 1100;
  private int p0y = -300;
  private int p1x;
  private int p1y;
  private double x;
  private double y;
  private double ballStartingX = 1100;
  private double ballStartingY = -50;



  private Button start = new Button();
  private Point startLocation = new Point(150, 620);
  private Button quit = new Button();
  private Point quitLocation = new Point(150, 660);

  private const double delta = 12.5; //Animation Speed: distance travelled/tick
  private const double animationClockSpeed = 190.7;//Hz; times/sec coordinates of
  //of ball updated.
  private const double refreshClockSpeed = 30.0; //times/sec ui repainted.
  private const double millisecPerSec = 1000.0;
  private const double animationInterval = millisecPerSec/animationClockSpeed;
  private const double refreshClockInterval = millisecPerSec/refreshClockSpeed;
  private static System.Timers.Timer userInterfaceRefresh = new System.Timers.Timer();
  private static System.Timers.Timer ballUpdate = new System.Timers.Timer();
  private bool clocksStopped = true;



  public FallingAppleUI() {
    Text = "Falling Apples";
    Size = new Size(formWidth, formHeight);
    userInterfaceRefresh.Enabled = false;
    userInterfaceRefresh.Elapsed += new ElapsedEventHandler(refreshUserInterface);
    ballUpdate.Enabled = false;
    ballUpdate.Elapsed += new ElapsedEventHandler(updateBallCoords);

    start.Text = "Start";
    start.Size = new Size(85, 25);
    start.Location = startLocation;
    quit.Text = "Quit";
    quit.Size = new Size(85, 25);
    quit.Location = quitLocation;
    applesCaught.Size = new Size(50, 25);
    applesCaught.Location = new Point(500, 640);

    x = (double)ballStartingX - ballRadius;
    y = (double)ballStartingY - ballRadius;



    Controls.Add(start);
    Controls.Add(quit);
    Controls.Add(applesCaught);

    start.Click += new EventHandler(startButton);
    quit.Click += new EventHandler(quitButton);


  }

  protected override void OnPaint(PaintEventArgs ee) {
    Graphics draw = ee.Graphics;
    draw.FillRectangle(Brushes.Yellow, 0, 600, 1280, 250);
    draw.FillRectangle(Brushes.Brown, 0 , 350, 1280, 250);
    draw.FillRectangle(Brushes.LightBlue, 0, 5, 1280, 355);
    if(!caught) {
      draw.FillEllipse(Brushes.Red, (int)System.Math.Round(x),
      (int)System.Math.Round(y),(int)System.Math.Round(2.0*ballRadius),
      (int)System.Math.Round(2.0*ballRadius));
    }
  }

  protected override void OnMouseDown(MouseEventArgs e) {
    mouse_x = e.X;
    mouse_y = e.Y;
    double distsq = Math.Pow(mouse_x -(x + ballRadius), 2)+ Math.Pow(mouse_y-(y+ballRadius), 2);
    if(distsq < ballRadius * ballRadius && y > 300) {
      caught = true;
    }
    else {
      caught = false;
    }
    base.OnMouseDown(e);
    Invalidate();
  }

  protected void refreshUserInterface(System.Object sender, ElapsedEventArgs even) {
  Invalidate();
  }

  public int RandomNumber(int min, int max) {
    Random random = new Random();
    return random.Next(min, max);
  }

  protected void updateBallCoords(System.Object sender, ElapsedEventArgs even) {
    y = y + delta;
    string caughtString = applesCaughtNum.ToString();
    applesCaught.Text = caughtString;
    ballStartingX = RandomNumber(100, 1180);
    if((int)System.Math.Round(y) >= 600) {
      x = (double)ballStartingX - ballRadius;
      y = (double)ballStartingY - ballRadius;
    }
    else if(caught == true) {
      x = (double)ballStartingX - ballRadius;
      y = (double)ballStartingY - ballRadius;
      applesCaughtNum++;
    }
    else if(applesCaughtNum == 10) {
      ballUpdate.Enabled = false;
      applesCaught.Text = "Done";
    }
    caught = false;
  }

  protected void startButton(Object sender, EventArgs events) {
    if(clocksStopped){
      userInterfaceRefresh.Enabled = true;
      ballUpdate.Enabled = true;
      start.Text = "Pause";
    }
    else {
      userInterfaceRefresh.Enabled = false;
      ballUpdate.Enabled = false;
      start.Text = "Go";
    }
    clocksStopped = !clocksStopped;
  }

  protected void quitButton(Object sender, EventArgs events) {
    Close();
  }
}
