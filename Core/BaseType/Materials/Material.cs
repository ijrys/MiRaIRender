using System.ComponentModel;
using Float = System.Single;
using MiRaIRender.BaseType.Spectrum;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {

	/// <summary>
	/// 材质
	/// </summary>
	public class Material {
		/// <summary>
		/// 基础色
		/// </summary>
		public IMaterialMapAble<XYZSpectrum> BaseColor = new PureXYZColorMaterialMap();

		/// <summary>
		/// 法线贴图【未启用】
		/// </summary>
		public NormalProperty NormalMap = null;

		/// <summary>
		/// 表面粗糙度
		/// </summary>
		public Float Roughness = 1.0f;

		/// <summary>
		/// 金属感
		/// </summary>
		public MetallicProperty Metallic = new MetallicProperty();

		/// <summary>
		/// 反射率，折射率为1-Specular
		/// </summary>
		public Float Specular  { 
			get {
				if (Refraction!= null && Refraction.Enable == true) {
					return 1.0f - Refraction.Refraction;
				}
				return 1.0f;
			}
		}

		/// <summary>
		/// 折射
		/// </summary>
		public RefractionProperty Refraction = new RefractionProperty();

		/// <summary>
		/// 发光
		/// </summary>
		public LightProperty Light = new LightProperty();
	}
}
