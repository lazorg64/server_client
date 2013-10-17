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
		public static List<Message> MessageBuffer;
		public static List<String> Users;
	    
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
            }
            else
            {
                Console.WriteLine("Клиент: " + getname(input));
            }
			buffer = new byte[256];

			while (true) {
				Client.GetStream ().Read (buffer, 0, buffer.Length);
				String message = Encoding.UTF8.GetString (buffer);
				if(message.IndexOf("<end>")!=-1)
				{
					break;
				}
				String send=getsend(message);
				Console.WriteLine("Полученные данные: "+getsend(message));
				Console.WriteLine("Сообщение: "+ getmessage(send));
				Console.WriteLine("Адресат: "+ getto(send));
				buffer = new byte[256];

			}
			Console.WriteLine("Соединение завершено");
			Console.Read();
			Client.Close();
   
	    }

        static void reg(String username,String password)
        {
            StreamWriter sw = new StreamWriter("1.txt",true, Encoding.UTF8);
            sw.WriteLine("<name>"+username+"</name><pass>"+password+"</pass>");
            sw.Close();
        }





		//Все что ниже - парсинг полученных данных
		public String getname(String input)
		{
            int pos1 = input.IndexOf("<name>");
            int pos2 = input.IndexOf("</name>");
            return input.Substring(pos1 + 6, pos2 - pos1 - 6);
		}
        public String getpass(String input)
        {
            int pos1 = input.IndexOf("<pass>");
            int pos2 = input.IndexOf("</pass>");
            return input.Substring(pos1 + 6, pos2 - pos1 - 6);
        }
		public String getsend(String send)
        {
            int pos1 = send.IndexOf("<send>");
            int pos2 = send.IndexOf("</send>");
			return send.Substring(pos1+6,pos2-pos1-6);
		}		
        public String getmessage (String message)
		{
			int pos = message.IndexOf("</message>");
			return message.Substring(9,pos-9);
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

