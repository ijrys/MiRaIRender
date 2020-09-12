using MiRaIRender.BaseType.Spectrum;

namespace MiRaIRender.BaseType.Skybox {
	public class DefaultSkyBox : ISkyBoxAble {
		private RGBSpectrum _c;
		public RGBSpectrum Color {
			get => _c;
			set  { _c = value; }
		}

		public DefaultSkyBox () {
			_c = new RGBSpectrum(0.1f, 0.1f, 0.1f);
		}
		public DefaultSkyBox(RGBSpectrum c) {
			_c = c;
		}

		public static DefaultSkyBox Default = new DefaultSkyBox();


		public RGBSpectrum SkyColor(Ray ray) {
			return _c;
		}
	}
}
