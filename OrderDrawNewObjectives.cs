using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000637 RID: 1591
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderDrawNewObjectives : ActionableOrder
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x00065B28 File Offset: 0x00063D28
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.None;
			}
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x00065B2B File Offset: 0x00063D2B
		[JsonConstructor]
		public OrderDrawNewObjectives()
		{
		}
	}
}
