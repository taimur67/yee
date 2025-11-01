using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000200 RID: 512
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MessageReceivedEvent : GameEvent
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x0002D99D File Offset: 0x0002BB9D
		public MessageReceivedEvent()
		{
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0002D9A5 File Offset: 0x0002BBA5
		public MessageReceivedEvent(Message message)
		{
			this.TriggeringPlayerID = message.FromPlayerId;
			base.AddAffectedPlayerId(message.ToPlayerId);
			this.MessageId = message.Id;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0002D9D1 File Offset: 0x0002BBD1
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == base.AffectedPlayerID)
			{
				return TurnLogEntryType.MessageReceived;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0002D9E4 File Offset: 0x0002BBE4
		public override void DeepClone(out GameEvent clone)
		{
			MessageReceivedEvent messageReceivedEvent = new MessageReceivedEvent
			{
				MessageId = this.MessageId.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneGameEventParts<MessageReceivedEvent>(messageReceivedEvent);
			clone = messageReceivedEvent;
		}

		// Token: 0x040004BD RID: 1213
		[JsonProperty]
		public Identifier MessageId;
	}
}
