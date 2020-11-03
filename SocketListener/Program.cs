using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketListener
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // creating a new socket where we will listen for incoming connections, using IP4, stream and TCP protocols
            IPAddress ipaddr = IPAddress.Any;
            // the IPAdress.Any sets the IP adress to the local host IP address
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);
            // we are just assigning the socket number for the enpoint, socket numbers can be any number from  ____ to ____, though 1-____ are reserved
            listenerSocket.Bind(ipep);
            listenerSocket.Listen(5);
            // this tells the system how many clients can wait for a connection while the systems is busy handling other connection
            Console.WriteLine("About to accept incoming connection.");
            Socket client = listenerSocket.Accept();
            // note that the Socket.Accept operation is a synchronous BLOCKING operation, (PURE EVIL -lol) nothing else happens until this operation is finished.
            Console.WriteLine($"Client connected." + client.ToString() + "IP End Point " + client.RemoteEndPoint.ToString());
            byte[] buff = new byte[128];
            int numberOfReceivedBytes = 0;
            while (true)
            {  //this creates a nice infinite loop unless break is reached
                numberOfReceivedBytes = client.Receive(buff);
                Console.WriteLine($"Number of received bytes:  + {numberOfReceivedBytes}");
                Console.WriteLine("Data sent by client is :" + buff);
                // the line above doesn't print the data in the correct text format
                string receivedText = Encoding.ASCII.GetString(buff, 0, numberOfReceivedBytes);
                // the 0 means encode the array from the 0 index to the number of received bytes index. 
                Console.WriteLine($"Data sent by client is : {receivedText}");
                client.Send(buff);
                // we already had the client socket set up above.
                //this is going to just send the contents of the buffer back to the client
                if (receivedText == "x")
                { break; }
                Array.Clear(buff, 0, buff.Length);
                numberOfReceivedBytes = 0;
                //the above two lines clear the buff array, and buff.Length as well as resetting int numberOfReceivedBytes to zero
            }
        }
    }
}
