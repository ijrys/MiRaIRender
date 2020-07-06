using ImageTools;
using MiRaIRender.BaseType;
using System;
using Float = System.Single;
namespace Test {
	class Program {
		public static Float GetRadianByAngle(Float angle) {
			return (angle % 360) * MathF.PI / 180.0f;
		}
		static void Main(string[] args) {

			Color[,] img = new Color[16, 32];
			for (int h = 0; h < 16; h ++) {
				for (int w = 0; w < 32; w ++) {
					float rg = h / 16.0f;
					float b = w / 32.0f;
					img[h, w] = new Color(rg, rg, b);
				}
			}

			ColorRGB8[,] simg = ImageTools.ColorConverter.ConvertToRGB8Image(img);
			ImageTools.ImageSave.ImageSave_PPM(simg, "A:\\testimg.ppm");
		}
	}
}
