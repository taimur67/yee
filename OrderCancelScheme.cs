using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000629 RID: 1577
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderCancelScheme : ActionableOrder
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x00064D08 File Offset: 0x00062F08
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Scheme;
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00064D0C File Offset: 0x00062F0C
		[JsonConstructor]
		public OrderCancelScheme()
		{
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00064D1B File Offset: 0x00062F1B
		public OrderCancelScheme(Identifier schemeCardId)
		{
			this.SchemeCardId = schemeCardId;
		}

		// Token: 0x04000C86 RID: 3206
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier SchemeCardId = Identifier.Invalid;
	}
}
