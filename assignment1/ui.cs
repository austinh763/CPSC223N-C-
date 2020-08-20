/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 1
Program name: Flashing Red Light
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class ui : Form {
  private Label title = new Label();
  private Button exitButton = new Button();
  private Button pauseResumeButton = new Button();
  private Size maxInterface = new Size(1280,720);
  private Size minInterface = new Size(1280,720);
  private static System.Timers.Timer rolex = new System.Timers.Timer();
  private bool redVisible = true;



  public ui() {
    MaximumSize = maxInterface;
    MinimumSize = minInterface;

    rolex.Interval = 500;

    Text = "red light";
    title.Text = "Red Light by Austin Hoang";
    exitButton.Text = "Exit";
    pauseResumeButton.Text = "Pause";
    pauseResumeButton.Enabled = true;
    rolex.Enabled = true;

    rolex.Elapsed += new ElapsedEventHandler(update_red_light);
    exitButton.Click += new EventHandler(stoprun);
    pauseResumeButton.Click += new EventHandler(pauseButton);



    Size = new Size (1280,720);
    title.Size = new Size (240,60);
    exitButton.Size = new Size(170, 60);
    pauseResumeButton.Size = new Size(170,60);

    title.Location = new Point(640, 0);
    pauseResumeButton.Location = new Point(500, 500);
    exitButton.Location = new Point(740, 500);

    Controls.Add(title);
    Controls.Add(exitButton);
    Controls.Add(pauseResumeButton);
}





    protected void pauseButton(Object sender, EventArgs events) {
      if(pauseResumeButton.Text == "Pause") {
        rolex.Enabled = false;
        pauseResumeButton.Text = "Resume";

      }
      else if (pauseResumeButton.Text == "Resume") {
        rolex.Enabled = true;
        pauseResumeButton.Text = "Pause";

      }
    }

    protected override void OnPaint(PaintEventArgs a) {
      Graphics lights = a.Graphics;
      if(redVisible){
      lights.FillEllipse(Brushes.Red,540,160,300,300);
    }
      else {
        lights.FillEllipse(Brushes.Gray,540,160,300,300);
      }
        base.OnPaint(a);
    }

    private void update_red_light(Object sender, EventArgs events) {
      redVisible = !redVisible;
      Invalidate();
    }

    protected void stoprun(Object sender, EventArgs events) {
      Close();
    }



}
