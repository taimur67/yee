using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000110 RID: 272
	public class ActionEquipArtifactToGamePiece : ActionOrderGOAPNode<OrderAttachGameItemToGamePiece>
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00014A13 File Offset: 0x00012C13
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00014A16 File Offset: 0x00012C16
		public override ActionID ID
		{
			get
			{
				return ActionID.Attach_Artifact_Legion;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00014A1A File Offset: 0x00012C1A
		public override string ActionName
		{
			get
			{
				return string.Format("Equip {0} to {1}", this.Artifact, this.TargetGamePiece);
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00014A32 File Offset: 0x00012C32
		public ActionEquipArtifactToGamePiece(GameItem artifact, GamePiece targetGamePiece)
		{
			this.TargetGamePiece = targetGamePiece;
			this.Artifact = artifact;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00014A48 File Offset: 0x00012C48
		public override void Prepare()
		{
			base.AddConstraint(new WPGamePieceHasFreeSlot(this.TargetGamePiece, FreeSlotsMode.Any));
			base.AddConstraint(new WPGamePieceActive(this.TargetGamePiece));
			base.AddPrecondition(new WPHasAnyArtifact());
			base.AddPrecondition(new WPArtifactInVault(this.Artifact));
			base.AddEffect(new WPLegionHasArtifact(this.TargetGamePiece, true));
			if (this.Artifact == null)
			{
				base.Disable(string.Format("Invalid artifact {0}", this.Artifact));
				return;
			}
			if (this.TargetGamePiece == null)
			{
				base.Disable(string.Format("Invalid game piece {0}", this.TargetGamePiece));
				return;
			}
			CombatStats combatStats = this.OwningPlanner.EstimateArtifactBonusFor(this.Artifact, this.TargetGamePiece);
			if (combatStats.EnumerateStatValues().Any((ModifiableValue boost) => boost > 0))
			{
				base.AddEffect(WPCombatAdvantage.BonusFor(this.TargetGamePiece, combatStats));
			}
			base.Prepare();
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00014B54 File Offset: 0x00012D54
		protected override OrderAttachGameItemToGamePiece GenerateOrder()
		{
			GameItem gameItem = this.OwningPlanner.AIPreviewTurn.FetchGameItem<GameItem>(this.Artifact);
			return new OrderAttachGameItemToGamePiece(this.TargetGamePiece, gameItem, GameItemCategory.None);
		}

		// Token: 0x0400026F RID: 623
		public readonly GameItem Artifact;

		// Token: 0x04000270 RID: 624
		public readonly GamePiece TargetGamePiece;
	}
}
