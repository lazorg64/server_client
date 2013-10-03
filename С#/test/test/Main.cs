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
