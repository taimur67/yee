using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200060A RID: 1546
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class PlayGrandEventOrder : TargetedActionableOrder
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x0006348B File Offset: 0x0006168B
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.GrandEventCard;
			}
		}

		// Token: 0x04000C7F RID: 3199
		[BindableValue("event", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier EventCardId = Identifier.Invalid;
	}
}
