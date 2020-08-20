/*
Author: Austin Hoang
Email: austinhoang47@csu.fullerton.edu
Course: CPSC 223N
Semester: Fall 2019
Assignment #: 2
Program name: Traffic Light
*/
using System;
using System.Windows.Forms;


public class TrafficLightMain {
  static void Main(string[] args) {
    System.Console.WriteLine("start up");
    TrafficLight traffic = new TrafficLight();
    Application.Run(traffic);
    System.Console.WriteLine("shutdown");
  }
}
