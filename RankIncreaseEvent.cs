using System;
using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000223 RID: 547
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class RankIncreaseEvent : GameEvent
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0002EB6A File Offset: 0x0002CD6A
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0002EB6D File Offset: 0x0002CD6D
		[JsonConstructor]
		private RankIncreaseEvent()
		{
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002EB75 File Offset: 0x0002CD75
		public RankIncreaseEvent(int playerId, Rank newRank) : base(playerId)
		{
			this.NewRank = newRank;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002EB85 File Offset: 0x0002CD85
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} increased their infernal rank to {1}", this.TriggeringPlayerID, this.NewRank);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002EBA7 File Offset: 0x0002CDA7
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.RankIncrease;
			}
			return TurnLogEntryType.RankIncreaseWitnessed;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0002EBC0 File Offset: 0x0002CDC0
		public override void DeepClone(out GameEvent clone)
		{
			RankIncreaseEvent rankIncreaseEvent = new RankIncreaseEvent
			{
				NewRank = this.NewRank,
				Unlocks = this.Unlocks.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneGameEventParts<RankIncreaseEvent>(rankIncreaseEvent);
			clone = rankIncreaseEvent;
		}

		// Token: 0x040004ED RID: 1261
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Rank NewRank;

		// Token: 0x040004EE RID: 1262
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public List<ConfigRef> Unlocks;
	}
}
