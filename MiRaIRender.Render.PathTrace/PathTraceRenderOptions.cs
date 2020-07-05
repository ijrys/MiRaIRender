using MiRaIRender.BaseType;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiRaIRender.Render.PathTrace {
	public class PathTraceRenderOptions : RenderOptions {
		private int _subSampleNumberPerPixel = 8;
		private int _traceDeep = 4;

		public int SubSampleNumberPerPixel { 
			get => _subSampleNumberPerPixel;
			set {
				if (value < 1) { value = 1; }
				else if (value > 0x1000000‬) { value = 0x1000000; }
				_subSampleNumberPerPixel = value;
			}
		}
		public int TraceDeep { 
			get => _traceDeep; 
			set {
				if (value < 1) { value = 1; }
				if (value > 512) { value = 512; }
				_traceDeep = value;
			}
		}
	}
}
