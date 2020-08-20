using System;
using System.Windows.Forms;

public class FallingAppleMain {
  public static void Main() {
    System.Console.WriteLine("program has begun.");
    FallingAppleUI t = new FallingAppleUI();
    Application.Run(t);
    System.Console.WriteLine("Bye.");
  }
}
