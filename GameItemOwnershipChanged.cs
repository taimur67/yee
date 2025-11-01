using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200024D RID: 589
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class GameItemOwnershipChanged : ItemAcquiredEvent
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0002FEFC File Offset: 0x0002E0FC
		[JsonIgnore]
		public int OriginalOwner
		{
			get
			{
				return base.AffectedPlayerID;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0002FF04 File Offset: 0x0002E104
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				if (this.Category != GameItemCategory.GamePiece)
				{
					return base.GameEventVisibility;
				}
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002FF17 File Offset: 0x0002E117
		[JsonConstructor]
		public GameItemOwnershipChanged()
		{
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002FF1F File Offset: 0x0002E11F
		public GameItemOwnershipChanged(int originalOwner, int newOwner, Identifier item, GameItemCategory category) : base(newOwner, item)
		{
			base.AddAffectedPlayerId(originalOwner);
			this.Category = category;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002FF38 File Offset: 0x0002E138
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Item {0} Captured by player {1}", this.Item, base.Owner);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002FF5A File Offset: 0x0002E15A
		protected void DeepCloneGameItemOwnershipChangedParts(GameItemOwnershipChanged gameItemOwnershipChanged)
		{
			gameItemOwnershipChanged.Item = this.Item;
			gameItemOwnershipChanged.Category = this.Category;
			base.DeepCloneGameEventParts<GameItemOwnershipChanged>(gameItemOwnershipChanged);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002FF7C File Offset: 0x0002E17C
		public override void DeepClone(out GameEvent clone)
		{
			GameItemOwnershipChanged gameItemOwnershipChanged = new GameItemOwnershipChanged();
			this.DeepCloneGameItemOwnershipChangedParts(gameItemOwnershipChanged);
			clone = gameItemOwnershipChanged;
		}

		// Token: 0x0400051A RID: 1306
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public GameItemCategory Category;
	}
}
