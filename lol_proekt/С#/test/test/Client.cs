using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerNamespace
{
	class Listener
	{
		private static int clientCounter;
		private int clientListenerID;
		private bool Active;
		public static List<Message> MessageBuffer = new List<Message>();
		public static List<String> Users = new List<string>();
		private String Username;
		public Byte[] ReadBuff;
       
	    public Listener (TcpClient Client)
		{
			clientCounter++;
			clientListenerID = clientCounter;
			Console.WriteLine ("Новое подключение #" + clientListenerID);

			byte[] TempBytes = new Byte[512];
			Active=true;
			Client.GetStream ().Read (TempBytes, 0, TempBytes.Length);
			String input = Encoding.UTF8.GetString (TempBytes);

            if (input.IndexOf("<reg>") != -1)
            {
                Console.WriteLine("Регистрируем нового пользователя!");
                Console.WriteLine("Логин: " + Parsing.getname(input));
                Console.WriteLine("Пароль: " + Parsing.getpass(input));
                registerClient(Parsing.getname(input), Parsing.getpass(input));
                Username = Parsing.getname(input);
            }

            Console.WriteLine("Клиент: " + Parsing.getname(input));

            if (authCheck(Parsing.getname(input), Parsing.getpass(input)))
			{
				TempBytes = new Byte[512];
				TempBytes = System.Text.Encoding.UTF8.GetBytes("<auth>OK</auth>");
				Client.GetStream().Write(TempBytes,0,TempBytes.Length);

				Username = Parsing.getname(input);
				Users.Add(Username);

				ReadBuff = new Byte[512];
				Client.GetStream ().BeginRead (ReadBuff, 0, ReadBuff.Length,new AsyncCallback(myCallBack),Client);
				try
                {
				    while (Active) {

					    readmail(Client);
					    Thread.Sleep(10);
				    }
                } 
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }

				Console.WriteLine("Соединение завершено");
				Console.Read();
				Client.Close();
			}
			else
			{
				Console.WriteLine("Ошибка авторизации");
				Client.Close();
			}
   
	    }

		public void myCallBack (IAsyncResult ar)
		{
			TcpClient cli = (TcpClient) ar.AsyncState;
			String message = Encoding.UTF8.GetString (ReadBuff);
            if (message.IndexOf("<end>") != -1)
            {
                Users.Remove(Username);
                Active = false;
            }
            else
            {
                if (message.IndexOf("<send>") != -1) {
                    String send = Parsing.getsend(message);
                    Console.WriteLine("Адресат: " + Parsing.getto(send));
                    Console.WriteLine("Получает данные");
                    sendMessage(new Message(Username, Parsing.getto(send), Parsing.getmessage(send)), Parsing.getto(send), cli);
                    ReadBuff = new Byte[512];
                } else {
                    Console.WriteLine("buffer: " + message);
                }
                cli.GetStream().BeginRead(ReadBuff, 0, ReadBuff.Length, new AsyncCallback(myCallBack), cli);
            }
			
		}

		public static void sendMessage (Message input, String clientName,TcpClient Client)
		{
			if (isOnline (clientName)) {
				Console.WriteLine("User Online");
				MessageBuffer.Add (input);
			} else {
				Console.WriteLine("User Offline");
				MessageBuffer.Add (input);
			}
		}
		public static bool isOnline (String clientName)
		{
			bool flag=false;
			foreach (String str in Users) {
				if(str.Equals(clientName))
					flag=true;
			}
			return flag;
		}
        public static void registerClient(String username,String password)
        {
            StreamWriter sw = new StreamWriter("Clients.txt", true, Encoding.UTF8);
            sw.WriteLine("<name>"+username+"</name><pass>"+password+"</pass>");
            sw.Close();
        }
		public static bool authCheck (String user, String pass)
		{
            StreamReader sr = new StreamReader("Clients.txt", true);
			String check;
			check = "<name>"+user+"</name><pass>"+pass+"</pass>";
			while (!sr.EndOfStream) {
				String line;
				line = sr.ReadLine ();
				if(line == check)
				{
					sr.Close();
					return true;
				}
			}
			sr.Close();
			return false;
		}
		public void readmail (TcpClient Client)
		{
            Byte[] buffer;
            foreach (Message m in MessageBuffer)
            {
                if ((m.getto() == Username)&&(!m.isCheck()))
                {
                    buffer = new Byte[512];
                    buffer = System.Text.Encoding.UTF8.GetBytes("<send><message>" + m.getmessage() + "</message><to>"+m.getto()+"</to><from>"+m.getfrom()+"</from></send>");
                    Client.GetStream().Write(buffer, 0, buffer.Length);
					m.setCheck(true);
                }
            }
		}
	}
}

