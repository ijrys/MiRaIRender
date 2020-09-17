using MiRaIRender.BaseType;
using MiRaIRender.BaseType.Materials;
using MiRaIRender.BaseType.SceneObject;
using MiRaIRender.BaseType.Skybox;
using MiRaIRender.BaseType.Spectrum;
using MiRaIRender.Objects.LightSource;
using MiRaIRender.Objects.SceneObject;
using System;
using System.Collections.Generic;
using System.IO;
using Vector2f = System.Numerics.Vector2;
using Vector3f = System.Numerics.Vector3;

namespace ModelLoader {
	public static class MiRaIRanderProjectLoader {
		private static Vector3f ReadAVector(BinaryReader br) {
			float f0, f1, f2;
			f0 = br.ReadSingle();
			f1 = br.ReadSingle();
			f2 = br.ReadSingle();
			return new Vector3f(f0, f1, f2);
		}
		private static MashTrigle GetMashTrigle(string fname) {
			MashTrigle result = new MashTrigle();
			using (FileStream fs = new FileStream(fname, FileMode.Open)) {
				using (BinaryReader reader = new BinaryReader(fs)) {
					long flen = fs.Length;
					if (flen % 108 != 0) { throw new Exception("不是正确的文件"); }
					int facecount = (int)(flen / 108);
					TrigleFace[] trigles = new TrigleFace[facecount];

					for (int i = 0; i < facecount; i++) {
						Vector3f v0, v1, v2, vt0, vt1, vt2, vn0, vn1, vn2;

						v0 = ReadAVector(reader);
						vt0 = ReadAVector(reader);
						vn0 = ReadAVector(reader);
						v1 = ReadAVector(reader);
						vt1 = ReadAVector(reader);
						vn1 = ReadAVector(reader);
						v2 = ReadAVector(reader);
						vt2 = ReadAVector(reader);
						vn2 = ReadAVector(reader);
						TrigleFace face = new TrigleFace(v0, v1, v2) {
							sp0 = new Vector2f(vt0.X, vt0.Y),
							sp1 = new Vector2f(vt1.X, vt1.Y),
							sp2 = new Vector2f(vt2.X, vt2.Y),
							n0 = vn0,
							n1 = vn1,
							n2 = vn2
						};
						trigles[i] = face;
					}

					result.SetTrigles(trigles);
				}
			}

			return result;
		}

		private static IMaterialMapAble<XYZSpectrum> MaterialMapXYZ(string cmd) {
			int idx = cmd.IndexOf(' ');
			string type = cmd.Substring(0, idx);
			string value = cmd.Substring(idx + 1);
			if (type.ToLower() == "color") {
				RGBSpectrum color = Color(value);
				return new PureXYZColorMaterialMap(color.ToXYZ());
			}
			else if (type.ToLower() == "img") {
				throw new Exception("尚不支持图片贴图");
			}
			else {
				throw new Exception("不支持的命令");
			}
		}
		private static IMaterialMapAble<RGBSpectrum> MaterialMapRGB(string cmd) {
			int idx = cmd.IndexOf(' ');
			string type = cmd.Substring(0, idx);
			string value = cmd.Substring(idx + 1);
			if (type.ToLower() == "color") {
				RGBSpectrum color = Color(value);
				return new PureRGBColorMaterialMap(color);
			}
			else if (type.ToLower() == "img") {
				throw new Exception("尚不支持图片贴图");
			}
			else {
				throw new Exception("不支持的命令");
			}
		}
		//private static IMaterialMapAble MaterialGrayMap(string cmd) {
		//	int idx = cmd.IndexOf(' ');
		//	string type = cmd.Substring(0, idx);
		//	string value = cmd.Substring(idx + 1);
		//	if (type.ToLower() == "color") {
		//		RGBSpectrum color = Color(value);
		//		return new PureXYZColorMaterialMap(color.ToXYZ());
		//	}
		//	else if (type.ToLower() == "img") {
		//		throw new Exception("尚不支持图片贴图");
		//	}
		//	else {
		//		throw new Exception("不支持的命令");
		//	}
		//}
		private static RGBSpectrum Color(string cmd) {
			string[] strs = cmd.Split(',');
			RGBSpectrum color;
			if (strs.Length == 0) {
				float v = 0.0f;
				float.TryParse(strs[0], out v);
				color = new RGBSpectrum(v);
			}
			else if (strs.Length == 3) {
				float v0 = 0.0f, v1 = 0.0f, v2 = 0.0f;
				float.TryParse(strs[0], out v0);
				float.TryParse(strs[1], out v1);
				float.TryParse(strs[2], out v2);
				color = new RGBSpectrum(v0, v1, v2);
			}
			else {
				throw new Exception("错误的数据个数");
			}
			return color;
		}

