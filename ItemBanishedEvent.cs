using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000247 RID: 583
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ItemBanishedEvent : GameEvent
	{
		// Token: 0x06000B61 RID: 2913 RVA: 0x0002FC3D File Offset: 0x0002DE3D
		[JsonConstructor]
		public ItemBanishedEvent()
		{
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0002FC50 File Offset: 0x0002DE50
		[JsonIgnore]
		public int OriginalOwner
		{
			get
			{
				return base.AffectedPlayerID;
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0002FC58 File Offset: 0x0002DE58
		public ItemBanishedEvent(Identifier itemId, int owner, GameItemCategory itemCategory) : this(itemId, owner, owner, itemCategory)
		{
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0002FC64 File Offset: 0x0002DE64
		public ItemBanishedEvent(Identifier itemId, int instigator, int owner, GameItemCategory itemCategory) : base(instigator)
		{
			this.ItemId = itemId;
			this.ItemCategory = itemCategory;
			base.AddAffectedPlayerId(owner);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0002FC8E File Offset: 0x0002DE8E
		protected void DeepCloneItemBanishedEventParts(ItemBanishedEvent itemBanishedEvent)
		{
			itemBanishedEvent.ItemId = this.ItemId;
			itemBanishedEvent.HeldItems = this.HeldItems.DeepClone();
			itemBanishedEvent.ItemCategory = this.ItemCategory;
			base.DeepCloneGameEventParts<ItemBanishedEvent>(itemBanishedEvent);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002FCC4 File Offset: 0x0002DEC4
		public override void DeepClone(out GameEvent clone)
		{
			ItemBanishedEvent itemBanishedEvent = new ItemBanishedEvent();
			this.DeepCloneItemBanishedEventParts(itemBanishedEvent);
			clone = itemBanishedEvent;
		}

		// Token: 0x04000512 RID: 1298
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier ItemId;

		// Token: 0x04000513 RID: 1299
		[JsonProperty]
		public List<Identifier> HeldItems = new List<Identifier>();

		// Token: 0x04000514 RID: 1300
		[JsonProperty]
		public GameItemCategory ItemCategory;
	}
}
