using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
 
namespace Client
{
   
	class MainClass
	{
        
		static TcpClient client;
		static Byte [] newdata;
		public static void Main (string[] args)
		{


            Console.WriteLine("Клиент запущен!");
            Console.WriteLine("Укажите адрес сервера:");
            String server_addr = Console.ReadLine();
            client = new TcpClient(server_addr, 1235);
            Console.WriteLine("Зарегистрироваться --- 1");
            Console.WriteLine("Войти ---------------- 2");
            Console.WriteLine("Выйти ---------------- 3");
            Console.WriteLine("Ваш выбор :");
			newmail();
            int caseSwitch = Convert.ToInt32(Console.ReadLine());
            switch (caseSwitch)
            {
                case 1:
                    string login,passw;
                    Console.WriteLine("Логин: ");
                    login = Console.ReadLine();
                    Console.WriteLine("Пароль: ");
                    passw = Console.ReadLine();
                    reg(login,passw);
					act ();

                break;

                case 2:
                    string loginname,password;
                    Console.WriteLine("Логин: ");
                    loginname = Console.ReadLine();
                    Console.WriteLine("Пароль: ");
                    password = Console.ReadLine();
                    auth(loginname,password);		
					act();

                break;

                case 3:
                    Console.WriteLine("Досвидания!");
                break;

            };
			stop();
		}


        static void act()
        {
            
                bool flag = false;
                while (true)
                {
                   
                        Console.Clear();
                        Console.WriteLine("Здравствуйте !");
                        Console.WriteLine("");
                        Console.WriteLine("Отправить сообщение --- 1");
                        Console.WriteLine("Просмотр почты -------- 2");
                        Console.WriteLine("Выйти ----------------- 3");
						String str1 = Encoding.UTF8.GetString(newdata);
						if(str1.IndexOf("<message>")!=-1)
						{
						Console.WriteLine("Есть новые сообщения!");
						str1=null;
						newdata = new byte[256];
						}
                        int caseSwitch = Convert.ToInt32(Console.ReadLine());
                        switch (caseSwitch)
                        {
                            case 1:
                                Console.WriteLine("Кому отправить сообщение ?");
                                string name, mes;
                                name = Console.ReadLine();
                                Console.WriteLine("Сообщение :");
                                mes = Console.ReadLine();
                                message(mes, name);
                                Console.WriteLine("Cообщение отправленно " + name + "");
                                break;
                            case 2:
                                Console.WriteLine("Запрос на просмотр почты!");
                                getmail();
                                break;
                            case 3:
                                flag = true;
                                break;
							case 4:
								break;
                        }
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadLine();
                        if (flag)
                            break;

                    }
                
        }
        
        public static void newmail ()
		{

			NetworkStream stream = client.GetStream ();

			newdata = new byte[256];
           
               
			stream.BeginRead (newdata, 0, newdata.Length, new AsyncCallback (myCallBack), client);


            
        }

		public static void myCallBack (IAsyncResult ar)
		{
			TcpClient cli = (TcpClient)ar.AsyncState;
			cli.GetStream().EndRead(ar);
			Console.Write("#");

			String str1 = Encoding.UTF8.GetString(newdata);
			Console.WriteLine(str1);
			//Byte[] data = new byte[256];  


			newdata = new byte[256];
		

			cli.GetStream().BeginRead(newdata, 0, newdata.Length,new AsyncCallback(myCallBack) ,cli);
		}
        
		public static void getmail ()
		{
		    Byte[] data = System.Text.Encoding.UTF8.GetBytes("<read>");         
		    NetworkStream stream = client.GetStream();
		    stream.Write(data, 0, data.Length);

            String answ;
            while (true)
            {
                answ = null;
                data = new byte[256];
                stream.Read(data, 0, data.Length);
                answ = Encoding.UTF8.GetString(data);
                Console.WriteLine(answ);
                if (answ.IndexOf("</read>") != -1)
                    break;
            }
            
		}

		public static bool getcheck ()
		{
			Byte[] buffer = new byte[256];
			client.GetStream ().Read (buffer, 0, buffer.Length);
			String answ = Encoding.UTF8.GetString (buffer);
			Console.WriteLine (answ);
			if (answ.IndexOf("<auth>OK</auth>")!=-1) {
				return true;
			} else {
				return false;
			}

		}

        public static void reg(String username,String password)
        {
            try
            {
                Byte[] data = System.Text.Encoding.UTF8.GetBytes("<reg><name>" + username + "</name><pass>"+password+"</pass></reg>");
                NetworkStream stream = client.GetStream();
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

		public static void stop ()
		{
		  try 
		  {
		    Byte[] data = System.Text.Encoding.UTF8.GetBytes("<end>");         
		    NetworkStream stream = client.GetStream();
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

		  Console.WriteLine("\n Для выхода нажмите любую клавишу...");
          Console.ReadLine();
		}


		public static void auth (String name, String password)
		{

		  try 
		  {
		    Byte[] data = System.Text.Encoding.UTF8.GetBytes("<login><name>"+name+"</name><pass>"+password+"</pass><login>");         
		    NetworkStream stream = client.GetStream();
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

		public static void message(String message,String to) 
		{
		 try 
		  {
		    Byte[] data = System.Text.Encoding.UTF8.GetBytes("<send><message>"+message+"</message><to>"+to+"</to></send>");         
		    NetworkStream stream = client.GetStream();
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
