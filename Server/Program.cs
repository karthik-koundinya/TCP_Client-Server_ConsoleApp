using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress serverIP = IPAddress.Any;

            int serverPort = 23000;

            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, serverPort);

            serverSocket.Bind(serverEndPoint);

            serverSocket.Listen(5);

            Console.WriteLine("About to accept incoming connection");

            Socket clientSocket = serverSocket.Accept();

            Console.WriteLine("Client connected " + clientSocket.ToString() + " IP end point is : " + clientSocket.RemoteEndPoint.ToString());
            
            byte[] dataBuffer = new byte[128];

            int numberOfBytesReceived = 0;

            string receivedTextData = null;

            while (true)
            {
                try
                {
                    numberOfBytesReceived = clientSocket.Receive(dataBuffer);

                    Console.WriteLine("Number of bytes received is " + numberOfBytesReceived);

                    receivedTextData = Encoding.ASCII.GetString(dataBuffer, 0, numberOfBytesReceived);

                    Console.WriteLine("Data received from client is " + receivedTextData);

                    clientSocket.Send(dataBuffer);

                    Array.Clear(dataBuffer, 0, dataBuffer.Length);

                    numberOfBytesReceived = 0;

                    if (receivedTextData.ToLower() == "exit")
                    {
                        Console.WriteLine("Connection terminated by the client intentionally");
                        break;
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine("Exception received. I think connection with client is lost ");
                    Console.WriteLine("Exception message is " + exception.Message);
                    break;
                }
            }

            Console.WriteLine("Server program ending");

            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }
    }
}
