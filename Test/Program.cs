//using ImageTools;
using MiRaIRender.BaseType;
using System;
using Float = System.Single;
namespace Test {
	class Program {
		public static Float GetRadianByAngle(Float angle) {
			return (angle % 360) * MathF.PI / 180.0f;
		}
		private static int QuickCoordation(Span<int> objs) {
			int len = objs.Length;
			int aimid = len / 2;
			int s = 0, e = len, f = 1, t = len - 1;
			while (s < e - 1) {
				f = s + 1;
				t = e - 1;
				while (f < t) {
					// 前找大
					while (f < t) {
						if (objs[f] > objs[s]) {
							break;
						}
						f++;
					}
					// 后找小
					while (f < t) {
						if (objs[t] < objs[s]) {
							break;
						}
						t--;
					}
					// 交换
					if (f != t) {
						var tmp = objs[f];
						objs[f] = objs[t];
						objs[t] = tmp;
					}
				}
				if (objs[f] > objs[s]) {
					f--;
				}
				if (f != s) {
					var tmp = objs[f];
					objs[f] = objs[s];
					objs[s] = tmp;
				}
				if (f == aimid) {
					return aimid;
				}
				if (f < aimid) {
					s = f + 1;
				}
				else {
					e = f;
				}
			}
			return aimid;
		}
		static void Main(string[] args) {
			//TrigleFace face = new TrigleFace(
			//	new Vector3f(-1, -1, -1),
			//	new Vector3f(0, 1, -1),
			//	new Vector3f(1, -1, -1));
			//Vector3f normal = face.e1 ^ face.e2;
			//face.n0 = face.n1 = face.n2 = normal;

			//Ray ray = new Ray(new Vector3f(), new Vector3f(0, 0, -1));

			//RayCastResult rcr = face.Intersection(ray);

			//Console.WriteLine(rcr.happened) ;

			Vector3f v1 = new Vector3f(1f, 1f, 1f);
			Vector3f v2 = new Vector3f(2.001f, 2.001f, 2.001f);
			Vector3f v3 = v1 ^ v2;
			Console.WriteLine(v3);

			Console.ReadLine();
		}
	}
}
