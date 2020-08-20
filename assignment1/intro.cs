/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 1
Program name: Flashing Red Light
*/
using System;

using System.Windows.Forms;


public class intro {
  static void Main(string[] args) {
    System.Console.WriteLine("start up screen");
    ui userinterface = new ui();
    Application.Run(userinterface);
    System.Console.WriteLine("shutdown");
  }
}
