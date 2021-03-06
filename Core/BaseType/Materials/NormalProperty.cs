﻿using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;
using MiRaIRender.BaseType.Spectrum;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	public class NormalProperty {
		private bool _enable = false;

		public bool Enable {
			get => _enable;
			set => _enable = value;
		}

		public IMaterialMapAble<RGBSpectrum> NormalMap;

		public Vector3f GetNormal(Vector2f xy) {
			if (!Enable || NormalMap == null) {
				return Vector3f.Zero;
			}
			RGBSpectrum color = NormalMap.Color(xy);
			return Vector3f.Normalize(new Vector3f(color.R - 0.5f, color.G - 0.5f, color.B - 0.5f));
		}
	}
}
