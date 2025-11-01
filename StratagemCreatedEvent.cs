using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000271 RID: 625
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class StratagemCreatedEvent : GameItemAssignmentEvent
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x000310FA File Offset: 0x0002F2FA
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Secret;
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x000310FD File Offset: 0x0002F2FD
		[JsonConstructor]
		private StratagemCreatedEvent()
		{
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00031105 File Offset: 0x0002F305
		public StratagemCreatedEvent(PlayerIndex playerIndex, Identifier stratagemId, Identifier gamePieceId) : base((int)playerIndex, stratagemId, gamePieceId)
		{
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00031110 File Offset: 0x0002F310
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.GameItemAttached;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00031117 File Offset: 0x0002F317
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} Stratagem forged for {1}", this.GameItemId, this.GamePieceId);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0003113C File Offset: 0x0002F33C
		public override void DeepClone(out GameEvent clone)
		{
			StratagemCreatedEvent stratagemCreatedEvent = new StratagemCreatedEvent
			{
				GamePieceId = this.GamePieceId
			};
			base.DeepCloneGameItemAssignmentEventParts(stratagemCreatedEvent);
			clone = stratagemCreatedEvent;
		}
	}
}
