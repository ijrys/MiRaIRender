using System;
using System.Collections.Generic;
using System.Text;
using Float = System.Single;

namespace MiRaIRender.BaseType.Spectrum {
	public interface ISpectrum {
		public Float Lightness { get; }
	}
}
