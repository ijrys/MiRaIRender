using System;
using Float = System.Single;
namespace Test {
	class Program {
		public static Float GetRadianByAngle(Float angle) {
			return (angle % 360) * MathF.PI / 180.0f;
		}
		static void Main(string[] args) {
			
			float a = GetRadianByAngle(45.0f);
			Console.WriteLine(a);
			Console.WriteLine(MathF.Tan(a));
		}
	}
}
