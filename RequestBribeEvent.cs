using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000227 RID: 551
	[BindableGameEvent]
	[Serializable]
	public class RequestBribeEvent : GrandEventPlayed
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0002ED65 File Offset: 0x0002CF65
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Private;
			}
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002ED68 File Offset: 0x0002CF68
		[JsonConstructor]
		private RequestBribeEvent()
		{
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002ED70 File Offset: 0x0002CF70
		public RequestBribeEvent(int playerID, string eventId) : base(playerID, eventId)
		{
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002ED7C File Offset: 0x0002CF7C
		public override void DeepClone(out GameEvent clone)
		{
			RequestBribeEvent requestBribeEvent = new RequestBribeEvent
			{
				Cost = this.Cost.DeepClone<Cost>(),
				PlayerId = this.PlayerId
			};
			base.DeepCloneGrandEventPlayedParts(requestBribeEvent);
			clone = requestBribeEvent;
		}

		// Token: 0x040004F4 RID: 1268
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public Cost Cost;

		// Token: 0x040004F5 RID: 1269
		[BindableValue("archfiend_name", BindingOption.IntPlayerId)]
		[JsonProperty]
		public int PlayerId;
	}
}
