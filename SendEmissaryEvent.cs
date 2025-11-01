using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200023E RID: 574
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SendEmissaryEvent : DiplomaticEvent
	{
		// Token: 0x06000B38 RID: 2872 RVA: 0x0002F83D File Offset: 0x0002DA3D
		[JsonConstructor]
		private SendEmissaryEvent()
		{
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0002F845 File Offset: 0x0002DA45
		public SendEmissaryEvent(int triggeringPlayerId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0002F84F File Offset: 0x0002DA4F
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} sent and emissary to player {1}", this.TriggeringPlayerID, base.AffectedPlayerID);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0002F871 File Offset: 0x0002DA71
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.EmissarySentInitiator;
			}
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.EmissarySentWitness;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0002F88C File Offset: 0x0002DA8C
		public override void DeepClone(out GameEvent clone)
		{
			SendEmissaryEvent sendEmissaryEvent = new SendEmissaryEvent();
			base.DeepCloneDiplomaticEventParts(sendEmissaryEvent);
			clone = sendEmissaryEvent;
		}
	}
}
