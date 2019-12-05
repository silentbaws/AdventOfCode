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
				// Opcode: ABCDE
				// DE - instruction
				// ABC - Argument 3, 2, 1 positional/value
				//     0 - positional -> argument is a position of a value in code
				//     1 - value -> argument is used as a raw input value

				// Add a leading 0 to opcode if it's too small
				if (opcode.Length == 1) {
					opcode = "0" + opcode;
				}

				// Create refs and shit -> this is way easier in c++ but whatever
				// Kind of annoying I can't initialize them to null
				ref int arg1 = ref program[position];
				ref int arg2 = ref program[position];
				ref int arg3 = ref program[position];

				// Set the values based on if they're positional or value based
				if (position + 1 < program.Length)
					arg1 = ref (opcode.Length > 2 && opcode[opcode.Length - 3].ToString() == "1" ? ref program[position + 1] : ref (program[position + 1] < program.Length ? ref program[program[position + 1]] : ref program[position]));

				if (position + 2 < program.Length)
					arg2 = ref (opcode.Length > 3 && opcode[opcode.Length - 4].ToString() == "1" ? ref program[position + 2] : ref (program[position + 2] < program.Length ? ref program[program[position + 2]] : ref program[position]));

				if (position + 3 < program.Length && program[position + 3] < program.Length)
					arg3 = ref program[program[position + 3]];

				switch (Int32.Parse(opcode[opcode.Length - 2].ToString() + opcode[opcode.Length-1].ToString())) {
					case 1:
						arg3 = arg1 + arg2;
						position += 3;
						break;
					case 2:
						arg3 = arg1 * arg2;
						position += 3;
						break;
					case 3:
						string input = Console.In.ReadLine();
						arg1 = Int32.Parse(input);
						position++;
						break;
					case 4:
						Console.WriteLine(arg1);
						position++;
						break;
					case 5:
						position = arg1 != 0 ? arg2 - 1 : position + 2;
						break;
					case 6:
						position = arg1 == 0 ? arg2 - 1 : position + 2;
						break;
					case 7:
						arg3 = arg1 < arg2 ? 1 : 0;
						position += 3;
						break;
					case 8:
						arg3 = arg1 == arg2 ? 1 : 0;
						position += 3;
						break;
					case 99:
						position = program.Length;
						Console.WriteLine("HALT");
						break;
					default:
						position = program.Length;
						break;
				}
			}
		}
	}
}
