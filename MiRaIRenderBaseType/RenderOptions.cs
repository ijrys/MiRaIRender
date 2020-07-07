using System;
using System.Collections.Generic;
using System.Text;
using Float = System.Single;

namespace MiRaIRender.BaseType {
	public class RenderOptions {
		private int _width = 1280;
		private int _height = 720;
		private Float _fovHorizon = 135;
		private int _randonSeed = 2020;
		private Random _random = new Random(2020);

		public int Width {
			get => _width;
			set {
				if (value < 1) value = 1;
				_width = value;
			}
		}
		public int Height {
			get => _height;
			set {
				if (value < 1) value = 1;
				_height = value;
			}
		}
		public Float FovHorizon {
			get => _fovHorizon;
			set {
				if (value < 1) value = 1;
				else if (value > 170) value = 170;
				_fovHorizon = value;
			}
		}
		public int RandonSeed {
			get => _randonSeed;
			set {
				_randonSeed = value;
				_random = new Random(value);
			}
		}
		public Random Random{
			get => _random;
		}
	}
}
