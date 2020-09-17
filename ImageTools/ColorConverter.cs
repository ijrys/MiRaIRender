using MiRaIRender.BaseType;
using System;
using System.Security.Cryptography;
using MiRaIRender.BaseType.Spectrum;
using Math = System.MathF;
using Float = System.Single;

namespace ImageTools {

	public static class ColorConverter {
		private static readonly Float Idx22 = 1.0f / 2.2f;
		public static ColorRGB8 GetColor8Gamma22(RGBSpectrum color) {
			Float fr = Math.Pow(color.R, Idx22) * 255.0f;
			Float fg = Math.Pow(color.G, Idx22) * 255.0f;
			Float fb = Math.Pow(color.B, Idx22) * 255.0f;
			byte r, g, b;
			if (fr < 0) { r = 0; }
			else if (fr > 255) { r = 255; }
			else { r = (byte)fr; }

			if (fg < 0) { g = 0; }
			else if (fg > 255) { g = 255; }
			else { g = (byte)fg; }

			if (fb < 0) { b = 0; }
			else if (fb > 255) { b = 255; }
			else { b = (byte)fb; }

			return new ColorRGB8(r, g, b);
		}
		public static ColorRGB8 GetColor8(RGBSpectrum color) {
			color *= 255;
			byte r, g, b;
			if (color.R < 0) { r = 0; }
			else if (color.R > 255) { r = 255; }
			else { r = (byte)color.R; }

			if (color.G < 0) { g = 0; }
			else if (color.G > 255) { g = 255; }
			else { g = (byte)color.G; }

			if (color.B < 0) { b = 0; }
			else if (color.B > 255) { b = 255; }
			else { b = (byte)color.B; }

			return new ColorRGB8(r, g, b);
		}

		public static ColorRGB8[,] ConvertToRGB8Image(RGBSpectrum[,] img) {
			int imgh = img.GetLength(0), imgw = img.GetLength(1);
			ColorRGB8[,] re = new ColorRGB8[imgh, imgw];
			for (int j = 0; j < imgh; j++) {
				for (int i = 0; i < imgw; i++) {
					re[j, i] = GetColor8Gamma22(img[j, i]);
				}
			}
			return re;
		}
		public static ColorRGB8[,] ConvertToRGB8Image(XYZSpectrum[,] img) {
			int imgh = img.GetLength(0), imgw = img.GetLength(1);
			ColorRGB8[,] re = new ColorRGB8[imgh, imgw];
			for (int j = 0; j < imgh; j++) {
				for (int i = 0; i < imgw; i++) {
					re[j, i] = GetColor8Gamma22(img[j, i].ToRGB());
				}
			}
			return re;
		}

		public static ColorRGB8[,] ConvertToRGB8Image(Float[,] img) {
			int imgh = img.GetLength(0), imgw = img.GetLength(1);
			ColorRGB8[,] re = new ColorRGB8[imgh, imgw];
			for (int j = 0; j < imgh; j++) {
				for (int i = 0; i < imgw; i++) {
					byte g = (byte)(Tools.Clamp(img[j, i] * 255, 0.0f, 10.0f));
					re[j, i] = new ColorRGB8(g);
				}
			}
			return re;
		}

		public static ColorRGB8[,] ConvertToRGB8Image(int[,] data, int maxvalue) {
			int imgh = data.GetLength(0), imgw = data.GetLength(1);
			ColorRGB8[,] re = new ColorRGB8[imgh, imgw];
			float mvalue = 1.0f / maxvalue;
			for (int j = 0; j < imgh; j++) {
				for (int i = 0; i < imgw; i++) {
					byte b = (byte)(Tools.Clamp(data[j, i] * mvalue, 0.0f, 1.0f) * 255);
					re[j, i] = new ColorRGB8(b);
				}
			}
			return re;
		}
	}
}
