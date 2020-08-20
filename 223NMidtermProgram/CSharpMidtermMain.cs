/*
Austin Hoang
CPSC 223N
C Sharp Midterm Test
Oct 14, 2019
*/
using System;
using System.Windows.Forms;


public class TravellingBallMain {
  static void Main(string[] args) {
    System.Console.WriteLine("start up");
    CSharpMidtermUI t = new CSharpMidtermUI();
    Application.Run(t);
    System.Console.WriteLine("shutdown");
  }
}
