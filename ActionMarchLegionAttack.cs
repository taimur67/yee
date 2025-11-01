using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000115 RID: 277
	public class ActionMarchLegionAttack : ActionOrderGOAPNode<OrderMarchLegion>
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0001541C File Offset: 0x0001361C
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return this.MarchingLegion != null && this.MarchingLegion.SubCategory != GamePieceCategory.Titan;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00015439 File Offset: 0x00013639
		public override ActionID ID
		{
			get
			{
				return ActionID.March_Legion_Attack;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001543C File Offset: 0x0001363C
		public override string ActionName
		{
			get
			{
				return string.Format("March legion {0} towards {1}", this.MarchingLegion, this.TargetLegion);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00015454 File Offset: 0x00013654
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00015457 File Offset: 0x00013657
		public ActionMarchLegionAttack(GamePiece legionID, GamePiece targetID, int targetControllingPlayer, float advantageRequired, bool legionHasUpkeep = false)
		{
			this.MarchingLegion = legionID;
			this.TargetLegion = targetID;
			this.AdvantageRequired = advantageRequired;
			this.LegionHasUpkeep = legionHasUpkeep;
			this.TargetControllingPlayerID = targetControllingPlayer;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00015484 File Offset: 0x00013684
		public override void Prepare()
		{
			TurnState aipreviewTurn = this.OwningPlanner.AIPreviewTurn;
			if (this.MarchingLegion == null)
			{
				base.Disable("Invalid attacking legion");
				return;
			}
			if (this.MarchingLegion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostModifier(-0.5f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPEveryTitanHasOrders());
			}
			if (this.TargetLegion == null || this.TargetLegion.Status != GameItemStatus.InPlay || this.TargetLegion.Location == HexCoord.Invalid)
			{
				base.Disable("Invalid target legion");
				return;
			}
			if (this.TargetControllingPlayerID != -1)
			{
				base.AddPrecondition(new WPCanAttack(this.TargetControllingPlayerID, false));
			}
			if (this.OwningPlanner.PlayerState.ArchfiendId == "Beelzebub" && this.OwningPlanner.PlayerState.RitualState.HasSpace)
			{
				base.AddPrecondition(new WPHellsMaw());
			}
			InfluenceData influenceData;
			int num;
			if (!this.OwningPlanner.TerrainInfluenceMap.InfMap.TryGetValue(this.TargetLegion.Location, out influenceData) || !influenceData.TryGetTurnsToReach(this.MarchingLegion, out num, true))
			{
				base.Disable("Unreachable destination");
				return;
			}
			if (this.OwningPlanner.TrueContext.CurrentTurn.GetDiplomaticStatus(this.OwningPlanner.PlayerId, this.TargetControllingPlayerID).DiplomaticState is DiplomaticState_DraconicRazzia && num > 1)
			{
				base.Disable(string.Format("Razzia will end before {0} reaches {1}", this.MarchingLegion, this.TargetLegion));
				return;
			}
			base.AddTurnsToReachCostModifier(this.MarchingLegion, num);
			base.AddConstraint(new WPGamePieceActive(this.MarchingLegion));
			base.AddConstraint(new WPGamePieceActive(this.TargetLegion));
			base.AddConstraint(new WPLegionCanMove(this.MarchingLegion));
			base.AddPrecondition(WPCombatAdvantage.MeetsThreshold(this.MarchingLegion, this.TargetLegion, this.AdvantageRequired));
			base.AddEffect(new WPCombatVsGamepiece(this.TargetLegion));
			base.AddEffect(new WPCombatVsPlayer(this.TargetControllingPlayerID));
			if (this.OwningPlanner.TileIsSafeForLegion(this.MarchingLegion, this.TargetLegion.Location))
			{
				base.AddEffect(WPLegionTileSafety.SafetyFromMovement(this.OwningPlanner, this.MarchingLegion, this.TargetLegion.Location));
			}
			if (this.TargetControllingPlayerID == -1)
			{
				foreach (PlayerState playerState in aipreviewTurn.EnumeratePlayerStatesExcept(this.OwningPlanner.PlayerId, false, false))
				{
					base.AddEffect(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, playerState.Id, 0.5f));
				}
			}
			Cost cost = new Cost();
			int battlePrestigeReward = BattleProcessor.GetBattlePrestigeReward(this.OwningPlanner.PlayerViewOfTurnState, this.MarchingLegion, this.TargetLegion);
			cost.Set(ResourceTypes.Prestige, battlePrestigeReward);
			base.AddEffect(new WPTribute(cost));
			if (this.MarchingLegion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostReduction(0.5f, PFCostModifier.Heuristic_Bonus);
			}
			base.Prepare();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000157A0 File Offset: 0x000139A0
		private bool DoesPathRequireVendetta(ref List<HexCoord> path)
		{
			if (path.Count <= this.MarchingLegion.GroundMoveDistance)
			{
				return true;
			}
			int count = (int)MathF.Min((float)path.Count, (float)this.MarchingLegion.GroundMoveDistance);
			foreach (HexCoord hexCoord in path.GetRange(0, count))
			{
				if (!LegionMovementProcessor.HasRightOfEntry(this.OwningPlanner.PlayerViewOfTurnState, this.OwningPlanner.PlayerState, this.MarchingLegion, hexCoord))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00015858 File Offset: 0x00013A58
		private bool DeterminePath(ref List<HexCoord> path)
		{
			path = IEnumerableExtensions.ToList<HexCoord>(this.OwningPlanner.FindTerrainPath(this.MarchingLegion, this.MarchingLegion.Location, this.TargetLegion.Location, ~GamePieceAvoidance.FriendlyFixture, true, true, false));
			if (path.Count > 0)
			{
				path.RemoveAt(0);
			}
			return path.Count > 0;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x000158B4 File Offset: 0x00013AB4
		protected override OrderMarchLegion GenerateOrder()
		{
			return new OrderMarchLegion(this.MarchingLegion, AttackOutcomeIntent.Default, FlankIntent.Undefined, Array.Empty<HexCoord>());
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x000158D0 File Offset: 0x00013AD0
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			List<HexCoord> list = new List<HexCoord>();
			if (!this.DeterminePath(ref list) || list.Count == 0)
			{
				return Result.Failure;
			}
			int numberOfHexesToClaim;
			if (!this.OwningPlanner.CanClaimPathForMovement(list, out numberOfHexesToClaim, this.MarchingLegion))
			{
				return Result.Failure;
			}
			base.Order.MovePath = list;
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (list.Count <= this.MarchingLegion.GroundMoveDistance)
			{
				HexCoord location = this.TargetLegion.Location;
				this.OwningPlanner.AIPersistentData.RegisterExpectedBattle(context, location, this.MarchingLegion);
			}
			this.OwningPlanner.AIPersistentData.ClaimPathForMovement(list, numberOfHexesToClaim);
			this.OwningPlanner.RecordPotentialCaptures(base.Order);
			return Result.Success;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001599F File Offset: 0x00013B9F
		public override void OnActionFailed()
		{
			base.OnActionFailed();
			ActionMovement.RegisterBlockages(this.OwningPlanner, this.MarchingLegion, this.TargetLegion.Location, true, true, true);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000159C6 File Offset: 0x00013BC6
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			return objectiveCondition is ObjectiveCondition_DestroyLegions || objectiveCondition is ObjectiveCondition_LevelUpLegion || base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x0400027D RID: 637
		public int TargetControllingPlayerID;

		// Token: 0x0400027E RID: 638
		public float AdvantageRequired;

		// Token: 0x0400027F RID: 639
		public bool LegionHasUpkeep;

		// Token: 0x04000280 RID: 640
		public readonly GamePiece MarchingLegion;

		// Token: 0x04000281 RID: 641
		public readonly GamePiece TargetLegion;
	}
}
