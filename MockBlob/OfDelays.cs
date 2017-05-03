using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockBlob
{
	public class OfDelays
	{
		[JsonProperty(PropertyName = "late aircraft")]
		public int lateAircraft { get; set; }

		public int weather { get; set; }
		public int security { get; set; }

		[JsonProperty(PropertyName = "national aviation system")]
		public int nationalAviationSystem { get; set; }

		public int carrier { get; set; }
	
	}
}
