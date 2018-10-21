using System;
using System.Collections.Generic;

namespace CodingChallenge
{
	/// <summary>
	/// this programm is a simple GUI
	/// </summary>
	internal class Program
	{
		public const string EUR = "EUR";
		public const string GBP = "GBP";
		public const string USD = "USD";

		private static void Main(string[] args)
		{
            Console.WriteLine("Creating Portfolio GUI");
		    Console.WriteLine("Done... (Press a key to close)");
			Console.ReadKey();
		}
	}
}