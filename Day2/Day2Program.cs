using System;
using System.IO;

// Problem set https://adventofcode.com/2019/day/2

namespace Day2 {
	class Day2Program {
		static void Main(string[] args) {
			StreamReader reader = new StreamReader("Input.txt");

			string programInput = reader.ReadToEnd();

			int[] programDefault = Array.ConvertAll(programInput.Split(','), s => Int32.Parse(s));

			int[] program = new int[programDefault.Length];

			for (int verb = 0; verb < 100 && program[0] != 19690720; verb++) {
				for (int noun = 0; noun < 100; noun++) {
					Array.Copy(programDefault, program, programDefault.Length);

					program[1] = noun;
					program[2] = verb;

					for (int position = 0; position < program.Length; position += 4) {
						switch (program[position]) {
							case 1:
								program[program[position + 3]] = program[program[position + 1]] + program[program[position + 2]];
								break;
							case 2:
								program[program[position + 3]] = program[program[position + 1]] * program[program[position + 2]];
								break;
							case 99:
								position = program.Length;
								break;
							default:
								position = program.Length;
								break;
						}
					}
					if (program[0] == 19690720) {
						Console.WriteLine(100 * noun + verb);
						break;
					}
				}
			}

			Console.WriteLine(program[0]);
		}
	}
}
