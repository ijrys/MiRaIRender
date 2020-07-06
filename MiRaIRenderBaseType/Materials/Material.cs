using Float = System.Single;

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
		public IMaterialMapAble BaseColor= new PureColorMaterialMap();

		/// <summary>
		/// 法线贴图【未启用】
		/// </summary>
		public IMaterialMapAble NormalMap = null;

		/// <summary>
		/// 表面粗糙度
		/// </summary>
		public Float Roughness = 1.0f;

		/// <summary>
		/// 金属感
		/// </summary>
		public MetallicProperty Metallic = new MetallicProperty();

		/// <summary>
		/// 透射率
		/// </summary>
		public Float Specular = 0.0f;

		public LightProperty Light = new LightProperty();
	}
}
