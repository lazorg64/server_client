using System;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace Client
{
   
	class MainClass
	{
        
		static TcpClient client;
		public static void Main (string[] args)
		{//первое окошко


            Console.WriteLine("Клиент запущен!");
            client = new TcpClient("127.0.0.1", 1235);
            Console.WriteLine("Зарегистрироваться --- 1");
            Console.WriteLine("Войти ---------------- 2");
            Console.WriteLine("Выйти ---------------- 3");
            Console.WriteLine("Ваш выбор :");
            int caseSwitch = Convert.ToInt32(Console.ReadLine());
            switch (caseSwitch)
            {
                case 1:
                string login,passw = "empty";
                Console.WriteLine("Логин :");
                login = Console.ReadLine();
                reg(login,passw);
                act();
                break;

                case 2:
                string loginname,password = "empty";
                Console.WriteLine("Логин :");
                loginname = Console.ReadLine();
                // Console.WriteLine("Пароль :");
                // password = Console.ReadLine();
                auth(loginname,password);//логинимся

                act();

                break;

                case 3:
                Console.WriteLine("Досвидания!");
                break;

            };
            //auth("Client1","password");
			//message("MY LITTLE message","Client2");
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
                Console.WriteLine("Выйти ----------------  2 ");

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
                        flag = true;
                        break;


                }
                Console.WriteLine("\n Нажмите любую клавишу...");
                Console.ReadLine();
                if (flag)
                    break;
            }
            
        }




        static void reg(String username,String password)
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

		static void stop ()
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


		static void auth (String name, String password)
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

		static void message(String message,String to) 
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
        /*
        static void Reg()
        {
            string loginreg, passwordreg;
            
             * // Открываем поток дял записи в файл с кодировкой
            // по умолчанию
            StreamWriter sw = new StreamWriter("C:/Users/Lenovo/Documents/Visual Studio 2010/Projects/laba1/Client/1.txt", 
                true, Encoding.UTF8);
            //string line;
            do
            {
                // Считываем строку с клавиатуры
                Console.WriteLine("Логин :");
                loginreg = Console.ReadLine();
                sw.WriteLine(loginreg);// Записываем строку в файл
                Console.WriteLine("Пароль :");
                passwordreg = Console.ReadLine();
                sw.WriteLine(passwordreg);
            }
            while ((loginreg == "") || (passwordreg == ""));
            // Закрываем поток
            sw.Close();
              
             
            
        }
         */

    }
	
}
