using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000266 RID: 614
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MultiplayerIdlePlayerPossessedEvent : GameEvent
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x000308DE File Offset: 0x0002EADE
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000308E1 File Offset: 0x0002EAE1
		[JsonConstructor]
		private MultiplayerIdlePlayerPossessedEvent()
		{
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000308E9 File Offset: 0x0002EAE9
		public MultiplayerIdlePlayerPossessedEvent(int triggeringPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(triggeringPlayerId);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000308F9 File Offset: 0x0002EAF9
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} was idle for too long and has been possessed!", this.TriggeringPlayerID);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00030910 File Offset: 0x0002EB10
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.MpPossessedRecipient;
			}
			return TurnLogEntryType.MpPossessedWitness;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00030928 File Offset: 0x0002EB28
		public override void DeepClone(out GameEvent clone)
		{
			MultiplayerIdlePlayerPossessedEvent multiplayerIdlePlayerPossessedEvent = new MultiplayerIdlePlayerPossessedEvent();
			base.DeepCloneGameEventParts<MultiplayerIdlePlayerPossessedEvent>(multiplayerIdlePlayerPossessedEvent);
			clone = multiplayerIdlePlayerPossessedEvent;
		}
	}
}
