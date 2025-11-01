using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002E5 RID: 741
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SchemeCard : GameItem
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x00039BDD File Offset: 0x00037DDD
		public override GameItemCategory Category
		{
			get
			{
				return GameItemCategory.SchemeCard;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x00039BE1 File Offset: 0x00037DE1
		public override bool RemoveIfBanished
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00039BE4 File Offset: 0x00037DE4
		public sealed override void DeepClone(out GameItem gameItem)
		{
			SchemeCard schemeCard = new SchemeCard();
			base.DeepCloneGameItemParts(schemeCard);
			schemeCard.Scheme = this.Scheme.DeepClone<SchemeObjective>();
			schemeCard.Awarded = this.Awarded;
			gameItem = schemeCard;
		}

		// Token: 0x04000663 RID: 1635
		[JsonProperty]
		public SchemeObjective Scheme;

		// Token: 0x04000664 RID: 1636
		[JsonProperty]
		public bool Awarded;
	}
}
