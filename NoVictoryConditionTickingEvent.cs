using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200022B RID: 555
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class NoVictoryConditionTickingEvent : GameEvent
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0002EE82 File Offset: 0x0002D082
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002EE85 File Offset: 0x0002D085
		[JsonConstructor]
		public NoVictoryConditionTickingEvent() : base(-1)
		{
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002EE8E File Offset: 0x0002D08E
		public override string GetDebugName(TurnContext context)
		{
			return "No Victory Conditions are Ticking";
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0002EE95 File Offset: 0x0002D095
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.NoVictoryConditionTicking;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002EE9C File Offset: 0x0002D09C
		public override void DeepClone(out GameEvent clone)
		{
			NoVictoryConditionTickingEvent noVictoryConditionTickingEvent = new NoVictoryConditionTickingEvent();
			base.DeepCloneGameEventParts<NoVictoryConditionTickingEvent>(noVictoryConditionTickingEvent);
			clone = noVictoryConditionTickingEvent;
		}
	}
}
