using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020004B1 RID: 1201
	public abstract class GameItemDecisionResponse : DecisionResponse
	{
		// Token: 0x06001681 RID: 5761 RVA: 0x00052E8A File Offset: 0x0005108A
		public void SubmitDecision(List<GameItem> Items)
		{
			this.SubmitDecision(IEnumerableExtensions.ToList<Identifier>(from t in Items
			select t.Id));
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00052EBC File Offset: 0x000510BC
		public void SubmitDecision(List<Identifier> identifiers)
		{
			this.GameItems = identifiers;
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00052EC5 File Offset: 0x000510C5
		public virtual IEnumerable<GameItem> GetOptions(int playerId, GameState state)
		{
			return this.GetOptions(playerId, state.CurrentTurn);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00052ED4 File Offset: 0x000510D4
		public virtual IEnumerable<GameItem> GetOptions(int playerId, TurnState turn)
		{
			return this.GetOptions(turn.FindPlayerState(playerId, null), turn);
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00052EE5 File Offset: 0x000510E5
		public virtual IEnumerable<GameItem> GetOptions(PlayerState player, TurnState turn)
		{
			return Enumerable.Empty<GameItem>();
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x00052EEC File Offset: 0x000510EC
		protected void DeepCloneGameItemDecisionResponseParts(GameItemDecisionResponse gameItemDecisionResponse)
		{
			gameItemDecisionResponse.GameItems = this.GameItems.DeepClone();
			base.DeepCloneDecisionResponseParts(gameItemDecisionResponse);
		}

		// Token: 0x06001687 RID: 5767
		public abstract override void DeepClone(out DecisionResponse clone);

		// Token: 0x04000B29 RID: 2857
		public List<Identifier> GameItems = new List<Identifier>();
	}
}
