using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000113 RID: 275
	public class ActionEquipPrestigeArtifactToPoP : ActionOrderGOAPNode<OrderAttachGameItemToGamePiece>
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00014D77 File Offset: 0x00012F77
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00014D7A File Offset: 0x00012F7A
		public override ActionID ID
		{
			get
			{
				return ActionID.Attach_Prestige_Artifact_To_PoP;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00014D7E File Offset: 0x00012F7E
		public override string ActionName
		{
			get
			{
				return "Attach " + base.Context.DebugName(this.ArtifactID) + " to PoP to produce prestige";
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00014DA0 File Offset: 0x00012FA0
		public ActionEquipPrestigeArtifactToPoP(Identifier artifactID)
		{
			this.ArtifactID = artifactID;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00014DAF File Offset: 0x00012FAF
		public override void Prepare()
		{
			base.AddConstraint(new WPAnyFixturesWithFreeSlots());
			base.AddPrecondition(new WPArtifactInVault(this.ArtifactID));
			base.AddEffect(new WPPrestigeProduction(WorldProperty.MaxWeight));
			base.Prepare();
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00014DE3 File Offset: 0x00012FE3
		protected override OrderAttachGameItemToGamePiece GenerateOrder()
		{
			return new OrderAttachGameItemToGamePiece(Identifier.Invalid, null, GameItemCategory.None)
			{
				Priority = ActionOrderPriority.Low
			};
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00014DF4 File Offset: 0x00012FF4
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			GamePiece gamePiece = IEnumerableExtensions.FirstOrDefault<GamePiece>(from t in context.CurrentTurn.GetAllGamePiecesForPlayer(playerState.Id)
			where this.OwningPlanner.AIPreviewContext.IsValidTransfer(this.ArtifactID, t)
			select t);
			if (gamePiece == null)
			{
				return Result.Failure;
			}
			base.Order.GameItemId = this.ArtifactID;
			base.Order.TargetPieceId = gamePiece.Id;
			return base.SubmitAction(context, playerState);
		}

		// Token: 0x0400027A RID: 634
		public Identifier ArtifactID;
	}
}
