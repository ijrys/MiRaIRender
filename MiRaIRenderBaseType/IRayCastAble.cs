using System;
using System.Collections.Generic;
using System.Text;

namespace MiRaIRender.BaseType {
	public interface IRayCastAble {
		public Bounds3 BoundBox { get; }
		public RayCastResult Intersection(Ray ray, float nowbest);
	}
}
