using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000267 RID: 615
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MultiplayerIdlePlayerBanishedEvent : GameEvent
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00030946 File Offset: 0x0002EB46
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00030949 File Offset: 0x0002EB49
		[JsonConstructor]
		private MultiplayerIdlePlayerBanishedEvent()
		{
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00030951 File Offset: 0x0002EB51
		public MultiplayerIdlePlayerBanishedEvent(int triggeringPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(triggeringPlayerId);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00030961 File Offset: 0x0002EB61
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} was idle for too long and has been banished from the match!", this.TriggeringPlayerID);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00030978 File Offset: 0x0002EB78
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.MpBanishedWitness;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00030980 File Offset: 0x0002EB80
		public override void DeepClone(out GameEvent clone)
		{
			MultiplayerIdlePlayerBanishedEvent multiplayerIdlePlayerBanishedEvent = new MultiplayerIdlePlayerBanishedEvent();
			base.DeepCloneGameEventParts<MultiplayerIdlePlayerBanishedEvent>(multiplayerIdlePlayerBanishedEvent);
			clone = multiplayerIdlePlayerBanishedEvent;
		}
	}
}
