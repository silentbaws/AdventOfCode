using System;
using System.Collections.Generic;
using System.IO;

// Problem set https://adventofcode.com/2019/day/3

namespace Day3 {
	class Point {
		public int x, y;

		public Point(int X, int Y) {
			x = X;
			y = Y;
		}

		public Point(Point p1) {
			this.x = p1.x;
			this.y = p1.y;
		}

		public int Distance(Point p2) {
			return (int)Math.Sqrt(Math.Pow(p2.x - x, 2) + Math.Pow(p2.y - y, 2));
		}

		public bool Equals(Point p2) {
			return this.x == p2.x && this.y == p2.y;
		}
	}

	class IntersectionPoint : Point{
		public int steps = 0;

		public IntersectionPoint(int X, int Y) : base(X, Y) {}
		public IntersectionPoint(Point p1) : base(p1) {}
	}

	class Line {
		public Point p1, p2;

		public Line(Point P1, Point P2) {
			p1 = P1;
			p2 = P2;
		}

		public int Length() {
			return (int)Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
		}

		public IntersectionPoint Intersection(Line l2) {
			//This line is vertical
			if(p1.x == p2.x) {
				//Second line is horizontal
				if(l2.p1.y == l2.p2.y) {
					IntersectionPoint intersection = new IntersectionPoint(p1.x, l2.p1.y);
					if(intersection.x >= Math.Min(l2.p1.x, l2.p2.x) && intersection.x <= Math.Max(l2.p1.x, l2.p2.x) && 
						intersection.y >= Math.Min(p1.y, p2.y) && intersection.y <= Math.Max(p1.y, p2.y)) {
						return intersection;
					}
				//Second line is also vertical
				} else {

				}
			//This line is horizontal
			} else if(p1.y == p2.y) {
				//Second line is vertical
				if(l2.p1.x == l2.p2.x) {
					IntersectionPoint intersection = new IntersectionPoint(l2.p1.x, p1.y);
					if (intersection.y >= Math.Min(l2.p1.y, l2.p2.y) && intersection.y <= Math.Max(l2.p1.y, l2.p2.y) &&
						intersection.x >= Math.Min(p1.x, p2.x) && intersection.x <= Math.Max(p1.x, p2.x)) {
						return intersection;
					}
					//Second line is also horizontal
				} else {

				}
			}
			return null;
		}
	}

	class Day3Program {
		static List<Line> InputToCircuit(string input) {
			string[] lines = input.Split(',');

			Point currentPoint = new Point(0, 0);

			List<Line> circuit = new List<Line>();

			foreach(string line in lines) {
				Line newLine = new Line(new Point(currentPoint), new Point(currentPoint));
				switch (line[0]) {
					case 'R':
						currentPoint.x += Int32.Parse(line.Remove(0, 1));
						newLine.p2 = new Point(currentPoint);
						break;
					case 'L':
						currentPoint.x -= Int32.Parse(line.Remove(0, 1));
						newLine.p2 = new Point(currentPoint);
						break;
					case 'U':
						currentPoint.y += Int32.Parse(line.Remove(0, 1));
						newLine.p2 = new Point(currentPoint);
						break;
					case 'D':
						currentPoint.y -= Int32.Parse(line.Remove(0, 1));
						newLine.p2 = new Point(currentPoint);
						break;
				}
				circuit.Add(newLine);
			}

			return circuit;
		}

		static void Main(string[] args) {
			StreamReader reader = new StreamReader("Input.txt");

			string[] circuits = reader.ReadToEnd().Split('\n');

			List<Line> circuit1 = InputToCircuit(circuits[0]);
			List<Line> circuit2 = InputToCircuit(circuits[1]);

			List<IntersectionPoint> intersections = new List<IntersectionPoint>();

			int circuit1Steps = 0;
			foreach(Line line1 in circuit1) {

				int circuit2Steps = 0;
				foreach(Line line2 in circuit2) {
					IntersectionPoint intersection = line1.Intersection(line2);
					if (intersection != null) {
						intersection.steps = circuit1Steps + line1.p1.Distance(intersection);
						intersection.steps += circuit2Steps + line2.p1.Distance(intersection);

						intersections.Add(intersection);
					}
					circuit2Steps += line2.Length();
				}

				circuit1Steps += line1.Length();
			}

			IntersectionPoint closest = new IntersectionPoint(0, 0);
			foreach (IntersectionPoint point in intersections) {
				if(closest.steps > point.steps || closest.steps == 0) {
					closest = point;
				}
			}

			Console.WriteLine(closest.steps);
		}
	}
}
