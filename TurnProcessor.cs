using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Game.Simulation.StaticData;
using Game.StaticData;
using Zenject;

namespace LoG
{
	// Token: 0x020006D9 RID: 1753
	public class TurnProcessor
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x0006E8D8 File Offset: 0x0006CAD8
		public GameDatabase Database
		{
			get
			{
				return this._database;
			}
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0006E8E0 File Offset: 0x0006CAE0
		public TurnProcessor()
		{
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0006E8F3 File Offset: 0x0006CAF3
		public TurnProcessor(GameDatabase database)
		{
			this._database = database;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x0006E90D File Offset: 0x0006CB0D
		public IReadOnlyList<Exception> SuppressedExceptions
		{
			get
			{
				return this._suppressedExceptions;
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600202A RID: 8234 RVA: 0x0006E918 File Offset: 0x0006CB18
		// (remove) Token: 0x0600202B RID: 8235 RVA: 0x0006E950 File Offset: 0x0006CB50
		public event TurnProcessor.OnDecisionResult DecicisonResultEvent;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600202C RID: 8236 RVA: 0x0006E988 File Offset: 0x0006CB88
		// (remove) Token: 0x0600202D RID: 8237 RVA: 0x0006E9C0 File Offset: 0x0006CBC0
		public event TurnProcessor.OnActionResult ActionResultEvent;

		// Token: 0x0600202E RID: 8238 RVA: 0x0006E9F5 File Offset: 0x0006CBF5
		public IEnumerable<PlayerState> EnumerateOrderedPlayerTurns(TurnState turn)
		{
			return from t in turn.EnumeratePlayerStatesInTurnOrder(true, false)
			where t.PlayerTurn != null
			select t;
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x0006EA23 File Offset: 0x0006CC23
		public IEnumerable<TurnProcessor.PlayerAction> GenerateActionContexts(TurnProcessContext turnContext)
		{
			int highestOrderCount = 0;
			foreach (PlayerState playerState in turnContext.CurrentTurn.PlayerStates)
			{
				if (!playerState.Eliminated)
				{
					highestOrderCount = Math.Max(highestOrderCount, playerState.OrderSlots);
				}
			}
			int num;
			for (int i = 0; i < highestOrderCount; i = num + 1)
			{
				bool executedOrder = false;
				foreach (PlayerState playerState2 in this.EnumerateOrderedPlayerTurns(turnContext.CurrentTurn))
				{
					List<ActionableOrder> orders = playerState2.PlayerTurn.Orders;
					ActionProcessContext context = new ActionProcessContext(i);
					if (i < orders.Count)
					{
						ActionableOrder actionableOrder = orders[i];
						ActionProcessor actionProcessor = ActionProcessorFactory.PrepareProcessor(turnContext, playerState2, actionableOrder);
						yield return new TurnProcessor.PlayerAction(playerState2, actionableOrder, actionProcessor, context);
						executedOrder = true;
					}
					else
					{
						yield return new TurnProcessor.PlayerAction(playerState2, context);
					}
				}
				IEnumerator<PlayerState> enumerator2 = null;
				if (!executedOrder)
				{
					break;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x0006EA3A File Offset: 0x0006CC3A
		public TurnProcessContext CreateTurnProcessContext(GameState state, TurnState turn)
		{
			return new TurnProcessContext(state.Rules, turn, this._database);
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x0006EA4E File Offset: 0x0006CC4E
		public TurnState ProcessNewTurn(GameState state)
		{
			return this.ProcessNewTurn(state, state.TurnCount - 1).CurrentTurn;
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0006EA64 File Offset: 0x0006CC64
		public TurnProcessContext ProcessNewTurn(GameState state, int turnValue)
		{
			TurnState turnState = state.CreateNewTurn(turnValue);
			TurnProcessContext result = this.ProcessTurn(state, turnState);
			turnState.AssignGameEventIds();
			return result;
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0006EA87 File Offset: 0x0006CC87
		public void ProcessAsNewTurn(TurnProcessContext context)
		{
			context.CurrentTurn.PrepareForNewTurn(context.CurrentTurn.TurnValue + 1);
			this.ProcessTurn(context);
			context.CurrentTurn.AssignGameEventIds();
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x0006EAB8 File Offset: 0x0006CCB8
		public TurnState ProcessNewTurn(GameState state, TurnState turnState, out TurnProcessContext context)
		{
			TurnState newTurn = state.CreateNewTurn(turnState);
			context = this.ProcessTurn(state, newTurn);
			return turnState;
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x0006EAD8 File Offset: 0x0006CCD8
		public TurnProcessContext ProcessTurn(GameState state, TurnState newTurn)
		{
			TurnProcessContext processContext = this.CreateTurnProcessContext(state, newTurn);
			return this.ProcessTurn(processContext);
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x0006EAF8 File Offset: 0x0006CCF8
		public GameState ProcessTurnZero(GameState gameState)
		{
			TurnProcessContext turnProcessContext = new TurnProcessContext(gameState.Rules, gameState.CurrentTurn, this._database);
			TurnState currentTurn = turnProcessContext.CurrentTurn;
			GameDatabase database = turnProcessContext.Database;
			currentTurn.InitializeRegencyOrder();
			turnProcessContext.RecalculateAllPlayerModifiers();
			currentTurn.SetupDiplomacyPairs();
			currentTurn.AssignInitialHexOwnership(this._database);
			turnProcessContext.PopulateNeutralForces();
			turnProcessContext.InitializeBazaarSlots();
			PlayerState playerState = currentTurn.FindPlayerState(currentTurn.RegentPlayerId, null);
			turnProcessContext.DrawEventCard(playerState);
			if (playerState != null)
			{
				currentTurn.ConclaveFavouriteId = playerState.Id;
			}
			VictoryProcessor.ProcessKingmakerDecisions(turnProcessContext);
			this.ProcessGameAbilities(turnProcessContext);
			this.PrepareStartingEconomy(turnProcessContext);
			currentTurn.AssignGameEventIds();
			foreach (PlayerState playerState2 in currentTurn.EnumeratePlayerStates(false, false))
			{
				playerState2.RegenerateAITags(database);
			}
			return gameState;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x0006EBDC File Offset: 0x0006CDDC
		private void ProcessGameAbilities(TurnProcessContext context)
		{
			TurnState turn = context.CurrentTurn;
			GameDatabase database = context.Database;
			Func<Identifier, IEnumerable<Ability>> <>9__0;
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(true, false))
			{
				IEnumerable<Identifier> activeRelics = playerState.ActiveRelics;
				Func<Identifier, IEnumerable<Ability>> selector;
				if ((selector = <>9__0) == null)
				{
					selector = (<>9__0 = ((Identifier r) => database.GetLocalAbilities(turn.FetchGameItem(r))));
				}
				foreach (Ability ability in activeRelics.SelectMany(selector))
				{
					foreach (GameAbilityEffect gameAbilityEffect in ability.Effects.OfType<GameAbilityEffect>())
					{
						gameAbilityEffect.Process(context, playerState);
					}
				}
			}
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0006ECF0 File Offset: 0x0006CEF0
		private void PrepareStartingEconomy(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			TributeEconomyStaticData tributeEconomyStaticData = context.Database.FetchSingle<TributeEconomyStaticData>();
			if (tributeEconomyStaticData == null)
			{
				return;
			}
			foreach (PlayerState player in currentTurn.EnumeratePlayerStates(false, false))
			{
				this.PrepareStartingEconomy(context, player, tributeEconomyStaticData);
			}
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x0006ED58 File Offset: 0x0006CF58
		private void PrepareStartingEconomy(TurnProcessContext context, PlayerState player, TributeEconomyStaticData economy)
		{
			CardGenerationData cardGenerationData = context.Database.Fetch(economy.DemandTributeGenerationData);
			player.ResourceQueue.ConfigureFrom(cardGenerationData.ResourceDistribution);
			player.ManuscriptQueue.ConfigureDefault();
			for (int i = 0; i < economy.NumResourceDrawsToStart; i++)
			{
				DemandTributeUtils.CreateOfferingWithEvent(context, player);
			}
			foreach (ResourceAccumulation resourceAccumulation in economy.StartingTributeCards)
			{
				player.GiveResources(new ResourceNFT[]
				{
					context.CurrentTurn.CreateNFT(new ResourceAccumulation[]
					{
						resourceAccumulation
					})
				});
			}
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x0006EE0C File Offset: 0x0006D00C
		private void BroadcastTurnProcessStage(TurnProcessContext processContext, TurnProcessStage stage)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				processContext.CurrentStage = stage;
				this.ProcessTurnStageAbilities(processContext, stage);
				foreach (TurnModuleProcessor turnModuleProcessor in IEnumerableExtensions.ToList<TurnModuleProcessor>(processContext.TurnModuleProcessors))
				{
					try
					{
						turnModuleProcessor.Invoke(stage);
					}
					catch (Exception e)
					{
						this.OnException(e);
					}
				}
			}
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0006EEB0 File Offset: 0x0006D0B0
		private void BroadcastActiveRitualsTurnEnd(TurnProcessContext context)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				foreach (ActiveRitual activeRitual in IEnumerableExtensions.ToList<ActiveRitual>(context.CurrentTurn.GetFieldedGameItemsControlledBy(playerState.Id).OfType<ActiveRitual>()))
				{
					activeRitual.OnEndOfTurn(context, playerState);
				}
			}
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x0006EF54 File Offset: 0x0006D154
		private void ProcessTurnStageAbilities(TurnProcessContext processContext, TurnProcessStage stage)
		{
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				TurnState currentTurn = processContext.CurrentTurn;
				foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(true, false))
				{
					foreach (GamePiece gamePiece in IEnumerableExtensions.ToList<GamePiece>(currentTurn.GetAllGamePiecesForPlayer(playerState.Id)))
					{
						foreach (ValueTuple<Ability, TurnAbilityEffect> valueTuple in processContext.GetAbilityEffects(gamePiece))
						{
							Ability item = valueTuple.Item1;
							TurnAbilityEffect item2 = valueTuple.Item2;
							if (item2.HasEffectInStage == stage)
							{
								try
								{
									item2.OnStageOfTurn(stage, item, processContext, gamePiece);
								}
								catch (Exception e)
								{
									this.OnException(e);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x0006F090 File Offset: 0x0006D290
		public TurnProcessContext ProcessTurn(TurnProcessContext processContext)
		{
			return this.ProcessTurnInternal(processContext);
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x0006F09C File Offset: 0x0006D29C
		private TurnProcessContext ProcessTurnInternal(TurnProcessContext processContext)
		{
			processContext.SetExceptionHandler(new Func<Exception, Result>(this.OnException));
			TurnState currentTurn = processContext.CurrentTurn;
			processContext.BeginProcess(TurnProcessContext.ProcessType.Process);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_TurnStart);
			this.ProcessBanishedEntities(processContext);
			this.ProcessAbilityCooldowns(processContext);
			this.ProcessPlayerTurns(currentTurn, processContext);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_NeutralForces);
			BidProcessor.ProcessBids(processContext, processContext.BiddingContext.Bids);
			processContext.RePopulateBazaar(false);
			DiplomaticOrderProcessor.ProcessDiplomaticActions(currentTurn, processContext.DiplomaticContext.DiplomaticActions);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_Edicts);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_PostEdicts);
			this.ProcessDeferredEvents(processContext, processContext.EventsContext.GrandEventOrders);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_Events);
			DiplomaticStateProcessor.UpdateDiplomaticState(processContext);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_Damage);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_LateDamage);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_Healing);
			this.ProcessHealing(currentTurn);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_Prestige);
			this.ProcessPassivePrestige(processContext);
			this.ProcessLevelUps(processContext);
			this.ProcessActiveRituals(processContext);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_Tribute);
			processContext.ProcessSiphonedTribute();
			this.ProcessOfferings(processContext);
			this.ProcessActiveRituals(processContext);
			VotingProcessor.ProcessTurn(processContext);
			this.ProcessUpkeepPayments(processContext);
			this.ProcessRitualSlotChanges(processContext);
			this.ProcessEndingActiveRituals(processContext);
			this.ProcessRitualsWithDurations(processContext);
			this.CreateUpkeepRequests(processContext.CurrentTurn);
			this.CreateRitualRequests(processContext.CurrentTurn);
			this.ProcessStrongholdEliminations(processContext);
			this.ProcessRepatriation(processContext);
			this.ClampGamePieceHitPoints(currentTurn);
			this.ProcessVaultOverflow(processContext);
			this.ProcessConclaveFavourite(currentTurn);
			VictoryProcessor.ProcessTurn(processContext);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_PreRegency);
			this.ProcessRegency(processContext);
			this.ProcessEventDraws(processContext);
			this.ProcessLeakInformation(processContext);
			processContext.UpdateSchemes();
			RivalryProcessor.ProcessTurn(processContext);
			this.ProcessMessageTriggers(currentTurn, processContext);
			this.UpdateAccolades(processContext);
			this.CleanupAbilityCooldowns(processContext);
			this.CleanupTemporaryAbilities(processContext);
			this.RecalculateSupportModifiers(processContext);
			this.BroadcastActiveRitualsTurnEnd(processContext);
			this.BroadcastTurnProcessStage(processContext, TurnProcessStage.TurnModule_TurnEnd);
			processContext.EndProcess();
			return processContext;
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x0006F26C File Offset: 0x0006D46C
		private void ProcessEventDraws(TurnProcessContext processContext)
		{
			foreach (PlayerState playerState in processContext.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				int numDraws = processContext.EventDrawContext.GetNumDraws(playerState.Id);
				if (numDraws > 0)
				{
					GameEvent ev = processContext.DrawEventCard(playerState, numDraws);
					RegencyChangedEvent regencyChangedEvent;
					if (processContext.CurrentTurn.RegentPlayerId == playerState.Id && processContext.CurrentTurn.TryGetGameEvent<RegencyChangedEvent>(out regencyChangedEvent) && regencyChangedEvent.NewRegentIndex == playerState.Id)
					{
						regencyChangedEvent.AddChildEvent(ev);
					}
					else
					{
						DrawEventCardEvent drawEventCardEvent = new DrawEventCardEvent(playerState.Id);
						drawEventCardEvent.AddChildEvent(ev);
						processContext.CurrentTurn.AddGameEvent<DrawEventCardEvent>(drawEventCardEvent);
					}
				}
			}
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x0006F340 File Offset: 0x0006D540
		private void ProcessLeakInformation(TurnProcessContext processContext)
		{
			foreach (PlayerState playerState in processContext.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				foreach (EntityTag_RevealPowersOnEndTurn entityTag_RevealPowersOnEndTurn in playerState.TagList.OfType<EntityTag_RevealPowersOnEndTurn>())
				{
					foreach (PlayerState playerState2 in processContext.CurrentTurn.EnumeratePlayerStates(false, false))
					{
						if (playerState2.Id != playerState.Id)
						{
							processContext.RevealPowersToPlayer(playerState, playerState2, entityTag_RevealPowersOnEndTurn.WhatToReveal);
						}
					}
				}
			}
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x0006F430 File Offset: 0x0006D630
		private void ProcessVaultOverflow(TurnProcessContext processContext)
		{
			foreach (PlayerState playerState in processContext.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				int num;
				if (playerState.IsVaultOverflowing(processContext.Rules.VaultSize, out num))
				{
					List<ResourceNFT> list = IEnumerableExtensions.ToList<ResourceNFT>(Enumerable.TakeLast<ResourceNFT>(playerState.Resources, num));
					VaultOverflowedEvent vaultOverflowedEvent = new VaultOverflowedEvent(playerState.Id, list);
					PaymentRemovedEvent ev = processContext.RemovePayment(playerState, new Payment(list, 0), null);
					vaultOverflowedEvent.AddChildEvent<PaymentRemovedEvent>(ev);
					processContext.CurrentTurn.AddGameEvent<VaultOverflowedEvent>(vaultOverflowedEvent);
				}
			}
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x0006F4DC File Offset: 0x0006D6DC
		private void ProcessMessageTriggers(TurnState newTurn, TurnProcessContext context)
		{
			if (context.IsPreview)
			{
				return;
			}
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false).Shuffle(context.Random))
			{
				if (playerState.Role != PlayerRole.Human)
				{
					foreach (CannedMessageTrigger cannedMessageTrigger in playerState.MessageTriggers)
					{
						if (!cannedMessageTrigger.IsCompleted && cannedMessageTrigger.Condition.Evaluate(newTurn, context, this._database))
						{
							cannedMessageTrigger.Message.Id = context.CurrentTurn.GenerateIdentifier();
							cannedMessageTrigger.Message.TurnId = context.CurrentTurn.TurnValue;
							context.CurrentTurn.FindPlayerState(cannedMessageTrigger.Message.ToPlayerId, null).MessageLog.AddIncomingMessage(cannedMessageTrigger.Message);
							context.CurrentTurn.AddGameEvent<MessageReceivedEvent>(new MessageReceivedEvent(cannedMessageTrigger.Message));
							cannedMessageTrigger.MarkCompleted();
						}
					}
					if (context.Rules.TriggerGenericCannedMessages)
					{
						NemesisChangeMessage.TriggerForValidRecipients(context, playerState);
					}
				}
			}
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x0006F634 File Offset: 0x0006D834
		private void ProcessAbilityCooldowns(TurnProcessContext context)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(true, true))
			{
				foreach (string text in IEnumerableExtensions.ToList<string>(playerState.AbilityCooldowns.Keys))
				{
					Dictionary<string, int> abilityCooldowns = playerState.AbilityCooldowns;
					string key = text;
					abilityCooldowns[key]--;
				}
			}
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x0006F6E0 File Offset: 0x0006D8E0
		private void CleanupAbilityCooldowns(TurnProcessContext context)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(true, true))
			{
				foreach (string key in IEnumerableExtensions.ToList<string>(playerState.AbilityCooldowns.Keys))
				{
					if (playerState.AbilityCooldowns[key] <= 0)
					{
						playerState.AbilityCooldowns.Remove(key);
					}
				}
			}
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0006F790 File Offset: 0x0006D990
		private void CleanupTemporaryAbilities(TurnContext context)
		{
			foreach (GamePiece gamePiece in context.CurrentTurn.EnumerateAllGamePieces())
			{
				if (gamePiece.Status == GameItemStatus.Banished && gamePiece.DeathTurn == context.CurrentTurn.TurnValue)
				{
					foreach (ValueTuple<Ability, List<AbilityEffect_RemoveOnDeath>> valueTuple in IEnumerableExtensions.ToList<ValueTuple<Ability, List<AbilityEffect_RemoveOnDeath>>>(from ability in context.GetAllAbilitiesFor(gamePiece)
					select new ValueTuple<Ability, List<AbilityEffect_RemoveOnDeath>>(ability, IEnumerableExtensions.ToList<AbilityEffect_RemoveOnDeath>(ability.Effects.OfType<AbilityEffect_RemoveOnDeath>()))))
					{
						Ability item = valueTuple.Item1;
						foreach (AbilityEffect_RemoveOnDeath abilityEffect_RemoveOnDeath in valueTuple.Item2)
						{
							abilityEffect_RemoveOnDeath.OnOwnerBanished(item, context, gamePiece);
						}
					}
				}
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x0006F8B0 File Offset: 0x0006DAB0
		private void RecalculateSupportModifiers(TurnProcessContext context)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				context.RecalculateSupportModifiers(playerState.Id);
			}
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x0006F90C File Offset: 0x0006DB0C
		public void UpdateAccolades(TurnProcessContext context)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, true))
			{
				PlayerGameStatistics gameStatistics = playerState.GameStatistics;
				foreach (AccoladeStaticData accoladeStaticData in context.Database.Enumerate<AccoladeStaticData>())
				{
					int value = accoladeStaticData.DetermineMetric(context, playerState);
					gameStatistics.Process(accoladeStaticData.Id, accoladeStaticData.Accumulation, value);
				}
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x0006F9BC File Offset: 0x0006DBBC
		public void ProcessBanishedEntities(TurnProcessContext context)
		{
			TurnProcessor.<>c__DisplayClass44_0 CS$<>8__locals1 = new TurnProcessor.<>c__DisplayClass44_0();
			CS$<>8__locals1.context = context;
			foreach (GameItem item in IEnumerableExtensions.ToList<GameItem>(CS$<>8__locals1.context.CurrentTurn.AllGameItems.Where(new Func<GameItem, bool>(CS$<>8__locals1.<ProcessBanishedEntities>g__CanRemoveEntity|0))))
			{
				CS$<>8__locals1.context.RemoveGameItemFromGame(item);
			}
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x0006FA44 File Offset: 0x0006DC44
		public void ProcessUpkeepPayments(TurnProcessContext context)
		{
			if (!context.Rules.UpkeepEnabled)
			{
				return;
			}
			if (!context.IsPreview)
			{
				this.ProcessUpkeepRequests(context);
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x0006FA64 File Offset: 0x0006DC64
		private void ProcessConclaveFavourite(TurnState turn)
		{
			PlayerState conclaveFavourite = turn.GetConclaveFavourite();
			List<int> list = new List<int>();
			List<PlayerState> list2 = IEnumerableExtensions.ToList<PlayerState>(from x in IEnumerableExtensions.ExceptFor<PlayerState>(turn.EnumeratePlayerStates(false, false), new PlayerState[]
			{
				conclaveFavourite
			})
			where x.IsValidConclaveFavourite(turn)
			select x);
			if (list2.Count <= 0)
			{
				return;
			}
			int num = 0;
			foreach (PlayerState playerState in list2)
			{
				if (playerState.SpendablePrestige > num)
				{
					num = playerState.SpendablePrestige;
					list.Clear();
					list.Add(playerState.Id);
				}
				else if (playerState.SpendablePrestige == num)
				{
					list.Add(playerState.Id);
				}
			}
			if (list.Count < 0)
			{
				return;
			}
			if (num <= conclaveFavourite.SpendablePrestige && conclaveFavourite.IsValidConclaveFavourite(turn))
			{
				return;
			}
			if (list.Count == 1)
			{
				turn.ConclaveFavouriteId = list[0];
				turn.AddGameEvent<ConclaveFavouriteChangedEvent>(new ConclaveFavouriteChangedEvent(conclaveFavourite.Id, turn.ConclaveFavouriteId));
				return;
			}
			int conclaveFavouriteId = list.OrderByDescending(new Func<int, int>(turn.TurnsUntilRegency)).Last<int>();
			turn.ConclaveFavouriteId = conclaveFavouriteId;
			turn.AddGameEvent<ConclaveFavouriteChangedEvent>(new ConclaveFavouriteChangedEvent(conclaveFavourite.Id, turn.ConclaveFavouriteId));
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0006FBFC File Offset: 0x0006DDFC
		private void ProcessDeferredEvents(TurnProcessContext context, List<PlayerGrandEventAction> eventOrders)
		{
			foreach (PlayerGrandEventAction playerGrandEventAction in eventOrders)
			{
				try
				{
					playerGrandEventAction.Processor.ProcessDeferred(playerGrandEventAction.Order);
				}
				catch (Exception e)
				{
					this.OnException(e);
				}
			}
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x0006FC70 File Offset: 0x0006DE70
		private void ProcessRepatriation(TurnProcessContext context)
		{
			foreach (GamePiece gamePiece in context.CurrentTurn.GetAllActiveLegions())
			{
				if (!gamePiece.CanOccupyCanton(context, gamePiece.Location))
				{
					RepatriateLegionEvent gameEvent = gamePiece.Repatriate(context);
					context.CurrentTurn.AddGameEvent<RepatriateLegionEvent>(gameEvent);
				}
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x0006FCE0 File Offset: 0x0006DEE0
		public void ProcessStrongholdEliminations(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
			{
				GamePiece stronghold = currentTurn.GetStronghold(playerState.Id);
				if (stronghold != null && (!stronghold.IsAlive() || stronghold.ControllingPlayerId != playerState.Id))
				{
					PlayerState killer = currentTurn.FindPlayerState(stronghold.ControllingPlayerId, null);
					context.EliminatePlayer(playerState, killer);
				}
			}
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x0006FD78 File Offset: 0x0006DF78
		private void ProcessRitualSlotChanges(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
			{
				int num = playerState.RitualState.SlottedItems.Count - playerState.RitualState.NumRitualSlots.Value;
				for (int i = 0; i < num; i++)
				{
					List<Identifier> slottedItems = playerState.RitualState.SlottedItems;
					int index = slottedItems.Count - 1;
					Identifier id = slottedItems[index];
					ActiveRitual activeRitual = currentTurn.FetchGameItem(id) as ActiveRitual;
					if (activeRitual != null)
					{
						context.BanishGameItem(activeRitual, int.MinValue);
					}
				}
			}
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x0006FE40 File Offset: 0x0006E040
		private void ProcessOfferings(TurnProcessContext context)
		{
			if (!context.IsOfferingTurn())
			{
				return;
			}
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (!playerState.BlockOfferings)
				{
					DemandTributeUtils.CreateOfferingWithEvent(context, playerState);
				}
			}
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x0006FEAC File Offset: 0x0006E0AC
		private void ProcessLevelUps(TurnProcessContext context)
		{
			GamePropertiesStaticData gamePropertiesStaticData = context.Database.FetchSingle<GamePropertiesStaticData>();
			if (gamePropertiesStaticData == null)
			{
				return;
			}
			LegionLevelTable legionLevelTable = context.Database.Fetch(gamePropertiesStaticData.LegionLevelTable);
			if (legionLevelTable == null)
			{
				return;
			}
			foreach (GamePiece gamePiece in context.CurrentTurn.GetActiveGamePieces())
			{
				try
				{
					gamePiece.ProcessLevelUp(context, legionLevelTable, 1);
				}
				catch (Exception e)
				{
					this.OnException(e);
				}
			}
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x0006FF44 File Offset: 0x0006E144
		private void ProcessRegency(TurnProcessContext context)
		{
			TurnProcessor.<>c__DisplayClass53_0 CS$<>8__locals1;
			CS$<>8__locals1.turn = context.CurrentTurn;
			CS$<>8__locals1.startingRegentId = CS$<>8__locals1.turn.RegentPlayerId;
			CS$<>8__locals1.validNextRegentId = int.MinValue;
			CS$<>8__locals1.playerStatesCount = CS$<>8__locals1.turn.PlayerStates.Count;
			for (int i = 1; i <= CS$<>8__locals1.playerStatesCount; i++)
			{
				int playerIdInRegencyOrder = CS$<>8__locals1.turn.GetPlayerIdInRegencyOrder(CS$<>8__locals1.startingRegentId, i);
				PlayerState playerState = CS$<>8__locals1.turn.FindPlayerState(playerIdInRegencyOrder, null);
				if (!playerState.Eliminated)
				{
					ModifiableBool validRegent = playerState.ValidRegent;
					if (validRegent)
					{
						CS$<>8__locals1.validNextRegentId = playerIdInRegencyOrder;
						break;
					}
					foreach (BooleanModifier booleanModifier in validRegent.ActiveModifiers.OfType<BooleanModifier>())
					{
						if (!booleanModifier.Value)
						{
							PlayerContext playerContext = booleanModifier.Provider as PlayerContext;
							if (playerContext != null)
							{
								RegencySkippedEvent gameEvent = new RegencySkippedEvent(playerContext.PlayerId, playerIdInRegencyOrder);
								CS$<>8__locals1.turn.AddGameEvent<RegencySkippedEvent>(gameEvent);
							}
						}
					}
				}
			}
			if (CS$<>8__locals1.validNextRegentId == -2147483648)
			{
				TurnProcessor.<ProcessRegency>g__AssignFallbackRegent|53_0(ref CS$<>8__locals1);
			}
			if (CS$<>8__locals1.startingRegentId != CS$<>8__locals1.validNextRegentId)
			{
				CS$<>8__locals1.turn.RegentPlayerId = CS$<>8__locals1.validNextRegentId;
				RegencyChangedEvent gameEvent2 = new RegencyChangedEvent(CS$<>8__locals1.startingRegentId, CS$<>8__locals1.validNextRegentId);
				PlayerState playerState2 = CS$<>8__locals1.turn.FindPlayerState(CS$<>8__locals1.turn.RegentPlayerId, null);
				context.QueueEventCardDraw(playerState2.Id, playerState2.EventCardDraw.Value);
				CS$<>8__locals1.turn.AddGameEvent<RegencyChangedEvent>(gameEvent2);
				playerState2.RecordPlayerWasRegent(CS$<>8__locals1.turn.TurnValue);
			}
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00070108 File Offset: 0x0006E308
		private void CreateUpkeepRequests(TurnState turn)
		{
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(false, false))
			{
				if (!playerState.HasTag<EntityTag_CheatNoUpkeep>() || playerState.Resources.Count <= 0)
				{
					foreach (GameItem gameItem in from x in turn.GetGameItemsControlledBy(playerState.Id)
					where x.Status == GameItemStatus.InPlay
					where x.UpkeepOutstanding.AnyGreaterThan(ResourceAccumulation.Empty, true)
					select x)
					{
						if (gameItem.Category != GameItemCategory.ActiveRitual)
						{
							gameItem.NextUpkeepTurn = turn.TurnValue + 1;
							this.CreateUpkeepRequest(turn, gameItem, playerState);
						}
					}
				}
			}
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x00070214 File Offset: 0x0006E414
		private void CreateRitualRequests(TurnState turn)
		{
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Resources.Count > 0)
				{
					if (playerState.TagList.OfType<EntityTag_CheatNoUpkeep>().Any((EntityTag_CheatNoUpkeep t) => t.NoUpkeepFromRituals))
					{
						continue;
					}
				}
				foreach (Identifier id in playerState.RitualState.SlottedItems)
				{
					ActiveRitual activeRitual;
					if (turn.TryFetchGameItem<ActiveRitual>(id, out activeRitual))
					{
						activeRitual.NextUpkeepTurn = turn.TurnValue + 1;
						this.CreateMaintainRequest(turn, activeRitual, playerState);
					}
				}
			}
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x00070308 File Offset: 0x0006E508
		private void ProcessActiveRituals(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
			{
				foreach (ActiveRitual activeRitual in IEnumerableExtensions.ToList<ActiveRitual>(currentTurn.GetFieldedGameItemsControlledBy(playerState.Id).OfType<ActiveRitual>()))
				{
					try
					{
						if (!activeRitual.ValidateRitual(context, playerState).successful)
						{
							ItemBanishedEvent ev = context.BanishGameItem(activeRitual, int.MinValue);
							UpkeepFailed upkeepFailed = new UpkeepFailed(playerState.Id, activeRitual.Id, GameItemCategory.ActiveRitual, 0);
							upkeepFailed.AddChildEvent<ItemBanishedEvent>(ev);
							currentTurn.AddGameEvent<UpkeepFailed>(upkeepFailed);
						}
						else if (context.CurrentStage == TurnProcessStage.TurnModule_Prestige)
						{
							activeRitual.OnProcessPrestige(context, playerState);
						}
						else if (context.CurrentStage == TurnProcessStage.TurnModule_Tribute)
						{
							activeRitual.OnProcessTribute(context, playerState);
						}
					}
					catch (Exception e)
					{
						this.OnException(e);
					}
				}
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x00070444 File Offset: 0x0006E644
		private void ProcessEndingActiveRituals(TurnProcessContext context)
		{
			HashSet<PlayerState> hashSet = new HashSet<PlayerState>();
			foreach (ValueTuple<PlayerState, ActiveRitual> valueTuple in context.ProcessContexts.RitualsToEnd)
			{
				PlayerState item = valueTuple.Item1;
				ActiveRitual item2 = valueTuple.Item2;
				if (item2.Status != GameItemStatus.Banished)
				{
					UpkeepFailed upkeepFailed = new UpkeepFailed(item.Id, item2, GameItemCategory.ActiveRitual, item2.UpkeepCost.ValueSum);
					upkeepFailed.AddChildEvent<ItemBanishedEvent>(context.BanishGameItem(item2, int.MinValue));
					context.CurrentTurn.AddGameEvent<UpkeepFailed>(upkeepFailed);
					hashSet.Add(item);
				}
			}
			foreach (PlayerState player in hashSet)
			{
				context.RecalculateAllModifiersFor(player);
			}
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00070540 File Offset: 0x0006E740
		private void ProcessRitualsWithDurations(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
			{
				foreach (ActiveRitual activeRitual in IEnumerableExtensions.ToList<ActiveRitual>(currentTurn.GetFieldedGameItemsControlledBy(playerState.Id).OfType<ActiveRitual>()))
				{
					if (activeRitual.FixedDuration && currentTurn.TurnValue >= activeRitual.EndTurn)
					{
						context.BanishGameItem(activeRitual, int.MinValue);
					}
				}
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x00070608 File Offset: 0x0006E808
		private void ProcessHealing(TurnState turn)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (PlayerState playerState in turn.EnumeratePlayerStates(true, false))
			{
				dictionary[playerState.Id] = playerState.HealingRate;
			}
			foreach (GamePiece gamePiece in turn.GetActiveGamePieces())
			{
				if (!gamePiece.HasTag<EntityTag_CannotRegenerateHealth>())
				{
					if (gamePiece.SubCategory.IsFixture())
					{
						int amount = dictionary[gamePiece.ControllingPlayerId];
						gamePiece.Heal(amount);
					}
					else
					{
						foreach (HexCoord coord in turn.HexBoard.GetNeighbours(gamePiece.Location, false))
						{
							GamePiece gamePieceAt = turn.GetGamePieceAt(coord);
							if (gamePieceAt != null)
							{
								bool flag = gamePieceAt.IsFixture();
								bool flag2 = gamePiece.CanBeHealedBy(turn, gamePieceAt);
								if (flag && flag2)
								{
									int amount2 = dictionary[gamePiece.ControllingPlayerId];
									gamePiece.Heal(amount2);
									break;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00070768 File Offset: 0x0006E968
		private void ClampGamePieceHitPoints(TurnState turn)
		{
			foreach (GamePiece gamePiece in turn.GetActiveGamePieces())
			{
				if (gamePiece.HP > gamePiece.TotalHP)
				{
					gamePiece.HP = gamePiece.TotalHP;
				}
			}
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000707D4 File Offset: 0x0006E9D4
		private void ProcessUpkeepRequests(TurnProcessContext context)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (!playerState.HasTag<EntityTag_CheatNoUpkeep>() || playerState.Resources.Count <= 0)
				{
					this.ProcessUpkeepRequests(context, playerState);
				}
			}
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00070840 File Offset: 0x0006EA40
		private void ProcessUpkeepRequests(TurnProcessContext context, PlayerState player)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (GameItem gameItem in from x in currentTurn.GetGameItemsControlledBy(player.Id)
			where x.Status == GameItemStatus.InPlay
			where x.NextUpkeepTurn == context.CurrentTurn.TurnValue
			where !x.UpkeepCost.IsZero
			select x)
			{
				try
				{
					if (gameItem.UpkeepOutstanding.AnyGreaterThan(Cost.None, true))
					{
						UpkeepFailed upkeepFailed = new UpkeepFailed(player.Id, gameItem, gameItem.Category, gameItem.UpkeepCost.ValueSum);
						upkeepFailed.AddChildEvent<ItemBanishedEvent>(context.BanishGameItem(gameItem, int.MinValue));
						context.RecalculateAllModifiersFor(player);
						currentTurn.AddGameEvent<UpkeepFailed>(upkeepFailed);
					}
					else
					{
						gameItem.UseUpkeep(!context.Rules.AllowUpkeepOverpay);
					}
				}
				catch (Exception e)
				{
					this.OnException(e);
				}
			}
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000709A4 File Offset: 0x0006EBA4
		private void CreateUpkeepRequest(TurnState turn, GameItem gameItem, PlayerState player)
		{
			UpkeepDecisionRequest decisionRequest = new UpkeepDecisionRequest(turn, gameItem.Id, gameItem.UpkeepOutstanding);
			turn.AddDecisionToAskPlayer(player.Id, decisionRequest);
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000709D8 File Offset: 0x0006EBD8
		private void CreateMaintainRequest(TurnState turn, GameItem gameItem, PlayerState player)
		{
			MaintainRitualDecisionRequest decisionRequest = new MaintainRitualDecisionRequest(turn, gameItem.Id, gameItem.UpkeepOutstanding);
			turn.AddDecisionToAskPlayer(player.Id, decisionRequest);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00070A0C File Offset: 0x0006EC0C
		public void PreviewTurn(GameState state, TurnState newTurn)
		{
			TurnProcessContext turnProcessContext = new TurnProcessContext(state.Rules, newTurn, this._database);
			turnProcessContext.BeginProcess(TurnProcessContext.ProcessType.Preview);
			this.ProcessPlayerTurns(newTurn, turnProcessContext);
			turnProcessContext.EndProcess();
			newTurn.AssignGameEventIds();
			turnProcessContext.RecalculateAllPlayerModifiers();
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x00070A50 File Offset: 0x0006EC50
		public void AIPreviewTurn(TurnState newTurn, GameRules gameRules)
		{
			TurnProcessContext turnProcessContext = new TurnProcessContext(gameRules, newTurn, this._database);
			turnProcessContext.BeginProcess(TurnProcessContext.ProcessType.AIPreview);
			this.ProcessPlayerTurns(newTurn, turnProcessContext);
			turnProcessContext.EndProcess();
			newTurn.AssignGameEventIds();
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00070A88 File Offset: 0x0006EC88
		public void ProcessPlayerTurns(TurnState newTurn, TurnProcessContext context)
		{
			if (!context.IsAIPreview)
			{
				Dictionary<PlayerState, List<Message>> dictionary = new Dictionary<PlayerState, List<Message>>();
				foreach (PlayerState playerState in newTurn.EnumeratePlayerStates(false, false))
				{
					dictionary.Add(playerState, playerState.PlayerTurn.OutgoingMessages);
				}
				this.ProcessMessages(context, dictionary);
			}
			Dictionary<PlayerState, List<DecisionRequest>> dictionary2 = new Dictionary<PlayerState, List<DecisionRequest>>();
			if (!context.IsPreview)
			{
				context.RecalculateAllPlayerModifiers();
			}
			foreach (PlayerState playerState2 in newTurn.EnumeratePlayerStates(false, false))
			{
				dictionary2[playerState2] = IEnumerableExtensions.ToList<DecisionRequest>(playerState2.DecisionRequests);
			}
			if (!context.IsPreview)
			{
				newTurn.ClearDecisionRequests();
			}
			this.ProcessDecisions(context, dictionary2);
			foreach (PlayerState playerState3 in newTurn.EnumeratePlayerStates(false, false))
			{
				while (playerState3.PlayerTurn.Orders.Count > Math.Max(playerState3.OrderSlots, 0))
				{
					ActionableOrder action = ListExtensions.PopLast<ActionableOrder>(playerState3.PlayerTurn.Orders);
					this.PublishActionResult(context, action, playerState3, Result.NotEnoughSlots());
				}
			}
			List<TurnProcessor.PlayerAction> list = IEnumerableExtensions.ToList<TurnProcessor.PlayerAction>(this.GenerateActionContexts(context));
			this.ReorderPlayerActions(list);
			this.ProcessActionables(newTurn, list, context);
			if (!context.IsPreview)
			{
				foreach (PlayerState playerState4 in newTurn.EnumeratePlayerStates(false, false))
				{
					if (playerState4.PlayerTurn.DataChanges != null)
					{
						playerState4.AIPersistentData = playerState4.PlayerTurn.DataChanges;
					}
				}
				context.RecalculateAllPlayerModifiers();
				if (!context.ForcePreservePlayerTurns)
				{
					newTurn.ClearPlayerTurns();
				}
			}
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x00070C88 File Offset: 0x0006EE88
		private void ReorderPlayerActions(List<TurnProcessor.PlayerAction> playerActions)
		{
			for (int i = playerActions.Count - 1; i >= 0; i--)
			{
				TurnProcessor.PlayerAction playerAction = playerActions[i];
				IRitualProcessor ritualProcessor = playerAction.ActionActionProcessor as IRitualProcessor;
				if (ritualProcessor != null)
				{
					CastRitualOrder castRitualOrder = playerAction.Action as CastRitualOrder;
					if (castRitualOrder != null && ritualProcessor.GetRitualMaskingMode() == RitualMaskingMode.Framed)
					{
						int currentMaskingTargetId = castRitualOrder.RitualMaskingSettings.CurrentMaskingTargetId;
						playerActions.RemoveAt(i);
						bool flag = false;
						for (int j = i; j < playerActions.Count; j++)
						{
							if (playerActions[j].Player.Id == currentMaskingTargetId)
							{
								playerActions.Insert(j + 1, playerAction);
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							playerActions.Add(playerAction);
						}
					}
				}
			}
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00070D3C File Offset: 0x0006EF3C
		private void ProcessPassivePrestige(TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (PlayerState playerState in currentTurn.EnumeratePlayerStates(false, false))
			{
				int prestige = currentTurn.GetPiecesControlledBy(playerState.Id).Sum((GamePiece piece) => piece.GetPrestigeGeneration());
				PaymentReceivedEvent gameEvent = context.GivePrestige(playerState, prestige);
				currentTurn.AddGameEvent<PaymentReceivedEvent>(gameEvent);
			}
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x00070DD0 File Offset: 0x0006EFD0
		public void ProcessMessages(TurnProcessContext context, Dictionary<PlayerState, List<Message>> outgoingMessagesPerPlayerState)
		{
			if (context.IsPreview)
			{
				return;
			}
			foreach (KeyValuePair<PlayerState, List<Message>> keyValuePair in outgoingMessagesPerPlayerState)
			{
				PlayerState playerState;
				List<Message> list;
				keyValuePair.Deconstruct(out playerState, out list);
				foreach (Message message in list)
				{
					message.Id = context.CurrentTurn.GenerateIdentifier();
					context.CurrentTurn.FindPlayerState(message.FromPlayerId, null).MessageLog.AddOutgoingMessage(message);
					if (!message.IsMuted)
					{
						context.CurrentTurn.FindPlayerState(message.ToPlayerId, null).MessageLog.AddIncomingMessage(message);
						context.CurrentTurn.AddGameEvent<MessageReceivedEvent>(new MessageReceivedEvent(message));
					}
				}
			}
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x00070ED8 File Offset: 0x0006F0D8
		public void ProcessDecisions(TurnProcessContext context, Dictionary<PlayerState, List<DecisionRequest>> requests)
		{
			TurnProcessor.<>c__DisplayClass71_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.preview = CS$<>8__locals1.context.IsPreview;
			foreach (KeyValuePair<PlayerState, List<DecisionRequest>> keyValuePair in requests)
			{
				PlayerState playerState;
				List<DecisionRequest> list;
				keyValuePair.Deconstruct(out playerState, out list);
				PlayerState playerState2 = playerState;
				foreach (DecisionRequest decisionRequest in list)
				{
					DecisionResponse decision = playerState2.PlayerTurn.GetDecision(decisionRequest.DecisionId);
					if (!CS$<>8__locals1.preview || decision != null)
					{
						Result result = Result.Failure;
						try
						{
							result = TurnProcessor.<ProcessDecisions>g__ProcessDecision|71_0(playerState2, decisionRequest, decision, ref CS$<>8__locals1);
						}
						catch (Exception e)
						{
							result = this.OnException(e);
						}
						this.PublishDecisionResult(CS$<>8__locals1.context, decisionRequest, decision, result);
					}
				}
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x00070FEC File Offset: 0x0006F1EC
		public void ProcessActionables(TurnState turn, List<TurnProcessor.PlayerAction> actionables, TurnProcessContext turncontext)
		{
			foreach (TurnProcessor.PlayerAction playerAction in IEnumerableExtensions.ToList<TurnProcessor.PlayerAction>(actionables))
			{
				try
				{
					if (playerAction.IsEmptyOrder)
					{
						actionables.Remove(playerAction);
					}
					else
					{
						Problem problem = playerAction.ActionActionProcessor.Validate() as Problem;
						if (problem != null)
						{
							this.PublishActionResult(turncontext, playerAction, problem);
							actionables.Remove(playerAction);
						}
					}
				}
				catch (Exception e)
				{
					this.PublishActionResult(turncontext, playerAction, this.OnException(e));
					actionables.Remove(playerAction);
				}
			}
			foreach (TurnProcessor.PlayerAction playerAction2 in IEnumerableExtensions.ToList<TurnProcessor.PlayerAction>(actionables))
			{
				try
				{
					Cost cost = playerAction2.ActionActionProcessor.CalculateCost();
					Problem problem2 = turn.ValidatePayment(playerAction2.Player, cost, playerAction2.Action.Payment) as Problem;
					if (problem2 != null)
					{
						AbilityStaticData dataForRequest = turncontext.GetDataForRequest(playerAction2.Action);
						if (dataForRequest != null)
						{
							problem2 = playerAction2.Action.PaymentFailedProblem(dataForRequest.ConfigRef, problem2);
						}
						this.PublishActionResult(turncontext, playerAction2, problem2);
						actionables.Remove(playerAction2);
					}
					else
					{
						Problem problem3 = playerAction2.Player.AcceptPayment(playerAction2.Action.Payment) as Problem;
						if (problem3 != null)
						{
							this.PublishActionResult(turncontext, playerAction2, problem3);
							actionables.Remove(playerAction2);
						}
					}
				}
				catch (Exception e2)
				{
					this.PublishActionResult(turncontext, playerAction2, this.OnException(e2));
					actionables.Remove(playerAction2);
				}
			}
			foreach (TurnProcessor.PlayerAction playerAction3 in actionables)
			{
				ActionProcessor actionActionProcessor = playerAction3.ActionActionProcessor;
				ActionProcessContext context = playerAction3.Context;
				Result result = Result.Failure;
				try
				{
					Result result2;
					switch (turncontext.Process)
					{
					case TurnProcessContext.ProcessType.Process:
						result2 = actionActionProcessor.Process(context);
						break;
					case TurnProcessContext.ProcessType.Preview:
						result2 = actionActionProcessor.Preview(context);
						break;
					case TurnProcessContext.ProcessType.AIPreview:
						result2 = actionActionProcessor.AIPreview(context);
						break;
					default:
						result2 = Result.Failure;
						break;
					}
					result = result2;
				}
				catch (Exception e3)
				{
					result = this.OnException(e3);
				}
				this.PublishActionResult(turncontext, playerAction3, result);
			}
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x00071280 File Offset: 0x0006F480
		public void PublishActionResult(TurnProcessContext turnContext, TurnProcessor.PlayerAction context, Result result)
		{
			if (result.successful)
			{
				AbilityStaticData abilityData = context.ActionActionProcessor.AbilityData;
				if (abilityData != null && abilityData.Cooldown > 0)
				{
					context.Player.SetAbilityCooldown(abilityData);
				}
			}
			else
			{
				context.ActionActionProcessor.OnActionFailure(context.Context, result);
			}
			this.PublishActionResult(turnContext, context.Action, context.Player, result);
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x000712E4 File Offset: 0x0006F4E4
		public void PublishActionResult(TurnProcessContext context, ActionableOrder action, PlayerState player, Result result)
		{
			context.AddActionResult(action.ActionInstanceId, result);
			TurnState currentTurn = context.CurrentTurn;
			if (!context.IsPreview && !result.successful)
			{
				currentTurn.AddGameEvent<ActionFailedEvent>(new ActionFailedEvent(action, result, player.Id));
			}
			TurnProcessor.OnActionResult actionResultEvent = this.ActionResultEvent;
			if (actionResultEvent == null)
			{
				return;
			}
			actionResultEvent(action, result);
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x0007133F File Offset: 0x0006F53F
		public void PublishDecisionResult(TurnProcessContext context, DecisionRequest decisionRequest, DecisionResponse decisionResponse, Result result)
		{
			context.AddDecisionResult(decisionRequest.DecisionId, result);
			TurnProcessor.OnDecisionResult decicisonResultEvent = this.DecicisonResultEvent;
			if (decicisonResultEvent == null)
			{
				return;
			}
			decicisonResultEvent(decisionRequest, decisionResponse, result);
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00071364 File Offset: 0x0006F564
		private Result OnException(Exception e)
		{
			if (!this.SuppressExceptions)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Caught an exception at " + e.StackTrace);
				}
				throw e;
			}
			this._suppressedExceptions.Add(e);
			return new SimulationError(e.Message);
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x000713B4 File Offset: 0x0006F5B4
		[CompilerGenerated]
		internal static void <ProcessRegency>g__AssignFallbackRegent|53_0(ref TurnProcessor.<>c__DisplayClass53_0 A_0)
		{
			for (int i = 1; i <= A_0.playerStatesCount; i++)
			{
				int playerIdInRegencyOrder = A_0.turn.GetPlayerIdInRegencyOrder(A_0.startingRegentId, i);
				if (!A_0.turn.FindPlayerState(playerIdInRegencyOrder, null).Eliminated)
				{
					A_0.validNextRegentId = playerIdInRegencyOrder;
					break;
				}
			}
			if (A_0.validNextRegentId == -2147483648)
			{
				A_0.validNextRegentId = A_0.startingRegentId;
			}
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x0007141C File Offset: 0x0006F61C
		[CompilerGenerated]
		internal static Result <ProcessDecisions>g__ProcessDecision|71_0(PlayerState playerState, DecisionRequest decisionRequest, DecisionResponse decisionResponse, ref TurnProcessor.<>c__DisplayClass71_0 A_3)
		{
			DecisionProcessor decisionProcessor = DecisionProcessorFactory.PrepareProcessor(A_3.context, playerState, decisionRequest, decisionResponse);
			if (decisionResponse == null || !decisionProcessor.GetResponseType().IsInstanceOfType(decisionResponse))
			{
				decisionResponse = decisionProcessor.GenerateFallbackResponse();
			}
			Result result = decisionProcessor.Validate(decisionResponse);
			if (!A_3.preview)
			{
				if (!result.successful)
				{
					decisionResponse = decisionProcessor.GenerateFallbackResponse();
				}
				return decisionProcessor.Process(decisionResponse);
			}
			if (!result.successful)
			{
				return result;
			}
			return decisionProcessor.Preview(decisionResponse);
		}

		// Token: 0x04000E40 RID: 3648
		[Inject]
		public GameDatabase _database;

		// Token: 0x04000E41 RID: 3649
		public bool SuppressExceptions;

		// Token: 0x04000E42 RID: 3650
		private List<Exception> _suppressedExceptions = new List<Exception>();

		// Token: 0x02000AC6 RID: 2758
		// (Invoke) Token: 0x060032BB RID: 12987
		public delegate void OnDecisionResult(DecisionRequest decisionRequest, DecisionResponse decisionResponse, Result result);

		// Token: 0x02000AC7 RID: 2759
		// (Invoke) Token: 0x060032BF RID: 12991
		public delegate void OnActionResult(ActionableOrder order, Result result);

		// Token: 0x02000AC8 RID: 2760
		public readonly struct PlayerAction
		{
			// Token: 0x060032C2 RID: 12994 RVA: 0x0009D57D File Offset: 0x0009B77D
			public PlayerAction(PlayerState player, ActionableOrder action, ActionProcessor actionProcessor, ActionProcessContext context)
			{
				this.Player = player;
				this.Action = action;
				this.ActionActionProcessor = actionProcessor;
				this.Context = context;
			}

			// Token: 0x060032C3 RID: 12995 RVA: 0x0009D59C File Offset: 0x0009B79C
			public PlayerAction(PlayerState player, ActionProcessContext context)
			{
				this.Player = player;
				this.Action = null;
				this.ActionActionProcessor = null;
				this.Context = context;
			}

			// Token: 0x1700075F RID: 1887
			// (get) Token: 0x060032C4 RID: 12996 RVA: 0x0009D5BA File Offset: 0x0009B7BA
			public bool IsEmptyOrder
			{
				get
				{
					return this.Action == null && this.ActionActionProcessor == null;
				}
			}

			// Token: 0x04001AEA RID: 6890
			public readonly PlayerState Player;

			// Token: 0x04001AEB RID: 6891
			public readonly ActionableOrder Action;

			// Token: 0x04001AEC RID: 6892
			public readonly ActionProcessor ActionActionProcessor;

			// Token: 0x04001AED RID: 6893
			public readonly ActionProcessContext Context;
		}
	}
}
