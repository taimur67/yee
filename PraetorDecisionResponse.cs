using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020004B3 RID: 1203
	[Serializable]
	public class PraetorDecisionResponse : GameItemDecisionResponse
	{
		// Token: 0x0600168C RID: 5772 RVA: 0x00052F4D File Offset: 0x0005114D
		public override IEnumerable<GameItem> GetOptions(PlayerState player, TurnState turn)
		{
			return from t in turn.GetGameItemsControlledBy(player.Id)
			where t.Category == GameItemCategory.Praetor
			select t;
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00052F80 File Offset: 0x00051180
		public override void DeepClone(out DecisionResponse clone)
		{
			PraetorDecisionResponse praetorDecisionResponse = new PraetorDecisionResponse();
			base.DeepCloneGameItemDecisionResponseParts(praetorDecisionResponse);
			clone = praetorDecisionResponse;
		}
	}
}
