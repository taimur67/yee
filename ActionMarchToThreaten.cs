using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000118 RID: 280
	public class ActionMarchToThreaten : ActionOrderGOAPNode<OrderMoveLegion>
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0001632E File Offset: 0x0001452E
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return this.Legion != null && this.Legion.SubCategory != GamePieceCategory.Titan;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0001634B File Offset: 0x0001454B
		public override ActionID ID
		{
			get
			{
				return ActionID.March_Legion_Threaten;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0001634F File Offset: 0x0001454F
		public override string ActionName
		{
			get
			{
				return string.Format("{0} legion {1} to threaten {2}", this.movementType, this.Legion, base.Context.DebugName(this.TargetPlayerID));
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00016378 File Offset: 0x00014578
		private string movementType
		{
			get
			{
				if (!this.TryTeleport)
				{
					return "March";
				}
				return "Teleport";
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001638D File Offset: 0x0001458D
		public ActionMarchToThreaten(GamePiece legion, int legionOwnerID, int targetPlayerID, bool legionHasUpkeep = false, bool tryTeleport = false)
		{
			this.Legion = legion;
			this.LegionOwnerID = legionOwnerID;
			this.LegionHasUpkeep = legionHasUpkeep;
			this.TargetPlayerID = targetPlayerID;
			this.TryTeleport = tryTeleport;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000163BC File Offset: 0x000145BC
		private IEnumerable<HexCoord> GetCandidateDestinations()
		{
			if (this.Legion == null)
			{
				return Enumerable.Empty<HexCoord>();
			}
			return from h in base.Context.HexBoard.GetBorderCantons(this.TargetPlayerID, true)
			where LegionMovementProcessor.IsTraversable(this.OwningPlanner.TrueContext, this.Legion, h, PathMode.March)
			where !LegionMovementProcessor.IsOccupied(this.OwningPlanner.PlayerViewOfTurnState, h)
			where !this.OwningPlanner.AIPersistentData.IsHexClaimedForMovement(h)
			select h;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00016424 File Offset: 0x00014624
		public override void Prepare()
		{
			GamePiece marchingLegion;
			if (!base.Context.CurrentTurn.TryFetchGameItem<GamePiece>(this.Legion, out marchingLegion))
			{
				base.Disable(string.Format("Could not find GamePiece with id: {0}", this.Legion));
				return;
			}
			if (!marchingLegion.CanMove)
			{
				base.Disable(string.Format("Legion {0} can't move", marchingLegion));
				return;
			}
			if (base.Context.HexBoard.GetBorderCantons(this.TargetPlayerID, true).Any((HexCoord candidate) => marchingLegion.Location == candidate))
			{
				base.Disable(string.Format("Legion {0} is already threatening {1}", marchingLegion, this.TargetPlayerID));
				return;
			}
			int num = int.MaxValue;
			HexCoord location = HexCoord.Invalid;
			foreach (HexCoord hexCoord in this.GetCandidateDestinations())
			{
				int num2;
				if (this.OwningPlanner.TerrainInfluenceMap[hexCoord].TryGetTurnsToReach(marchingLegion, out num2, false) && num2 < num)
				{
					location = hexCoord;
					num = num2;
				}
			}
			if (num == 2147483647)
			{
				base.Disable(string.Format("Legion {0} cannot reach {1}'s border", marchingLegion, base.Context.DebugName(this.TargetPlayerID)));
				return;
			}
			base.AddTurnsToReachCostModifier(marchingLegion, num);
			base.AddConstraint(new WPGamePieceActive(this.Legion));
			base.AddConstraint(new WPLegionCanMove(this.Legion));
			base.AddEffect(new WPThreaten(this.LegionOwnerID, this.TargetPlayerID));
			base.AddEffect(WPLegionTileSafety.SafetyFromMovement(this.OwningPlanner, this.Legion, location));
			if (marchingLegion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostModifier(-0.3f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPEveryTitanHasOrders());
			}
			base.Prepare();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00016614 File Offset: 0x00014814
		protected override OrderMoveLegion GenerateOrder()
		{
			if (this.TryTeleport)
			{
				return new OrderTeleportLegion(this.Legion, AttackOutcomeIntent.Default, FlankIntent.Undefined);
			}
			HexCoord[] coords = new HexCoord[]
			{
				HexCoord.Invalid
			};
			return new OrderMarchLegion(this.Legion, AttackOutcomeIntent.Default, FlankIntent.Undefined, coords);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00016664 File Offset: 0x00014864
		private Result SubmitTeleportAction(TurnContext context, PlayerState playerState, OrderTeleportLegion teleportOrder, GamePiece movingLegion, List<HexCoord> candidateDestinations)
		{
			HexCoord intendedDestination = candidateDestinations.SelectMinOrDefault((HexCoord t) => context.HexBoard.ShortestDistance(t, movingLegion.Location), default(HexCoord));
			if (intendedDestination == HexCoord.Invalid)
			{
				return Result.Failure;
			}
			HexCoord hexCoord;
			if (!context.TryFindTeleportDestination(movingLegion, intendedDestination, out hexCoord, null))
			{
				return Result.Failure;
			}
			teleportOrder.DestinationHex = hexCoord;
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			this.OwningPlanner.AIPersistentData.RegisterHexClaimedForMovement(hexCoord);
			this.OwningPlanner.RecordPotentialCaptures(teleportOrder);
			return Result.Success;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00016718 File Offset: 0x00014918
		private Result SubmitMarchAction(TurnContext context, PlayerState playerState, OrderMarchLegion marchOrder, GamePiece movingLegion, List<HexCoord> candidateDestinations)
		{
			PathfinderHexboard hexPathfinder = this.OwningPlanner.HexPathfinder;
			PFAgentGamePiece agentData = new PFAgentGamePiece(null)
			{
				GamePiece = movingLegion,
				AvoidanceType = ~GamePieceAvoidance.FriendlyFixture,
				DestinationAlwaysValid = false,
				AllowRedeployToDestination = true
			};
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>((from coord in candidateDestinations
			where this.OwningPlanner.TerrainInfluenceMap[coord].CanReach(movingLegion, false)
			select coord).OrderBy(delegate(HexCoord coord)
			{
				int result;
				if (this.OwningPlanner.TerrainInfluenceMap[coord].TryGetShortestPathLength(movingLegion, out result, false))
				{
					return result;
				}
				return int.MaxValue;
			}).Take(5));
			if (list.Count == 0)
			{
				return Result.Failure;
			}
			List<HexCoord> list2 = null;
			foreach (HexCoord end in list)
			{
				if (hexPathfinder.TryFindPath(movingLegion.Location, end, agentData, out list2))
				{
					break;
				}
			}
			if (list2 == null || list2.Count == 0)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Warn(string.Format("TerrainInfluenceMap's top {0} 'nearest hex' suggestions all proved inaccessible.", 5));
				}
				return Result.Failure;
			}
			list2.RemoveAt(0);
			marchOrder.MoveIntent = FlankIntent.ThreatenBorders;
			marchOrder.MovePath = list2;
			int numberOfHexesToClaim;
			if (!this.OwningPlanner.CanClaimPathForMovement(list2, out numberOfHexesToClaim, movingLegion))
			{
				return Result.Failure;
			}
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			this.OwningPlanner.AIPersistentData.ClaimPathForMovement(list2, numberOfHexesToClaim);
			this.OwningPlanner.RecordPotentialCaptures(marchOrder);
			return Result.Success;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000168A8 File Offset: 0x00014AA8
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			GamePiece movingLegion = this.Legion;
			if (movingLegion == null)
			{
				return Result.Failure;
			}
			if (context.CurrentTurn.FindPlayerState(this.TargetPlayerID, null) == null)
			{
				return Result.Failure;
			}
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>(from h in this.GetCandidateDestinations()
			where h != movingLegion.Location
			select h);
			if (list.Count == 0)
			{
				return Result.Failure;
			}
			OrderMoveLegion order = base.Order;
			OrderTeleportLegion orderTeleportLegion = order as OrderTeleportLegion;
			Result result;
			if (orderTeleportLegion == null)
			{
				OrderMarchLegion orderMarchLegion = order as OrderMarchLegion;
				if (orderMarchLegion == null)
				{
					result = Result.Failure;
				}
				else
				{
					result = this.SubmitMarchAction(context, playerState, orderMarchLegion, movingLegion, list);
				}
			}
			else
			{
				result = this.SubmitTeleportAction(context, playerState, orderTeleportLegion, movingLegion, list);
			}
			return result;
		}

		// Token: 0x0400028A RID: 650
		private const int _shortlistSize = 5;

		// Token: 0x0400028B RID: 651
		public readonly GamePiece Legion;

		// Token: 0x0400028C RID: 652
		public bool LegionHasUpkeep;

		// Token: 0x0400028D RID: 653
		public int LegionOwnerID;

		// Token: 0x0400028E RID: 654
		public int TargetPlayerID;

		// Token: 0x0400028F RID: 655
		public bool TryTeleport;
	}
}
