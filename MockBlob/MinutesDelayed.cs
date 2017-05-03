using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockBlob
{
	public class MinutesDelayed
	{
		[JsonProperty(PropertyName = "late aircraft")]
		public int lateAircraft { get; set; }
		public int weather { get; set; }
		public int carrier { get; set; }
		public int security { get; set; }
		public int total { get; set; }
		[JsonProperty(PropertyName = "national aviation system")]
		public int nationalAviationSystem { get; set; }

	}
}
