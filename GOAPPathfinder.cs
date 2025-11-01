using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000131 RID: 305
	public class GOAPPathfinder : Pathfinder<GOAPNode, PFAgent>
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x0001AED0 File Offset: 0x000190D0
		public IReadOnlyList<GOAPNode> Goals
		{
			get
			{
				return this._goals;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0001AED8 File Offset: 0x000190D8
		public IReadOnlyList<GOAPNode> Actions
		{
			get
			{
				return this._actions;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0001AEE0 File Offset: 0x000190E0
		public IEnumerable<GOAPNode> AllNodes
		{
			get
			{
				return this._goals.Concat(this._actions);
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001AEF3 File Offset: 0x000190F3
		public GOAPPathfinder(GOAPPlanner planner)
		{
			this._planner = planner;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001AF18 File Offset: 0x00019118
		private void AddGoal(GOAPNode goal)
		{
			this._goals.Add(goal.Initialize(this._planner));
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001AF31 File Offset: 0x00019131
		public void AddAction(GOAPNode action)
		{
			this._actions.Add(action.Initialize(this._planner));
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001AF4C File Offset: 0x0001914C
		public void PopulateMap(TurnState playerViewOfTurnState, PlayerState playerState, GOAPPlanner owningPlanner, List<GOAPPlaystyle> playStyles, GameDatabase database, HashSet<ActionID> actionWhiteList = null)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				PlayerState playerState2;
				if (playerViewOfTurnState.TryGetNemesis(playerState, out playerState2))
				{
					this.AddGoal(new GoalUndermine(playerState2.Id));
					if (playerState.AITags.Contains(AITag.CanEliminate))
					{
						this.AddGoal(new GoalEliminate(playerState2.Id));
					}
					this.AddGoal(new GoalBoostDuellingPower(playerState2.Id));
				}
				GamePiece pandaemonium = playerViewOfTurnState.GetPandaemonium();
				if (pandaemonium != null && playerState.AITags.Contains(AITag.CanUsurp))
				{
					this.AddGoal(new GoalConquerPandaemonium(pandaemonium.Id));
				}
				this.GenerateEasyPopGoals(owningPlanner, playerViewOfTurnState, playerState);
				this.AddGoal(new GoalExpandTerritory());
				this.AddGoal(new GoalIncreasePrestigeProduction());
				this.AddGoal(new GoalIncreaseTributeProduction());
				this.AddGoal(new GoalBoostMilitaryPower());
				this.AddGoal(new GoalAvoidElimination());
				this.AddGoal(new GoalPursueScheme());
				this.GenerateVendettaGoals(playerViewOfTurnState, playerState);
				this.GenerateArchfiendSpecificNodes(owningPlanner, playerState, database);
				this.AddAction(new ActionSeekTribute());
				this.AddAction(new ActionSeekManuscripts());
				this.GenerateDiplomaticNodes(playerViewOfTurnState, playerState);
				this.GenerateAttachPraetorNodes(owningPlanner, playerState, playerViewOfTurnState);
				this.GenerateBazaarNodes(playerState, playerViewOfTurnState, database);
				this.GeneratePowerLevelupNodes(owningPlanner, database);
				this.GenerateEdictNodes(playerViewOfTurnState);
				this.GenerateArtifactNodes(playerState, playerViewOfTurnState, database);
				this.GenerateCapturePopNodes(playerState, playerViewOfTurnState);
				this.GenerateVendettaNodes(playerViewOfTurnState, playerState);
				this.GenerateRitualNodes(playerViewOfTurnState, playerState);
				this.GenerateInvokeManuscriptNodes(playerViewOfTurnState, playerState, database);
				this.GenerateStratagemNodes(owningPlanner, playerState, database);
				this.GenerateCombatNodes(owningPlanner, playerState, owningPlanner.AIPreviewContext);
				this.GenerateConquerPandaemoniumNodes(owningPlanner, playerState, owningPlanner.AIPreviewContext);
				this.GenerateSuicideMissionAgainstPandaemoniumNodes(owningPlanner, playerState, owningPlanner.AIPreviewContext);
				this.GenerateHealLegionNodes(playerState, playerViewOfTurnState);
				this.GenerateReinforceStrongholdNodes(playerState, playerViewOfTurnState);
				this.AddAction(new ActionPlotScheme());
				if (owningPlanner.AIPreviewPlayerState.Rank < Rank.Prince)
				{
					this.AddAction(new ActionPurchaseInfernalRank());
				}
				foreach (GamePiece gamePiece in from t in owningPlanner.PlayerViewOfTurnState.GetActiveGamePiecesForPlayer(owningPlanner.PlayerState.Id)
				where !t.IsFixture()
				select t)
				{
					bool legionHasUpkeep = !gamePiece.UpkeepCost.IsZero;
					this.AddAction(new ActionMoveExpandBorders(gamePiece, legionHasUpkeep));
				}
				this.GenerateDecisionResponseNodes(playerViewOfTurnState, playerState);
				this.CullActions(actionWhiteList);
				foreach (GOAPPlaystyle playStyle in playStyles)
				{
					this.ApplyPlayStyle(playStyle);
				}
				this.Map.Clear();
				foreach (GOAPNode item in this.Actions)
				{
					this.Map.Add(item);
				}
				this.PrepareGraph(false);
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001B290 File Offset: 0x00019490
		public void PrepareGraph(bool clearAndReset = false)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				if (clearAndReset)
				{
					foreach (GOAPNode goapnode in this.Map)
					{
						goapnode.Neighbours.Clear();
						goapnode.Reset();
					}
				}
				this.ConnectNeighbours();
				this.SetupHeuristic();
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001B320 File Offset: 0x00019520
		public void ApplyPlayStyle(GOAPPlaystyle playStyle)
		{
			foreach (GOAPNode goapnode in this.AllNodes)
			{
				GOAPPlaystyleValue goapplaystyleValue;
				if (playStyle.TryGetActionPlaystyleValue(goapnode.ID, out goapplaystyleValue))
				{
					if (goapplaystyleValue.Chance == ActionChance.Forbidden)
					{
						this.ForbidNode(goapnode);
					}
					else
					{
						goapnode.AddScalarCostModifier(goapplaystyleValue.ChanceScalar, PFCostModifier.Archfiend_Bonus);
					}
				}
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001B39C File Offset: 0x0001959C
		private void ForbidNode(GOAPNode forbidden)
		{
			forbidden.Disable("disabled by AI playstyle.");
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001B3AC File Offset: 0x000195AC
		private void CullActions(HashSet<ActionID> actionWhiteList)
		{
			if (actionWhiteList == null || actionWhiteList.Count == 0)
			{
				return;
			}
			this._actions.RemoveAll((GOAPNode t) => !actionWhiteList.Contains(t.ID));
			this._goals.RemoveAll((GOAPNode t) => !actionWhiteList.Contains(t.ID));
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001B40C File Offset: 0x0001960C
		public void EvaluateNodeConstraints()
		{
			foreach (GOAPNode goapnode in this.AllNodes)
			{
				goapnode.EvaluateAndCachePropertyStatus();
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001B458 File Offset: 0x00019658
		private void GenerateVendettaGoals(TurnState turnState, PlayerState playerState)
		{
			foreach (PlayerState playerState2 in turnState.EnumeratePlayerStates(false, false))
			{
				VendettaState vendettaState;
				if (playerState2.Id != playerState.Id && turnState.CheckInstigatedVendettaWithPlayer(playerState, playerState2, out vendettaState, true))
				{
					if (IEnumerableExtensions.Any<ObjectiveCondition_CaptureCantons>(vendettaState.Vendetta.Objective.Conditions.OfType<ObjectiveCondition_CaptureCantons>()))
					{
						this.AddGoal(new GoalVendettaCaptureCantons(playerState2.Id));
					}
					else if (IEnumerableExtensions.Any<ObjectiveCondition_CapturePlacesOfPower>(vendettaState.Vendetta.Objective.Conditions.OfType<ObjectiveCondition_CapturePlacesOfPower>()))
					{
						this.AddGoal(new GoalVendettaCapturePoPs(playerState2.Id));
					}
					else if (IEnumerableExtensions.Any<ObjectiveCondition_DestroyLegions>(vendettaState.Vendetta.Objective.Conditions.OfType<ObjectiveCondition_DestroyLegions>()))
					{
						this.AddGoal(new GoalVendettaDestroyLegions(playerState2.Id));
					}
				}
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001B554 File Offset: 0x00019754
		private void GenerateEasyPopGoals(GOAPPlanner owningPlanner, TurnState turnState, PlayerState playerState)
		{
			if (owningPlanner.IsInvasionPending())
			{
				return;
			}
			GamePiece gamePiece = null;
			int num = int.MaxValue;
			foreach (PlayerState playerState2 in turnState.EnumeratePlayerStates(true, false))
			{
				if (turnState.CombatAuthorizedBetween(playerState.Id, playerState2.Id))
				{
					foreach (GamePiece gamePiece2 in turnState.GetAllActivePoPsForPlayer(playerState2.Id, false))
					{
						int num2;
						if (!gamePiece2.IsPandaemonium() && gamePiece2.Id != playerState2.StrongholdId && gamePiece2.Level <= 3 && owningPlanner.ArchfiendHeuristics.CouldReachGamePieceIfOwnersTerritoryCouldBeCrossed(playerState.Id, gamePiece2, out num2) && num2 != -1 && num2 <= 3 && num2 < num)
						{
							gamePiece = gamePiece2;
							num = num2;
						}
					}
				}
			}
			if (gamePiece != null)
			{
				this.AddGoal(new GoalCaptureSpecificPoP(gamePiece.Id, num));
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001B670 File Offset: 0x00019870
		private void GenerateVendettaNodes(TurnState turnState, PlayerState playerState)
		{
			IEnumerable<GamePiece> allActiveLegionsForPlayer = turnState.GetAllActiveLegionsForPlayer(playerState.Id);
			foreach (PlayerState playerState2 in turnState.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != playerState2.Id)
				{
					foreach (GamePiece gamePiece in allActiveLegionsForPlayer)
					{
						bool legionHasUpkeep = !gamePiece.UpkeepCost.IsZero;
						this.AddAction(new ActionMoveCapturePlayersTerritory(gamePiece, playerState2.Id, legionHasUpkeep));
					}
				}
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001B72C File Offset: 0x0001992C
		private void GenerateInvokeManuscriptNodes(TurnState turnState, PlayerState playerState, GameDatabase database)
		{
			foreach (ManuscriptStaticData manuscriptStaticData in database.Enumerate<ManuscriptStaticData>())
			{
				if (manuscriptStaticData.ManuscriptCategory == ManuscriptCategory.Primer)
				{
					using (IEnumerator<GamePiece> enumerator2 = turnState.GetAllActiveLegionsForPlayer(playerState.Id).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							GamePiece gamePiece = enumerator2.Current;
							if (gamePiece.UpkeepCost.IsZero && gamePiece.Level > 1)
							{
								this.AddAction(new ActionInvokeManuscript_Primer(manuscriptStaticData, gamePiece));
							}
						}
						continue;
					}
				}
				if (manuscriptStaticData.ManuscriptCategory == ManuscriptCategory.Treatise)
				{
					this.AddAction(new ActionInvokeManuscript_Treatise(manuscriptStaticData));
				}
				else if (manuscriptStaticData.ManuscriptCategory == ManuscriptCategory.Manual)
				{
					Praetor praetor;
					ConfigRef<PraetorCombatMoveStaticData> techniqueToReplace;
					bool replacesDuplicate;
					if (this._planner.TryGetBestPraetorForManual(manuscriptStaticData, out praetor, out techniqueToReplace, out replacesDuplicate))
					{
						this.AddAction(new ActionInvokeManuscript_Manual(manuscriptStaticData, praetor.Id, techniqueToReplace, replacesDuplicate));
					}
				}
				else if (manuscriptStaticData.ManuscriptCategory == ManuscriptCategory.Schematic)
				{
					this.AddAction(new ActionInvokeManuscript_Schematic(manuscriptStaticData));
				}
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001B840 File Offset: 0x00019A40
		private void GenerateRitualNodes(TurnState turnState, PlayerState playerState)
		{
			this.AddAction(new ActionCastIncreasePassivePrestige());
			this.AddAction(new ActionCastGainResources());
			this.AddAction(new ActionCastModifyArchfiend(playerState.Id, ActionCastModifyArchfiend.ModificationType.IncreaseTributeQuality));
			foreach (GamePiece gamePiece in turnState.GetActiveGamePiecesForPlayer(playerState.Id))
			{
				if (gamePiece.IsLegionOrTitan())
				{
					if (gamePiece.HP < gamePiece.TotalHP)
					{
						this.AddAction(new ActionCastHealGamePiece(gamePiece.Id));
					}
					this.AddAction(new ActionCastInfernalJuggernaut(gamePiece.Id));
				}
			}
			foreach (PlayerState playerState2 in turnState.EnumeratePlayerStates(true, false))
			{
				if (playerState.Id != playerState2.Id)
				{
					if (playerState.Id != -1)
					{
						this.AddAction(new ActionCastDarkAugury(playerState2.Id));
						this.AddAction(new ActionCastStealTribute(playerState2.Id));
						this.AddAction(new ActionCastDestroyTribute(playerState2.Id));
						this.AddAction(new ActionCastModifyArchfiend(playerState2.Id, ActionCastModifyArchfiend.ModificationType.BlockEvents));
						this.AddAction(new ActionCastModifyArchfiend(playerState2.Id, ActionCastModifyArchfiend.ModificationType.BlockOrders));
						this.AddAction(new ActionCastBlockRitualSlots(playerState2.Id));
						this.AddAction(new ActionCastStealManuscripts(playerState2.Id));
						this.AddAction(new ActionCastDestroyManuscripts(playerState2.Id));
					}
					foreach (GameItem gameItem in this._planner.TrueTurn.GetGameItemsControlledBy(playerState2.Id))
					{
						Artifact artifact = gameItem as Artifact;
						if (artifact == null)
						{
							Praetor praetor = gameItem as Praetor;
							if (praetor == null)
							{
								GamePiece gamePiece2 = gameItem as GamePiece;
								if (gamePiece2 != null)
								{
									bool flag = gamePiece2.IsFixture();
									if (!flag && gamePiece2.CanBeConverted)
									{
										this.AddAction(new ActionCastConvertGamePiece(gameItem.Id));
									}
									if (playerState2.Id != -1 || flag)
									{
										this.AddAction(new ActionCastDebuffGamePiece(gameItem.Id));
										this.AddAction(new ActionCastDamageGamePieceRitual(gameItem.Id, false));
										this.AddAction(new ActionCastDamageGamePieceRitual(gameItem.Id, true));
									}
								}
							}
							else
							{
								this.AddAction(new ActionCastStealGameItem(playerState2.Id, praetor, GameItemCategory.Praetor));
								this.AddAction(new ActionCastBanishGameItem(praetor));
							}
						}
						else
						{
							this.AddAction(new ActionCastStealGameItem(playerState2.Id, artifact, GameItemCategory.Artifact));
						}
					}
				}
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001BB34 File Offset: 0x00019D34
		private void GenerateDiplomaticNodes(TurnState turnState, PlayerState playerState)
		{
			int num;
			if (turnState.CurrentDiplomaticTurn.IsVassalOfAny(playerState.Id, out num))
			{
				return;
			}
			float gameProgress = this._planner.GameProgress;
			PlayerState playerState2;
			bool flag = turnState.TryGetNemesis(playerState, out playerState2);
			bool flag2 = turnState.CheckInstigatedVendettaWithAnyPlayer(playerState, false);
			foreach (PlayerState playerState3 in turnState.EnumeratePlayerStates(false, false))
			{
				if (playerState3.Id != playerState.Id)
				{
					if (DiplomaticStateProcessor.ValidateOrderType(turnState, playerState.Id, playerState3.Id, OrderTypes.SendEmissary, false))
					{
						this.AddAction(new ActionArmistice(playerState3.Id));
					}
					if (ActionLureOfExcess.CanBeUsedByArchfiend(playerState))
					{
						this.AddAction(new ActionLureOfExcess(playerState3.Id));
					}
					if (!flag2)
					{
						if (DiplomaticStateProcessor.ValidateOrderType(turnState, playerState.Id, playerState3.Id, OrderTypes.Demand, false))
						{
							this.AddAction(new ActionMakeDemand(playerState3.Id, DiplomaticBackingType.LegionThreateningPositions));
							this.AddAction(new ActionMakeDemand(playerState3.Id, DiplomaticBackingType.Praetor));
						}
						if (DiplomaticStateProcessor.ValidateOrderType(turnState, playerState.Id, playerState3.Id, OrderTypes.Insult, false))
						{
							this.AddAction(new ActionHurlInsult(playerState3.Id));
						}
						this.AddAction(new ActionExtort(playerState3.Id, ExtortType.Artifact, DiplomaticBackingType.Praetor));
						this.AddAction(new ActionExtort(playerState3.Id, ExtortType.Artifact, DiplomaticBackingType.LegionThreateningPositions));
						this.AddAction(new ActionExtort(playerState3.Id, ExtortType.Praetor, DiplomaticBackingType.Praetor));
						this.AddAction(new ActionExtort(playerState3.Id, ExtortType.Praetor, DiplomaticBackingType.LegionThreateningPositions));
						this.AddAction(new ActionAssertWeakness(playerState3.Id));
						this.AddAction(new ActionHumiliate(playerState3.Id));
						this.AddAction(new ActionDeclareBloodFeud(playerState3.Id));
					}
					if (DiplomaticStateProcessor.ValidateOrderType(turnState, playerState.Id, playerState3.Id, OrderTypes.RequestToBeVassalizedByTarget, false))
					{
						this.AddAction(new ActionBecomeVassal(playerState3.Id));
					}
					if (gameProgress > 0.5f && (!flag || playerState3.Id != playerState2.Id) && DiplomaticStateProcessor.ValidateOrderType(turnState, playerState.Id, playerState3.Id, OrderTypes.Vassalage, false))
					{
						this.AddAction(new ActionVassalize(playerState3.Id));
					}
				}
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001BD98 File Offset: 0x00019F98
		private void GenerateCombatNodes(GOAPPlanner owningPlanner, PlayerState playerState, TurnContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			PlayerState playerState2;
			currentTurn.TryGetNemesis(playerState, out playerState2);
			IEnumerable<GamePiece> enumerable = from t in context.CurrentTurn.GetActiveGamePiecesForPlayer(playerState.Id)
			where t.SubCategory.IsLegion()
			select t;
			float advantageRequired = 0.5f;
			foreach (PlayerState playerState3 in currentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState3.Id != playerState.Id)
				{
					DiplomaticPairStatus diplomaticStatus = context.CurrentTurn.GetDiplomaticStatus(playerState, playerState3);
					if (!(diplomaticStatus.DiplomaticState is BloodVassalageState))
					{
						int aggressor;
						int target;
						if (owningPlanner.IsInvasionPending(playerState3, out aggressor, out target))
						{
							this.AddGoal(new GoalPrepareInvasion(aggressor, target));
						}
						bool flag = playerState2 != null && playerState3.Id == playerState2.Id;
						bool flag2 = currentTurn.CheckInstigatedVendettaWithPlayer(playerState3, playerState);
						bool flag3 = diplomaticStatus.DiplomaticState is DiplomaticState_DraconicRazzia;
						if ((flag || flag2 || flag3) && !currentTurn.CheckInstigatedVendettaWithPlayer(playerState, playerState3))
						{
							this.AddGoal(new GoalAttackPlayer(playerState3.Id));
						}
						foreach (GamePiece gamePiece in from t in context.CurrentTurn.GetActiveGamePiecesForPlayer(playerState3.Id)
						where t.SubCategory.IsLegion()
						select t)
						{
							foreach (GamePiece gamePiece2 in enumerable)
							{
								bool legionHasUpkeep = !gamePiece2.UpkeepCost.IsZero;
								this.AddAction(new ActionMarchLegionAttack(gamePiece2, gamePiece, gamePiece.ControllingPlayerId, advantageRequired, legionHasUpkeep));
							}
						}
					}
				}
			}
			foreach (GamePiece gamePiece3 in context.CurrentTurn.GetAllActiveLegionsForPlayer(-1))
			{
				int ownership = context.HexBoard.GetOwnership(gamePiece3.Location);
				if (ownership == playerState.Id || ownership == -1)
				{
					foreach (GamePiece gamePiece4 in enumerable)
					{
						bool legionHasUpkeep2 = !gamePiece4.UpkeepCost.IsZero;
						this.AddAction(new ActionMarchLegionAttack(gamePiece4, gamePiece3, -1, advantageRequired, legionHasUpkeep2));
					}
				}
			}
			foreach (PlayerState playerState4 in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState4.Id != playerState.Id)
				{
					foreach (GamePiece gamePiece5 in enumerable)
					{
						bool legionHasUpkeep3 = !gamePiece5.UpkeepCost.IsZero;
						bool tryTeleport = gamePiece5.CanTeleport && gamePiece5.TeleportDistance > gamePiece5.GroundMoveDistance;
						this.AddAction(new ActionMarchToThreaten(gamePiece5, gamePiece5.ControllingPlayerId, playerState4.Id, legionHasUpkeep3, tryTeleport));
					}
				}
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001C1AC File Offset: 0x0001A3AC
		private void GenerateSuicideMissionAgainstPandaemoniumNodes(GOAPPlanner owningPlanner, PlayerState playerState, TurnContext context)
		{
			if (playerState.Excommunicated)
			{
				return;
			}
			if (!owningPlanner.IsEndGame || owningPlanner.IsWinning(-2147483648))
			{
				return;
			}
			if (owningPlanner.IsInitiatingDiplomacy())
			{
				return;
			}
			IEnumerable<GamePiece> source = from t in context.CurrentTurn.GetActiveGamePiecesForPlayer(playerState.Id)
			where t.IsLegionOrTitan()
			select t;
			if (source.Count<GamePiece>() <= 1)
			{
				return;
			}
			float num;
			if (!owningPlanner.ArchfiendHeuristics.TryGetMilitaryPower(playerState.Id, out num) || num <= 0.3f)
			{
				return;
			}
			GamePiece gamePiece = source.SelectMinOrDefault((GamePiece legion) => legion.Level - legion.GroundMoveDistance, null);
			if (gamePiece != null && gamePiece.Level <= 2 && gamePiece.GroundMoveDistance >= 2)
			{
				GamePiece pandaemonium = context.CurrentTurn.GetPandaemonium();
				this.AddAction(new ActionMarchLegionOnPandaemonium(gamePiece, pandaemonium, 0f, 0));
				foreach (GameItem gameItemToReassign in from itemId in gamePiece.Slots
				select context.CurrentTurn.FetchGameItem(itemId) into item
				where item.CanBePlacedInVault
				select item)
				{
					this.AddAction(new ActionRemoveAttachmentFromGamePiece(gamePiece, gameItemToReassign));
				}
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001C344 File Offset: 0x0001A544
		private void GenerateConquerPandaemoniumNodes(GOAPPlanner owningPlanner, PlayerState playerState, TurnContext context)
		{
			GamePiece pandaemonium = context.CurrentTurn.GetPandaemonium();
			if (pandaemonium == null || pandaemonium.ControllingPlayerId == playerState.Id)
			{
				return;
			}
			Identifier id;
			float num;
			if (!owningPlanner.ArchfiendHeuristics.TryGetBestLegionToAttackPandaemonium(playerState.Id, out id, out num))
			{
				return;
			}
			GamePiece legion;
			if (!context.CurrentTurn.TryFetchGameItem<GamePiece>(id, out legion))
			{
				return;
			}
			if (num >= 0.2f)
			{
				this.AddAction(new ActionMarchLegionOnPandaemonium(legion, pandaemonium, 0f, 2));
			}
			IEnumerable<HexCoord> neighbours = owningPlanner.PlayerViewOfTurnState.HexBoard.GetNeighbours(pandaemonium.Location, false);
			foreach (GamePiece gamePiece in from t in context.CurrentTurn.GetActiveGamePiecesForPlayer(playerState.Id)
			where t.IsLegionOrTitan()
			select t)
			{
				float num2 = GamePiece.CalcCombatAdvantageAtPosition(context, gamePiece, pandaemonium);
				if (num2 > 0.5f)
				{
					this.AddAction(new ActionMarchLegionOnPandaemonium(gamePiece, pandaemonium, 0.6f, 0));
					this.AddAction(new ActionMarchLegionOnPandaemonium(gamePiece, pandaemonium, 0.5f, 1));
				}
				if (num2 >= 0.05f && !IEnumerableExtensions.Contains<HexCoord>(neighbours, gamePiece.Location))
				{
					this.AddAction(ActionMoveToFlank.GamePiece(FlankIntent.FlankForAttack, gamePiece, pandaemonium, gamePiece.ShouldTeleport()));
				}
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001C4A0 File Offset: 0x0001A6A0
		private bool WillProvidePrestigeForPlayer(GameDatabase database, GameItem artifact, PlayerState playerState)
		{
			return IEnumerableExtensions.Accumulate<GamePieceModifierStaticData>(artifact.GetModifiers(database).OfType<GamePieceModifierStaticData>(), (GamePieceModifierStaticData t) => t.CalculatePowerChange(GamePieceStat.Prestige, 0)) > 0f;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001C4DC File Offset: 0x0001A6DC
		private void GenerateArtifactNodes(PlayerState playerState, TurnState playerViewOfTurnState, GameDatabase database)
		{
			IEnumerable<GameItem> first = playerState.VaultedItems.Select(new Func<Identifier, GameItem>(playerViewOfTurnState.FetchGameItem)).OfType<Artifact>();
			IEnumerable<GameItem> allBazaarItemsForCategory = playerViewOfTurnState.GetAllBazaarItemsForCategory(GameItemCategory.Artifact, false);
			foreach (GameItem gameItem in first.Concat(allBazaarItemsForCategory))
			{
				if (gameItem.AttachableTo.HasFlag(SlotType.Legion))
				{
					foreach (GamePiece targetGamePiece in from t in playerViewOfTurnState.GetActiveGamePiecesForPlayer(playerState.Id)
					where t.SubCategory.IsLegion()
					select t)
					{
						this.AddAction(new ActionEquipArtifactToGamePiece(gameItem, targetGamePiece));
					}
				}
				if (gameItem.AttachableTo.HasFlag(SlotType.Fixture) && this.WillProvidePrestigeForPlayer(database, gameItem, playerState))
				{
					this.AddAction(new ActionEquipPrestigeArtifactToPoP(gameItem.Id));
				}
				gameItem.AttachableTo.HasFlag(SlotType.Ritual);
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001C620 File Offset: 0x0001A820
		private void GenerateAttachPraetorNodes(GOAPPlanner owningPlanner, PlayerState playerState, TurnState playerViewOfTurnState)
		{
			IEnumerable<Praetor> enumerable = playerState.VaultedItems.Select(new Func<Identifier, GameItem>(playerViewOfTurnState.FetchGameItem)).OfType<Praetor>();
			IEnumerable<GamePiece> enumerable2 = from t in playerViewOfTurnState.GetAllActiveLegionsForPlayer(playerState.Id)
			where t.SubCategory == GamePieceCategory.Legion
			select t;
			IEnumerable<GamePiece> allActivePoPsForPlayer = playerViewOfTurnState.GetAllActivePoPsForPlayer(playerState.Id, true);
			foreach (GamePiece targetGamePiece in enumerable2)
			{
				foreach (Praetor praetor in enumerable)
				{
					this.AddAction(new ActionEquipPraetor(targetGamePiece, praetor, false, EquipPraetorMode.Legion, null));
				}
			}
			foreach (GamePiece targetGamePiece2 in allActivePoPsForPlayer)
			{
				foreach (Praetor praetor2 in enumerable)
				{
					ResourceAccumulation resourceAccumulation;
					if (praetor2.IsPoPAdministrator(owningPlanner.Database, out resourceAccumulation))
					{
						Cost resourcesGenerated = new Cost(new ResourceAccumulation[]
						{
							resourceAccumulation
						});
						this.AddAction(new ActionEquipPraetor(targetGamePiece2, praetor2, false, EquipPraetorMode.PoP, resourcesGenerated));
					}
				}
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001C7A0 File Offset: 0x0001A9A0
		private void GenerateHealLegionNodes(PlayerState playerState, TurnState playerViewOfTurnState)
		{
			IEnumerable<GamePiece> enumerable = from t in playerViewOfTurnState.GetAllActiveLegionsForPlayer(playerState.Id)
			where t.SubCategory == GamePieceCategory.Legion
			select t;
			IEnumerable<GamePiece> allActivePoPsForPlayer = playerViewOfTurnState.GetAllActivePoPsForPlayer(playerState.Id, false);
			foreach (GamePiece gamePiece in enumerable)
			{
				foreach (GamePiece flankedGamePieceIdentifier in allActivePoPsForPlayer)
				{
					this.AddAction(ActionMoveToFlank.GamePiece(FlankIntent.Flank_Heal, gamePiece, flankedGamePieceIdentifier, gamePiece.ShouldTeleport()));
				}
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001C864 File Offset: 0x0001AA64
		private void GenerateReinforceStrongholdNodes(PlayerState playerState, TurnState turnState)
		{
			if (!turnState.IsPlayerDisgraced(playerState) && !turnState.StrongholdCaptureAuthorizedWithAny(playerState.Id))
			{
				return;
			}
			IEnumerable<GamePiece> enumerable = from t in turnState.GetAllActiveLegionsForPlayer(playerState.Id)
			where t.SubCategory == GamePieceCategory.Legion
			select t;
			GamePiece stronghold = turnState.GetStronghold(playerState.Id);
			if (stronghold != null)
			{
				foreach (GamePiece gamePiece in enumerable)
				{
					this.AddAction(ActionMoveToFlank.GamePiece(FlankIntent.Flank_ReinforceStronghold, gamePiece, stronghold, gamePiece.ShouldTeleport()));
				}
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001C914 File Offset: 0x0001AB14
		private void GenerateCapturePopNodes(PlayerState playerState, TurnState playerViewOfTurnState)
		{
			float advantageRequired = 0.6f;
			IEnumerable<GamePiece> enumerable = from gamePiece in playerViewOfTurnState.GetAllActiveFixtures()
			where gamePiece.ControllingPlayerId != playerState.Id
			select gamePiece;
			IEnumerable<GamePiece> enumerable2 = from t in playerViewOfTurnState.GetPiecesControlledBy(playerState.Id).Where(new Func<GamePiece, bool>(LegionMovementProcessor.CanCreateMovementOrder))
			where t.CanInitiateCombat
			select t;
			foreach (GamePiece gamePiece2 in enumerable)
			{
				GamePiece pandaemonium = playerViewOfTurnState.GetPandaemonium();
				if (pandaemonium == null || gamePiece2.Id != pandaemonium.Id)
				{
					foreach (GamePiece legion in enumerable2)
					{
						this.AddAction(new ActionMarchLegionOnPoP(legion, gamePiece2, advantageRequired));
					}
				}
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001CA28 File Offset: 0x0001AC28
		private void GenerateBazaarNodes(PlayerState playerState, TurnState playerViewOfTurnState, GameDatabase database)
		{
			IEnumerable<GamePiece> allActiveLegionsForPlayer = playerViewOfTurnState.GetAllActiveLegionsForPlayer(playerState.Id);
			IEnumerable<GamePiece> allActivePoPsForPlayer = playerViewOfTurnState.GetAllActivePoPsForPlayer(playerState.Id, true);
			foreach (GameItem gameItem in playerViewOfTurnState.GetAllBazaarItems())
			{
				Praetor praetor = gameItem as Praetor;
				if (praetor != null)
				{
					this.AddAction(new ActionBidOnPraetor(gameItem.Id));
					foreach (GamePiece gamePiece in allActiveLegionsForPlayer)
					{
						if (gamePiece.SubCategory == GamePieceCategory.Legion)
						{
							this.AddAction(new ActionEquipPraetor(gamePiece, praetor, true, EquipPraetorMode.Legion, null));
						}
					}
					ResourceAccumulation resourceAccumulation;
					if (praetor.IsPoPAdministrator(database, out resourceAccumulation))
					{
						Cost resourcesGenerated = new Cost(new ResourceAccumulation[]
						{
							resourceAccumulation
						});
						foreach (GamePiece targetGamePiece in allActivePoPsForPlayer)
						{
							this.AddAction(new ActionEquipPraetor(targetGamePiece, praetor, true, EquipPraetorMode.PoP, resourcesGenerated));
						}
					}
				}
				GamePiece gamePiece2 = gameItem as GamePiece;
				if (gamePiece2 != null)
				{
					this.AddAction(new ActionBidOnLegion(gamePiece2, 5));
				}
				if (gameItem.Category == GameItemCategory.Artifact)
				{
					this.AddAction(new ActionBidOnArtifact(gameItem.Id));
				}
				if (gameItem.Category == GameItemCategory.ManuscriptPiece)
				{
					this.AddAction(new ActionBidOnManuscript(gameItem.Id));
				}
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001CBE0 File Offset: 0x0001ADE0
		private void GenerateArchfiendSpecificNodes(GOAPPlanner owningPlanner, PlayerState playerState, GameDatabase database)
		{
			TurnState aipreviewTurn = owningPlanner.AIPreviewTurn;
			if (ActionDraconicRazzia.CanBeUsedByArchfiend(playerState))
			{
				foreach (PlayerState playerState2 in aipreviewTurn.EnumeratePlayerStates(false, false))
				{
					if (playerState2.Id != playerState.Id && !owningPlanner.AIPreviewContext.CurrentTurn.CombatAuthorizedBetween(playerState.Id, playerState2.Id))
					{
						this.AddAction(new ActionDraconicRazzia(playerState2.Id));
					}
				}
			}
			if (ActionVanitysAnointed.CanBeUsedByArchfiend(playerState))
			{
				foreach (GameItem gameItem in from t in aipreviewTurn.GetFieldedGameItemsControlledBy<GameItem>(playerState.Id)
				where t.Category == GameItemCategory.Praetor
				select t)
				{
					this.AddAction(new ActionVanitysAnointed(gameItem.Id));
				}
			}
			if (ActionHellsMaw.CanBeUsedByArchfiend(playerState))
			{
				this.AddAction(new ActionHellsMaw());
			}
			if (ActionBalefulGaze.CanBeUsedByArchfiend(playerState))
			{
				foreach (GamePiece gamePiece in from t in aipreviewTurn.GetActiveGamePieces()
				where t.ControllingPlayerId != -1 && t.ControllingPlayerId != owningPlanner.PlayerState.Id
				select t)
				{
					this.AddAction(new ActionBalefulGaze(gamePiece.Id));
				}
			}
			if (ActionRaiseDarkPylon.CanBeUsedByArchfiend(playerState))
			{
				foreach (Hex hex in aipreviewTurn.HexBoard.Hexes)
				{
					float num;
					if (owningPlanner.TerrainInfluenceMap[hex.HexCoord].TryGetPylonDesirability(out num) && num > 0f)
					{
						this.AddAction(new ActionRaiseDarkPylon(hex.HexCoord));
					}
				}
			}
			if (ActionChainsOfAvarice.CanBeUsedByArchfiend(playerState))
			{
				foreach (PlayerState playerState3 in aipreviewTurn.EnumeratePlayerStates(false, false))
				{
					if (playerState3.Id != owningPlanner.AIPreviewPlayerState.Id)
					{
						this.AddAction(new ActionChainsOfAvarice(playerState3.Id));
					}
				}
			}
			if (ActionVileCalumny.CanBeUsedByArchfiend(playerState))
			{
				PlayerState aipreviewPlayerState = owningPlanner.AIPreviewPlayerState;
				int cooldownTurns = 3;
				foreach (PlayerState playerState4 in aipreviewTurn.EnumeratePlayerStates(false, false))
				{
					if (playerState4.Id != aipreviewPlayerState.Id)
					{
						foreach (PlayerState playerState5 in aipreviewTurn.EnumeratePlayerStates(false, false))
						{
							if (playerState5.Id != aipreviewPlayerState.Id && playerState5.Id != playerState4.Id)
							{
								DiplomaticPairStatus diplomaticStatus = aipreviewTurn.GetDiplomaticStatus(aipreviewPlayerState, playerState4);
								DiplomaticPairStatus diplomaticStatus2 = aipreviewTurn.GetDiplomaticStatus(playerState5, playerState4);
								Result result = diplomaticStatus.IsOrderAllowed(aipreviewTurn, OrderTypes.VileCalumny, aipreviewPlayerState.Id);
								if (diplomaticStatus2.IsOrderAllowed(aipreviewTurn, OrderTypes.Insult, playerState5.Id).successful && result.successful)
								{
									this.AddAction(new ActionVileCalumny(playerState4.Id, playerState5.Id, cooldownTurns));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001CFA4 File Offset: 0x0001B1A4
		private void GenerateStratagemNodes(GOAPPlanner owningPlanner, PlayerState playerState, GameDatabase database)
		{
			foreach (GamePiece gamePiece in from t in owningPlanner.AIPreviewTurn.GetAllActiveLegionsForPlayer(playerState.Id)
			where t.SubCategory == GamePieceCategory.Legion
			select t)
			{
				this.AddAction(new ActionForgeCombatCard(gamePiece.Id));
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001D02C File Offset: 0x0001B22C
		private void GeneratePowerLevelupNodes(GOAPPlanner owningPlanner, GameDatabase database)
		{
			foreach (PowersStaticData powersStaticData in this.GetPowersForLevelUp(owningPlanner.PlayerState, database))
			{
				this.AddAction(new ActionLevelUpArchfiend(powersStaticData.PowerType, powersStaticData._level));
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001D090 File Offset: 0x0001B290
		private void GenerateEdictNodes(TurnState playerViewOfTurnState)
		{
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001D092 File Offset: 0x0001B292
		private IEnumerable<PowersStaticData> GetPowersForLevelUp(PlayerState player, GameDatabase database)
		{
			foreach (PowerType powerType in PlayerPowersLevels.PowerTypes)
			{
				PlayerPowerLevel level = player.PowersLevels[powerType];
				List<PowersStaticData> list = IEnumerableExtensions.ToList<PowersStaticData>(database.EnumeratePowersOfType(powerType));
				IEnumerable<PowersStaticData> source = list;
				Func<PowersStaticData, bool> predicate;
				Func<PowersStaticData, bool> <>9__0;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((PowersStaticData t) => t.Level == level.CurrentLevel + 1));
				}
				foreach (PowersStaticData powersStaticData in source.Where(predicate))
				{
					yield return powersStaticData;
				}
				IEnumerator<PowersStaticData> enumerator2 = null;
			}
			IEnumerator<PowerType> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001D0AC File Offset: 0x0001B2AC
		private void GenerateDecisionResponseNodes(TurnState playerViewOfTurnState, PlayerState playerState)
		{
			foreach (DecisionRequest decisionRequest in playerState.DecisionRequests)
			{
				InsultDecisionRequest insultDecisionRequest = decisionRequest as InsultDecisionRequest;
				if (insultDecisionRequest != null)
				{
					RespondToInsult action = new RespondToInsult(YesNo.Yes, insultDecisionRequest);
					this.AddAction(action);
					RespondToInsult action2 = new RespondToInsult(YesNo.No, insultDecisionRequest);
					this.AddAction(action2);
				}
				SelectTributeDecisionRequest selectTributeDecisionRequest = decisionRequest as SelectTributeDecisionRequest;
				if (selectTributeDecisionRequest != null && IEnumerableExtensions.Any<Manuscript>(selectTributeDecisionRequest.Candidates.Manuscripts))
				{
					int selectionMax = selectTributeDecisionRequest.SelectionMax;
					foreach (IEnumerable<int> selection in MathUtils.GetCombinations<int>(selectTributeDecisionRequest.Candidates.ItemIds, selectionMax))
					{
						RespondToManuscriptSelect action3 = new RespondToManuscriptSelect(selectTributeDecisionRequest, selection);
						this.AddAction(action3);
					}
				}
				MakeDemandDecisionRequest makeDemandDecisionRequest = decisionRequest as MakeDemandDecisionRequest;
				if (makeDemandDecisionRequest != null)
				{
					RespondToDemand action4 = new RespondToDemand(YesNo.No, makeDemandDecisionRequest);
					this.AddAction(action4);
				}
				GrievanceDecisionRequest grievanceDecisionRequest = decisionRequest as GrievanceDecisionRequest;
				if (grievanceDecisionRequest != null)
				{
					ArchfiendHeuristics archfiendHeuristics = this._planner.ArchfiendHeuristics;
					PlayerState aipreviewPlayerState = this._planner.AIPreviewPlayerState;
					PlayerState target = this._planner.AIPreviewContext.CurrentTurn.FindPlayerState(grievanceDecisionRequest.GrievanceTargetPlayerId, null);
					bool flag = grievanceDecisionRequest.TriggeringOrderType == OrderTypes.Demand;
					GrievanceContext grievanceContext;
					bool flag2;
					float num;
					if (!this._planner.TryGenerateGrievance(target, out grievanceContext, out flag2, out num) || (num >= 1f && !grievanceDecisionRequest.MustAccept) || (!flag2 && !flag))
					{
						break;
					}
					if (grievanceContext is PraetorBattleContext)
					{
						RespondToGrievanceChoice action5 = new RespondToGrievanceChoice(grievanceDecisionRequest, grievanceContext, GrievanceType.Duel, YesNo.Yes);
						this.AddAction(action5);
					}
					else
					{
						RespondToGrievanceChoice action6 = new RespondToGrievanceChoice(grievanceDecisionRequest, grievanceContext, GrievanceType.Combat, YesNo.Yes);
						this.AddAction(action6);
					}
				}
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001D298 File Offset: 0x0001B498
		private void SetupHeuristic()
		{
			foreach (GOAPNode heuristicsViaDepthSearch in this.Goals)
			{
				this.SetHeuristicsViaDepthSearch(heuristicsViaDepthSearch);
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001D2E8 File Offset: 0x0001B4E8
		private void SetHeuristicsViaDepthSearch(GOAPNode start)
		{
			HashSet<GOAPNode> layer = new HashSet<GOAPNode>(start.GetGOAPNeighbours());
			this.SetHeuristicsViaDepthSearch(start, layer, 0);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001D30C File Offset: 0x0001B50C
		private void SetHeuristicsViaDepthSearch(GOAPNode start, HashSet<GOAPNode> layer, int depth)
		{
			HashSet<GOAPNode> hashSet = new HashSet<GOAPNode>();
			foreach (GOAPNode goapnode in layer)
			{
				if (goapnode.AddHeuristicIfBetter(start, (float)depth))
				{
					foreach (GOAPNode item in goapnode.GetGOAPNeighbours())
					{
						hashSet.Add(item);
					}
				}
			}
			if (hashSet.Count > 0)
			{
				this.SetHeuristicsViaDepthSearch(start, hashSet, depth + 1);
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001D3B8 File Offset: 0x0001B5B8
		public void ConnectNeighbours()
		{
			List<GOAPNode> list = IEnumerableExtensions.ToList<GOAPNode>(from t in this.AllNodes
			where !t.IsDisabled()
			select t);
			foreach (GOAPNode goapnode in list)
			{
				foreach (GOAPNode goapnode2 in list)
				{
					if (goapnode != goapnode2 && goapnode.ShouldBeNeighbour(goapnode2))
					{
						goapnode.AddNeighbour(goapnode2);
					}
				}
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001D480 File Offset: 0x0001B680
		public void AddLateAction(GOAPNode action)
		{
			action = action.Initialize(this._planner);
			foreach (GOAPNode other in this.AllNodes)
			{
				action.TryAddNeighbour(other);
			}
			HashSet<GOAPNode> layer = new HashSet<GOAPNode>
			{
				action
			};
			foreach (GOAPNode goapnode in this.AllNodes)
			{
				if (goapnode.TryAddNeighbour(action))
				{
					if (goapnode is GoalGOAPNode)
					{
						this.SetHeuristicsViaDepthSearch(goapnode, layer, 0);
					}
					else
					{
						foreach (GOAPNode goapnode2 in this._goals)
						{
							float num;
							if (goapnode.TryGetHeuristic(goapnode2, out num))
							{
								this.SetHeuristicsViaDepthSearch(goapnode2, layer, (int)num + 1);
							}
						}
					}
				}
			}
			action.EvaluateAndCachePropertyStatus();
			this._actions.Add(action);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001D5A4 File Offset: 0x0001B7A4
		public override bool IsDestination(PFNode node)
		{
			return !node.IsDisabled() && (node as GOAPNode).IsDestination();
		}

		// Token: 0x040002CF RID: 719
		private const int StartingHeuristicDepth = 0;

		// Token: 0x040002D0 RID: 720
		private List<GOAPNode> _goals = new List<GOAPNode>();

		// Token: 0x040002D1 RID: 721
		private List<GOAPNode> _actions = new List<GOAPNode>();

		// Token: 0x040002D2 RID: 722
		private GOAPPlanner _planner;
	}
}
