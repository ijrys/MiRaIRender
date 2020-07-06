namespace ImageTools {
	public struct ColorRGB8 {
		public byte R;
		public byte G;
		public byte B;
		public ColorRGB8(byte c) {
			R = c;
			G = c;
			B = c;
		}
		public ColorRGB8(byte r, byte g, byte b) {
			R = r;
			G = g;
			B = b;
		}
	}
}
