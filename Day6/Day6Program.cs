using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6 {
	class Orbit {
		public Orbit orbiting;

		public string name = "";

		public Orbit(string Name) {
			name = Name;
		}
	}

	class Day6Program {
		static void Main(string[] args) {
			StreamReader reader = new StreamReader("Input.txt");

			string programInput = reader.ReadToEnd();

			string[] orbits = programInput.Split("\n");

			Dictionary<string, Orbit> orbitObjects = new Dictionary<string, Orbit>();

			Orbit santaOrbit = null, youOrbit = null;

			foreach (string orbit in orbits) {
				string[] bodies = orbit.Trim().Split(")");
				Orbit orbit1 = null, orbit2 = null;
				orbitObjects.TryGetValue(bodies[0], out orbit1);
				orbitObjects.TryGetValue(bodies[1], out orbit2);

				if (orbit1 == null) {
					orbit1 = new Orbit(bodies[0]);
					orbitObjects.Add(orbit1.name, orbit1);
				}
				if (orbit2 == null) {
					orbit2 = new Orbit(bodies[1]);
					orbitObjects.Add(orbit2.name, orbit2);
				}

				if (orbit2.name.Equals("YOU")) {
					youOrbit = orbit2;
				}else if (orbit2.name.Equals("SAN")) {
					santaOrbit = orbit2;
				}

				orbit2.orbiting = orbit1;
			}

			int totalOrbits = 0;
			foreach (KeyValuePair<string, Orbit> orbit in orbitObjects) {
				Orbit parent = orbit.Value.orbiting;
				while (parent != null) {
					totalOrbits++;
					parent = parent.orbiting;
				}
			}

			Console.WriteLine("Total Orbits: " + totalOrbits);

			if (santaOrbit != null && youOrbit != null) {
				int steps = 0;
				Orbit youOrbiting = youOrbit.orbiting;

				int youSteps = 0;
				while (youOrbiting != null) {
					Orbit santaOrbiting = santaOrbit.orbiting;
					int santaSteps = 0;
					while (santaOrbiting != null) {
						if (santaOrbiting == youOrbiting) {
							steps = santaSteps + youSteps;
							break;
						}
						santaOrbiting = santaOrbiting.orbiting;
						santaSteps++;
					}
					youOrbiting = youOrbiting.orbiting;
					youSteps++;

					if (steps != 0)
						break;
				}

				Console.WriteLine("Minimum orbital transfers: " + steps);
			}
		}
	}
}
