using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000296 RID: 662
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class AbilityPlaceholder : GameItem
	{
		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x000343CD File Offset: 0x000325CD
		public override GameItemCategory Category
		{
			get
			{
				return GameItemCategory.AbilityPlaceholder;
			}
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x000343D0 File Offset: 0x000325D0
		public AbilityPlaceholder SetId(Guid id)
		{
			this.ActionableOrderId = id;
			return this;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000343DA File Offset: 0x000325DA
		public AbilityPlaceholder SetStaticDataId(string id)
		{
			base.StaticDataId = id;
			return this;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x000343E4 File Offset: 0x000325E4
		public AbilityPlaceholder SetItemType(GameItemCategory itemCategory)
		{
			this.GameItemCategory = itemCategory;
			return this;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x000343EE File Offset: 0x000325EE
		public AbilityPlaceholder SetAttachableTo(SlotType attachableTo)
		{
			this.AttachableTo = attachableTo;
			return this;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x000343F8 File Offset: 0x000325F8
		public AbilityPlaceholder SetLinkedItem(GameItem item)
		{
			this.LinkedId = item.Id;
			this.SetStaticDataId(item.StaticDataId);
			this.SetItemType(item.Category);
			this.SetAttachableTo(item.AttachableTo);
			return this;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00034430 File Offset: 0x00032630
		public sealed override void DeepClone(out GameItem gameItem)
		{
			AbilityPlaceholder abilityPlaceholder = new AbilityPlaceholder
			{
				ActionableOrderId = this.ActionableOrderId,
				LinkedId = this.LinkedId,
				GameItemCategory = this.GameItemCategory,
				AttachableTo = this.AttachableTo
			};
			base.DeepCloneGameItemParts(abilityPlaceholder);
			gameItem = abilityPlaceholder;
		}

		// Token: 0x040005C1 RID: 1473
		[JsonProperty]
		public Guid ActionableOrderId;

		// Token: 0x040005C2 RID: 1474
		[JsonProperty]
		public Identifier LinkedId;

		// Token: 0x040005C3 RID: 1475
		[JsonProperty]
		public GameItemCategory GameItemCategory;
	}
}
