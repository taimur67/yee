using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200063A RID: 1594
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderMakeBid : ActionableOrder
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x00065CE7 File Offset: 0x00063EE7
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Bid;
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x00065CEB File Offset: 0x00063EEB
		[JsonConstructor]
		public OrderMakeBid()
		{
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x00065CFA File Offset: 0x00063EFA
		public OrderMakeBid(Identifier item)
		{
			this.Item = item;
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x00065D10 File Offset: 0x00063F10
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new ValueConflict<int>(ConflictScopeFlags.Bid, (int)this.Item);
			yield break;
		}

		// Token: 0x04000C8D RID: 3213
		[BindableValue("target", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier Item = Identifier.Invalid;
	}
}
