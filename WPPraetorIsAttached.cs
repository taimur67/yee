using System;

namespace LoG
{
	// Token: 0x0200018A RID: 394
	public class WPPraetorIsAttached : WorldProperty
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x00022ED9 File Offset: 0x000210D9
		public WPPraetorIsAttached(Identifier praetorId, GamePieceCategory controllingGamePieceCategory = GamePieceCategory.None)
		{
			this.PraetorID = praetorId;
			this.ControllingGamePieceCategory = controllingGamePieceCategory;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00022EF0 File Offset: 0x000210F0
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			GamePiece controllingPiece = viewContext.CurrentTurn.GetControllingPiece(this.PraetorID);
			return controllingPiece != null && controllingPiece.ControllingPlayerId == playerState.Id && (this.ControllingGamePieceCategory == GamePieceCategory.None || controllingPiece.SubCategory == this.ControllingGamePieceCategory);
		}

		// Token: 0x0400035D RID: 861
		public Identifier PraetorID;

		// Token: 0x0400035E RID: 862
		public GamePieceCategory ControllingGamePieceCategory;
	}
}
