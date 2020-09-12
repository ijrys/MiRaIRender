using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;
using MiRaIRender.BaseType.Spectrum;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	public interface IMaterialMapAble {
		public RGBSpectrum Color(Vector2f position);
	}

	/// <summary>
	/// 灰度贴图
	/// </summary>
	public interface IMaterialGrayMapAble : IMaterialMapAble {
		public Float Lightness(Vector2f position);
		RGBSpectrum IMaterialMapAble.Color(Vector2f position) {
			Float l = Lightness(position);
			return new RGBSpectrum(l);
		}
	}
}
