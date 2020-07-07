//using ImageTools;
//using MiRaIRender.BaseType;
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

			//Color[,] img = new Color[16, 32];
			//for (int h = 0; h < 16; h ++) {
			//	for (int w = 0; w < 32; w ++) {
			//		float rg = h / 16.0f;
			//		float b = w / 32.0f;
			//		img[h, w] = new Color(rg, rg, b);
			//	}
			//}

			//ColorRGB8[,] simg = ImageTools.ColorConverter.ConvertToRGB8Image(img);
			//ImageTools.ImageSave.ImageSave_PPM(simg, "A:\\testimg.ppm");

			int[] nmbs = new int[10];
			Random r = new Random();
			for (int i = 0; i < nmbs.Length; i ++) {
				nmbs[i] = r.Next(100);
			}
			for (int i = 0; i < nmbs.Length; i++) {
				Console.Write($"{nmbs[i]}\t");
			}
			Console.WriteLine();
			//int sid = QuickCoordation(nmbs.AsSpan());
			//Console.WriteLine(sid);
			//for (int i = 0; i < nmbs.Length; i++) {
			//	Console.Write($"{nmbs[i]}\t");
			//}
			//Console.WriteLine();

			Span<int> t = nmbs.AsSpan();
			Span<int> lspan = t.Slice(0, 5);
			Span<int> rspan = t.Slice(5);
			foreach (var item in lspan) {
				Console.Write($"{item}\t");
			}
			Console.WriteLine();
			foreach (var item in rspan) {
				Console.Write($"{item}\t");
			}
			Console.WriteLine();
		}
	}
}
