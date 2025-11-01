using System;
using System.Linq;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020000FA RID: 250
	public class ActionCastStealGameItem : ActionCastRitual<StealGameItemRitualOrder>
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00012408 File Offset: 0x00010608
		public override ActionID ID
		{
			get
			{
				GameItemCategory targetCategory = this._targetCategory;
				ActionID result;
				if (targetCategory != GameItemCategory.Artifact)
				{
					if (targetCategory != GameItemCategory.Praetor)
					{
						result = ActionID.Undefined;
					}
					else
					{
						result = ActionID.Cast_Steal_Praetor;
					}
				}
				else
				{
					result = ActionID.Cast_Steal_Artifact;
				}
				return result;
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00012434 File Offset: 0x00010634
		protected override string GetRitualId()
		{
			GameItemCategory targetCategory = this._targetCategory;
			string result;
			if (targetCategory != GameItemCategory.Artifact)
			{
				if (targetCategory != GameItemCategory.Praetor)
				{
					result = null;
				}
				else
				{
					result = "bribe_praetor";
				}
			}
			else
			{
				result = "pilfer_artifacts";
			}
			return result;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00012465 File Offset: 0x00010665
		protected override PowerType GetPowerType()
		{
			return PowerType.Deceit;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00012468 File Offset: 0x00010668
		protected override Identifier GetTargetItemId()
		{
			return this._targetItemId;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00012470 File Offset: 0x00010670
		protected override int GetTargetPlayerId()
		{
			return this._targetPlayer;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00012478 File Offset: 0x00010678
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0001247B File Offset: 0x0001067B
		protected override int CooldownDuration
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001247E File Offset: 0x0001067E
		public ActionCastStealGameItem(int player, Identifier targetItemId, GameItemCategory targetItemCategory)
		{
			this._targetPlayer = player;
			this._targetItemId = targetItemId;
			this._targetCategory = targetItemCategory;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001249C File Offset: 0x0001069C
		private void PrepareArtifact(PlayerState targetPlayer, Artifact artifactToSteal)
		{
			base.AddEffect(new WPArtifactInVault(artifactToSteal.Id));
			if (artifactToSteal.SubCategory == ArtifactCategory.War)
			{
				base.AddEffect(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, targetPlayer.Id, 0.5f));
				GamePiece gamePiece;
				if (this.OwningPlanner.PlayerViewOfTurnState.TryFindControllingPiece(artifactToSteal, out gamePiece))
				{
					CombatStats bonus = this.OwningPlanner.EstimateArtifactBonusFor(artifactToSteal, gamePiece);
					base.AddEffect(WPCombatAdvantage.BonusFor(gamePiece, bonus));
				}
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00012520 File Offset: 0x00010720
		private void PreparePraetor(PlayerState targetPlayer, Praetor praetorToSteal)
		{
			base.AddEffect(new WPHasAnyPraetor());
			base.AddEffect(new WPDuelAdvantage(targetPlayer.Id));
			GamePiece gamePiece;
			if (this.OwningPlanner.PlayerViewOfTurnState.TryFindControllingPiece(praetorToSteal, out gamePiece))
			{
				PraetorStaticData praetorStaticData = base.GameDatabase.Fetch<PraetorStaticData>(praetorToSteal.StaticDataId);
				if (praetorStaticData == null)
				{
					base.Disable(string.Format("Item {0} has no PraetorStaticData", praetorToSteal));
					return;
				}
				CombatStats bonus = praetorStaticData.Components.OfType<GamePieceModifierStaticData>().CalculatePowerChange(gamePiece);
				base.AddEffect(WPCombatAdvantage.BonusAgainst(gamePiece.Id, bonus));
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000125B4 File Offset: 0x000107B4
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			int targetPlayerId = this.GetTargetPlayerId();
			PlayerState playerState = this.OwningPlanner.TrueTurn.FindPlayerState(targetPlayerId, null);
			if (playerState == null || playerState.Id == -2147483648 || playerState.Id == -1)
			{
				base.Disable(string.Format("Invalid target player {0}", targetPlayerId));
				return;
			}
			GameItem gameItem = this.OwningPlanner.TrueTurn.FetchGameItem(this._targetItemId);
			if (gameItem == null)
			{
				base.Disable(string.Format("Invalid target item {0}", this._targetItemId));
				return;
			}
			Artifact artifact = gameItem as Artifact;
			if (artifact == null)
			{
				Praetor praetor = gameItem as Praetor;
				if (praetor == null)
				{
					base.Disable(string.Format("Target item has invalid type {0}", gameItem.Category));
					return;
				}
				this.PreparePraetor(playerState, praetor);
			}
			else
			{
				this.PrepareArtifact(playerState, artifact);
			}
			if (IEnumerableExtensions.Contains<Identifier>(playerState.VaultedItems, this._targetItemId))
			{
				base.AddPrecondition(new WPVaultRevealed(targetPlayerId));
			}
			base.AddEffect(new WPUndermineArchfiend(targetPlayerId));
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000126C4 File Offset: 0x000108C4
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			ObjectiveCondition_Steal objectiveCondition_Steal = objectiveCondition as ObjectiveCondition_Steal;
			if (objectiveCondition_Steal != null)
			{
				if (this.ID == ActionID.Cast_Steal_Artifact)
				{
					GameItemCategory? category = objectiveCondition_Steal.Category;
					GameItemCategory gameItemCategory = GameItemCategory.Artifact;
					if (category.GetValueOrDefault() == gameItemCategory & category != null)
					{
						return true;
					}
				}
				if (this.ID == ActionID.Cast_Steal_Praetor)
				{
					GameItemCategory? category = objectiveCondition_Steal.Category;
					GameItemCategory gameItemCategory = GameItemCategory.Praetor;
					if (category.GetValueOrDefault() == gameItemCategory & category != null)
					{
						return true;
					}
				}
			}
			return base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x04000238 RID: 568
		public const string RitualIdArtifact = "pilfer_artifacts";

		// Token: 0x04000239 RID: 569
		public const string RitualIdPraetor = "bribe_praetor";

		// Token: 0x0400023A RID: 570
		private readonly GameItemCategory _targetCategory;

		// Token: 0x0400023B RID: 571
		private readonly Identifier _targetItemId;

		// Token: 0x0400023C RID: 572
		private readonly int _targetPlayer;
	}
}
