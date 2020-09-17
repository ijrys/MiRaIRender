using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;
using MiRaIRender.BaseType.Spectrum;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	public interface IMaterialMapAble {
		public Float Lightness(Vector2f position);
	}

	/// <summary>
	/// 贴图接口
	/// </summary>
	/// <typeparam name="LightMod"></typeparam>
	public interface IMaterialMapAble<LightMod> : IMaterialMapAble where LightMod : ISpectrum {
		public LightMod Color(Vector2f position);
	}
}
