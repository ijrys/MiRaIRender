using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Float = System.Double;

namespace WpfApp1 {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}
		public static Float FresnelEquation(Float cosAlpha, Float niOverNt) {
			Float sinAlphaSq = 1.0f - cosAlpha * cosAlpha;
			Float sinBetaSq = niOverNt * niOverNt * sinAlphaSq;
			if (sinBetaSq > 1.0f) { return 1.0f; }
			Float cosBeta = Math.Sqrt(1.0f - sinBetaSq);
			Float p1, p2, n = 1.0f / niOverNt;
			p1 = (cosAlpha - n * cosBeta) / (cosAlpha + n * cosBeta);
			p2 = (cosBeta - n * cosAlpha) / (cosBeta + n * cosAlpha);
			return (p1 * p1 + p2 * p2) * 0.5f;
		}

		public static Float Schlick(Float f0, Float cosa) {
			cosa = 1.0 - cosa;
			Float c = cosa;
			cosa *= cosa;
			cosa *= cosa;
			cosa *= c;
			return f0 + (1.0f - f0) * cosa;
		}


		private void btn_Generate_Click(object sender, RoutedEventArgs e) {
			//Float niOverNt = Float.Parse(txt_niOverNt.Text);

			List<Point> points = new List<Point>();
			canvasShow.Children.Clear();
			Float max = Math.PI / 2.0;
			for (Float i = 0.0; i < max; i += 0.01f) {
				Float x = i / max * 500.0;
				//Float y = (1.0 - FresnelEquation(Math.Cos(i), niOverNt)) * 500.0;
				Float y = (1.0 - Schlick(0.1, Math.Cos(i))) * 500.0;
				Point p = new Point(x, y);
				points.Add(p);
			}

			Polyline line = new Polyline();
			line.Points = new PointCollection(points.ToArray());
			line.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));

			canvasShow.Children.Add(line);
		}
	}
}
