using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020004BD RID: 1213
	[Serializable]
	public class SelectEventCardResponse : DecisionResponse
	{
		// Token: 0x060016AE RID: 5806 RVA: 0x0005330C File Offset: 0x0005150C
		public bool IsSelected(Identifier id)
		{
			return this.SelectedCardIds.Any((Identifier x) => x == id);
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0005333D File Offset: 0x0005153D
		public void Select(Identifier id)
		{
			if (!this.SelectedCardIds.Contains(id))
			{
				this.SelectedCardIds.Add(id);
			}
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x00053359 File Offset: 0x00051559
		public void Deselect(Identifier id)
		{
			this.SelectedCardIds.Remove(id);
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00053368 File Offset: 0x00051568
		public void Select(params Identifier[] ids)
		{
			foreach (Identifier id in ids)
			{
				this.Select(id);
			}
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00053390 File Offset: 0x00051590
		public override void DeepClone(out DecisionResponse clone)
		{
			SelectEventCardResponse selectEventCardResponse = new SelectEventCardResponse
			{
				SelectedCardIds = this.SelectedCardIds.DeepClone()
			};
			base.DeepCloneDecisionResponseParts(selectEventCardResponse);
			clone = selectEventCardResponse;
		}

		// Token: 0x04000B36 RID: 2870
		public List<Identifier> SelectedCardIds = new List<Identifier>();
	}
}
