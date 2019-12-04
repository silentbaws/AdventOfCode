using System;
using System.Collections.Generic;
using System.Drawing;
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
		static List<Line> InputToCircuit(string input, out Rectangle bounds) {
			string[] lines = input.Split(',');

			Point currentPoint = new Point(0, 0);

			List<Line> circuit = new List<Line>();

			bounds = new Rectangle(0, 0, 0, 0);

			foreach(string line in lines) {
				Line newLine = new Line(new Point(currentPoint), new Point(currentPoint));
				switch (line[0]) {
					case 'R':
						currentPoint.x += Int32.Parse(line.Remove(0, 1));
						newLine.p2 = new Point(currentPoint);
						if (currentPoint.x > bounds.Width) {
							bounds.Width = currentPoint.x;
						}
						break;
					case 'L':
						currentPoint.x -= Int32.Parse(line.Remove(0, 1));
						newLine.p2 = new Point(currentPoint);
						if(currentPoint.x < bounds.X) {
							bounds.X = currentPoint.x;
						}
						break;
					case 'U':
						currentPoint.y += Int32.Parse(line.Remove(0, 1));
						newLine.p2 = new Point(currentPoint);
						if (currentPoint.y > bounds.Height) {
							bounds.Height = currentPoint.y;
						}
						break;
					case 'D':
						currentPoint.y -= Int32.Parse(line.Remove(0, 1));
						newLine.p2 = new Point(currentPoint);
						if (currentPoint.y < bounds.Y) {
							bounds.Y = currentPoint.y;
						}
						break;
				}
				circuit.Add(newLine);
			}

			return circuit;
		}

		static void Main(string[] args) {
			StreamReader reader = new StreamReader("Input.txt");

			string[] circuits = reader.ReadToEnd().Split('\n');

			Rectangle circuit1Bounds, circuit2Bounds;

			List<Line> circuit1 = InputToCircuit(circuits[0], out circuit1Bounds);
			List<Line> circuit2 = InputToCircuit(circuits[1], out circuit2Bounds);

			Rectangle bounds = new Rectangle(Math.Min(circuit1Bounds.X, circuit2Bounds.X) - 10, Math.Min(circuit1Bounds.Y, circuit2Bounds.Y) - 10,
				Math.Max(circuit1Bounds.Width, circuit2Bounds.Width) + 10, Math.Max(circuit1Bounds.Height, circuit2Bounds.Height) + 10);

			List<IntersectionPoint> intersections = new List<IntersectionPoint>();

			Bitmap drawShit = new Bitmap(bounds.Width - bounds.X, bounds.Height - bounds.Y);
			var graphics = Graphics.FromImage(drawShit);

			graphics.DrawEllipse(new Pen(Color.FromArgb(0, 255, 0), 10), new Rectangle(-bounds.X - 100, -bounds.Y - 100, 200, 200));

			int circuit1Steps = 0;
			foreach(Line line1 in circuit1) {
				graphics.DrawLine(new Pen(Color.FromArgb(255, 0, 0), 10), line1.p1.x - bounds.X, line1.p1.y - bounds.Y, line1.p2.x - bounds.X, line1.p2.y - bounds.Y);

				int circuit2Steps = 0;
				foreach(Line line2 in circuit2) {
					if(line1 == circuit1[0]) {
						graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 255), 10), line2.p1.x - bounds.X, line2.p1.y - bounds.Y, line2.p2.x - bounds.X, line2.p2.y - bounds.Y);
					}

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

			//Find the closest intersections to the origin by steps and manhattan distance
			IntersectionPoint closestSteps = new IntersectionPoint(0, 0);
			IntersectionPoint closestManhattan = new IntersectionPoint(0, 0);
			foreach (IntersectionPoint point in intersections) {
				if(closestSteps.steps > point.steps || closestSteps.steps == 0) {
					closestSteps = point;
				}
				if(Math.Abs(closestManhattan.x) + Math.Abs(closestManhattan.y) > Math.Abs(point.x) + Math.Abs(point.y) || closestManhattan.Equals(new Point(0, 0))) {
					closestManhattan = point;
				}
			}

			// Draw intersection areas
			graphics.DrawEllipse(new Pen(Color.FromArgb(255, 242, 0), 10), closestManhattan.x - bounds.X - 100, closestManhattan.y - bounds.Y - 100, 200, 200);
			graphics.DrawEllipse(new Pen(Color.FromArgb(43, 177, 76), 10), closestSteps.x - bounds.X - 100, closestSteps.y - bounds.Y - 100, 200, 200);

			drawShit.Save("lmao.jpg");

			Console.WriteLine(closestSteps.steps);
		}
	}
}
