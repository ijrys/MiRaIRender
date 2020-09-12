﻿using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;
using MiRaIRender.BaseType.Spectrum;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	/// <summary>
	/// 发光属性
	/// </summary>
	public class LightProperty {
		/// <summary>
		/// 启用属性
		/// </summary>
		public bool Enable = false;
		/// <summary>
		/// 最大光强
		/// </summary>
		public RGBSpectrum Intensity = RGBSpectrum.Dark;

		/// <summary>
		/// 是否启用强度贴图
		/// </summary>
		public bool EnableMap = false;
		/// <summary>
		/// 强度贴图
		/// </summary>
		public IMaterialMapAble IntensityMap;

		public RGBSpectrum GetLight(Vector2f xy) {
			if (!Enable) {
				return RGBSpectrum.Dark;
			}
			RGBSpectrum re = Intensity;
			if (EnableMap && (IntensityMap != null)) {
				re *= IntensityMap.Color(xy);
			}
			return re;
		}
	}
}