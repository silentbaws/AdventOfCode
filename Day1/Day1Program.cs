using System;
using System.IO;

//Problem Set https://adventofcode.com/2019/day/1

namespace Day1 {
	class Day1Program {
		static void Main(string[] args) {
			StreamReader reader = new StreamReader("Input.txt");

			int totalFuel = 0;

			string input = reader.ReadLine();
			while(input != null){
				int fuelRequired = (int)Math.Floor(float.Parse(input) / 3f) - 2;
				while(fuelRequired > 0) {
					totalFuel += fuelRequired;
					fuelRequired = (int)Math.Floor((float)fuelRequired / 3f) - 2;
				}
				input = reader.ReadLine();
			}

			Console.WriteLine(totalFuel);
		}
	}
}
