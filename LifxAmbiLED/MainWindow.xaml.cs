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
using System.Timers;

namespace LifxAmbiLED {
	public partial class MainWindow : Window {
		
		AmbiLED leds;
		LIFX lifx;
		System.Timers.Timer updateTimer;
		Lights.RootObject selectedLight = null;
        const string lightName = "Room Light";

        //Struct for storing color as RGB value
		public struct ColorRGB {
			public byte R;
			public byte G;
			public byte B;
			public ColorRGB(Color value) {
				this.R = value.R;
				this.G = value.G;
				this.B = value.B;
			}
		}

        public MainWindow() {
            InitializeComponent();
            leds = new AmbiLED();

            lifx = new LIFX();
            List<Lights.RootObject> currentLightStatus = lifx.LightStatus();
            selectedLight = null;
            foreach (Lights.RootObject light in currentLightStatus) {
                //check if object value isnt null (object still created with no data from JSON
                if (light.label != null) {
                    if (light.label.Contains(lightName)) {
                        selectedLight = light;
                    }


                    if (selectedLight != null && selectedLight.color != null && selectedLight.power != "off") {
                        Color color = HsvToRgb(selectedLight.color.hue, selectedLight.color.saturation, selectedLight.brightness);
                        leds.setLEDColor(color.R, color.G, color.B);
                    } else if (selectedLight.power == "off") {
                        leds.setLEDColor(0, 0, 0);
                    }

                    updateTimer = new System.Timers.Timer();
                    updateTimer.Interval = 1100;
                    updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.time_tick);
                    updateTimer.Enabled = true;
                }
            }
        }

	
		/*
         *  time_tick runs every timer interval, checks light status and sets the leds to match the lights
         */
		private void time_tick(object sender, EventArgs e) {
			lifx = new LIFX();
			List<Lights.RootObject> test = lifx.LightStatus();
			selectedLight = null;
			foreach (Lights.RootObject light in test) {
                if (light.label.Contains(lightName)) {
					selectedLight = light;
				}
			}

			if (selectedLight != null && selectedLight.color != null && selectedLight.power != "off") {
                //Convert HSV from LIFX to RGB values for LED
				Color color = HsvToRgb(selectedLight.color.hue, selectedLight.color.saturation, selectedLight.brightness);
				leds.setLEDColor(color.R, color.G, color.B);
			} else if (selectedLight.power == "off") {
				leds.setLEDColor(0, 0, 0);
			}
			
		}
        /*
         *  Converts HSV to RGB values
         */
		public static Color HsvToRgb(double h, double s, double v) {
			int hi = (int)Math.Floor(h / 60.0) % 6;
			double f = (h / 60.0) - Math.Floor(h / 60.0);

			double p = v * (1.0 - s);
			double q = v * (1.0 - (f * s));
			double t = v * (1.0 - ((1.0 - f) * s));

			Color ret;

			switch (hi) {
				case 0:
					ret = GetRgb(v, t, p);
					break;
				case 1:
					ret = GetRgb(q, v, p);
					break;
				case 2:
					ret = GetRgb(p, v, t);
					break;
				case 3:
					ret = GetRgb(p, q, v);
					break;
				case 4:
					ret = GetRgb(t, p, v);
					break;
				case 5:
					ret = GetRgb(v, p, q);
					break;
				default:
					ret = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
					break;
			}
			return ret;
		}

		public static Color GetRgb(double r, double g, double b) {
			return Color.FromArgb(255, (byte)(r * 255.0), (byte)(g * 255.0), (byte)(b * 255.0));
		}

	}
}
