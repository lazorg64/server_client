using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ServerNamespace
{
    class Server
	{
	    public TcpListener Listener;
	    public Server(int Port)
	    {
	        Listener = new TcpListener(IPAddress.Any, Port);
	        Listener.Start();
	        while (true)
	        {
				TcpClient Client = Listener.AcceptTcpClient();
				Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
				Thread.Start(Client);
	        }
	    }
		static void ClientThread(Object StateInfo)
		{

			new Listener((TcpClient)StateInfo);
		}
	    ~Server()
	    {
	        if (Listener != null)
	        {
	            Listener.Stop();
	        }
	    }
	}
}
 