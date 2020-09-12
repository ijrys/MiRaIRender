using System;
using System.Collections.Generic;
using System.Text;

namespace MiRaIRender.BaseType.SceneObject {
	public abstract class SceneObjectA:RenderObject {
		private Materials.Material _material = new Materials.Material();

		public Materials.Material Material {
			get => _material;
			set {
				_material = value;
			}
		}
	}
}
