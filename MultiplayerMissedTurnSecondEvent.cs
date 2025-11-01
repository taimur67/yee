using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000262 RID: 610
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MultiplayerMissedTurnSecondEvent : GameEvent
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0003075E File Offset: 0x0002E95E
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00030761 File Offset: 0x0002E961
		private MultiplayerMissedTurnSecondEvent()
		{
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00030769 File Offset: 0x0002E969
		public MultiplayerMissedTurnSecondEvent(int triggeringPlayerID) : base(triggeringPlayerID)
		{
			base.AddAffectedPlayerId(triggeringPlayerID);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00030779 File Offset: 0x0002E979
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} once again failed to submit orders before deadline!", base.AffectedPlayerID);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00030790 File Offset: 0x0002E990
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.MpMissedTurnSecondRecipient;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000307A4 File Offset: 0x0002E9A4
		public override void DeepClone(out GameEvent clone)
		{
			MultiplayerMissedTurnSecondEvent multiplayerMissedTurnSecondEvent = new MultiplayerMissedTurnSecondEvent();
			base.DeepCloneGameEventParts<MultiplayerMissedTurnSecondEvent>(multiplayerMissedTurnSecondEvent);
			clone = multiplayerMissedTurnSecondEvent;
		}
	}
}
