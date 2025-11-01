using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000221 RID: 545
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class RecoverGameItemsEvent : GameEvent
	{
		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002EA18 File Offset: 0x0002CC18
		[JsonConstructor]
		public RecoverGameItemsEvent()
		{
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002EA2C File Offset: 0x0002CC2C
		public RecoverGameItemsEvent(GamePiece winner, GamePiece loser, params Identifier[] salvagedItems)
		{
			this.TriggeringPlayerID = winner.ControllingPlayerId;
			this.WinningGamePiece = winner.Id;
			base.AddAffectedPlayerId(loser.ControllingPlayerId);
			this.LosingGamePiece = loser.Id;
			this.RecoveredItems = IEnumerableExtensions.ToList<Identifier>(salvagedItems);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002EA86 File Offset: 0x0002CC86
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.ItemsRecovered;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002EA98 File Offset: 0x0002CC98
		public override void DeepClone(out GameEvent clone)
		{
			RecoverGameItemsEvent recoverGameItemsEvent = new RecoverGameItemsEvent
			{
				WinningGamePiece = this.WinningGamePiece,
				LosingGamePiece = this.LosingGamePiece,
				RecoveredItems = this.RecoveredItems.DeepClone()
			};
			base.DeepCloneGameEventParts<RecoverGameItemsEvent>(recoverGameItemsEvent);
			clone = recoverGameItemsEvent;
		}

		// Token: 0x040004E8 RID: 1256
		[BindableValue("winner", BindingOption.None)]
		[JsonProperty]
		public Identifier WinningGamePiece;

		// Token: 0x040004E9 RID: 1257
		[BindableValue("loser", BindingOption.None)]
		[JsonProperty]
		public Identifier LosingGamePiece;

		// Token: 0x040004EA RID: 1258
		[BindableValue("recovered_items", BindingOption.None)]
		[JsonProperty]
		public List<Identifier> RecoveredItems = new List<Identifier>();
	}
}