		private static Material GetMaterial(string fname) {
			string[] lines = File.ReadAllLines(fname);
			Material result = new Material();
			foreach (string line in lines) {
				if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) {
					continue;
				}
				int index = line.IndexOf('=');
				string property = line.Substring(0, index).Trim();
				string value = line.Substring(index + 1).Trim();
				switch (property.ToLower()) {
					case "basecolor":
						result.BaseColor = MaterialMapXYZ(value);
						break;
					case "normal":
						result.NormalMap.Enable = true;
						result.NormalMap.NormalMap = MaterialMapRGB(value);
						break;
					case "roughness":
						result.Roughness = float.Parse(value);
						break;
					case "metallic.metallic":
						result.Metallic.Metallic = float.Parse(value);
						break;
					case "metallic.intensitymap":
						result.Metallic.IntensityMap = MaterialMapRGB(value);
						break;
					case "metallic.metalliccolor":
						result.Metallic.MetallicColorMap = MaterialMapXYZ(value);
						break;
					case "refraction": {
						string[] values = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
						if (values.Length != 2) {
							throw new Exception("期待参数为2个"); ;
						}
						result.Refraction.Refraction = float.Parse(values[0]);
						result.Refraction.IOR = float.Parse(values[1]);
						result.Refraction.Enable = true;
					}
					break;
					case "refraction.intensitymap":
						result.Refraction.IntensityMap = MaterialMapRGB(value);
						break;
					case "light":
						result.Light.Intensity = Color(value).ToXYZ();
						result.Light.Enable = true;
						break;
					case "light.intensitymap":
						result.Light.IntensityMap = MaterialMapXYZ(value);
						result.Light.EnableMap = true;
						break;
					default:
						break;
				}
			}

			return result;
		}
		
		public static (Scene, bool) LoadScene(string proPath, ISkyBoxAble skyBox) {
			bool readed = true;
			string basePath = Path.GetDirectoryName(proPath);
			string fname = Path.Combine(proPath);

			string[] lines = File.ReadAllLines(fname);
			HashSet<string> mtlFiles = new HashSet<string>();
			Scene result = new Scene(skyBox);
			int linenum = 0;
			try {
				foreach (string line in lines) {
					linenum++;
					if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) {
						continue;
					}
					string[] cmds = line.Split(' ');
					if (cmds.Length != 3) {
						throw new Exception("错误的行指令数量: " + fname);
					}
					RenderObject robj;
					switch (cmds[0]) {
						case "mt": // mashtrigle
							{
							string mtfile = Path.Combine(basePath, cmds[1] + ".mritri");
							robj = GetMashTrigle(mtfile);
						}
							break;
						case "l.p": // point light
							{
							PointLight p = new PointLight();
							string[] values = cmds[1].Split(',');
							if (values.Length != 4) {
								throw new Exception("错误的描述信息");
							}
							float x = 0.0f, y = 0.0f, z = 0.0f, r = 0.0f;
							float.TryParse(values[0], out x);
							float.TryParse(values[1], out y);
							float.TryParse(values[2], out z);
							float.TryParse(values[3], out r);
							p.Position = new Vector3f(x, y, z);
							p.R = r;
							robj = p;
						}
						break;
						default:
							throw new Exception("未知的实体类型" + cmds[0]);
							//break;
					}

					string mtlPath = Path.Combine(basePath, cmds[2] + ".mrimtl");
					Material material = GetMaterial(mtlPath);
					robj.Material = material;
					result.Objects.Add(robj);
				}
			}
			catch (Exception ex) {
				Console.WriteLine($"error on line {linenum} : {ex.Message}");
				readed = false;
			}
			return (result, readed);
		}
	}
}
