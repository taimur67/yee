using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000116 RID: 278
	public class ActionMarchLegionOnPandaemonium : ActionOrderGOAPNode<OrderMarchLegion>
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x000159E3 File Offset: 0x00013BE3
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return this.Legion != null && this.Legion.SubCategory != GamePieceCategory.Titan;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00015A00 File Offset: 0x00013C00
		public override ActionID ID
		{
			get
			{
				return ActionID.March_On_Pandaemonium;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00015A04 File Offset: 0x00013C04
		public override string ActionName
		{
			get
			{
				return string.Format("March legion {0} towards {1} w/ {2} flankers", this.Legion, this.Pandaemonium, this.FlankersRequired);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00015A27 File Offset: 0x00013C27
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00015A2A File Offset: 0x00013C2A
		public ActionMarchLegionOnPandaemonium(GamePiece legion, GamePiece pandaemonium, float advantageRequired, int flankersRequired = 0)
		{
			this.FlankersRequired = flankersRequired;
			this.AdvantageRequired = advantageRequired;
			this.Pandaemonium = pandaemonium;
			this.Legion = legion;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00015A58 File Offset: 0x00013C58
		public override void Prepare()
		{
			if (this.Legion == null)
			{
				base.Disable(string.Format("Invalid marching Game Piece {0}", this.Legion));
				return;
			}
			if (this.Pandaemonium == null)
			{
				base.Disable(string.Format("Invalid target POP {0}", this.Pandaemonium));
				return;
			}
			int turnsToReach;
			if (!this.OwningPlanner.TerrainInfluenceMap[this.Pandaemonium.Location].TryGetTurnsToReach(this.Legion, out turnsToReach, false))
			{
				base.Disable(string.Format("{0} unable to reach Pandaemonium", this.Legion));
				return;
			}
			base.AddTurnsToReachCostModifier(this.Legion, turnsToReach);
			base.AddConstraint(new WPGamePieceActive(this.Pandaemonium));
			base.AddConstraint(new WPLegionCanMove(this.Legion));
			base.AddConstraint(new WPAccessToGamePiece(this.Legion, this.Pandaemonium));
			if (this.Legion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostModifier(-0.8f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPEveryTitanHasOrders());
			}
			if (this.FlankersRequired > 0)
			{
				base.AddPrecondition(WPForcesFlanking.RequiresFlankersToAssist(this.Pandaemonium, this.Legion, this.FlankersRequired));
			}
			if (this.AdvantageRequired > 0f)
			{
				base.AddPrecondition(WPCombatAdvantage.MeetsThreshold(this.Legion, this.Pandaemonium, this.AdvantageRequired));
			}
			base.AddEffect(new WPCombatVsGamepiece(this.Pandaemonium));
			base.AddEffect(new WPCanEliminate(int.MinValue));
			if (GamePiece.CalcCombatAdvantageAtPosition(this.OwningPlanner.TrueContext, this.Legion, this.Pandaemonium) > 0.1f)
			{
				base.AddEffect(new WPSpecificPopCaptured(this.Pandaemonium));
			}
			else if (this.AdvantageRequired <= 0f)
			{
				base.AddPrecondition(new WPGamePieceHasVaultItemsAttached(this.Legion)
				{
					InvertLogic = true
				});
			}
			base.AddEffect(WPLegionTileSafety.SafetyFromMovement(this.OwningPlanner, this.Legion, this.Pandaemonium.Location));
			base.Prepare();
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00015C67 File Offset: 0x00013E67
		protected override OrderMarchLegion GenerateOrder()
		{
			return new OrderMarchLegion(this.Legion, AttackOutcomeIntent.Default, FlankIntent.Undefined, Array.Empty<HexCoord>());
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00015C80 File Offset: 0x00013E80
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			List<HexCoord> list = new List<HexCoord>();
			if (!this.DeterminePath(ref list) || list.Count == 0)
			{
				return Result.Failure;
			}
			int numberOfHexesToClaim;
			if (!this.OwningPlanner.CanClaimPathForMovement(list, out numberOfHexesToClaim, this.Legion))
			{
				return Result.Failure;
			}
			base.Order.MovePath = list;
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			this.OwningPlanner.AIPersistentData.ClaimPathForMovement(list, numberOfHexesToClaim);
			this.OwningPlanner.RecordPotentialCaptures(base.Order);
			return Result.Success;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00015D0E File Offset: 0x00013F0E
		public override void OnActionFailed()
		{
			base.OnActionFailed();
			ActionMovement.RegisterBlockages(this.OwningPlanner, this.Legion, this.Pandaemonium.Location, true, false, true);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00015D38 File Offset: 0x00013F38
		private bool DeterminePath(ref List<HexCoord> path)
		{
			path = IEnumerableExtensions.ToList<HexCoord>(this.OwningPlanner.FindTerrainPath(this.Legion, this.Legion.Location, this.Pandaemonium.Location, ~GamePieceAvoidance.FriendlyFixture, true, false, true));
			if (path.Count > 0)
			{
				path.RemoveAt(0);
			}
			return path.Count > 0;
		}

		// Token: 0x04000282 RID: 642
		private float AdvantageRequired;

		// Token: 0x04000283 RID: 643
		private readonly GamePiece Legion;

		// Token: 0x04000284 RID: 644
		private readonly GamePiece Pandaemonium;

		// Token: 0x04000285 RID: 645
		private int FlankersRequired = 2;
	}
}
