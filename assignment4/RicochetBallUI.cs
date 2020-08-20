using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class RicochetBallUI : Form {
  private const int FORM_WIDTH = 1000;
  private const int FORM_HEIGHT = 900;
  private const int TITLE_HEIGHT = 40;
  private const int GRAPHIC_HEIGHT = 800;
  private const int LOWER_PANEL_HEIGHT = FORM_HEIGHT - TITLE_HEIGHT - GRAPHIC_HEIGHT;

  private const double ballRadius = 5;
  private double ballLinearSpeedPixPerSec;
  private double ballLinearSpeedPixPerTic;
  private double ballDirectionX;
  private double ballDirectionY;
  private double ballDeltaX;
  private double ballDeltaY;
  private const double ballCenterInitialCoordX = (double)FORM_WIDTH * 0.65;
  private const double ballCenterInitialCoordY = (double)GRAPHIC_HEIGHT/2.0 + TITLE_HEIGHT;
  private double ballCenterCurrentCoordX;
  private double ballCenterCurrentCoordY;
  private double ballUpperLeftCurrentCoordX;
  private double ballUpperLeftCurrentCoordY;

  private static System.Timers.Timer ballMotionControlClock = new System.Timers.Timer();
  private const double ballMotionControlClockRate = 43.5;

  private static System.Timers.Timer graphicAreaRefreshClock = new System.Timers.Timer();
  private const double graphicRefreshRate = 23.3;

  private Font styleMessage = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
  private String title = "Ricochet Ball by Austin Hoang";
  private Brush writingTool = new SolidBrush(System.Drawing.Color.Black);
  private Point titleLocation = new Point(FORM_WIDTH/2-15, 10);
  private Button newButton = new Button();
  private TextBox speedBox = new TextBox();
  private Button startButton = new Button();
  private Point startLocation = new Point(FORM_WIDTH/10, TITLE_HEIGHT+GRAPHIC_HEIGHT+6);
  private Button quitButton = new Button();
  private TextBox X_output = new TextBox();
  private TextBox Y_output = new TextBox();
  private TextBox speedInput = new TextBox();
  private TextBox directionInput = new TextBox();

  private bool newClicked = false;

  public RicochetBallUI() {
    Text = "Ricochet Ball";
    Size = new Size(FORM_WIDTH, FORM_HEIGHT);
    BackColor = Color.Green;
    double speed;
    double directionDegrees;
    double.TryParse(speedInput.Text, out speed);
    double.TryParse(directionInput.Text, out directionDegrees);
    ballLinearSpeedPixPerSec = 100;
    ballDirectionX = 58.3;
    ballDirectionY = 56.9;


    ballLinearSpeedPixPerTic = ballLinearSpeedPixPerSec/ballMotionControlClockRate;
    double hypotenuseSquared = ballDirectionX*ballDirectionX +
    ballDirectionY*ballDirectionY;
    double hypotenuse = System.Math.Sqrt(hypotenuseSquared);
    ballDeltaX = ballLinearSpeedPixPerTic * ballDirectionX / hypotenuse;
    ballDeltaY = ballLinearSpeedPixPerTic * ballDirectionY / hypotenuse;

    ballCenterCurrentCoordX = ballCenterInitialCoordX;
    ballCenterCurrentCoordY = ballCenterInitialCoordY;

    ballMotionControlClock.Enabled = false;
    ballMotionControlClock.Elapsed += new ElapsedEventHandler(updateBallPos);

    graphicAreaRefreshClock.Enabled = false;
    graphicAreaRefreshClock.Elapsed += new ElapsedEventHandler(updateDisplay);

    speedInput.Size = new Size(60, 30);
    speedInput.Location = new Point(350, 850);

    newButton.Text = "New";
    newButton.Size = new Size(60,20);
    newButton.Location = new Point(FORM_WIDTH/2, TITLE_HEIGHT+GRAPHIC_HEIGHT+6);

    startButton.Text = "Start";
    startButton.Size = new Size(60,20);
    startButton.Location = startLocation;
    startButton.BackColor = Color.LimeGreen;

    quitButton.Text = "Quit";
    quitButton.Size = new Size(60,20);
    quitButton.Location = new Point(FORM_WIDTH/5, TITLE_HEIGHT+GRAPHIC_HEIGHT+6);

    directionInput.Size = new Size(60, 30);
    directionInput.Location = new Point(350, 830);


    X_output.Size = new Size (45, 16);
    Y_output.Size = new Size(45, 16);
    X_output.Location = new Point(800, 830);
    Y_output.Location = new Point(800, 850);

    Controls.Add(startButton);
    Controls.Add(quitButton);
    Controls.Add(X_output);
    Controls.Add(Y_output);
    Controls.Add(speedInput);
    Controls.Add(directionInput);
    Controls.Add(newButton);

    startButton.Click += new EventHandler(start);
    quitButton.Click += new EventHandler(quit);
    newButton.Click += new EventHandler(newClick);

  }
  protected override void OnPaint(PaintEventArgs ee) {
    Graphics graph = ee.Graphics;
    graph.FillRectangle(Brushes.Pink, 0, 0, FORM_WIDTH, TITLE_HEIGHT);
    graph.FillRectangle(Brushes.Yellow, 0, 800, FORM_WIDTH,
    800);
    graph.DrawString(title,styleMessage, writingTool,titleLocation);
    ballUpperLeftCurrentCoordX = ballCenterCurrentCoordX - ballRadius;
    ballUpperLeftCurrentCoordY = ballCenterCurrentCoordY - ballRadius;
    if(newClicked == true) {
      graph.FillEllipse(Brushes.Red,(int)ballUpperLeftCurrentCoordX, (int)ballUpperLeftCurrentCoordY,
      (float)(2.0*ballRadius), (float)(2.0*ballRadius));
    }
    else {
      graph.FillEllipse(Brushes.Red,1005, 905,
      (float)(2.0*ballRadius), (float)(2.0*ballRadius));
    }
    base.OnPaint(ee);
  }

  protected void start(Object sender, EventArgs events) {
    StartGraphicClock(graphicRefreshRate);
    StartBallClock(ballMotionControlClockRate);
  }

  protected void StartGraphicClock(double refreshRate) {
    double actualRefreshRate = 1.0;
    double elapsedTimeBetweenTics;
    if(refreshRate > actualRefreshRate)
    actualRefreshRate = refreshRate;
    elapsedTimeBetweenTics = 1000.0/actualRefreshRate;
    graphicAreaRefreshClock.Interval = (int)System.Math.Round(elapsedTimeBetweenTics);
    graphicAreaRefreshClock.Enabled = true;
  }

  protected void StartBallClock(double updateRate) {
    double elapsedTimeBetweenBallMoves;
    if(updateRate < 1.0)
    updateRate = 1.0;
    elapsedTimeBetweenBallMoves = 1000.0/updateRate;
    ballMotionControlClock.Interval = (int)System.Math.Round(elapsedTimeBetweenBallMoves);
    ballMotionControlClock.Enabled = true;
  }

  protected void updateDisplay(System.Object sender, ElapsedEventArgs evt) {
    Invalidate();
    if(!ballMotionControlClock.Enabled) {
      graphicAreaRefreshClock.Enabled = false;
    }
  }

  protected void updateBallPos(System.Object sender, ElapsedEventArgs evt) {
    ballCenterCurrentCoordX += ballDeltaX;
    ballCenterCurrentCoordY -= ballDeltaY;
    X_output.Text = ballCenterCurrentCoordX.ToString();
    Y_output.Text = ballCenterCurrentCoordY.ToString();
    if((int)System.Math.Round(ballCenterCurrentCoordX+ballRadius) >= FORM_WIDTH)
    ballDeltaX = -ballDeltaX;
    else if((int)System.Math.Round(ballCenterCurrentCoordY+ballRadius) >= 800)
    ballDeltaY = -ballDeltaY;
    else if((int)System.Math.Round(ballCenterCurrentCoordX) == 0)
    ballDeltaX = -ballDeltaX;
    else if((int)System.Math.Round(ballCenterCurrentCoordY) == 45)
    ballDeltaY = -ballDeltaY;


  }

  protected void quit(Object sender, EventArgs events) {
    Close();
  }

  protected void newClick(Object sender, EventArgs events) {
    newClicked = true;
    StartGraphicClock(graphicRefreshRate);
    X_output.Text = "";
    Y_output.Text = "";
    directionInput.Text = "";
    speedInput.Text = "";
    ballCenterCurrentCoordX = ballCenterInitialCoordX;
    ballCenterCurrentCoordY = ballCenterInitialCoordY;

  }

}
