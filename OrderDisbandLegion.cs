using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000636 RID: 1590
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderDisbandLegion : ActionableOrder
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x00065B1C File Offset: 0x00063D1C
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.DisbandLegion;
			}
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x00065B20 File Offset: 0x00063D20
		[JsonConstructor]
		public OrderDisbandLegion()
		{
		}
	}
}
