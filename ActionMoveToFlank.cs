using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x0200011C RID: 284
	public class ActionMoveToFlank : ActionOrderGOAPNode<OrderMoveLegion>
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001737F File Offset: 0x0001557F
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00017382 File Offset: 0x00015582
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return this.FlankingLegion != null && this.FlankingLegion.SubCategory != GamePieceCategory.Titan;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x000173A0 File Offset: 0x000155A0
		public override ActionID ID
		{
			get
			{
				ActionID result;
				switch (this.FlankIntent)
				{
				case FlankIntent.FlankForAttack:
					result = ActionID.March_Legion_Flank;
					break;
				case FlankIntent.Flank_Heal:
					result = ActionID.March_Legion_Heal;
					break;
				case FlankIntent.Flank_ReinforceStronghold:
					result = ActionID.March_Legion_Reinforce_Stronghold;
					break;
				case FlankIntent.Flank_Support_Battle:
					result = ActionID.March_Support_Battle;
					break;
				default:
					result = ActionID.Undefined;
					break;
				}
				return result;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x000173E3 File Offset: 0x000155E3
		public override ActionOrderPriority Priority
		{
			get
			{
				if (this.FlankIntent != FlankIntent.Flank_Support_Battle)
				{
					return base.Priority;
				}
				return ActionOrderPriority.High_AlwaysFirst;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x000173F6 File Offset: 0x000155F6
		private string targetName
		{
			get
			{
				GamePiece flankedGamePiece = this.FlankedGamePiece;
				return ((flankedGamePiece != null) ? flankedGamePiece.ToString() : null) ?? this.FlankedHexCoord.ToString();
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001741F File Offset: 0x0001561F
		private string movementMode
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00017434 File Offset: 0x00015634
		public override string ActionName
		{
			get
			{
				return string.Format("{0} legion {1} flank {2}, intent {3}", new object[]
				{
					this.movementMode,
					this.FlankingLegion,
					this.targetName,
					this.FlankIntent
				});
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001746F File Offset: 0x0001566F
		public static ActionMoveToFlank Location(FlankIntent intent, GamePiece flankingLegionID, HexCoord location, bool tryTeleport = false)
		{
			return new ActionMoveToFlank(intent, flankingLegionID, null, location, tryTeleport);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001747B File Offset: 0x0001567B
		public static ActionMoveToFlank GamePiece(FlankIntent intent, GamePiece flankingLegionID, GamePiece flankedGamePieceIdentifier, bool tryTeleport = false)
		{
			return new ActionMoveToFlank(intent, flankingLegionID, flankedGamePieceIdentifier, HexCoord.Invalid, tryTeleport);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001748B File Offset: 0x0001568B
		private ActionMoveToFlank(FlankIntent intent, GamePiece flankingLegion, GamePiece flankedGamePiece, HexCoord flankedLocation, bool tryTeleport)
		{
			this.FlankingLegion = flankingLegion;
			this.FlankedGamePiece = flankedGamePiece;
			this.FlankIntent = intent;
			this.FlankedHexCoord = flankedLocation;
			this.TryTeleport = tryTeleport;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000174C4 File Offset: 0x000156C4
		public override void Prepare()
		{
			TurnContext aipreviewContext = this.OwningPlanner.AIPreviewContext;
			TurnState currentTurn = aipreviewContext.CurrentTurn;
			if (this.FlankingLegion == null)
			{
				base.Disable(string.Format("Could not find legion {0}", this.FlankingLegion));
				return;
			}
			if (this.FlankedGamePiece == null && this.FlankedHexCoord == HexCoord.Invalid)
			{
				base.Disable(string.Format("Could not find legion {0}, and no destination was specified", this.FlankedGamePiece));
				return;
			}
			if (this.FlankedHexCoord == HexCoord.Invalid)
			{
				this.FlankedHexCoord = this.FlankedGamePiece.Location;
			}
			if (currentTurn.HexBoard.AreAdjacent(this.FlankedHexCoord, this.FlankingLegion.Location))
			{
				base.Disable(string.Format("{0} is already flanking {1}", this.FlankingLegion.NameKey, this.FlankedHexCoord));
				return;
			}
			base.AddConstraint(new WPGamePieceActive(this.FlankingLegion));
			base.AddConstraint(new WPLegionCanMove(this.FlankingLegion));
			if (this.FlankedGamePiece != null)
			{
				base.AddConstraint(new WPGamePieceActive(this.FlankedGamePiece));
			}
			switch (this.FlankIntent)
			{
			case FlankIntent.FlankForAttack:
				if (this.FlankedGamePiece == null)
				{
					base.Disable("FlankForAttack intention requires a non-null target GamePiece");
					return;
				}
				base.AddEffect(WPForcesFlanking.ProvidesFlanking(this.FlankedGamePiece, this.FlankingLegion));
				base.AddEffect(WPCombatAdvantage.BonusAgainst(this.FlankedGamePiece, this.FlankedGamePiece.CombatStats));
				break;
			case FlankIntent.Flank_Heal:
				if (this.FlankedGamePiece == null)
				{
					base.Disable(this.FlankingLegion.NameKey + " cannot be healed by null GamePiece");
					return;
				}
				if (!this.FlankingLegion.CanBeHealedBy(currentTurn, this.FlankedGamePiece) || !this.FlankedGamePiece.IsFixture())
				{
					base.Disable(this.FlankedGamePiece.NameKey + " cannot provide healing to " + this.FlankingLegion.NameKey);
					return;
				}
				base.AddConstraint(new WPLegionBadlyDamaged(this.FlankingLegion, 0.5f));
				base.AddEffect(new WPOpportunisticHeal());
				base.AddEffect(WPCombatAdvantage.FromHealing(this.FlankingLegion, 1f));
				break;
			case FlankIntent.Flank_ReinforceStronghold:
				base.AddEffect(new WPReinforceStronghold());
				break;
			case FlankIntent.Flank_Support_Battle:
				base.AddEffect(new WPOpportunisticSupport());
				if (this.FlankedGamePiece != null)
				{
					base.AddEffect(WPCombatAdvantage.BonusAgainst(this.FlankedGamePiece, this.FlankingLegion.CombatStats));
				}
				break;
			}
			if (this.TryTeleport)
			{
				HexCoord teleportFlankingPosition = this.GetTeleportFlankingPosition(aipreviewContext);
				if (teleportFlankingPosition == HexCoord.Invalid)
				{
					base.Disable(string.Format("{0} cannot find a tile adjacent to ${1} to teleport to", this.FlankingLegion, this.FlankedHexCoord));
					return;
				}
				if (this.OwningPlanner.TileIsSafeForLegion(this.FlankingLegion, teleportFlankingPosition))
				{
					base.AddEffect(WPLegionTileSafety.SafetyFromMovement(this.OwningPlanner, this.FlankingLegion, teleportFlankingPosition));
				}
				else if (this.FlankIntent == FlankIntent.Flank_Heal)
				{
					base.Disable(this.FlankingLegion.NameKey + " cannot be healed in Fumes");
					return;
				}
				if (aipreviewContext.HexBoard.ShortestDistance(this.FlankingLegion.Location, teleportFlankingPosition) <= this.FlankingLegion.TeleportDistance)
				{
					base.AddScalarCostModifier(-0.7f, PFCostModifier.Terrain_Cost);
				}
			}
			else
			{
				HexCoord marchFlankingPosition = this.GetMarchFlankingPosition(aipreviewContext);
				if (marchFlankingPosition == HexCoord.Invalid)
				{
					base.Disable(string.Format("{0} cannot find a tile adjacent to ${1} to march to", this.FlankingLegion, this.FlankedHexCoord));
					return;
				}
				int num;
				if (!this.OwningPlanner.TerrainInfluenceMap[marchFlankingPosition].TryGetTurnsToReach(this.FlankingLegion, out num, false))
				{
					base.Disable(string.Format("{0} cannot find influence data for ${1}", this.FlankingLegion, marchFlankingPosition));
					return;
				}
				if (num > 1 && ActionDraconicRazzia.CanBeUsedByArchfiend(this.OwningPlanner.PlayerState))
				{
					foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
					{
						if (playerState.Id != this.OwningPlanner.PlayerId && this.OwningPlanner.TrueContext.CurrentTurn.GetDiplomaticStatus(this.OwningPlanner.PlayerId, playerState.Id).DiplomaticState is DiplomaticState_DraconicRazzia)
						{
							base.Disable(string.Format("Razzia will end before {0} reaches {1}", this.FlankingLegion, marchFlankingPosition));
							return;
						}
					}
				}
				if (this.OwningPlanner.TileIsSafeForLegion(this.FlankingLegion, marchFlankingPosition))
				{
					base.AddEffect(WPLegionTileSafety.SafetyFromMovement(this.OwningPlanner, this.FlankingLegion, marchFlankingPosition));
				}
				else if (this.FlankIntent == FlankIntent.Flank_Heal)
				{
					base.Disable(this.FlankingLegion.NameKey + " cannot be healed in Fumes");
					return;
				}
				if (num > 1)
				{
					base.AddTurnsToReachCostModifier(this.FlankingLegion, num);
				}
				else
				{
					base.AddScalarCostModifier(-0.7f, PFCostModifier.Terrain_Cost);
				}
			}
			if (this.FlankingLegion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostIncrease(0.5f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPEveryTitanHasOrders());
			}
			base.Prepare();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00017A00 File Offset: 0x00015C00
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			return (this.FlankIntent == FlankIntent.Flank_Support_Battle && objectiveCondition is ObjectiveCondition_WinBattlesWithSupport) || base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00017A1C File Offset: 0x00015C1C
		private HexCoord GetTeleportFlankingPosition(TurnContext context)
		{
			HexBoard board = context.HexBoard;
			return IEnumerableExtensions.FirstOrDefault<HexCoord>(from t in board.GetNeighbours(this.FlankedHexCoord, false)
			where !this.OwningPlanner.AIPersistentData.IsHexClaimedForMovement(t)
			where LegionMovementProcessor.CanMoveBeExecuted(context, this.FlankingLegion, board.ToRelativeHex(t), PathMode.Teleport, null)
			orderby board.ShortestDistance(this.FlankingLegion.Location, t)
			select t);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00017A98 File Offset: 0x00015C98
		private HexCoord GetMarchFlankingPosition(TurnContext context)
		{
			HexBoard board = context.HexBoard;
			InfluenceData.TerrainInfluenceMap influenceMap = this.OwningPlanner.TerrainInfluenceMap;
			return IEnumerableExtensions.FirstOrDefault<HexCoord>((from t in board.GetNeighbours(this.FlankedHexCoord, false)
			where influenceMap[t].CanReach(this.FlankingLegion, false)
			where !this.OwningPlanner.AIPersistentData.IsHexClaimedForMovement(t)
			where LegionMovementProcessor.CanMoveBeExecuted(context, this.FlankingLegion, board.ToRelativeHex(t), PathMode.March, null)
			select t).OrderBy(delegate(HexCoord t)
			{
				int result;
				if (influenceMap[t].TryGetShortestPathLength(this.FlankingLegion, out result, false))
				{
					return result;
				}
				return int.MaxValue;
			}));
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00017B38 File Offset: 0x00015D38
		protected override OrderMoveLegion GenerateOrder()
		{
			OrderTeleportLegion orderTeleportLegion = this.TryTeleport ? new OrderTeleportLegion(this.FlankingLegion, AttackOutcomeIntent.Default, this.FlankIntent) : new OrderMarchLegion(this.FlankingLegion, AttackOutcomeIntent.Default, this.FlankIntent, new HexCoord[]
			{
				HexCoord.Invalid
			});
			orderTeleportLegion.Priority = this.Priority;
			return orderTeleportLegion;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00017B9C File Offset: 0x00015D9C
		private Result SubmitTeleportAction(TurnContext context, PlayerState playerState, OrderTeleportLegion teleportOrder)
		{
			HexCoord teleportFlankingPosition = this.GetTeleportFlankingPosition(context);
			if (teleportFlankingPosition == HexCoord.Invalid)
			{
				return Result.Failure;
			}
			AIPersistentData aipersistentData = this.OwningPlanner.AIPersistentData;
			HexCoord destinationHex;
			if (!context.TryFindTeleportDestination(this.FlankingLegion, teleportFlankingPosition, out destinationHex, aipersistentData.HexesSelectedForClaiming))
			{
				return Result.Failure;
			}
			teleportOrder.DestinationHex = destinationHex;
			if (this.FlankIntent == FlankIntent.Flank_Support_Battle)
			{
				aipersistentData.SetBattleSupportedAt(this.FlankedHexCoord, true);
			}
			aipersistentData.RegisterHexClaimedForMovement(teleportOrder.DestinationHex);
			this.OwningPlanner.RecordPotentialCaptures(teleportOrder);
			return base.SubmitAction(context, playerState);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00017C2C File Offset: 0x00015E2C
		private Result SubmitMarchAction(TurnContext context, PlayerState playerState, OrderMarchLegion marchOrder)
		{
			HexCoord marchFlankingPosition = this.GetMarchFlankingPosition(context);
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>(this.OwningPlanner.FindTerrainPath(this.FlankingLegion, this.FlankingLegion.Location, marchFlankingPosition, ~GamePieceAvoidance.FriendlyFixture, true, false, true));
			if (list.Count == 0)
			{
				return Result.Failure;
			}
			list.RemoveAt(0);
			if (list.Count > this.FlankingLegion.GroundMoveDistance)
			{
				list = list.GetRange(0, this.FlankingLegion.GroundMoveDistance);
			}
			AIPersistentData aipersistentData = this.OwningPlanner.AIPersistentData;
			GamePiece movingLegion = context.CurrentTurn.FetchGameItem<GamePiece>(this.FlankedGamePiece);
			int numberOfHexesToClaim;
			if (!this.OwningPlanner.CanClaimPathForMovement(list, out numberOfHexesToClaim, movingLegion))
			{
				return Result.Failure;
			}
			marchOrder.MoveIntent = this.FlankIntent;
			marchOrder.MovePath = list;
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (this.FlankIntent == FlankIntent.Flank_Support_Battle)
			{
				aipersistentData.SetBattleSupportedAt(this.FlankedHexCoord, true);
			}
			this.OwningPlanner.AIPersistentData.ClaimPathForMovement(list, numberOfHexesToClaim);
			this.OwningPlanner.RecordPotentialCaptures(base.Order);
			return Result.Success;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00017D50 File Offset: 0x00015F50
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			if (this.FlankingLegion == null)
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
					result = this.SubmitMarchAction(context, playerState, orderMarchLegion);
				}
			}
			else
			{
				result = this.SubmitTeleportAction(context, playerState, orderTeleportLegion);
			}
			return result;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00017DA7 File Offset: 0x00015FA7
		public override void OnActionFailed()
		{
			base.OnActionFailed();
			ActionMovement.RegisterBlockages(this.OwningPlanner, this.FlankingLegion, this.FlankingLegion.Location, true, false, true);
		}

		// Token: 0x0400029D RID: 669
		public readonly GamePiece FlankingLegion;

		// Token: 0x0400029E RID: 670
		public readonly GamePiece FlankedGamePiece;

		// Token: 0x0400029F RID: 671
		public HexCoord FlankedHexCoord = HexCoord.Invalid;

		// Token: 0x040002A0 RID: 672
		public FlankIntent FlankIntent;

		// Token: 0x040002A1 RID: 673
		public bool TryTeleport;
	}
}
