using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020004B2 RID: 1202
	[Serializable]
	public class LegionDecisionResponse : GameItemDecisionResponse
	{
		// Token: 0x06001689 RID: 5769 RVA: 0x00052F19 File Offset: 0x00051119
		public override IEnumerable<GameItem> GetOptions(PlayerState player, TurnState turn)
		{
			return turn.GetAllActiveLegionsForPlayer(player.Id);
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00052F28 File Offset: 0x00051128
		public override void DeepClone(out DecisionResponse clone)
		{
			LegionDecisionResponse legionDecisionResponse = new LegionDecisionResponse();
			base.DeepCloneGameItemDecisionResponseParts(legionDecisionResponse);
			clone = legionDecisionResponse;
		}
	}
}
