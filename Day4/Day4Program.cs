using System;

// Problem set https://adventofcode.com/2019/day/4

namespace Day4 {
	class Day4Program {
		static bool IsValid(int num) {
			string str = num.ToString();

			bool containsDouble = false;

			for(int i = 1; i < str.Length; i++) {
				if(Int32.Parse(str[i - 1].ToString()) > Int32.Parse(str[i].ToString())) {
					return false;
				}
				if(Int32.Parse(str[i - 1].ToString()) == Int32.Parse(str[i].ToString())) {
					bool isTriple = false;
					if (i - 2 >= 0) {
						if (Int32.Parse(str[i - 2].ToString()) == Int32.Parse(str[i].ToString())) {
							isTriple = true;
						}
					}
					if (i + 1 < str.Length) {
						if ((Int32.Parse(str[i + 1].ToString()) == Int32.Parse(str[i].ToString())))
							isTriple = true;
					}
					if (!isTriple)
						containsDouble = true;
				}
			}

			return containsDouble;
		}

		static void Main(string[] args) {
			string[] input = "256310-732736".Split("-");

			int min = Int32.Parse(input[0]);
			int max = Int32.Parse(input[1]);

			int valid = 0;
			for(int i = min; i <= max; i++) {
				if (IsValid(i)) {
					valid++;
				}
			}
			Console.WriteLine(valid);
		}
	}
}
