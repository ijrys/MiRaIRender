using System.IO;
using System.Text;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Drawing;
using System.Drawing;

namespace ImageTools {
	public static class ImageSave {
		public static void ImageSave_PPM(ColorRGB8[,] image, string imgPath) {
			int imgh = image.GetLength(0), imgw = image.GetLength(1);
			FileStream fs = new FileStream(imgPath, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);
			string imghead = $"P6\n{imgw} {imgh}\n255\n";
			byte[] headcont = Encoding.ASCII.GetBytes(imghead);
			bw.Write(headcont);
			bw.Flush();
			for (int i = 0; i < imgh; ++i) {
				for (int j = 0; j < imgw; j++) {
					ColorRGB8 c = image[i, j];
					bw.Write(c.R);
					bw.Write(c.G);
					bw.Write(c.B);
				}
				bw.Flush();
			}

			bw.Flush();
			bw.Close();
			fs.Close();
		}

		public static void ImageSave_PNG(ColorRGB8[,] image, string imgPath) {
			int imgh = image.GetLength(0), imgw = image.GetLength(1);
			//Image<SixLabors.ImageSharp.PixelFormats.Rgb24> img = new Image<SixLabors.ImageSharp.PixelFormats.Rgb24>(imgw, imgh);
			Bitmap img = new Bitmap(imgw, imgh);

			for (int i = 0; i < imgh; ++i) {
				for (int j = 0; j < imgw; j++) {
					ColorRGB8 c = image[i, j];
					img.SetPixel(j, i, Color.FromArgb(c.R, c.G, c.B));
				}
			}
			img.Save(imgPath, System.Drawing.Imaging.ImageFormat.Png);
		}
	}
}
