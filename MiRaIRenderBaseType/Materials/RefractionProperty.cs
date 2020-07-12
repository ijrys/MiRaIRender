﻿using Float = System.Single;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	public class RefractionProperty {
		private bool _enable = false;
		private Float _refraction;
		private Float _ior = 1.0f;

		public bool Enable {
			get => _enable;
			set => _enable = value;
		}
		public Float Refraction {
			get => _refraction;
			set {
				_refraction = Tools.Clamp(0.0f, 1.0f, value);
			}
		}

		public Float IOR {
			get => _ior;
			set {
				_ior = Tools.Clamp(0.001f, 100f, value);
			}
		}

		public bool EnableMap = false;
		/// <summary>
		/// 强度贴图
		/// </summary>
		public IMaterialGrayMapAble IntensityMap;

		public Float GetRefraction(Vector2f xy) {
			if (!Enable) {
				return 0.0f;
			}
			Float re = Refraction;
			if (EnableMap && (IntensityMap != null)) {
				re *= IntensityMap.Lightness(xy);
			}
			return re;
		}
	}
}
