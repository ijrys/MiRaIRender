using MiRaIRender.BaseType;
using System;
using System.Collections.Generic;
using System.Text;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.Render.PathTrace {
	public class PathTraceRenderOptions : RenderOptions {
		private int _subSampleNumberPerPixel = 8;
		private int _traceDeep = 4;

		public int SubSampleNumberPerPixel {
			get => _subSampleNumberPerPixel;
			set {
				if (value < 1) { value = 1; }
				else if (value > 0x1000000) { value = 0x1000000; }
				//else if (value > 0x1000000‬) { value = 0x1000000; }
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

		public Vector3f CameraOrigin = new Vector3f();

		public static (PathTraceRenderOptions, bool) LoadOptions(string[] options) {
			bool ok = true;
			int linenum = 0;
			PathTraceRenderOptions re = new PathTraceRenderOptions();
			try {
				foreach (string line in options) {
					linenum++;
					if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) {
						continue;
					}
					int idx = line.IndexOf("=");
					string key = line.Substring(0, idx).Trim();
					string value = line.Substring(idx + 1).Trim();
					switch (key.ToLower()) {
						case "width":
							re.Width = int.Parse(value);
							break;
						case "height":
							re.Height = int.Parse(value);
							break;
						case "fovhorizon":
							re.FovHorizon = float.Parse(value);
							break;
						case "randonseed":
							re.RandonSeed = int.Parse(value);
							break;
						case "tracedeep":
							re.TraceDeep = int.Parse(value);
							break;
						case "ssnpp":
						case "subsamplenumberperpixel":
							re.SubSampleNumberPerPixel = int.Parse(value);
							break;
						case "cameraorigin":
							string[] values = value.Split(',');
							Vector3f vector;
							if (values.Length == 1) {
								float v = float.Parse(values[0]);
								vector = new Vector3f(v);
							}
							else if (values.Length == 3) {
								float v0 = float.Parse(values[0]);
								float v1 = float.Parse(values[1]);
								float v2 = float.Parse(values[2]);
								vector = new Vector3f(v0, v1, v2);
							}
							else {
								throw new Exception("错误的数据个数");
							}
							re.CameraOrigin = vector;
							break;
						default:
							break;
					}
				}
			}
			catch (Exception ex) {
				Console.WriteLine($"error on line {linenum} : {ex.Message}");
				ok = false;
			}
			return (re, ok);
		}

		public static string[] SaveOptions(PathTraceRenderOptions option) {
			string[] re = new string[7];
			re[0] = "width=" + option.Width;
			re[1] = "height=" + option.Height;
			re[2] = "fovhorizon=" + option.FovHorizon;
			re[3] = "randonseed=" + option.RandonSeed;
			re[4] = "tracedeep=" + option.TraceDeep;
			re[5] = "ssnpp=" + option.SubSampleNumberPerPixel;
			re[6] = "cameraorigin=" + option.CameraOrigin.X + ',' + option.CameraOrigin.Y + ',' + option.CameraOrigin.Z;
			return re;
		}
	}
}
