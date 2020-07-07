namespace MiRaIRender.BaseType.Skybox {
	public class DefaultSkyBox : ISkyBoxAble {
		private Color _c;
		public Color Color {
			get => _c;
			set  { _c = value; }
		}

		public DefaultSkyBox () {
			_c = new Color(0.3f, 0.3f, 0.9f);
		}
		public DefaultSkyBox(Color c) {
			_c = c;
		}

		public static DefaultSkyBox Default = new DefaultSkyBox();


		public Color SkyColor(Ray ray) {
			return _c;
		}


	}
}
