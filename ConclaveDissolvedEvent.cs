using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000254 RID: 596
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ConclaveDissolvedEvent : GameEvent
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x000301DE File Offset: 0x0002E3DE
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x000301E1 File Offset: 0x0002E3E1
		public VictoryType RelatedVictoryRule
		{
			get
			{
				return VictoryType.Abyss;
			}
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x000301E4 File Offset: 0x0002E3E4
		public ConclaveDissolvedEvent() : base(-1)
		{
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x000301ED File Offset: 0x0002E3ED
		public override string GetDebugName(TurnContext context)
		{
			return "Conclave has been dissolved";
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000301F4 File Offset: 0x0002E3F4
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.ConclaveDissolved;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x000301FC File Offset: 0x0002E3FC
		public override void DeepClone(out GameEvent clone)
		{
			ConclaveDissolvedEvent conclaveDissolvedEvent = new ConclaveDissolvedEvent();
			base.DeepCloneGameEventParts<ConclaveDissolvedEvent>(conclaveDissolvedEvent);
			clone = conclaveDissolvedEvent;
		}
	}
}
