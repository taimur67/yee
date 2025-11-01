using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000239 RID: 569
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class InsultHurledEvent : DiplomaticEvent
	{
		// Token: 0x06000B1E RID: 2846 RVA: 0x0002F5B9 File Offset: 0x0002D7B9
		[JsonConstructor]
		private InsultHurledEvent()
		{
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0002F5C1 File Offset: 0x0002D7C1
		public InsultHurledEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0002F5CB File Offset: 0x0002D7CB
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} hurled an insult at player {1}", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0002F5ED File Offset: 0x0002D7ED
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.InsultSentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.InsultSentWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0002F608 File Offset: 0x0002D808
		public override void DeepClone(out GameEvent clone)
		{
			InsultHurledEvent insultHurledEvent = new InsultHurledEvent();
			base.DeepCloneDiplomaticEventParts(insultHurledEvent);
			clone = insultHurledEvent;
		}
	}
}
