using System;

namespace gitdb.Entities
{
	public class TwoWayAttribute : BaseLinkAttribute
	{
		public string OtherPropertyName { get;set; }

		public TwoWayAttribute (string otherPropertyName)
		{
			OtherPropertyName = otherPropertyName;
		}
	}
}

