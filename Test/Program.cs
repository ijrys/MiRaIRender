﻿//using ImageTools;
using MiRaIRender.BaseType;
using System;
using Float = System.Single;
using VectorT = MiRaIRender.BaseType.Vector3f;
//using VectorS = MiRaIRender.BaseType.SIMD.Vector3f;
//using VectorA = System.Numerics.Vector3;
using System.Numerics;
using MiRaIRender.BaseType.Spectrum;
namespace Test {
	class Program {

		//static void Test1 () {
		//	for (int i = 0; i < 10000000; i ++) {
		//		VectorT v1 = new VectorT(2.0f, 0.0f, 0.0f);
		//		VectorT v2 = new VectorT(0.0f, 2.0f, 0.0f);
		//		VectorT v3 = v1 + v2;
		//		VectorT v4 = v1 - v2;
		//		VectorT v5 = v1 * 0.5f;
		//		VectorT v6 = v1 / 0.5f;
		//	}
		//}
		//static void Test2() {
		//	for (int i = 0; i < 10000000; i++) {
		//		VectorS v1 = new VectorS(2.0f, 0.0f, 0.0f);
		//		VectorS v2 = new VectorS(0.0f, 2.0f, 0.0f);
		//		VectorS v3 = v1 + v2;
		//		VectorS v4 = v1 - v2;
		//		VectorS v5 = v1 * 0.5f;
		//		VectorS v6 = v1 / 0.5f;
		//	}
		//}

		//static void Test3() {
		//	for (int i = 0; i < 10000000; i++) {
		//		VectorA v1 = new VectorA(2.0f, 0.0f, 0.0f);
		//		VectorA v2 = new VectorA(0.0f, 2.0f, 0.0f);
		//		VectorA v3 = v1 + v2;
		//		VectorA v4 = v1 - v2;
		//		VectorA v5 = v1 * 0.5f;
		//		VectorA v6 = v1 / 0.5f;
		//	}
		//}
		static void Main(string[] args) {
			RGBSpectrum rgb = new RGBSpectrum(1.0f, 2.0f, 3.0f);
			XYZSpectrum xyz = (XYZSpectrum)rgb;
			RGBSpectrum rgb2 = (RGBSpectrum)xyz;

			Console.WriteLine(xyz);
			Console.WriteLine(rgb2);

			Console.ReadLine();
		}
	}
}
