using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000248 RID: 584
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class LegionKilledEvent : ItemBanishedEvent
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0002FCE1 File Offset: 0x0002DEE1
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x0002FCE9 File Offset: 0x0002DEE9
		[JsonProperty]
		public Identifier GamePieceId
		{
			get
			{
				return this.ItemId;
			}
			set
			{
				this.ItemId = value;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0002FCF2 File Offset: 0x0002DEF2
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002FCF5 File Offset: 0x0002DEF5
		[JsonConstructor]
		private LegionKilledEvent()
		{
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002FCFD File Offset: 0x0002DEFD
		public LegionKilledEvent(Identifier gamePieceId, int triggeringPlayerId, int affectedPlayerId) : base(gamePieceId, triggeringPlayerId, affectedPlayerId, GameItemCategory.GamePiece)
		{
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002FD09 File Offset: 0x0002DF09
		public override string GetDebugName(TurnContext context)
		{
			return context.CurrentTurn.FetchGameItem(this.ItemId).NameKey + " was killed";
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0002FD2C File Offset: 0x0002DF2C
		public override void DeepClone(out GameEvent clone)
		{
			LegionKilledEvent legionKilledEvent = new LegionKilledEvent();
			base.DeepCloneItemBanishedEventParts(legionKilledEvent);
			clone = legionKilledEvent;
		}
	}
}
