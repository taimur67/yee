using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000218 RID: 536
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GamePieceReformedEvent : GameEvent
	{
		// Token: 0x06000A72 RID: 2674 RVA: 0x0002E43E File Offset: 0x0002C63E
		[JsonConstructor]
		private GamePieceReformedEvent()
		{
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0002E446 File Offset: 0x0002C646
		public GamePieceReformedEvent(int triggeringPlayerId, Identifier gamePieceId, bool successful = true) : base(triggeringPlayerId)
		{
			this.GamePieceId = gamePieceId;
			this.Successful = successful;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0002E45D File Offset: 0x0002C65D
		public override string GetDebugName(TurnContext context)
		{
			if (!this.Successful)
			{
				return string.Format("{0} tried to reform after dying, but was blocked.", this.GamePieceId);
			}
			return string.Format("{0} was reformed after dying", this.GamePieceId);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0002E492 File Offset: 0x0002C692
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.TriggeringPlayerID != forPlayerID)
			{
				return TurnLogEntryType.None;
			}
			if (!this.Successful)
			{
				return TurnLogEntryType.GamePieceReformFailed;
			}
			return TurnLogEntryType.GamePieceReformed;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002E4B4 File Offset: 0x0002C6B4
		public override void DeepClone(out GameEvent clone)
		{
			GamePieceReformedEvent gamePieceReformedEvent = new GamePieceReformedEvent
			{
				GamePieceId = this.GamePieceId,
				Successful = this.Successful
			};
			base.DeepCloneGameEventParts<GamePieceReformedEvent>(gamePieceReformedEvent);
			clone = gamePieceReformedEvent;
		}

		// Token: 0x040004D8 RID: 1240
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier GamePieceId;

		// Token: 0x040004D9 RID: 1241
		[JsonProperty]
		public bool Successful;
	}
}
