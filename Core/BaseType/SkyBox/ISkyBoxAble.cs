using MiRaIRender.BaseType.Spectrum;

namespace MiRaIRender.BaseType.Skybox {
	public interface ISkyBoxAble {
		public RGBSpectrum SkyColor(Ray ray);
	}
}
