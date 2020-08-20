/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 3
Program name: Travelling Ball
*/
using System;
using System.Windows.Forms;


public class TravellingBallMain {
  static void Main(string[] args) {
    System.Console.WriteLine("start up");
    TravellingBallUI t = new TravellingBallUI();
    Application.Run(t);
    System.Console.WriteLine("shutdown");
  }
}
