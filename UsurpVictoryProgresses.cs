using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000253 RID: 595
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class UsurpVictoryProgresses : GameEvent
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00030172 File Offset: 0x0002E372
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00030175 File Offset: 0x0002E375
		[JsonConstructor]
		private UsurpVictoryProgresses()
		{
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0003017D File Offset: 0x0002E37D
		public UsurpVictoryProgresses(int turnsUntilGameEnds, int triggeringPlayerID) : base(triggeringPlayerID)
		{
			this.TurnsUntilGameEnds = turnsUntilGameEnds;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0003018D File Offset: 0x0002E38D
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("UsurpVictoryProgress {0} turns until {1} wins", this.TurnsUntilGameEnds, this.TriggeringPlayerID);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x000301AF File Offset: 0x0002E3AF
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.UsurpProgresses;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x000301B4 File Offset: 0x0002E3B4
		public override void DeepClone(out GameEvent clone)
		{
			UsurpVictoryProgresses usurpVictoryProgresses = new UsurpVictoryProgresses
			{
				TurnsUntilGameEnds = this.TurnsUntilGameEnds
			};
			base.DeepCloneGameEventParts<UsurpVictoryProgresses>(usurpVictoryProgresses);
			clone = usurpVictoryProgresses;
		}

		// Token: 0x04000525 RID: 1317
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int TurnsUntilGameEnds;
	}
}
