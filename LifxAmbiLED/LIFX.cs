using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace LifxAmbiLED {
	class LIFX {
        private RestClient client;
        private const string apiKey = "APIKEY";

		public LIFX() {
            //create new client for lifx rest API service
			client = new RestClient("https://api.lifx.com/v1/");
		}

        /* Returns a list of all lights for account (using stored api key) */
		public List<Lights.RootObject> LightStatus() {
			RestRequest lightRequest = new RestRequest("/lights/all", Method.GET);
			lightRequest.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
			lightRequest.AddHeader("Authorization", "Bearer" +  apiKey);
			RestResponse <List<Lights.RootObject>> response = (RestResponse<List<Lights.RootObject>>)client.Execute<List<Lights.RootObject>>(lightRequest);
			return response.Data;
		}

	}
}
