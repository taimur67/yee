using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000257 RID: 599
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class DrawEventCardEvent : GameEvent
	{
		// Token: 0x06000BB8 RID: 3000 RVA: 0x0003028A File Offset: 0x0002E48A
		[JsonConstructor]
		private DrawEventCardEvent()
		{
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00030292 File Offset: 0x0002E492
		public DrawEventCardEvent(int playerId) : base(playerId)
		{
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0003029B File Offset: 0x0002E49B
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Event card(s) drawn for player {0}", this.TriggeringPlayerID);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000302B2 File Offset: 0x0002E4B2
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.EventCardDrawn;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000302B8 File Offset: 0x0002E4B8
		public override void DeepClone(out GameEvent clone)
		{
			DrawEventCardEvent drawEventCardEvent = new DrawEventCardEvent();
			base.DeepCloneGameEventParts<DrawEventCardEvent>(drawEventCardEvent);
			clone = drawEventCardEvent;
		}
	}
}
