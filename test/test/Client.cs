using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace test
{
	class Client
	{
		private static int counter;
		private int id;
		public static List<Message> MessageBuffer = new List<Message>();
		public static List<String> Users = new List<string>();
		private String username;
	    public Client (TcpClient Client)
		{
			counter++;
			id = counter;
			Console.WriteLine ("Новое подключение #" + id);

			byte[] buffer = new byte[256];

			Client.GetStream ().Read (buffer, 0, buffer.Length);
			String input = Encoding.UTF8.GetString (buffer);
            if (input.IndexOf("<reg>") != -1)
            {
                Console.WriteLine("Регистрируем нового пользователя!");
                Console.WriteLine("Логин: " + getname(input));
                Console.WriteLine("Пароль: " + getpass(input));
                reg(getname(input), getpass(input));
				username = getname(input);
            }
            else
            {
                Console.WriteLine("Клиент: " + getname(input));

            }
			if(check(getname(input),getpass(input)))
			{
				buffer = new byte[256];
				Console.WriteLine("OK");
				buffer = System.Text.Encoding.UTF8.GetBytes("<auth>OK</auth>");

				Client.GetStream().Write(buffer,0,buffer.Length);
				username = getname(input);
				Users.Add(username);

				buffer = new byte[256];
				while (true) {
					Client.GetStream ().Read (buffer, 0, buffer.Length);
					String message = Encoding.UTF8.GetString (buffer);
					if(message.IndexOf("<end>")!=-1)
					{
						Users.Remove(username);
						break;
					}
					if(message.IndexOf("<send>")!=-1)
					{
						String send=getsend(message);
						Console.WriteLine("Полученные данные: "+getsend(message));
						Console.WriteLine("Сообщение: "+ getmessage(send));
						Console.WriteLine("Адресат: "+ getto(send));
						MessageBuffer.Add(new Message(username,getto(send),getmessage(send)));
						buffer = new byte[256];
					}
					else if(message.IndexOf("<read>")!=-1)
					{
						Console.WriteLine("Запрос на чтение почты!");
						readmail(Client);
						Console.WriteLine("Почта отправлена!");
					}

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



        public static void reg(String username,String password)
        {
            StreamWriter sw = new StreamWriter("1.txt",true, Encoding.UTF8);
            sw.WriteLine("<name>"+username+"</name><pass>"+password+"</pass>");
            sw.Close();
        }

		public static bool check (String user, String pass)
		{
			StreamReader sr = new StreamReader ("1.txt", true);
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
                if (m.getto() == username)
                {
                    buffer = new byte[256];
                    buffer = System.Text.Encoding.UTF8.GetBytes("<message>" + m.getmessage() + "</message><from>" + m.getfrom() + "</from>");
                    Client.GetStream().Write(buffer, 0, buffer.Length);
                    buffer = new byte[256];
                }
            }

            buffer = new byte[256];
            buffer = System.Text.Encoding.UTF8.GetBytes("</read>");
            Client.GetStream().Write(buffer, 0, buffer.Length);
		}

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
			//int pos = message.IndexOf("</message>");
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


	}
}

