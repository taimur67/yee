using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200022D RID: 557
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public abstract class GameItemAssignmentEvent : GameEvent
	{
		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002EFBE File Offset: 0x0002D1BE
		protected GameItemAssignmentEvent()
		{
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002EFC6 File Offset: 0x0002D1C6
		protected GameItemAssignmentEvent(int playerId, Identifier gameItemId, Identifier gamePieceId) : base(playerId)
		{
			this.GameItemId = gameItemId;
			this.GamePieceId = gamePieceId;
			base.AddAffectedPlayerId(playerId);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002EFE4 File Offset: 0x0002D1E4
		protected void DeepCloneGameItemAssignmentEventParts(GameItemAssignmentEvent gameItemAssignmentEvent)
		{
			gameItemAssignmentEvent.GameItemId = this.GameItemId;
			gameItemAssignmentEvent.GamePieceId = this.GamePieceId;
			base.DeepCloneGameEventParts<GameItemAssignmentEvent>(gameItemAssignmentEvent);
		}

		// Token: 0x06000AE6 RID: 2790
		public abstract override void DeepClone(out GameEvent clone);

		// Token: 0x040004FA RID: 1274
		[BindableValue("gameitem", BindingOption.None)]
		[JsonProperty]
		public Identifier GameItemId;

		// Token: 0x040004FB RID: 1275
		[BindableValue("gamepiece", BindingOption.None)]
		[JsonProperty]
		public Identifier GamePieceId;
	}
}
