using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000632 RID: 1586
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderDemandTribute : ActionableOrder
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x00065567 File Offset: 0x00063767
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.DemandTribute;
			}
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0006556A File Offset: 0x0006376A
		[JsonConstructor]
		public OrderDemandTribute()
		{
		}
	}
}
