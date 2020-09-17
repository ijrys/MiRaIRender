using MiRaIRender.BaseType;
using MiRaIRender.BaseType.Materials;
using MiRaIRender.BaseType.SceneObject;
using MiRaIRender.BaseType.Spectrum;
using MiRaIRender.Objects.LightSource;
using MiRaIRender.Objects.SceneObject;
using System;
using System.IO;
using Vector2f = System.Numerics.Vector2;
using Vector3f = System.Numerics.Vector3;

namespace ModelLoader {
	public static class MiRaIRanderProjectSaver {
		private static string ColorDescription(RGBSpectrum color) {
			return color.R + "," + color.G + "," + color.B;
		}
		private static string MaterialMapDescription(IMaterialMapAble materialMap) {
			if (materialMap.GetType() == typeof(PureXYZColorMaterialMap)) {
				return "color " + ColorDescription((materialMap as PureXYZColorMaterialMap).BaseColor.ToRGB());
			}
			//else if (materialMap.GetType() == typeof(PureGrayMaterialMap)) {
			//	RGBSpectrum c = new RGBSpectrum((materialMap as PureGrayMaterialMap).BaseGray);
			//	return "color " + ColorDescription(c);
			//}
			return "";
		}

		private static void SaveMaterial(Material material, string fpath) {
			using (FileStream fs = new FileStream(fpath, FileMode.Create)) {
				using (StreamWriter sw = new StreamWriter(fs)) {
					sw.AutoFlush = true;
					sw.WriteLine("basecolor=" + MaterialMapDescription(material.BaseColor));
					if (material.NormalMap != null && material.NormalMap.Enable) {
						sw.WriteLine("normal=" + MaterialMapDescription(material.NormalMap.NormalMap));
					}
					sw.WriteLine("roughness=" + material.Roughness);
					sw.WriteLine("metallic.metallic=" + material.Metallic.Metallic);
					if (material.Metallic.IntensityMap != null) {
						sw.WriteLine("metallic.intensitymap=" + MaterialMapDescription(material.Metallic.IntensityMap));
					}
					if (material.Metallic.MetallicColorMap != null) {
						sw.WriteLine("metallic.metalliccolor=" + MaterialMapDescription(material.Metallic.MetallicColorMap));
					}
					if (material.Refraction != null && material.Refraction.Enable) {
						sw.WriteLine("refraction=" + material.Refraction.Refraction + " " + material.Refraction.IOR);
						if (material.Refraction.IntensityMap != null) {
							sw.WriteLine("refraction.intensitymap=" + MaterialMapDescription(material.Refraction.IntensityMap));
						}
					}
					if (material.Light.Enable) {
						sw.WriteLine("light=" + ColorDescription(material.Light.Intensity.ToRGB()));
						if (material.Light.EnableMap && material.Light.IntensityMap != null) {
							sw.WriteLine("light.intensitymap=" + MaterialMapDescription(material.Light.IntensityMap));
						}
					}

				}
			}
		}

		private static void WriteVector(BinaryWriter bw, Vector3f vector) {
			bw.Write(vector.X);
			bw.Write(vector.Y);
			bw.Write(vector.Z);
		}
		private static void WriteVector(BinaryWriter bw, Vector2f vector) {
			bw.Write(vector.X);
			bw.Write(vector.Y);
			bw.Write(0.0f);
		}

		private static void SaveObject(MashTrigle mashTrigle, string fpath) {
			using (FileStream fs = new FileStream(fpath, FileMode.Create)) {
				using (BinaryWriter writer = new BinaryWriter(fs)) {
					foreach (TrigleFace trigle in mashTrigle.trigles) {
						WriteVector(writer, trigle.v0);
						WriteVector(writer, trigle.sp0);
						WriteVector(writer, trigle.n0);
						WriteVector(writer, trigle.v1);
						WriteVector(writer, trigle.sp1);
						WriteVector(writer, trigle.n1);
						WriteVector(writer, trigle.v2);
						WriteVector(writer, trigle.sp2);
						WriteVector(writer, trigle.n2);
					}
				}
			}
		}

		public static void Save(Scene scene, string basePath, string proName) {
			basePath = Path.Combine(basePath, proName);
			string profile = Path.Combine(basePath, proName + ".mrirpro");
			Directory.CreateDirectory(basePath);
			using (FileStream fs = new FileStream(profile, FileMode.Create)) {
				using (StreamWriter sw = new StreamWriter(fs)) {
					sw.AutoFlush = true;

					int objcount = -1;
					foreach (RenderObject obj in scene.Objects) {
						objcount++;
						string fpath = Path.Combine(basePath, objcount + ".mrimtl");
						SaveMaterial(obj.Material, fpath);

						Type type = obj.GetType();
						if (type == typeof(MashTrigle)) {
							MashTrigle mash = obj as MashTrigle;
							string mtlpath = Path.Combine(basePath, objcount + ".mritri");
							SaveObject(mash, mtlpath);

							sw.WriteLine($"mt {objcount} {objcount}");
							continue;
						}
						if (type == typeof(PointLight)) {
							PointLight pl = obj as PointLight;
							sw.WriteLine($"l.p {pl.Position.X},{pl.Position.Y},{pl.Position.Z},{pl.R} {objcount}");
							continue;
						}
					}
				}
			}
		}
	}
}
