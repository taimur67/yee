using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000119 RID: 281
	public class ActionMoveCapturePlayersTerritory : ActionOrderGOAPNode<OrderMarchLegion>
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000169B1 File Offset: 0x00014BB1
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return this.Legion != null && this.Legion.SubCategory != GamePieceCategory.Titan;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000169CE File Offset: 0x00014BCE
		public override ActionID ID
		{
			get
			{
				return ActionID.March_Capture_Players_Territory;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000169D2 File Offset: 0x00014BD2
		public override string ActionName
		{
			get
			{
				return string.Format("Legion {0} moves to capture player {1}'s territory", this.Legion, base.Context.DebugName(this.TargetPlayerID));
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000169F5 File Offset: 0x00014BF5
		public ActionMoveCapturePlayersTerritory(GamePiece id, int targetPlayerID, bool legionHasUpkeep = false)
		{
			this.Legion = id;
			this.TargetPlayerID = targetPlayerID;
			this._legionHasUpkeep = legionHasUpkeep;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00016A14 File Offset: 0x00014C14
		public override void Prepare()
		{
			TurnState aipreviewTurn = this.OwningPlanner.AIPreviewTurn;
			if (!aipreviewTurn.CurrentDiplomaticTurn.GetDiplomaticState(new PlayerPair(this.OwningPlanner.PlayerId, this.TargetPlayerID)).AllowCantonCapture(this.OwningPlanner.AIPreviewContext.Diplomacy, this.OwningPlanner.PlayerId, this.TargetPlayerID))
			{
				base.Disable("Node should only be present if canton capture is possible");
				return;
			}
			base.AddConstraint(new WPGamePieceActive(this.Legion));
			base.AddConstraint(new WPLegionCanMove(this.Legion));
			GamePiece stronghold = aipreviewTurn.GetStronghold(this.TargetPlayerID);
			if (stronghold == null)
			{
				base.Disable("Could not find enemy Stronghold");
				return;
			}
			int turnsToReach;
			if (!this.OwningPlanner.TerrainInfluenceMap[stronghold.Location].TryGetTurnsToReach(this.Legion, out turnsToReach, false))
			{
				base.Disable(string.Format("{0} cannot reach {1}", this.Legion, stronghold.Id));
				return;
			}
			base.AddTurnsToReachCostModifier(this.Legion, turnsToReach);
			if (this.Legion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostIncrease(0.3f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPEveryTitanHasOrders());
			}
			base.AddEffect(WPLegionTileSafety.SafetyFromMovement(this.OwningPlanner, this.Legion, stronghold.Location));
			base.AddEffect(new WPStolenCanton(this.TargetPlayerID));
			PlayerState playerState = this.OwningPlanner.TrueTurn.FindPlayerState(this.TargetPlayerID, null);
			if (this.OwningPlanner.TrueTurn.GetDiplomaticStatus(this.OwningPlanner.PlayerState, playerState).DiplomaticState is DiplomaticState_DraconicRazzia && ActionDraconicRazzia.CanBeUsedByArchfiend(this.OwningPlanner.PlayerState))
			{
				base.AddScalarCostIncrease(0.5f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPCombatVsPlayer(playerState.Id));
			}
			base.Prepare();
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00016BE8 File Offset: 0x00014DE8
		protected override OrderMarchLegion GenerateOrder()
		{
			HexCoord[] coords = new HexCoord[]
			{
				HexCoord.Invalid
			};
			return new OrderMarchLegion(this.Legion, AttackOutcomeIntent.Default, FlankIntent.Undefined, coords);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00016C1C File Offset: 0x00014E1C
		private bool TryCreateBestPath(TurnContext context, out List<HexCoord> bestPath)
		{
			bestPath = new List<HexCoord>();
			PathfinderHexboard pathfinderHexboard = new PathfinderHexboard();
			pathfinderHexboard.PopulateMap(context);
			GamePiece movingLegion = context.CurrentTurn.FetchGameItem<GamePiece>(this.Legion);
			PFAgentGamePiece pfagentGamePiece = new PFAgentGamePiece(movingLegion);
			pfagentGamePiece.AvoidanceType = ~GamePieceAvoidance.FriendlyFixture;
			pfagentGamePiece.DestinationAlwaysValid = false;
			pfagentGamePiece.AllowRedeployToDestination = true;
			HexBoard board = this.OwningPlanner.AIPreviewTurn.HexBoard;
			List<Hex> list = IEnumerableExtensions.ToList<Hex>(from hex in context.HexBoard.GetHexesControlledByPlayer(this.TargetPlayerID)
			where board.CantonBordersPlayersRealm(this.OwningPlanner.PlayerState.Id, hex.HexCoord, false) || board.CantonBordersPlayersRealm(-1, hex.HexCoord, true)
			select hex);
			if (IEnumerableExtensions.Any<Hex>(list))
			{
				list.SortOnValueAscending((Hex t) => context.HexBoard.ShortestDistance(t.HexCoord, movingLegion.Location));
				list.RemoveAll((Hex t) => this.OwningPlanner.AIPersistentData.IsHexClaimedForMovement(t.HexCoord));
				foreach (Hex hex2 in list)
				{
					if (pathfinderHexboard.TryFindPath(movingLegion.Location, hex2.HexCoord, pfagentGamePiece, out bestPath))
					{
						if (bestPath.Count > 0)
						{
							bestPath.RemoveAt(0);
						}
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00016D80 File Offset: 0x00014F80
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			List<HexCoord> list;
			if (!this.TryCreateBestPath(context, out list))
			{
				return Result.Failure;
			}
			GamePiece movingLegion = context.CurrentTurn.FetchGameItem<GamePiece>(this.Legion);
			int numberOfHexesToClaim;
			if (!this.OwningPlanner.CanClaimPathForMovement(list, out numberOfHexesToClaim, movingLegion))
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

		// Token: 0x04000290 RID: 656
		public GamePiece Legion;

		// Token: 0x04000291 RID: 657
		public int TargetPlayerID;

		// Token: 0x04000292 RID: 658
		private bool _legionHasUpkeep;

		// Token: 0x04000293 RID: 659
		private const float _pathLengthPenaltyScalar = 0.1f;

		// Token: 0x020007D8 RID: 2008
		private class ExpandTerritoryScoring
		{
			// Token: 0x0600256F RID: 9583 RVA: 0x00080B26 File Offset: 0x0007ED26
			public ExpandTerritoryScoring(HexCoord coord, float score)
			{
				this.HexCoord = coord;
				this.Score = score;
			}

			// Token: 0x04001120 RID: 4384
			public HexCoord HexCoord;

			// Token: 0x04001121 RID: 4385
			public float Score;
		}
	}
}
