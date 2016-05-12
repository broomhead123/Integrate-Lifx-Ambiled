using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifxAmbiLED {
	class Lights {
		public class Color {
			public double hue { get; set; }
			public double saturation { get; set; }
			public int kelvin { get; set; }
		}

		public class Group {
			public string id { get; set; }
			public string name { get; set; }
		}

		public class Location {
			public string id { get; set; }
			public string name { get; set; }
		}

		public class Capabilities {
			public bool has_color { get; set; }
			public bool has_variable_color_temp { get; set; }
		}

		public class Product {
			public string name { get; set; }
			public string company { get; set; }
			public string identifier { get; set; }
			public Capabilities capabilities { get; set; }
		}

		public class RootObject {
			public string id { get; set; }
			public string uuid { get; set; }
			public string label { get; set; }
			public bool connected { get; set; }
			public string power { get; set; }
			public Color color { get; set; }
			public double brightness { get; set; }
			public Group group { get; set; }
			public Location location { get; set; }
			public string last_seen { get; set; }
			public double seconds_since_seen { get; set; }
			public Product product { get; set; }
		}
	}
}
