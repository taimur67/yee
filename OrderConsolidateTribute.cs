using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200062B RID: 1579
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderConsolidateTribute : ActionableOrder
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x00064D57 File Offset: 0x00062F57
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.ConsolidateTribute;
			}
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x00064D5A File Offset: 0x00062F5A
		[JsonConstructor]
		public OrderConsolidateTribute()
		{
		}
	}
}
