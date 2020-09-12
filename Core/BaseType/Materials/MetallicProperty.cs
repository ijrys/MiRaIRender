using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;
using MiRaIRender.BaseType.Spectrum;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	/// <summary>
	/// 金属属性
	/// </summary>
	public class MetallicProperty {

		/// <summary>
		/// 最大金属感
		/// </summary>
		public Float Metallic = 0.2f;

		///// <summary>
		///// 是否启用强度贴图
		///// </summary>
		//public bool EnableMap = false;

		/// <summary>
		/// 强度贴图
		/// </summary>
		public IMaterialGrayMapAble IntensityMap = null; //new PureGrayMaterialMap() { BaseGray = 0.2f };

		/// <summary>
		/// 获取金属度
		/// </summary>
		/// <param name="xy"></param>
		/// <returns></returns>
		public Float GetMetallic(Vector2f xy) {
			Float re = Metallic;
			if (IntensityMap != null) {
				re *= IntensityMap.Lightness(xy);
			}
			return re;
		}

		/// <summary>
		/// 金属吸收色贴图
		/// </summary>
		public IMaterialMapAble MetallicColorMap = null;

		/// <summary>
		/// 获取金属吸收色
		/// </summary>
		/// <param name="xy"></param>
		/// <returns></returns>
		public RGBSpectrum GetMetallicColor(Vector2f xy) {
			if (MetallicColorMap == null) {
				return new RGBSpectrum(0.9f);
			}
			return MetallicColorMap.Color(xy);
		}
	}
}
