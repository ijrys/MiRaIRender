﻿using MiRaIRender.BaseType.SceneObject;
using System;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 三角形网格对象
	/// </summary>
	public class MashTrigle : SceneObject.SceneObjectA {
		public TrigleFace[] trigles;

		private BVH BVH;

		public void SetTrigles (TrigleFace[] trigles) {
			this.trigles = trigles;
			if (trigles != null && trigles.Length > 0) {
				Bounds3 bounds = trigles[0].BoundBox;
				foreach (var item in trigles) {
					bounds = Bounds3.Union(bounds, item.BoundBox);
					//item.material = _material;
				}
				this._boundBox = bounds;
			} else {
				this._boundBox = new Bounds3();
			}

			CenterPoint = (_boundBox.pMin + _boundBox.pMax) * 0.5f;
			BVH = BVH.Build(trigles.AsSpan());
		}

		/// <summary>
		/// 光线追踪测试
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public override RayCastResult Intersection(Ray ray) {
			// 包围盒测试失败
			if (!_boundBox.Intersection(ray)) {
				return null;
			}

			RayCastResult result = BVH.Intersection(ray);
			if (result != null) {
				result.material = Material;
			}
			return result;
		}

		public override Vector2f UV2XY(Vector2f uv) {
			return new Vector2f();
		}
		public override Vector3f SelectALightPoint(Vector3f rayFrom) {
			throw new NotImplementedException();
		}
	}
}
