/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 2
Program name: Traffic Light
*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class TrafficLight : Form {

  private Label title = new Label();
  private Button startButton = new Button();
  private Button exitButton = new Button();
  private Button pauseResumeButton = new Button();
  private RadioButton fast = new System.Windows.Forms.RadioButton();
  private RadioButton slow = new System.Windows.Forms.RadioButton();
  private GroupBox rateOfChange = new GroupBox();
  private Size maxInterface = new Size(1280, 900);
  private Size minInterface = new Size(1280, 900);
  private static System.Timers.Timer Tclock = new System.Timers.Timer();
  private ulong tickCounter = 0;
  private enum trafficColor {red, yellow, green};
  trafficColor Light = trafficColor.red;


  public TrafficLight() {
    MaximumSize = maxInterface;
    MinimumSize = minInterface;

    Tclock.Enabled = false;
    Tclock.Elapsed += new ElapsedEventHandler(signal);
    Tclock.Interval = 1000;

    Text = "Traffic light";
    title.Text = "Traffic light by Austin Hoang";
    exitButton.Text = "Exit";
    pauseResumeButton.Text = "Pause";
    startButton.Text = "Start";
    fast.Text = "Fast";
    slow.Text = "Slow";
    rateOfChange.Text = "Rate of Change";
    pauseResumeButton.Enabled = true;

    Size = new Size(1280, 900);
    title.Size = new Size(200, 60);
    exitButton.Size = new Size(60, 60);
    pauseResumeButton.Size = new Size(60, 60);
    rateOfChange.Size = new Size(120, 60);
    fast.Size = new Size(40, 16);
    slow.Size = new Size(60, 16);

    title.Location = new Point(550, 25);
    startButton.Location = new Point(200, 800);
    pauseResumeButton.Location = new Point(450, 800);
    exitButton.Location = new Point(800, 800);
    rateOfChange.Location = new Point(600, 800);


    exitButton.Click += new EventHandler(exit);
    startButton.Click += new EventHandler(start);
    pauseResumeButton.Click += new EventHandler(pauseResume);
    fast.Click += signal;
    slow.Click += signal;


    Controls.Add(exitButton);
    Controls.Add(rateOfChange);
    Controls.Add(pauseResumeButton);
    Controls.Add(startButton);
    Controls.Add(title);
    rateOfChange.Controls.Add(fast);
    fast.Location = new Point(30, 20);
    rateOfChange.Controls.Add(slow);
    slow.Location = new Point(30, 40);

  }


  protected void signal(System.Object send, EventArgs e) {
    tickCounter++;
    switch(Light) {
      case trafficColor.red:
      if(send == fast || tickCounter > 4) {
        tickCounter = 0;
        Light = trafficColor.green;
        Invalidate();
      }
      else if(send == slow || tickCounter > 8) {
        tickCounter = 0;
        Light = trafficColor.green;
        Invalidate();
      }
      break;
      case trafficColor.yellow:
      if(send == fast || tickCounter > 1) {
        tickCounter = 0;
        Light = trafficColor.red;
        Invalidate();
      }
      else if(send == slow || tickCounter > 2) {
        tickCounter = 0;
        Light = trafficColor.red;
        Invalidate();
      }
      break;
      case trafficColor.green:
      if(send == fast || tickCounter > 3) {
        tickCounter = 0;
        Light = trafficColor.yellow;
        Invalidate();
      }
      else if (send == slow || tickCounter > 6) {
        tickCounter = 0;
        Light = trafficColor.yellow;
        Invalidate();
      }
      break;
    }

}

  protected override void OnPaint(PaintEventArgs e) {
    Graphics graph = e.Graphics;
    switch(Light) {
      case trafficColor.red:
      graph.FillEllipse(Brushes.Red, 550, 125, 175, 175);
      graph.FillEllipse(Brushes.Gray, 550, 325, 175, 175);
      graph.FillEllipse(Brushes.Gray, 550, 525, 175, 175);
      break;
      case trafficColor.yellow:
      graph.FillEllipse(Brushes.Gray, 550, 125, 175, 175);
      graph.FillEllipse(Brushes.Yellow, 550, 325, 175, 175);
      graph.FillEllipse(Brushes.Gray, 550, 525, 175, 175);
      break;
      case trafficColor.green:
      graph.FillEllipse(Brushes.Gray, 550, 125, 175, 175);
      graph.FillEllipse(Brushes.Gray, 550, 325, 175, 175);
      graph.FillEllipse(Brushes.Green, 550, 525, 175, 175);
      break;
    }
    base.OnPaint(e);
  }


  protected void pauseResume(Object sender, EventArgs events) {
    if(pauseResumeButton.Text == "Pause") {
      Tclock.Enabled = false;
      pauseResumeButton.Text = "Resume";
    }
    else if(pauseResumeButton.Text == "Resume") {
      Tclock.Enabled = true;
      pauseResumeButton.Text = "Pause";
    }
  }



  protected void start(Object sender, EventArgs events) {
    Tclock.Enabled = true;
  }
  protected void exit(Object sender, EventArgs events) {
    Close();
  }


}
