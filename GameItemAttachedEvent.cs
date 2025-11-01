using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200022F RID: 559
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GameItemAttachedEvent : GameItemAssignmentEvent
	{
		// Token: 0x06000AEC RID: 2796 RVA: 0x0002F055 File Offset: 0x0002D255
		[JsonConstructor]
		private GameItemAttachedEvent()
		{
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0002F064 File Offset: 0x0002D264
		public GameItemAttachedEvent(int playerId, Identifier itemId, Identifier gamePieceId) : base(playerId, itemId, gamePieceId)
		{
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0002F076 File Offset: 0x0002D276
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} was attached to {1}", this.GameItemId, this.GamePieceId);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0002F098 File Offset: 0x0002D298
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.PreviousHostId == Identifier.Invalid)
			{
				return TurnLogEntryType.GameItemAttached;
			}
			return TurnLogEntryType.GameItemReassigned;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002F0B0 File Offset: 0x0002D2B0
		public override void DeepClone(out GameEvent clone)
		{
			GameItemAttachedEvent gameItemAttachedEvent = new GameItemAttachedEvent
			{
				PreviousHostId = this.PreviousHostId
			};
			base.DeepCloneGameItemAssignmentEventParts(gameItemAttachedEvent);
			clone = gameItemAttachedEvent;
		}

		// Token: 0x040004FC RID: 1276
		[BindableValue("previous", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier PreviousHostId = Identifier.Invalid;
	}
}
