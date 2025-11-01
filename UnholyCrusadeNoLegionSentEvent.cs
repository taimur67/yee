using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200022A RID: 554
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class UnholyCrusadeNoLegionSentEvent : GameEvent
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0002EE2D File Offset: 0x0002D02D
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002EE30 File Offset: 0x0002D030
		[JsonConstructor]
		private UnholyCrusadeNoLegionSentEvent()
		{
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002EE38 File Offset: 0x0002D038
		public UnholyCrusadeNoLegionSentEvent(int prestigePenalty, params int[] playerIds) : base(-1)
		{
			this.PrestigePenalty = prestigePenalty;
			base.AddAffectedPlayerIds(playerIds);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002EE4F File Offset: 0x0002D04F
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.UnholyCrusadeSentFailed;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002EE58 File Offset: 0x0002D058
		public override void DeepClone(out GameEvent clone)
		{
			UnholyCrusadeNoLegionSentEvent unholyCrusadeNoLegionSentEvent = new UnholyCrusadeNoLegionSentEvent
			{
				PrestigePenalty = this.PrestigePenalty
			};
			base.DeepCloneGameEventParts<UnholyCrusadeNoLegionSentEvent>(unholyCrusadeNoLegionSentEvent);
			clone = unholyCrusadeNoLegionSentEvent;
		}

		// Token: 0x040004F6 RID: 1270
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int PrestigePenalty;
	}
}
