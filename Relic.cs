using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003FB RID: 1019
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Relic : GameItem
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x0004D5D3 File Offset: 0x0004B7D3
		public override GameItemCategory Category
		{
			get
			{
				return GameItemCategory.Relic;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x0004D5D6 File Offset: 0x0004B7D6
		public override bool RecordAsDeadEntityOnBanish
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0004D5D9 File Offset: 0x0004B7D9
		public override void ConfigureFrom(IdentifiableStaticData data)
		{
			base.ConfigureFrom(data);
			RelicStaticData relicStaticData = data as RelicStaticData;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0004D5EC File Offset: 0x0004B7EC
		public sealed override void DeepClone(out GameItem gameItem)
		{
			Relic relic = new Relic();
			base.DeepCloneGameItemParts(relic);
			relic.Type = this.Type;
			gameItem = relic;
		}

		// Token: 0x04000903 RID: 2307
		[JsonProperty]
		public RelicType Type;
	}
}
