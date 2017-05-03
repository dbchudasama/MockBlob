using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockBlob
{
	public class Airline
	{

		public Airport airport { get; set; }
		public Statistics statistics { get; set; }
		public Time time { get; set; }
		public Carrier carrier { get; set; }
	}
}