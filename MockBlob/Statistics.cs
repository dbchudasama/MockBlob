using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockBlob
{
	public class Statistics
	{
		public Flights flights { get; set; }

		[JsonProperty(PropertyName = "# of delays")]
		public OfDelays ofDelays { get; set; }

		[JsonProperty(PropertyName = "minutes delayed")]
		public MinutesDelayed minutesDelayed { get; set; }
	}
}
