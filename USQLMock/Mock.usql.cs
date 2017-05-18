using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace USQLMock
{
	public class MyUdfs
	{
		//<summary>
		//calculatePercentage(SqlMap flattened 'Key' as an int. Consider using 'int.Parse(<key>), SqlMap flattened 'Key' as an int. Consider using 'int.Parse(<key>)) 
		static public int calculatePercentage(int value1, int value2)
		{
			//Setting value1 to be the MAX value of the key from the Sql Map for that row
			value1 = int.MaxValue;

			//Same here
			value2 = int.MaxValue;

			//Calcualting the percentage. Divide the first value by the second and multipltying by 100
			return ((value1 * 100) / value2);

		}
	}

}
