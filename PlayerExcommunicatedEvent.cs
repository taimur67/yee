using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200024F RID: 591
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class PlayerExcommunicatedEvent : GameEvent
	{
		// Token: 0x06000B8C RID: 2956 RVA: 0x0002FF99 File Offset: 0x0002E199
		[JsonConstructor]
		public PlayerExcommunicatedEvent()
		{
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002FFA1 File Offset: 0x0002E1A1
		public PlayerExcommunicatedEvent(int triggeringPlayer, int excommunicatedPlayer, ExcommunicationReason reason) : base(triggeringPlayer)
		{
			this.Reason = reason;
			base.AddAffectedPlayerId(excommunicatedPlayer);
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0002FFB8 File Offset: 0x0002E1B8
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002FFBC File Offset: 0x0002E1BC
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			TurnLogEntryType result;
			switch (this.Reason)
			{
			case ExcommunicationReason.Unknown:
				result = TurnLogEntryType.PlayerExcommunicatedGeneric;
				break;
			case ExcommunicationReason.CastRitualOnPandaemonium:
				result = TurnLogEntryType.PlayerExcommunicatedViolence;
				break;
			case ExcommunicationReason.AttackedPandaemonium:
				result = TurnLogEntryType.PlayerExcommunicatedViolence;
				break;
			case ExcommunicationReason.EdictEffect:
				result = TurnLogEntryType.PlayerExcommunicatedGeneric;
				break;
			default:
				result = TurnLogEntryType.PlayerExcommunicatedGeneric;
				break;
			}
			return result;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00030010 File Offset: 0x0002E210
		public override void DeepClone(out GameEvent clone)
		{
			PlayerExcommunicatedEvent playerExcommunicatedEvent = new PlayerExcommunicatedEvent();
			playerExcommunicatedEvent.Reason = this.Reason;
			base.DeepCloneGameEventParts<PlayerExcommunicatedEvent>(playerExcommunicatedEvent);
			clone = playerExcommunicatedEvent;
		}

		// Token: 0x04000520 RID: 1312
		[JsonProperty]
		public ExcommunicationReason Reason;
	}
}
