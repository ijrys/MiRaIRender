using System;
using Math = System.MathF;

using Float = System.Single;

namespace MiRaIRender.BaseType {
	public static class Tools {
		public const Float EPSILON = 0.000001f;

		public const Float PI = 3.1415926535897931f;

		public static bool FloatClosely (Float a, Float b, Float di) {
			return (b > a - di &&
				b < a + di);
		}

		public static Float GetRadianByAngle (Float angle) {
			return (angle % 360) * PI / 180.0f;
		}

		/// <summary>
		/// schlick反射率
		/// </summary>
		/// <param name="f0"></param>
		/// <param name="cosa"></param>
		/// <returns></returns>
		public static Float Schlick (Float f0, Float cosa) {
			Float c = cosa;
			cosa *= cosa;
			cosa *= cosa;
			cosa *= c;
			return f0 + (1.0f - f0) * cosa;
		}

		public static Vector3f RandomPointInSphere () {
			Random r = new Random();
			double x;
			double y;
			double z;
			do {
				x = r.NextDouble();
				y = r.NextDouble();
				z = r.NextDouble();
			} while (x * x + y * y + z * z <= 1.0);

			return new Vector3f((Float)x, (Float)y, (Float)z);
		}
	}
}
