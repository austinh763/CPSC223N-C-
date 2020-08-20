echo "Program: Final"

echo First remove old Final
rm *.dll
rm *.exe

echo "Compile FinalUI.cs"
mcs -target:library FinalUI.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:FinalUI.dll

echo "Compile FinalMain.cs"
mcs -target:exe FinalMain.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:FinalUI.dll -out:Final.exe

echo "Run the program"
./Final.exe

echo "The bash script will finish now"
