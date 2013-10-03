using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
 
namespace test
{
    class Server
	{
	    public TcpListener Listener; // Объект, принимающий TCP-клиентов


	    // Запуск сервера
	    public Server(int Port)
	    {
	        // Создаем "слушателя" для указанного порта
	        Listener = new TcpListener(IPAddress.Any, Port);
	        Listener.Start(); // Запускаем его
	 
	        // В бесконечном цикле
	        while (true)
	        {
				            // Принимаем нового клиента
				TcpClient Client = Listener.AcceptTcpClient();
				// Создаем поток
				Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
				// И запускаем этот поток, передавая ему принятого клиента
					Thread.Start(Client);
	        }
	    }


		static void ClientThread(Object StateInfo)
		{

			new Client((TcpClient)StateInfo);
		}
	 
	    // Остановка сервера
	    ~Server()
	    {
	        // Если "слушатель" был создан
	        if (Listener != null)
	        {
	            // Остановим его
	            Listener.Stop();
	        }
	    }
	 /*
	    static void Main(string[] args)
	    {
	        // Создадим новый сервер на порту 80
	        new Server(80);
	    }
	    */
	}
}
 