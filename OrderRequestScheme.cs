using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000642 RID: 1602
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderRequestScheme : ActionableOrder
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x000661E3 File Offset: 0x000643E3
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Scheme;
			}
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000661E7 File Offset: 0x000643E7
		[JsonConstructor]
		public OrderRequestScheme()
		{
		}
	}
}
