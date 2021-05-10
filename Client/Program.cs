using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = null;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipaddress = null;
            int portNumber = 0;

            try
            {
                Console.WriteLine("Enter the server IP address that you want to connect to");
                string serverIP = Console.ReadLine();

                Console.WriteLine("Enter the port number of the server application");
                string portNo = Console.ReadLine();

                if (!IPAddress.TryParse(serverIP, out ipaddress))
                {
                    Console.WriteLine("IP Address is invalid. Enter a valid IP Address");
                    return;
                }

                if (!int.TryParse(portNo, out portNumber))
                {
                    Console.WriteLine("Port number is invalid. Enter a valid port number");
                    return;
                }

                Console.WriteLine(string.Format("Ip address is {0} and port number is {1}", ipaddress, portNumber));

                client.Connect(ipaddress, portNumber);

                Console.WriteLine("Connected to server. Type any text and press enter to send it to server. Type EXIT to close the connection");

                string userInput = string.Empty;
                
                while(true)
                {
                    userInput = Console.ReadLine();

                    if(userInput == "EXIT")
                    {
                        byte[] exitMessage = Encoding.ASCII.GetBytes(userInput);

                        client.Send(exitMessage);

                        break;
                    }

                    byte[] dataToSend = Encoding.ASCII.GetBytes(userInput);

                    client.Send(dataToSend);

                    byte[] dataReceived = new byte[128];

                    int numberOfBytesReceived = client.Receive(dataReceived);

                    string serverResponse = Encoding.ASCII.GetString(dataReceived, 0, numberOfBytesReceived);

                    Console.WriteLine("Data received from server is {0}", serverResponse);
                }
            
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

            finally
            {
                if(client != null)
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);
                    }

                    client.Close();

                    client.Dispose();
                }
            }

            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }
    }
}
