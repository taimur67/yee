using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000258 RID: 600
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ReceiveEventCardEvent : GameEvent
	{
		// Token: 0x06000BBD RID: 3005 RVA: 0x000302D6 File Offset: 0x0002E4D6
		[JsonConstructor]
		private ReceiveEventCardEvent()
		{
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000302DE File Offset: 0x0002E4DE
		public ReceiveEventCardEvent(int playerId, Identifier eventCardId) : base(playerId)
		{
			base.AddAffectedPlayerId(playerId);
			this.EventCardId = eventCardId;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x000302F5 File Offset: 0x0002E4F5
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Event card(s) drawn for player {0}", this.TriggeringPlayerID);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0003030C File Offset: 0x0002E50C
		public override void DeepClone(out GameEvent clone)
		{
			ReceiveEventCardEvent receiveEventCardEvent = new ReceiveEventCardEvent
			{
				EventCardId = this.EventCardId
			};
			base.DeepCloneGameEventParts<ReceiveEventCardEvent>(receiveEventCardEvent);
			clone = receiveEventCardEvent;
		}

		// Token: 0x04000526 RID: 1318
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier EventCardId;
	}
}
