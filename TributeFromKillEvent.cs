using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200020F RID: 527
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class TributeFromKillEvent : GameEvent
	{
		// Token: 0x06000A4A RID: 2634 RVA: 0x0002DFAA File Offset: 0x0002C1AA
		[JsonConstructor]
		private TributeFromKillEvent()
		{
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0002DFB2 File Offset: 0x0002C1B2
		public TributeFromKillEvent(int triggeringPlayerId, Identifier killedId)
		{
			this.TriggeringPlayerID = triggeringPlayerId;
			this.KilledGamePieceId = killedId;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0002DFC8 File Offset: 0x0002C1C8
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.TributeFromKill;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0002DFD0 File Offset: 0x0002C1D0
		public override void DeepClone(out GameEvent clone)
		{
			TributeFromKillEvent tributeFromKillEvent = new TributeFromKillEvent
			{
				KilledGamePieceId = this.KilledGamePieceId
			};
			base.DeepCloneGameEventParts<TributeFromKillEvent>(tributeFromKillEvent);
			clone = tributeFromKillEvent;
		}

		// Token: 0x040004CE RID: 1230
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier KilledGamePieceId;
	}
}
