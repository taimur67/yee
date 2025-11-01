using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001F0 RID: 496
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class BazaarBidEvent : ItemAcquiredEvent
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0002D0F7 File Offset: 0x0002B2F7
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0002D0FA File Offset: 0x0002B2FA
		[JsonIgnore]
		public Identifier GameItemId
		{
			get
			{
				return this.Item;
			}
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002D102 File Offset: 0x0002B302
		[JsonConstructor]
		private BazaarBidEvent()
		{
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002D10A File Offset: 0x0002B30A
		public BazaarBidEvent(int triggeringPlayerID, Identifier gameItemId, GameItemCategory gameItemCategory, bool wasBackroomItem) : base(triggeringPlayerID, gameItemId)
		{
			this.GameItemCategory = gameItemCategory;
			this.WasBackroomItem = wasBackroomItem;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0002D123 File Offset: 0x0002B323
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} purchased item of type {1} with id {2}", this.TriggeringPlayerID, this.GameItemCategory, this.GameItemId);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0002D150 File Offset: 0x0002B350
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.TriggeringPlayerID == -2147483648)
			{
				return TurnLogEntryType.None;
			}
			bool flag = this.PurchaseKnowledgeHolders.IsSet(forPlayerID);
			bool flag2 = this.GameItemCategory == GameItemCategory.GamePiece || flag;
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.BazaarPurchase;
			}
			if (this.AffectedPlayerIds.Contains(forPlayerID))
			{
				if (!flag2)
				{
					return TurnLogEntryType.BazaarPurchaseFailed_UnknownPurchaser;
				}
				return TurnLogEntryType.BazaarPurchaseFailed_KnownPurchaser;
			}
			else
			{
				if (!flag2)
				{
					return TurnLogEntryType.BazaarPurchaseWitness_UnknownPurchaser;
				}
				return TurnLogEntryType.BazaarPurchaseWitness_KnownPurchaser;
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002D1B0 File Offset: 0x0002B3B0
		public override void DeepClone(out GameEvent clone)
		{
			clone = base.DeepCloneGameEventParts<BazaarBidEvent>(new BazaarBidEvent
			{
				PurchaseKnowledgeHolders = this.PurchaseKnowledgeHolders,
				GameItemCategory = this.GameItemCategory,
				WasBackroomItem = this.WasBackroomItem
			});
		}

		// Token: 0x040004A4 RID: 1188
		[JsonProperty]
		public BitMask PurchaseKnowledgeHolders;

		// Token: 0x040004A5 RID: 1189
		[JsonProperty]
		public GameItemCategory GameItemCategory;

		// Token: 0x040004A6 RID: 1190
		[JsonProperty]
		public bool WasBackroomItem;
	}
}
