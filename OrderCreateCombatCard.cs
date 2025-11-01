using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200062D RID: 1581
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderCreateCombatCard : ActionableOrder
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x00064EDA File Offset: 0x000630DA
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.CreateCombatCard;
			}
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x00064EDE File Offset: 0x000630DE
		[JsonConstructor]
		public OrderCreateCombatCard()
		{
		}
	}
}
