using System;
using System.Windows.Forms;


public class RicochetBallMain {
  static void Main(string[] args) {
    System.Console.WriteLine("start up");
    RicochetBallUI t = new RicochetBallUI();
    Application.Run(t);
    System.Console.WriteLine("shutdown");
  }
}
