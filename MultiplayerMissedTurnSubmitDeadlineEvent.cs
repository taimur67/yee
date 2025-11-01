using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000261 RID: 609
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MultiplayerMissedTurnSubmitDeadlineEvent : GameEvent
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x000306F9 File Offset: 0x0002E8F9
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Private;
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x000306FC File Offset: 0x0002E8FC
		[JsonConstructor]
		private MultiplayerMissedTurnSubmitDeadlineEvent()
		{
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00030704 File Offset: 0x0002E904
		public MultiplayerMissedTurnSubmitDeadlineEvent(int triggeringPlayerID) : base(triggeringPlayerID)
		{
			base.AddAffectedPlayerId(triggeringPlayerID);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00030714 File Offset: 0x0002E914
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} Failed to submit orders before deadline!", this.TriggeringPlayerID);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0003072B File Offset: 0x0002E92B
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.MpMissedTurnFirstRecipient;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00030740 File Offset: 0x0002E940
		public override void DeepClone(out GameEvent clone)
		{
			MultiplayerMissedTurnSubmitDeadlineEvent multiplayerMissedTurnSubmitDeadlineEvent = new MultiplayerMissedTurnSubmitDeadlineEvent();
			base.DeepCloneGameEventParts<MultiplayerMissedTurnSubmitDeadlineEvent>(multiplayerMissedTurnSubmitDeadlineEvent);
			clone = multiplayerMissedTurnSubmitDeadlineEvent;
		}
	}
}
