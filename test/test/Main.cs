<<<<<<< HEAD
using System;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Лабораторная работа #1, Сервис обмена сообщениями");
			StartServer();

		}
		public static void StartServer ()
		{
			Console.WriteLine("Сервер запущен");
			new Server(1235);		
			//Console.WriteLine("Сервер остановлен");
		}
	}
}
=======
using System;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Лабораторная работа #1, Сервис обмена сообщениями");
			StartServer();

		}
		public static void StartServer ()
		{
			Console.WriteLine("Сервер запущен");
			new Server(1235);		
		}
	}
}
>>>>>>> adb03089d6c3267ad52c5c77f20ff16070f3ea95
