using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
 

namespace test
{
	class Client
	{
		private static int counter;
		private int id;
		private bool active;
		public static List<Message> MessageBuffer = new List<Message>();
		public static List<String> Users = new List<string>();
		private String username;
		public Byte[] newbuff;
	    public Client (TcpClient Client)
		{
			counter++;
			id = counter;
			Console.WriteLine ("Новое подключение #" + id);

			byte[] buffer = new byte[256];
			active=true;
			Client.GetStream ().Read (buffer, 0, buffer.Length);
			String input = Encoding.UTF8.GetString (buffer);
            if (input.IndexOf("<reg>") != -1)
            {
                Console.WriteLine("Регистрируем нового пользователя!");
                Console.WriteLine("Логин: " + Parsing.getname(input));
                Console.WriteLine("Пароль: " + Parsing.getpass(input));
                reg(Parsing.getname(input), Parsing.getpass(input));
                username = Parsing.getname(input);
            }

                Console.WriteLine("Клиент: " + Parsing.getname(input));

            
            if (check(Parsing.getname(input), Parsing.getpass(input)))
			{
				buffer = new byte[256];
				Console.WriteLine("Check");
				buffer = System.Text.Encoding.UTF8.GetBytes("<auth>OK</auth>");

				Client.GetStream().Write(buffer,0,buffer.Length);
				username = Parsing.getname(input);
				Users.Add(username);
				Console.WriteLine("User Add");

				buffer = new byte[256];
				Console.WriteLine("Starting...");
				newbuff = new byte[256];
				Client.GetStream ().BeginRead (newbuff, 0, newbuff.Length,new AsyncCallback(myCallBack),Client);
				Console.WriteLine("After");
				while (active) {
					
					readmail(Client);
					Thread.Sleep(10);

				}
				Console.WriteLine("Соединение завершено");
				Console.Read();
				Client.Close();
			}
			else
			{
				Console.WriteLine("NOT OK");
				Client.Close();
			}
   
	    }

		public void myCallBack (IAsyncResult ar)
		{
			Console.WriteLine("callback!");
			TcpClient cli = (TcpClient) ar.AsyncState;
			String message = Encoding.UTF8.GetString (newbuff);
					if(message.IndexOf("<end>")!=-1)
					{
						Users.Remove(username);
						active=false;
					}
					else if(message.IndexOf("<send>")!=-1)
					{
                        String send = Parsing.getsend(message);
                        Console.WriteLine("Адресат: " + Parsing.getto(send));
                        Console.WriteLine("Получает данные");
						//Console.WriteLine("Полученные данные: "+getsend(message));//!
						//Console.WriteLine("Сообщение: "+ getmessage(send));

                        sendToClient(new Message(username, Parsing.getto(send), Parsing.getmessage(send)),Parsing.getto(send),cli);
						//readmail(cli);
						newbuff = new byte[256];
					}
					else if(message.IndexOf("<read>")!=-1)
					{
						Console.WriteLine("Запрос на чтение почты!");
						//readmail(cli);
						Console.WriteLine("Почта отправлена!");
					}
					else
					{
						Console.WriteLine("Message: "+message);
					}




			cli.GetStream ().BeginRead (newbuff, 0, newbuff.Length,new AsyncCallback(myCallBack),cli);
		}

		public static void sendToClient (Message input, String clientName,TcpClient Client)
		{
			Byte[] buffer;
			if (isOnline (clientName)) {
				Console.WriteLine("User Online");
				    //buffer = new byte[256];
                    //buffer = System.Text.Encoding.UTF8.GetBytes("<message>" + input.getmessage() + "</message><to>"+input.getfrom()+"</to>");
                    //Client.GetStream().Write(buffer, 0, buffer.Length);
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

        public static void reg(String username,String password)
        {
            StreamWriter sw = new StreamWriter("Clients.txt", true, Encoding.UTF8);
            sw.WriteLine("<name>"+username+"</name><pass>"+password+"</pass>");
            sw.Close();
        }

		public static bool check (String user, String pass)
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
			//List<Message> temp = new List<Message>();
			
            foreach (Message m in MessageBuffer)
            {
                if ((m.getto() == username)&&(!m.isCheck()))
                {
                    buffer = new byte[256];
                    buffer = System.Text.Encoding.UTF8.GetBytes("<message>" + m.getmessage() + "</message><to>"+m.getfrom()+"</to>");
                    Client.GetStream().Write(buffer, 0, buffer.Length);
					m.setCheck(true);
                }
            }


			//temp.RemoveAll();


            //buffer = new byte[256];
            //buffer = System.Text.Encoding.UTF8.GetBytes("");
            //Client.GetStream().Write(buffer, 0, buffer.Length);
		}
/*
		//Все что ниже - парсинг полученных данных
		public String getname(String input)
		{
            int pos1 = input.IndexOf("<name>");
            int pos2 = input.IndexOf("</name>");
			if(pos1==-1||pos2==-1)
				return "Ошибка парсинга";
			else
            return input.Substring(pos1 + 6, pos2 - pos1 - 6);
		}
        public String getpass(String input)
        {
            int pos1 = input.IndexOf("<pass>");
            int pos2 = input.IndexOf("</pass>");
			if(pos1==-1||pos2==-1)
				return "Ошибка парсинга";
			else
            return input.Substring(pos1 + 6, pos2 - pos1 - 6);
        }
		public String getsend(String input)
        {
            int pos1 = input.IndexOf("<send>");
            int pos2 = input.IndexOf("</send>");
			if(pos1==-1||pos2==-1)
				return "Ошибка парсинга";
			else
			return input.Substring(pos1+6,pos2-pos1-6);
		}		
        public String getmessage (String input)
		{
			int pos1 = input.IndexOf("<message>");
            int pos2 = input.IndexOf("</message>");
			if(pos1==-1||pos2==-1)
				return "Ошибка парсинга";
			else
			return input.Substring(pos1+9,pos2-pos1-9);
		}
		public String getto (String to)
		{
			int pos = to.IndexOf("<to>");
			int pos2 = to.IndexOf("</to>");
			if(pos==-1||pos2==-1)
				return "Ошибка парсинга";
			else
			return to.Substring(pos+4,pos2-pos-4);
		}
        */

	}
}

