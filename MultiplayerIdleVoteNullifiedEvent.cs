using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000263 RID: 611
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MultiplayerIdleVoteNullifiedEvent : GameEvent
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x000307C2 File Offset: 0x0002E9C2
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000307C5 File Offset: 0x0002E9C5
		private MultiplayerIdleVoteNullifiedEvent()
		{
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000307CD File Offset: 0x0002E9CD
		public MultiplayerIdleVoteNullifiedEvent(int triggeringPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(triggeringPlayerId);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000307DD File Offset: 0x0002E9DD
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} has returned before the idle player vote could execute!", this.TriggeringPlayerID);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000307F4 File Offset: 0x0002E9F4
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID != this.TriggeringPlayerID)
			{
				return TurnLogEntryType.MpIdleVoteNullifiedWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00030808 File Offset: 0x0002EA08
		public override void DeepClone(out GameEvent clone)
		{
			MultiplayerIdleVoteNullifiedEvent multiplayerIdleVoteNullifiedEvent = new MultiplayerIdleVoteNullifiedEvent();
			base.DeepCloneGameEventParts<MultiplayerIdleVoteNullifiedEvent>(multiplayerIdleVoteNullifiedEvent);
			clone = multiplayerIdleVoteNullifiedEvent;
		}
	}
}
