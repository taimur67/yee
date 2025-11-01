using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x0200011A RID: 282
	public class ActionMoveExpandBorders : ActionOrderGOAPNode<OrderMarchLegion>
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00016E13 File Offset: 0x00015013
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return this.Legion != null && this.Legion.SubCategory != GamePieceCategory.Titan;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00016E30 File Offset: 0x00015030
		public override string ActionName
		{
			get
			{
				return string.Format("Legion {0} moves to capture neutral cantons", this.Legion);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00016E42 File Offset: 0x00015042
		public override ActionID ID
		{
			get
			{
				return ActionID.March_Expand_Borders;
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00016E46 File Offset: 0x00015046
		public ActionMoveExpandBorders(GamePiece id, bool legionHasUpkeep = false)
		{
			this.Legion = id;
			this._legionHasUpkeep = legionHasUpkeep;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00016E5C File Offset: 0x0001505C
		private IEnumerable<HexCoord> GetCandidateDestinations()
		{
			if (this.Legion == null)
			{
				return Enumerable.Empty<HexCoord>();
			}
			return from t in base.Context.HexBoard.GetAllHexes()
			where t.IsUnclaimed() && base.Context.IsCapturableTileType(t)
			select t.HexCoord;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00016EBC File Offset: 0x000150BC
		public override void Prepare()
		{
			int num = int.MaxValue;
			HexCoord location = HexCoord.Invalid;
			foreach (HexCoord hexCoord in this.GetCandidateDestinations())
			{
				int num2;
				if (this.OwningPlanner.TerrainInfluenceMap[hexCoord].TryGetTurnsToReach(this.Legion, out num2, false) && num2 < num)
				{
					location = hexCoord;
					num = num2;
				}
			}
			if (num == 2147483647)
			{
				base.Disable(string.Format("{0} can't reach any neutral cantons", this.Legion));
				return;
			}
			base.AddTurnsToReachCostModifier(this.Legion, num);
			base.AddConstraint(new WPGamePieceActive(this.Legion));
			base.AddConstraint(new WPLegionCanMove(this.Legion));
			base.AddConstraint(new WPNeutralCantonsAvailable());
			GamePiece legion = this.Legion;
			GamePiece stronghold = this.OwningPlanner.PlayerViewOfTurnState.GetStronghold(this.OwningPlanner.PlayerId);
			if (stronghold != null && IEnumerableExtensions.Contains<HexCoord>(this.OwningPlanner.PlayerViewOfTurnState.HexBoard.GetNeighbours(stronghold.Location, false), legion.Location))
			{
				base.AddEffect(new WPSpawnPoint());
			}
			base.AddEffect(new WPNeutralCanton());
			base.AddEffect(WPLegionTileSafety.SafetyFromMovement(this.OwningPlanner, this.Legion, location));
			if (legion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostIncrease(0.5f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPEveryTitanHasOrders());
			}
			base.Prepare();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00017044 File Offset: 0x00015244
		protected override OrderMarchLegion GenerateOrder()
		{
			HexCoord[] coords = new HexCoord[]
			{
				HexCoord.Invalid
			};
			return new OrderMarchLegion(this.Legion, AttackOutcomeIntent.Default, FlankIntent.Undefined, coords);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00017078 File Offset: 0x00015278
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			PathfinderHexboard pathfinderHexboard = new PathfinderHexboard();
			pathfinderHexboard.PopulateMap(context);
			GamePiece gamePiece;
			if (!context.CurrentTurn.TryFetchGameItem<GamePiece>(playerState.StrongholdId, out gamePiece))
			{
				return Result.Failure;
			}
			List<ActionMoveExpandBorders.ExpandTerritoryScoring> list = new List<ActionMoveExpandBorders.ExpandTerritoryScoring>();
			foreach (HexCoord hexCoord in this.GetCandidateDestinations())
			{
				HexCoord hexCoord2 = context.HexBoard.ToRelativeHex(hexCoord);
				int num;
				float num2;
				int num3;
				if (this.OwningPlanner.TerrainInfluenceMap.InfMap[hexCoord2].TryGetNearestFixtureDistance(out num) && this.OwningPlanner.TerrainInfluenceMap.InfMap[hexCoord2].TryGetControl(playerState.Id, out num2) && this.OwningPlanner.TerrainInfluenceMap.InfMap[hexCoord2].TryGetTurnsToReach(this.Legion, out num3, false) && !this.OwningPlanner.AIPersistentData.IsHexClaimedForMovement(hexCoord2))
				{
					float score = num2 / MathF.Pow((float)num3, 1.5f);
					list.Add(new ActionMoveExpandBorders.ExpandTerritoryScoring(hexCoord2, score));
				}
			}
			if (list.Count <= 0)
			{
				return Result.Failure;
			}
			GamePiece gamePiece2 = context.CurrentTurn.FetchGameItem<GamePiece>(this.Legion);
			int num4 = gamePiece2.GroundMoveDistance;
			PFAgentGamePiece pfagentGamePiece = new PFAgentGamePiece(gamePiece2);
			pfagentGamePiece.AvoidanceType = ~GamePieceAvoidance.FriendlyFixture;
			pfagentGamePiece.DestinationAlwaysValid = false;
			pfagentGamePiece.AllowRedeployToDestination = true;
			list.SortOnValueDescending((ActionMoveExpandBorders.ExpandTerritoryScoring t) => t.Score);
			List<HexCoord> list2 = new List<HexCoord>();
			HexCoord start = gamePiece2.Location;
			while (num4 > 0 && list.Count > 0)
			{
				HexCoord hexCoord3 = ListExtensions.PopAt<ActionMoveExpandBorders.ExpandTerritoryScoring>(list, 0).HexCoord;
				List<HexCoord> list3;
				if (pathfinderHexboard.TryFindPath(start, hexCoord3, pfagentGamePiece, out list3))
				{
					bool flag = true;
					for (int i = 1; i < list3.Count; i++)
					{
						HexCoord item = list3[i];
						if (list2.Contains(item))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						int num5 = 1;
						while (num5 < list3.Count && num4 > 0)
						{
							HexCoord hexCoord4 = list3[num5];
							start = hexCoord4;
							list2.Add(hexCoord4);
							num4--;
							num5++;
						}
					}
				}
			}
			if (list2.Count == 0)
			{
				return Result.Failure;
			}
			int numberOfHexesToClaim;
			if (!this.OwningPlanner.CanClaimPathForMovement(list2, out numberOfHexesToClaim, gamePiece2))
			{
				return Result.Failure;
			}
			base.Order.MovePath = list2;
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			this.OwningPlanner.AIPersistentData.ClaimPathForMovement(list2, numberOfHexesToClaim);
			this.OwningPlanner.RecordPotentialCaptures(base.Order);
			return Result.Success;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00017354 File Offset: 0x00015554
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			return objectiveCondition is ObjectiveCondition_ControlTerritory || base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x04000294 RID: 660
		public readonly GamePiece Legion;

		// Token: 0x04000295 RID: 661
		private bool _legionHasUpkeep;

		// Token: 0x020007DA RID: 2010
		private class ExpandTerritoryScoring
		{
			// Token: 0x06002574 RID: 9588 RVA: 0x00080BC4 File Offset: 0x0007EDC4
			public ExpandTerritoryScoring(HexCoord coord, float score)
			{
				this.HexCoord = coord;
				this.Score = score;
			}

			// Token: 0x04001126 RID: 4390
			public HexCoord HexCoord;

			// Token: 0x04001127 RID: 4391
			public float Score;
		}
	}
}
