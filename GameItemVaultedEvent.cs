using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200022E RID: 558
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GameItemVaultedEvent : GameItemAssignmentEvent
	{
		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002F006 File Offset: 0x0002D206
		private GameItemVaultedEvent()
		{
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002F00E File Offset: 0x0002D20E
		public GameItemVaultedEvent(int playerId, Identifier gameItemId, Identifier gamePieceId) : base(playerId, gameItemId, gamePieceId)
		{
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002F019 File Offset: 0x0002D219
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} was vaulted", this.GameItemId);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0002F030 File Offset: 0x0002D230
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.GameItemVaulted;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0002F038 File Offset: 0x0002D238
		public override void DeepClone(out GameEvent clone)
		{
			GameItemVaultedEvent gameItemVaultedEvent = new GameItemVaultedEvent();
			base.DeepCloneGameItemAssignmentEventParts(gameItemVaultedEvent);
			clone = gameItemVaultedEvent;
		}
	}
}
