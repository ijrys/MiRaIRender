using MiRaIRender.BaseType.Spectrum;
using LightMod = MiRaIRender.BaseType.Spectrum.XYZSpectrum;

namespace MiRaIRender.BaseType.Skybox {
	public interface ISkyBoxAble {
		public LightMod RadiationColor(Ray ray);
		public LightMod ViewColor(Ray ray);
	}
}
