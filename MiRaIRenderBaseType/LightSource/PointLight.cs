using MiRaIRender.BaseType.SceneObject;
using System;
using System.Collections.Generic;
using System.Text;
using Math = System.MathF;

namespace MiRaIRender.BaseType.LightSource {
	public class PointLight: RenderObject {
		private Color _color;

		public Color MaxColor;
		public Color Color {
			get => _color;
			set {
				MaxColor = value * 4 * Math.PI;
				_color = value;
			}
		}
		public Vector3f Position;

		public override RayCastResult Intersection(Ray ray) {
			return new RayCastResult();
		}

		public override Vector2f UV2XY(Vector2f uv) {
			return new Vector2f();
		}

		public override Vector3f SelectALightPoint(Vector3f rayFrom) {
			return Position;
		}
	}
}
