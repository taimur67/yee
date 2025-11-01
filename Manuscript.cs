using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002C7 RID: 711
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Manuscript : GameItem
	{
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x000367FC File Offset: 0x000349FC
		public override GameItemCategory Category
		{
			get
			{
				return GameItemCategory.ManuscriptPiece;
			}
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00036800 File Offset: 0x00034A00
		public sealed override void DeepClone(out GameItem gameItem)
		{
			Manuscript manuscript = new Manuscript();
			base.DeepCloneGameItemParts(manuscript);
			gameItem = manuscript;
		}
	}
}
