using Float = System.Single;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRenderBaseType.Materials {

	/// <summary>
	/// 材质
	/// </summary>
	public class Material {
		/// <summary>
		/// 基础色
		/// </summary>
		public IMaterialMapAble BaseColor;

		/// <summary>
		/// 法线贴图【未启用】
		/// </summary>
		public IMaterialMapAble NormalMap;

		/// <summary>
		/// 表面粗糙度
		/// </summary>
		public Float Roughness;

		/// <summary>
		/// 金属感
		/// </summary>
		public MetallicProperty Metallic;

		/// <summary>
		/// 透射率
		/// </summary>
		public Float Specular;

		public LightProperty Light;
	}
}
