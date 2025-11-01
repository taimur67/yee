using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005D8 RID: 1496
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderDeclareDraconicRazzia : DiplomaticOrder
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001C19 RID: 7193 RVA: 0x000611E6 File Offset: 0x0005F3E6
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.DraconicRazzia;
			}
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x000611EA File Offset: 0x0005F3EA
		public OrderDeclareDraconicRazzia(int targetID) : base(targetID, null)
		{
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000611F4 File Offset: 0x0005F3F4
		[JsonConstructor]
		public OrderDeclareDraconicRazzia()
		{
		}
	}
}
