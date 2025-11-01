using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000136 RID: 310
	public class GOAPPlanner : AITurnPlanner
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001D9F8 File Offset: 0x0001BBF8
		public PlayerState PlayerState
		{
			get
			{
				return base.PristinePlayer;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001DA00 File Offset: 0x0001BC00
		public TurnState PlayerViewOfTurnState
		{
			get
			{
				return base.PristineTurn;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001DA08 File Offset: 0x0001BC08
		public PathfinderHexboard HexPathfinder
		{
			get
			{
				return this._terrainPathfinder;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0001DA10 File Offset: 0x0001BC10
		public GOAPPathfinder ActionPathfinder
		{
			get
			{
				return this._actionPathfinder;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0001DA18 File Offset: 0x0001BC18
		private GameDatabase _gameDatabase
		{
			get
			{
				return base.Database;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0001DA20 File Offset: 0x0001BC20
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x0001DA28 File Offset: 0x0001BC28
		public ResourceAccumulation PreviewAvailableResources { get; protected set; } = ResourceAccumulation.Empty;

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001DA31 File Offset: 0x0001BC31
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x0001DA39 File Offset: 0x0001BC39
		public int PreviewNumResources { get; protected set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001DA42 File Offset: 0x0001BC42
		public IReadOnlyList<GOAPNode> Goals
		{
			get
			{
				return this._actionPathfinder.Goals;
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001DA50 File Offset: 0x0001BC50
		public GOAPPlanner(List<AITag> aiTags)
		{
			this._aiTags = aiTags;
			this._goalSelector = new GoalSelector_RunAllCostsAndChoose();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001DADF File Offset: 0x0001BCDF
		private static bool OrderIsGoal(PFNode node)
		{
			return !node.ShouldIncludeInPath();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001DAEC File Offset: 0x0001BCEC
		public override void Setup()
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				base.Setup();
				this._terrainPathfinder.PopulateMap(this.TrueContext);
				this.RegisterLegionsInUnsafeLocations();
				this._actionPathfinder = new GOAPPathfinder(this);
				AIDecisionHandlerBasic decisionHandler;
				switch (this.AIPersistentData.DecisionHandlerType)
				{
				case DecisionHandlerType.Basic:
					decisionHandler = new AIDecisionHandlerBasic(this._gameDatabase);
					break;
				case DecisionHandlerType.Tutorial_Revelation:
					decisionHandler = new AIDecisionHandler_TutorialRevelation(this._gameDatabase);
					break;
				case DecisionHandlerType.Chronicle_Andromalius:
					decisionHandler = new AIDecisionHandler_ChronicleAndromalius(this._gameDatabase);
					break;
				default:
					decisionHandler = null;
					break;
				}
				this._decisionHandler = decisionHandler;
				if (this._decisionHandler == null)
				{
					SimLogger logger = SimLogger.Logger;
					if (logger != null)
					{
						logger.Error(string.Format("Could not create decision handler from type {0}", this.AIPersistentData.DecisionHandlerType));
					}
				}
				this.PraetorHeuristics.Populate(this.TrueContext);
				this.ArchfiendHeuristics.Populate(this.TrueContext, base.TerrainInfluenceMap, this.PraetorHeuristics);
				this.OnPreviewRefreshed();
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001DC04 File Offset: 0x0001BE04
		private IEnumerable<GOAPPlaystyle> GetCostModifiersFromTags(List<AITag> tags)
		{
			GOAPPlanner.<GetCostModifiersFromTags>d__36 <GetCostModifiersFromTags>d__ = new GOAPPlanner.<GetCostModifiersFromTags>d__36(-2);
			<GetCostModifiersFromTags>d__.<>4__this = this;
			<GetCostModifiersFromTags>d__.<>3__tags = tags;
			return <GetCostModifiersFromTags>d__;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001DC1C File Offset: 0x0001BE1C
		protected override Task PerformAnalysis(TurnContext turnContext, PlayerState playerState, InfluenceData.TerrainInfluenceMap influenceMap)
		{
			this.AIPersistentData.ClearExpectedBattles();
			this.AIPersistentData.UpdatePositionHistory(turnContext, playerState);
			this.AIPersistentData.ClearInvalidBlockers(turnContext);
			this.ContestedPOPs.Clear();
			this.GenerateExpectedDefensiveBattles(this.AIPreviewContext, this.PlayerState);
			return Task.CompletedTask;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001DC70 File Offset: 0x0001BE70
		protected override Task PlanActions(TurnContext turnContext, PlayerState playerState, InfluenceData.TerrainInfluenceMap influenceMap, HashSet<ActionID> actionWhiteList = null)
		{
			Task completedTask;
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				if (playerState.Eliminated)
				{
					completedTask = Task.CompletedTask;
				}
				else
				{
					if (influenceMap != null)
					{
						base.TerrainInfluenceMap = influenceMap;
					}
					this.SubmittedPlanPaths.Clear();
					this.SubmittedNodes.Clear();
					this._randomFallbackEnabled = (actionWhiteList == null || actionWhiteList.Count == 0);
					IEnumerable<GOAPPlaystyle> costModifiersFromTags = this.GetCostModifiersFromTags(this._aiTags);
					this._actionPathfinder.PopulateMap(turnContext.CurrentTurn, this.PlayerState, this, IEnumerableExtensions.ToList<GOAPPlaystyle>(costModifiersFromTags), base.Database, actionWhiteList);
					this.AIPersistentData.HexesSelectedForClaiming = new List<HexCoord>();
					this.PlanActions(this._actionPathfinder.Goals);
					completedTask = Task.CompletedTask;
				}
			}
			return completedTask;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001DD4C File Offset: 0x0001BF4C
		private void RegisterLegionsInUnsafeLocations()
		{
			int id = base.AIPreviewPlayerState.Id;
			foreach (GamePiece gamePiece in base.AIPreviewTurn.GetAllActiveLegionsForPlayer(id))
			{
				if (!this.TileIsSafeForLegion(gamePiece, gamePiece.Location))
				{
					this.AITransientData.RecordLegionInDanger(gamePiece);
				}
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001DDC4 File Offset: 0x0001BFC4
		public void PlanActions(IReadOnlyList<GOAPNode> goals)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				this._decisionsRespondedInPlanning.Clear();
				if (!base.AIPreviewPlayerState.Eliminated)
				{
					this.HandleTributeDecisions();
					int num = base.PristinePlayer.OrderSlots;
					List<Result.PlanningProblem> list = new List<Result.PlanningProblem>();
					int num2 = 0;
					if (this._randomFallbackEnabled && num > 1)
					{
						ActionableOrder actionableOrder = AIGrandEventHandler.DetermineGrandEvent(this.TrueContext, base.AIPreviewPlayerState, this.AIPersistentData);
						if (actionableOrder != null)
						{
							this.PlannedTurn.AddOrReplaceOrders(new ActionableOrder[]
							{
								actionableOrder
							});
							num2++;
							this.PlanHistory.AddGrandEvent(actionableOrder.AbilityId, this.AIPreviewContext.CurrentTurn.TurnValue);
						}
					}
					float num3 = 0f;
					int num4 = 0;
					while (num2 < num && num4 < 20)
					{
						num4++;
						this._actionPathfinder.EvaluateNodeConstraints();
						List<GoalRelevanceEntry> goalDebugData = new List<GoalRelevanceEntry>();
						float num5;
						float goalRelevanceScoreToBeat;
						GoalGOAPNode goalGOAPNode = this._goalSelector.SelectGoal(this.PlayerState, this.PlayerViewOfTurnState, goals, this.PlanHistory, this._actionPathfinder, out goalDebugData, out num5, out goalRelevanceScoreToBeat, this.AIPersistentData) as GoalGOAPNode;
						if (goalGOAPNode == null)
						{
							list.Add(new Result.PlanningProblem("No goal found", goalGOAPNode));
						}
						else
						{
							if (num5 > num3)
							{
								num3 = num5;
								this.AIPersistentData.RecordAIGoalSelected(goalGOAPNode, this.PlayerViewOfTurnState.TurnValue);
							}
							this.GenerateSupportNodes();
							List<GOAPNode> list2 = this._actionPathfinder.FindPath(goalGOAPNode, null, null);
							list2.Reverse();
							if (list2.Count == 0)
							{
								list.Add(new Result.PlanningProblem("No path found to goal", goalGOAPNode));
							}
							else
							{
								ActionHistory actionHistory = this.PlanHistory.AddActionPlan(goalGOAPNode, this.PlayerViewOfTurnState.TurnValue);
								actionHistory.GoalDebugData = goalDebugData;
								actionHistory.GoalRelevanceScoreToBeat = goalRelevanceScoreToBeat;
								string arg = (this.AIPersistentData.CurrentTarget != null) ? this.AIPersistentData.CurrentTarget.GetDebugValue() : "none";
								actionHistory.CurrentGoalAtTimeOfPlanning = string.Format("{0} / {1}", this.AIPersistentData.CurrentGoal, arg);
								foreach (GOAPNode goapnode in list2)
								{
									GOAPDebugInfo item = new GOAPDebugInfo(goapnode.ActionName, goapnode.TraversalCost, goapnode.HeuristicCost, GOAPPlanner.OrderIsGoal(goapnode), goapnode.NodeType);
									actionHistory.ActionPlan.Add(item);
								}
								actionHistory.ActionPlan.Reverse();
								list2.RemoveAll(new Predicate<GOAPNode>(GOAPPlanner.OrderIsGoal));
								if (list2.Count != 0)
								{
									foreach (GOAPNode goapnode2 in list2)
									{
										if (!goapnode2.EvaluatePreconditions())
										{
											list.Add(new Result.PlanningProblem("Preconditions not met", goapnode2));
										}
										else
										{
											if (goapnode2.SubmitAction(this.AIPreviewContext, base.AIPreviewPlayerState))
											{
												actionHistory.Action = goapnode2;
												if (goapnode2.ConsumesActionSlot)
												{
													num2++;
												}
												this.RefreshPreviewTurn();
												this.SubmittedNodes.Add(goapnode2);
												this.SubmittedPlanPaths.Add(list2);
												goapnode2.ClearAndPrepare();
												goapnode2.OnActionSubmitted();
												break;
											}
											goapnode2.OnActionFailed();
											list.Add(new Result.PlanningProblem("Submission failed", goapnode2));
										}
									}
								}
							}
						}
					}
					foreach (Result.PlanningProblem planningProblem in list)
					{
						SimLogger logger = SimLogger.Logger;
						if (logger != null)
						{
							logger.Warn(string.Concat(new string[]
							{
								"GOAP plan failure for ",
								base.PristinePlayer.ArchfiendId,
								" '",
								planningProblem.DebugString,
								"'"
							}));
						}
					}
					this.HandleFallback(this.AIPreviewContext);
					this.WrapUpDecisions();
					this.ReorderPlan();
				}
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001E238 File Offset: 0x0001C438
		public bool CanClaimPathForMovement(List<HexCoord> path, out int numberOfHexesToClaim, GamePiece movingLegion = null)
		{
			if (movingLegion == null)
			{
				numberOfHexesToClaim = path.Count;
			}
			else
			{
				numberOfHexesToClaim = 0;
				int num = 0;
				while (num < movingLegion.GroundMoveDistance && num < path.Count)
				{
					numberOfHexesToClaim++;
					if (this.HexPathfinder.DoesHexContainFixtureForPlayer(path[num], base.PlayerId))
					{
						numberOfHexesToClaim = Math.Min(numberOfHexesToClaim + 1, path.Count);
						break;
					}
					num++;
				}
			}
			for (int i = 0; i < numberOfHexesToClaim; i++)
			{
				HexCoord hexCoord = path[i];
				if (this.AIPersistentData.IsHexClaimedForMovement(hexCoord))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001E2CD File Offset: 0x0001C4CD
		private void ReorderPlan()
		{
			this.PlannedTurn.Orders.SortOnValueAscending((ActionableOrder t) => t.Priority);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001E300 File Offset: 0x0001C500
		private void GenerateExpectedDefensiveBattles(TurnContext context, PlayerState player)
		{
			IEnumerable<GamePiece> enumerable = context.CurrentTurn.GetActiveGamePiecesForPlayer(player.Id);
			int playerId;
			if (context.CurrentTurn.CurrentDiplomaticTurn.IsVassalOfAny(this._playerId, out playerId))
			{
				enumerable = enumerable.Concat(context.CurrentTurn.GetActiveGamePiecesForPlayer(playerId));
			}
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(enumerable);
			List<GamePiece> list2 = IEnumerableExtensions.ToList<GamePiece>(from gp in context.CurrentTurn.GetActiveGamePiecesForPlayer(-1)
			where !gp.IsLegionOrTitan()
			select gp);
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != player.Id)
				{
					bool flag = context.CurrentTurn.CombatAuthorizedBetween(player.Id, playerState.Id);
					foreach (GamePiece gamePiece in context.CurrentTurn.GetAllActiveLegionsForPlayer(playerState.Id))
					{
						if (!gamePiece.IsLegionOrTitan())
						{
							return;
						}
						if (flag)
						{
							using (List<GamePiece>.Enumerator enumerator3 = list.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									GamePiece gamePiece2 = enumerator3.Current;
									int num;
									if ((gamePiece2.Id != player.StrongholdId || context.CurrentTurn.StrongholdCaptureAuthorizedBetween(player.Id, playerState.Id)) && base.TerrainInfluenceMap[gamePiece2.Location].CanReach(gamePiece.Id, false) && base.TerrainInfluenceMap[gamePiece2.Location].TryGetTurnsToReach(gamePiece.Id, out num, false) && num <= 1 && GamePiece.CalcCombatAdvantageAtPosition(this.TrueContext, gamePiece, gamePiece2) >= 0.5f)
									{
										this.AIPersistentData.RegisterExpectedBattle(context, gamePiece2.Location, gamePiece2.Id);
									}
								}
								continue;
							}
						}
						foreach (GamePiece gamePiece3 in list2)
						{
							int num2;
							if (base.TerrainInfluenceMap[gamePiece3.Location].CanReach(gamePiece.Id, false) && base.TerrainInfluenceMap[gamePiece3.Location].TryGetTurnsToReach(gamePiece.Id, out num2, false) && num2 <= 1)
							{
								this.ContestedPOPs.Add(gamePiece3.Id);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001E60C File Offset: 0x0001C80C
		private void GenerateSupportNodes()
		{
			List<ExpectedBattle> list = new List<ExpectedBattle>();
			List<GamePiece> list2 = IEnumerableExtensions.ToList<GamePiece>(this.AIPreviewContext.CurrentTurn.GetAllActiveLegionsForPlayer(base.AIPreviewPlayerState.Id));
			if (list2.Count<GamePiece>() < 2)
			{
				return;
			}
			using (List<ExpectedBattle>.Enumerator enumerator = this.AIPersistentData.ExpectedBattlePositions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ExpectedBattle expectedBattle = enumerator.Current;
					List<ActionMoveToFlank> list3 = (from t in this._actionPathfinder.AllNodes
					where t.ID == ActionID.March_Support_Battle
					select t) as List<ActionMoveToFlank>;
					if (list3 == null || !list3.Any((ActionMoveToFlank t) => t.FlankedHexCoord == expectedBattle.Location))
					{
						list.Add(new ExpectedBattle(expectedBattle.Location, expectedBattle.OurLegion));
					}
				}
			}
			GamePiece pandaemonium = this.AIPreviewContext.CurrentTurn.GetPandaemonium();
			HexCoord hexCoord = (pandaemonium != null) ? pandaemonium.Location : HexCoord.Invalid;
			foreach (ExpectedBattle expectedBattle2 in list)
			{
				bool flag = pandaemonium.Location != HexCoord.Invalid && expectedBattle2.Location == hexCoord;
				foreach (GamePiece gamePiece in list2)
				{
					if ((!expectedBattle2.IsSupportAlreadyPlanned || flag) && gamePiece.Id != expectedBattle2.OurLegion && !gamePiece.IsHexWithinSupportRange(this.AIPreviewContext, expectedBattle2.Location))
					{
						int num = this.AIPreviewContext.HexBoard.ShortestDistance(expectedBattle2.Location, gamePiece.Location);
						bool tryTeleport = gamePiece.CanTeleport && num <= gamePiece.TeleportDistance + 1;
						ActionMoveToFlank action = ActionMoveToFlank.Location(FlankIntent.Flank_Support_Battle, gamePiece, expectedBattle2.Location, tryTeleport);
						this._actionPathfinder.AddLateAction(action);
					}
				}
				if (ActionDanseMacabre.CanBeUsedByArchfiend(this.PlayerState) && (!this.AIPersistentData.IsBattleSupported(expectedBattle2.Location) || flag))
				{
					this._actionPathfinder.AddLateAction(new ActionDanseMacabre(expectedBattle2.Location));
				}
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001E8D8 File Offset: 0x0001CAD8
		private void HandleFallback(TurnContext turnContext)
		{
			if (!this._randomFallbackEnabled)
			{
				return;
			}
			AbilityHelper abilityHelper = new AbilityHelper(turnContext, this.PlayerState, this.PlannedTurn);
			IList<WeightedValue<ActionableOrder>> candidates = this.GenerateFallbackCandidateActions(turnContext, this.PlayerState);
			int num = 0;
			while (base.RemainingActions > 0 && num < 100)
			{
				num++;
				ActionableOrder actionableOrder;
				if (AITurnPlanner_Random.SelectValidAction(candidates, turnContext, base.AIPreviewPlayerState, abilityHelper, out actionableOrder))
				{
					CastRitualOrder castRitualOrder = actionableOrder as CastRitualOrder;
					if (castRitualOrder != null && castRitualOrder.AbilityId == "strategic_confusion")
					{
						actionableOrder.Priority = ActionOrderPriority.High;
					}
					this.PlannedTurn.AddOrReplaceOrders(new ActionableOrder[]
					{
						actionableOrder
					});
					string text = actionableOrder.ToString();
					this.PlanHistory.AddFallback(text, turnContext.CurrentTurn.TurnValue);
				}
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001E99C File Offset: 0x0001CB9C
		private IList<WeightedValue<ActionableOrder>> GenerateFallbackCandidateActions(TurnContext turnContext, PlayerState playerState)
		{
			List<WeightedValue<ActionableOrder>> list = new List<WeightedValue<ActionableOrder>>();
			int weight = 1;
			int weight2 = 5;
			int weight3 = 1;
			int weight4 = 1;
			int weight5 = 1;
			IEnumerable<WeightedValue<ActionableOrder>> weightedFallbackAbilities = this.GetWeightedFallbackAbilities(turnContext.CurrentTurn, base.AIPreviewPlayerState);
			list.AddRange(weightedFallbackAbilities);
			IEnumerable<ActionableOrder> orders = AITurnPlanner_Random.GenerateBidOrders(turnContext, base.AIPreviewPlayerState);
			list.AddRange(this.GetWeightedOrdersFor(orders, weight));
			IEnumerable<ActionableOrder> orders2 = AITurnPlanner_Random.GeneratePowerIncreaseActions(turnContext, base.AIPreviewPlayerState);
			list.AddRange(this.GetWeightedOrdersFor(orders2, weight3));
			IEnumerable<ActionableOrder> orders3 = AITurnPlanner_Random.GenerateTributeManagementActions(turnContext, base.AIPreviewPlayerState);
			list.AddRange(this.GetWeightedOrdersFor(orders3, weight2));
			list.AddRange(this.GetWeightedOrdersFor(new OrderSeekManuscripts[]
			{
				new OrderSeekManuscripts()
			}, weight4));
			OrderRequestScheme orderRequestScheme = new OrderRequestScheme();
			list.AddRange(this.GetWeightedOrdersFor(new OrderRequestScheme[]
			{
				orderRequestScheme
			}, weight5));
			return list;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001EA6B File Offset: 0x0001CC6B
		private IEnumerable<WeightedValue<ActionableOrder>> GetWeightedOrdersFor(IEnumerable<ActionableOrder> orders, int weight)
		{
			foreach (ActionableOrder value in orders)
			{
				yield return new WeightedValue<ActionableOrder>
				{
					Value = value,
					Weight = (float)weight
				};
			}
			IEnumerator<ActionableOrder> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001EA82 File Offset: 0x0001CC82
		private IEnumerable<WeightedValue<ActionableOrder>> GetWeightedFallbackAbilities(TurnState turnState, PlayerState playerState)
		{
			foreach (AbilityStaticData abilityStaticData in this._gameDatabase.GetUnlockedAbilities(turnState, playerState, false))
			{
				GOAPRandomFallbackData goaprandomFallbackData;
				if (abilityStaticData.TryGetComponent<GOAPRandomFallbackData>(out goaprandomFallbackData))
				{
					WeightedValue<ActionableOrder> weightedValue = default(WeightedValue<ActionableOrder>);
					ActionableOrder actionableOrder = AITurnPlanner_Random.GenerateAbilityOrder(abilityStaticData, this.AIPreviewContext, playerState);
					if (actionableOrder != null && GOAPPlanner.TryConfigureOrder(actionableOrder, this.AIPreviewContext, playerState, base.Random))
					{
						weightedValue.Value = actionableOrder;
						weightedValue.Weight = goaprandomFallbackData.RandomWeight;
						yield return weightedValue;
					}
				}
			}
			IEnumerator<AbilityStaticData> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001EAA0 File Offset: 0x0001CCA0
		public static bool TryConfigureOrder(ActionableOrder order, TurnContext context, PlayerState player, SimulationRandom random)
		{
			return IEnumerableExtensions.Any<ActionPhase>(order.GetActionPhaseSteps(player, context.CurrentTurn, context.Database)) && GOAPPlanner.ConfigureAction(order, context, player, 5, random);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001EACD File Offset: 0x0001CCCD
		public static bool ConfigureAction(ActionableOrder order, TurnContext context, PlayerState caster, int attempts, SimulationRandom random)
		{
			while (attempts-- > 0)
			{
				if (GOAPPlanner.ConfigureAction(order, context, caster, random))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001EAEC File Offset: 0x0001CCEC
		public static bool ConfigureAction(ActionableOrder order, TurnContext context, PlayerState caster, SimulationRandom random)
		{
			foreach (ActionPhase step in order.GetActionPhaseSteps(caster, context.CurrentTurn, context.Database))
			{
				if (!GOAPPlanner.ConfigureStep(order, step, context, caster, random))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001EB54 File Offset: 0x0001CD54
		public static bool ConfigureStep(ActionableOrder order, ActionPhase step, TurnContext context, PlayerState caster, SimulationRandom random)
		{
			ActionPhase_TargetGamePiece actionPhase_TargetGamePiece = step as ActionPhase_TargetGamePiece;
			bool result;
			if (actionPhase_TargetGamePiece != null)
			{
				result = GOAPPlanner.ConfigureStep(order, actionPhase_TargetGamePiece, context, caster, random);
			}
			else
			{
				result = AITurnPlanner_Random.ConfigureStep(order, step, context, caster);
			}
			return result;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001EB84 File Offset: 0x0001CD84
		public static bool ConfigureStep(ActionableOrder order, ActionPhase_TargetGamePiece step, TurnContext context, PlayerState caster, SimulationRandom random)
		{
			TurnState currentTurn = context.CurrentTurn;
			GamePiece pandaemonium = currentTurn.GetPandaemonium();
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(from t in currentTurn.GetActiveGamePieces()
			where step.ValidateTarget(context, null, t, caster.Id)
			select t);
			list.Remove(pandaemonium);
			GamePiece obj;
			if (!list.TryGetRandom(random, out obj))
			{
				return false;
			}
			step.SetTarget(obj);
			return true;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001EC00 File Offset: 0x0001CE00
		public void RefreshPreviewTurn()
		{
			base.AIPreviewTurn.SubmitPlayerTurn(this._playerId, this._dirtyTurn);
			new TurnProcessor(this._gameDatabase).AIPreviewTurn(base.AIPreviewTurn, base.Rules);
			this.OnPreviewRefreshed();
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001EC3C File Offset: 0x0001CE3C
		public void OnPreviewRefreshed()
		{
			this.PreviewAvailableResources = base.AIPreviewPlayerState.TotalResourcesIncludingPrestige;
			this.PreviewNumResources = base.AIPreviewPlayerState.Resources.Count;
			this._dirtyTurn = new PlayerTurn();
			base.AIPreviewTurn.SubmitPlayerTurn(this._playerId, this.PlannedTurn);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001EC94 File Offset: 0x0001CE94
		public List<HexCoord> FindTerrainPath(GamePiece gamePiece, HexCoord start, HexCoord end, GamePieceAvoidance avoidOccupied = ~GamePieceAvoidance.FriendlyFixture, bool destinationAlwaysValid = true, bool destinationOwnersCantonsAlwaysValid = false, bool allowRedeployToDestination = true)
		{
			PFAgentAIGamePiece pfagentAIGamePiece = new PFAgentAIGamePiece(gamePiece, this);
			pfagentAIGamePiece.AvoidanceType = avoidOccupied;
			pfagentAIGamePiece.DestinationAlwaysValid = destinationAlwaysValid;
			pfagentAIGamePiece.AllowRedeployToDestination = allowRedeployToDestination;
			pfagentAIGamePiece.GamePiece = gamePiece;
			if (destinationOwnersCantonsAlwaysValid)
			{
				Hex hex = this.PlayerViewOfTurnState.HexBoard[end];
				if (hex != null)
				{
					pfagentAIGamePiece.IgnoreDiplomacyWithPlayers.Add(hex.ControllingPlayerID);
				}
			}
			return this._terrainPathfinder.FindPath(start, end, pfagentAIGamePiece);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001ED00 File Offset: 0x0001CF00
		public bool IsCombatScheduled(Identifier target)
		{
			GamePiece gamePiece;
			return base.AIPreviewTurn.TryFetchGameItem<GamePiece>(target, out gamePiece) && gamePiece.LastBattleTurn >= base.AIPreviewTurn.TurnValue;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001ED38 File Offset: 0x0001CF38
		public int CountTimesActionIsPlannedThisTurn(ActionID id)
		{
			int num = 0;
			using (List<GOAPNode>.Enumerator enumerator = this.SubmittedNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == id)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001ED94 File Offset: 0x0001CF94
		public bool IsActionPlannedThisTurn(Func<GOAPNode, bool> predicate)
		{
			foreach (GOAPNode arg in this.SubmittedNodes)
			{
				if (predicate(arg))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001EDF0 File Offset: 0x0001CFF0
		public bool IsActionPlannedThisTurn(ActionID id)
		{
			using (List<GOAPNode>.Enumerator enumerator = this.SubmittedNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001EE4C File Offset: 0x0001D04C
		public bool IsDecisionMadeThisTurn(DecisionId id)
		{
			using (List<DecisionRequest>.Enumerator enumerator = this._decisionsRespondedInPlanning.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.DecisionId == id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001EEA8 File Offset: 0x0001D0A8
		public bool IsActionPlannedThisTurn(GOAPNode action)
		{
			return this.IsActionPlannedThisTurn(action.ID);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001EEB6 File Offset: 0x0001D0B6
		public void SetDecisionHandledByPlan(DecisionRequest request, DecisionResponse response)
		{
			if (!this.GetDecisionHandledByPlan(request))
			{
				this._decisionsRespondedInPlanning.Add(request);
				this.AddDecisionToPlan(response);
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001EED4 File Offset: 0x0001D0D4
		public void AddDecisionToPlan(DecisionResponse response)
		{
			this.PlannedTurn.AddDecisions(new DecisionResponse[]
			{
				response
			});
			this._dirtyTurn.AddDecision<DecisionResponse>(response);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001EEF9 File Offset: 0x0001D0F9
		public void AddActionToPlan(ActionableOrder order)
		{
			this.PlannedTurn.AddOrReplaceOrders(new ActionableOrder[]
			{
				order
			});
			this._dirtyTurn.AddOrReplaceOrders(new ActionableOrder[]
			{
				order
			});
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001EF28 File Offset: 0x0001D128
		public bool GetDecisionHandledByPlan(DecisionRequest request)
		{
			using (List<DecisionRequest>.Enumerator enumerator = this._decisionsRespondedInPlanning.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.DecisionId == request.DecisionId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001EF88 File Offset: 0x0001D188
		public void HandleTributeDecisions()
		{
			foreach (DecisionRequest decisionRequest in this.PlayerState.DecisionRequests)
			{
				if (decisionRequest is SelectTributeDecisionRequest)
				{
					DecisionResponse decisionResponse = this._decisionHandler.HandleDecision(this, decisionRequest);
					if (decisionResponse != null)
					{
						this.AddDecisionToPlan(decisionResponse);
						this.PlanHistory.WrapUpDecisionResponses.Add(decisionResponse.GetDebugString());
					}
				}
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001F010 File Offset: 0x0001D210
		public void WrapUpDecisions()
		{
			this._decisionHandler.PreHandleDecisions(this, base.TrueTurn);
			foreach (DecisionRequest decisionRequest in this.PlayerState.DecisionRequests)
			{
				if (!(decisionRequest is SelectTributeDecisionRequest) && !this.GetDecisionHandledByPlan(decisionRequest))
				{
					DecisionResponse decisionResponse = this._decisionHandler.HandleDecision(this, decisionRequest);
					if (decisionResponse != null)
					{
						this.AddDecisionToPlan(decisionResponse);
						this.PlanHistory.WrapUpDecisionResponses.Add(decisionResponse.GetDebugString());
					}
				}
			}
		}

		// Token: 0x040002E3 RID: 739
		private const int FallbackRetryLimit = 100;

		// Token: 0x040002E4 RID: 740
		public readonly List<List<GOAPNode>> SubmittedPlanPaths = new List<List<GOAPNode>>();

		// Token: 0x040002E5 RID: 741
		public readonly List<GOAPNode> SubmittedNodes = new List<GOAPNode>();

		// Token: 0x040002E6 RID: 742
		private GOAPPathfinder _actionPathfinder;

		// Token: 0x040002E7 RID: 743
		private PathfinderHexboard _terrainPathfinder = new PathfinderHexboard();

		// Token: 0x040002E8 RID: 744
		private List<GOAPPlaystyle> _playStyles = new List<GOAPPlaystyle>();

		// Token: 0x040002E9 RID: 745
		private GoalSelector _goalSelector;

		// Token: 0x040002EA RID: 746
		private List<DecisionRequest> _decisionsRespondedInPlanning = new List<DecisionRequest>();

		// Token: 0x040002EB RID: 747
		private PlayerTurn _dirtyTurn = new PlayerTurn();

		// Token: 0x040002EC RID: 748
		private List<AITag> _aiTags = new List<AITag>();

		// Token: 0x040002ED RID: 749
		private bool _randomFallbackEnabled = true;

		// Token: 0x040002EE RID: 750
		private AIDecisionHandler _decisionHandler;

		// Token: 0x040002F1 RID: 753
		public List<Identifier> ContestedPOPs = new List<Identifier>();
	}
}
