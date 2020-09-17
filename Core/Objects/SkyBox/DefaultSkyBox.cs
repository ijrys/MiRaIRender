using MiRaIRender.BaseType;
using MiRaIRender.BaseType.Skybox;
using MiRaIRender.BaseType.Spectrum;

namespace MiRaIRender.Objects.Skybox {
	public class DefaultSkyBox : ISkyBoxAble {
		private XYZSpectrum _c;
		public XYZSpectrum Color {
			get => _c;
			set  { _c = value; }
		}

		public DefaultSkyBox () {
			_c = new RGBSpectrum(0.1f, 0.1f, 0.1f).ToXYZ();
		}
		public DefaultSkyBox(XYZSpectrum c) {
			_c = c;
		}

		public static DefaultSkyBox Default = new DefaultSkyBox();


		public XYZSpectrum RadiationColor(Ray ray) {
			return _c;
		}

		public XYZSpectrum ViewColor(Ray ray) {
			return _c;
		}
	}
}
