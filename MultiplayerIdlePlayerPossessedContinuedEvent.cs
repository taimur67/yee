using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000265 RID: 613
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MultiplayerIdlePlayerPossessedContinuedEvent : GameEvent
	{
		// Token: 0x06000C06 RID: 3078 RVA: 0x0003087E File Offset: 0x0002EA7E
		[JsonConstructor]
		private MultiplayerIdlePlayerPossessedContinuedEvent()
		{
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00030886 File Offset: 0x0002EA86
		public MultiplayerIdlePlayerPossessedContinuedEvent(int possessedPlayer)
		{
			base.AddAffectedPlayerId(possessedPlayer);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00030895 File Offset: 0x0002EA95
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} is still idle and remains possessed.", base.AffectedPlayerID);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000308AC File Offset: 0x0002EAAC
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.None;
			}
			return TurnLogEntryType.IdlePlayerPossessedContinued;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000308C0 File Offset: 0x0002EAC0
		public override void DeepClone(out GameEvent clone)
		{
			MultiplayerIdlePlayerPossessedContinuedEvent multiplayerIdlePlayerPossessedContinuedEvent = new MultiplayerIdlePlayerPossessedContinuedEvent();
			base.DeepCloneGameEventParts<MultiplayerIdlePlayerPossessedContinuedEvent>(multiplayerIdlePlayerPossessedContinuedEvent);
			clone = multiplayerIdlePlayerPossessedContinuedEvent;
		}
	}
}
