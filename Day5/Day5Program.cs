using System;
using System.IO;

namespace Day5 {
	class Day5Program {
		static void Main(string[] args) {
			StreamReader reader = new StreamReader("Input.txt");

			string programInput = reader.ReadToEnd();

			int[] programDefault = Array.ConvertAll(programInput.Split(','), s => Int32.Parse(s));

			int[] program = new int[programDefault.Length];

			Array.Copy(programDefault, program, programDefault.Length);

			for (int position = 0; position < program.Length; position ++) {
				string opcode = program[position].ToString();
				if (opcode.Length == 1) {
					opcode = "0" + opcode;
				}
				switch (Int32.Parse(opcode[opcode.Length - 2].ToString() + opcode[opcode.Length-1].ToString())) {
					case 1:
						program[program[position + 3]] = (opcode.Length > 2 && opcode[opcode.Length-3].ToString() == "1" ? program[position + 1] : program[program[position + 1]]) + (opcode.Length > 3 && opcode[opcode.Length - 4].ToString() == "1" ? program[position + 2] : program[program[position + 2]]);
						position += 3;
						break;
					case 2:
						program[program[position + 3]] = (opcode.Length > 2 && opcode[opcode.Length - 3].ToString() == "1" ? program[position + 1] : program[program[position + 1]]) * (opcode.Length > 3 && opcode[opcode.Length - 4].ToString() == "1" ? program[position + 2] : program[program[position + 2]]);
						position += 3;
						break;
					case 3:
						string input = Console.In.ReadLine();
						program[program[position + 1]] = Int32.Parse(input);
						position++;
						break;
					case 4:
						Console.WriteLine(program[program[position + 1]]);
						position++;
						break;
					case 5:
						if ((opcode.Length > 2 && opcode[opcode.Length - 3].ToString() == "1" ? program[position + 1] : program[program[position + 1]]) != 0) {
							position = (opcode.Length > 3 && opcode[opcode.Length - 4].ToString() == "1" ? program[position + 2] : program[program[position + 2]]) - 1;
						} else {
							position += 2;
						}
						break;
					case 6:
						if ((opcode.Length > 2 && opcode[opcode.Length - 3].ToString() == "1" ? program[position + 1] : program[program[position + 1]]) == 0) {
							position = (opcode.Length > 3 && opcode[opcode.Length - 4].ToString() == "1" ? program[position + 2] : program[program[position + 2]]) - 1;
						} else {
							position += 2;
						}
						break;
					case 7:
						if ((opcode.Length > 2 && opcode[opcode.Length - 3].ToString() == "1" ? program[position + 1] : program[program[position + 1]]) < (opcode.Length > 3 && opcode[opcode.Length - 4].ToString() == "1" ? program[position + 2] : program[program[position + 2]])){
							program[program[position + 3]] = 1;
						} else {
							program[program[position + 3]] = 0;
						}
						position += 3;
						break;
					case 8:
						if ((opcode.Length > 2 && opcode[opcode.Length - 3].ToString() == "1" ? program[position + 1] : program[program[position + 1]]) == (opcode.Length > 3 && opcode[opcode.Length - 4].ToString() == "1" ? program[position + 2] : program[program[position + 2]])){
							program[program[position + 3]] = 1;
						} else {
							program[program[position + 3]] = 0;
						}
						position += 3;
						break;
					case 99:
						position = program.Length;
						Console.Write("HALT");
						break;
					default:
						position = program.Length;
						break;
				}
			}
		}
	}
}
