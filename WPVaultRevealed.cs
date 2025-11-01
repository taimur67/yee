using System;

namespace LoG
{
	// Token: 0x0200019C RID: 412
	public class WPVaultRevealed : WorldProperty
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x0002346C File Offset: 0x0002166C
		public WPVaultRevealed(int targetPlayerId)
		{
			this.TargetPlayerId = targetPlayerId;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002347C File Offset: 0x0002167C
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			PlayerKnowledgeContext orCreateKnowledgeContext = playerState.GetOrCreateKnowledgeContext(this.TargetPlayerId);
			return orCreateKnowledgeContext.LastRevealedVault > 0 && orCreateKnowledgeContext.LastRevealedVault >= viewContext.CurrentTurn.TurnValue - 10;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000234BC File Offset: 0x000216BC
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPVaultRevealed wpvaultRevealed = precondition as WPVaultRevealed;
			if (wpvaultRevealed == null)
			{
				return WPProvidesEffect.No;
			}
			if (wpvaultRevealed.TargetPlayerId != this.TargetPlayerId)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x04000370 RID: 880
		public readonly int TargetPlayerId;
	}
}
