using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
 
namespace ClientNamespace
{
   
	class MainClass
	{
		private static TcpClient MyClient;
		private static Byte [] ReadBuff;
        private static String Username;
        private static List<Message> MessageBuffer = new List<Message>();
        private static bool Active;
        private static bool Auth;
		public static void Main (string[] args)
		{
            Auth = false;
            Console.WriteLine("Укажите адрес сервера:");
            String server_addr = "127.0.0.1";//= Console.ReadLine();
            Console.Clear();
            MyClient = new TcpClient(server_addr, 1235);
            printMenu();
			startListening();
            int caseSwitch = Convert.ToInt32(Console.ReadLine());
            string login, password;
            switch (caseSwitch)
            {
                case 1: 
                    Console.Write("Логин: ");
                    login = Console.ReadLine();
                    Username = login;
                    Console.Write("Пароль: ");
                    password = Console.ReadLine();
                    registerUser(login, password);
					act ();
                break;

                case 2:
                    Console.WriteLine("Логин: ");
                    login = Console.ReadLine();
                    Username = login;
                    Console.WriteLine("Пароль: ");
                    password = Console.ReadLine();
                    auth(login,password);		
					act();

                break;

                case 3:
                    Console.WriteLine("Досвидания!");
                break;

            };
			finish();
		}


        public static void printMenu()
        {
            Console.WriteLine("Зарегистрироваться --- [1]");
            Console.WriteLine("Войти ---------------- [2]");
            Console.WriteLine("Выйти ---------------- [3]");
            Console.WriteLine("Ваш выбор :");
        }

        public static void act()
        {
                Active = true;
                while (Active)
                {
                    Thread.Sleep(10);
                        Console.Clear();
                        Console.WriteLine("Здравствуйте " + Username + "!");
                        Console.WriteLine("Отправить сообщение -------------- [1]");
                        Console.WriteLine("Посмотреть входящие сообщения ---- [2]");
                        Console.WriteLine("Выйти ---------------------------- [3]");
                        Console.WriteLine("");

                        int caseSwitch = Convert.ToInt32(Console.ReadLine());
                        //Console.Clear();
                        switch (caseSwitch)
                        {
                            case 1:
                                Console.WriteLine("Кому отправить сообщение ?");
                                string name, mes;
                                name = Console.ReadLine();
                                Console.WriteLine("Сообщение :");
                                mes = Console.ReadLine();
                                sendMessage(mes, name);
                                Console.WriteLine("Cообщение отправленно " + name + "");
                                break;
                            case 2:
                                readMessages();
                                break;
                            case 3:
                                Active = false;
                                break;
                            default:
								break;
                        }
                    }
                Console.WriteLine("Для завершения работы нажмите любую клавишу...");
                Console.ReadLine();
                
        }
        
        public static void startListening () {
			NetworkStream stream = MyClient.GetStream ();
			ReadBuff = new Byte[512];
			stream.BeginRead (ReadBuff, 0, ReadBuff.Length, new AsyncCallback (listenerCallback), MyClient);
        }

		public static void listenerCallback (IAsyncResult ar)
		{
            Thread.Sleep(10);
			TcpClient cli = (TcpClient)ar.AsyncState;
			cli.GetStream().EndRead(ar);
			String str1 = Encoding.UTF8.GetString(ReadBuff);
            if (str1.IndexOf("<auth>OK</auth>") != -1)
            {
                Console.WriteLine("Успешная авторизация!");
                Auth = true;
            }
            if (str1.IndexOf("<message>") != -1)
            {
                Console.WriteLine("Получено входящее сообщение!");
                MessageBuffer.Add(new Message(Parsing.getfrom(str1), Parsing.getto(str1), Parsing.getmessage(str1)));
            }
            if(Active)
			cli.GetStream().BeginRead(ReadBuff, 0, ReadBuff.Length,new AsyncCallback(listenerCallback) ,cli);
		}
        
		public static void readMessages ()
		{
            Console.Clear();
            Console.WriteLine("----- Входящие сообщения ------------");
            Console.WriteLine("");
            foreach (Message m in MessageBuffer)
            {
                Console.Write("Отправитель: ["+m.getfrom()+"]    ");
                Console.Write("Адресат: [" + m.getfrom()+"]\n");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(m.getmessage());
                Console.WriteLine("-------------------------------------");
            }
            Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
            Console.ReadLine();
            
		}

        public static void registerUser(String username,String password)
        {
            try
            {
                Byte[] data = new Byte[512];
                data = System.Text.Encoding.UTF8.GetBytes("<reg><name>" + username + "</name><pass>"+password+"</pass></reg>");
                NetworkStream stream = MyClient.GetStream();
                stream.Write(data, 0, data.Length);


            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

		public static void finish()
		{
		     try {
		     Byte[] data = System.Text.Encoding.UTF8.GetBytes("<end>");         
		     NetworkStream stream = MyClient.GetStream();
		     stream.Write(data, 0, data.Length);
		     } 
		     catch (ArgumentNullException e) {
		     Console.WriteLine("ArgumentNullException: {0}", e);
		     } 
		     catch (SocketException e) {
		     Console.WriteLine("SocketException: {0}", e);
		     }
		     Console.WriteLine("\nДля выхода нажмите любую клавишу...");
             Console.ReadLine();
		}


		public static void auth (String name, String password)
		{
		     try{
                 Byte[] data = new Byte[512];
                data = System.Text.Encoding.UTF8.GetBytes("<login><name>"+name+"</name><pass>"+password+"</pass><login>");         
		     NetworkStream stream = MyClient.GetStream();
		     stream.Write(data, 0, data.Length);

		     } 
		     catch (ArgumentNullException e) {
		     Console.WriteLine("ArgumentNullException: {0}", e);
		     } 
		     catch (SocketException e) {
		     Console.WriteLine("SocketException: {0}", e);
		     }
        }

		public static void sendMessage(String message,String to) 
		{
		 try 
		  {
		    Byte[] data = System.Text.Encoding.UTF8.GetBytes("<send><message>"+message+"</message><to>"+to+"</to></send>");         
		    NetworkStream stream = MyClient.GetStream();
		    stream.Write(data, 0, data.Length);      
		             
		  } 
		  catch (ArgumentNullException e) 
		  {
		    Console.WriteLine("ArgumentNullException: {0}", e);
		  } 
		  catch (SocketException e) 
		  {
		    Console.WriteLine("SocketException: {0}", e);
		  }
		}
    }
}
