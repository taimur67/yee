using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005E2 RID: 1506
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderRequestChainsOfAvarice : DiplomaticOrder
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x000616D0 File Offset: 0x0005F8D0
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.ChainsOfAvarice;
			}
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000616D4 File Offset: 0x0005F8D4
		[JsonConstructor]
		public OrderRequestChainsOfAvarice()
		{
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000616DC File Offset: 0x0005F8DC
		public OrderRequestChainsOfAvarice(int targetID) : base(targetID, null)
		{
		}
	}
}
