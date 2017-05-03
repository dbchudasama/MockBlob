using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockBlob
{
	public class Flights
	{
		public int cancelled { get; set; }
		
		//Using JsonProperty keyword to map the field 'on time' from the JSON file to the variable onTime
		[JsonProperty(PropertyName = "on time")]
		public int onTime { get; set; }

		public int total { get; set; }
		public int delayed { get; set; }
		public int diverted { get; set; }
	}
}
