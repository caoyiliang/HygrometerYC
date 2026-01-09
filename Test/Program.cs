// See https://aka.ms/new-console-template for more information
using HygrometerYC;

Console.WriteLine("Hello, World!");
IHygrometerYC hygrometerYC = new HygrometerYC.HygrometerYC(new Communication.Bus.PhysicalPort.SerialPort("COM3", 38400));
await hygrometerYC.OpenAsync();
var rs = await hygrometerYC.Read("01");

Console.ReadLine();