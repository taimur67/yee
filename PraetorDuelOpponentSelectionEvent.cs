using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003AB RID: 939
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuelOpponentSelectionEvent : GameEvent
	{
		// Token: 0x06001255 RID: 4693 RVA: 0x000464A7 File Offset: 0x000446A7
		[JsonConstructor]
		public PraetorDuelOpponentSelectionEvent()
		{
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000464AF File Offset: 0x000446AF
		public PraetorDuelOpponentSelectionEvent(int triggeringPlayerId, int opponentPlayerId) : base(triggeringPlayerId)
		{
			base.AddAffectedPlayerId(opponentPlayerId);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x000464C0 File Offset: 0x000446C0
		public override void DeepClone(out GameEvent clone)
		{
			PraetorDuelOpponentSelectionEvent praetorDuelOpponentSelectionEvent = new PraetorDuelOpponentSelectionEvent();
			base.DeepCloneGameEventParts<PraetorDuelOpponentSelectionEvent>(praetorDuelOpponentSelectionEvent);
			clone = praetorDuelOpponentSelectionEvent;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000464DE File Offset: 0x000446DE
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID != this.TriggeringPlayerID)
			{
				return TurnLogEntryType.None;
			}
			return TurnLogEntryType.PraetorDuelOpponentSubmitting;
		}
	}
}
