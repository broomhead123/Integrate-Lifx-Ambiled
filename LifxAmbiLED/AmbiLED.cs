using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Timers;
using Microsoft.Win32;

namespace LifxAmbiLED {
	class AmbiLED {
		byte[] serialBuffer = new byte[2000];
		byte[] storedSerialBuffer = new byte[2000];
		int TOTAL_LED_COUNT = 40;
		float POWER_LEVEL = (float)0.3;
		int Refresh_Interval = 500;
		int Power_Level = 30;
		System.Timers.Timer refreshTimer;
		string SerialPortName = "COM0";
		private System.IO.Ports.SerialPort ambiLEDPort;

		public AmbiLED() {
            //Setup port object and timer hander
			ambiLEDPort = new System.IO.Ports.SerialPort();
			refreshTimer = new System.Timers.Timer();
			refreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.time_tick);
            SetupPort();
		}

		private void SetupPort() {
			if (SerialPortName == "COM0") {
				// Get all serial ports
				string[] ports = SerialPort.GetPortNames();
                //Try each port to find AmbiLED
				foreach (string port in ports) {
					try {
						ambiLEDPort.PortName = port;
						ambiLEDPort.Open();
						ambiLEDPort.Close();
						SerialPortName = port;
					} catch {
					// ignore is port fails
					}
				}
			}
            //If port is not COM0 then port has been found, open port for use
			if (SerialPortName != "COM0") {
				ambiLEDPort.PortName = SerialPortName;
				ambiLEDPort.BaudRate = 115200;
				ambiLEDPort.Open();
				ambiLEDPort.Encoding = System.Text.Encoding.UTF8;
			}
		}

        /* Takes in RGB value to be set for LEDS */
		public void setLEDColor(byte R, byte G, byte B) {
            //disable timer
			refreshTimer.Enabled = false;
            //set fill buffer with RGB values for leds
			for (int i = 0; i < TOTAL_LED_COUNT; i++) {
				serialBuffer[(i * 3) + 1] = (byte)(R >> 1);
				serialBuffer[(i * 3) + 2] = (byte)(G >> 1);
				serialBuffer[(i * 3) + 3] = (byte)(B >> 1);
			}
            //Set final byte to 255 to enable leds
			serialBuffer[TOTAL_LED_COUNT * 3] = (byte)255; 
			storedSerialBuffer = serialBuffer;
            //Check if port is enabled, if so write to it
			if ((SerialPortName != "COM0"))
				ambiLEDPort.Write(serialBuffer, 0, (TOTAL_LED_COUNT * 3) + 3);
			//Renable timer
			refreshTimer.Interval = Refresh_Interval;		
			refreshTimer.Enabled = true;
		}

        /* If port is set then write current stored buffers values to buffer again to refresh the leds */
		private void time_tick(object sender, EventArgs e) {
			if ((SerialPortName != "COM0"))
				ambiLEDPort.Write(storedSerialBuffer, 0, (TOTAL_LED_COUNT * 3) + 3);
		}
	}
}
