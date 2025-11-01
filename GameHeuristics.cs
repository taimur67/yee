using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020006E6 RID: 1766
	public class GameHeuristics
	{
		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x00074108 File Offset: 0x00072308
		public static IEnumerable<HeuristicKey> Keys
		{
			get
			{
				List<HeuristicKey> result;
				if ((result = GameHeuristics._keys) == null)
				{
					result = (GameHeuristics._keys = IEnumerableExtensions.ExceptFor<HeuristicKey>(EnumUtility.GetValues<HeuristicKey>(), new HeuristicKey[]
					{
						HeuristicKey.Invalid
					}).ToList<HeuristicKey>());
				}
				return result;
			}
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x00074134 File Offset: 0x00072334
		public float GetValue(HeuristicKey key, HeuristicValueType type)
		{
			float result;
			switch (type)
			{
			case HeuristicValueType.AveragePerTurn:
				result = this.GetAveragePerTurn(key);
				break;
			case HeuristicValueType.AveragePerPlayer:
				result = this.GetAveragePerPlayer(key);
				break;
			case HeuristicValueType.AveragePPPT:
				result = this.GetAveragePerPlayerPerTurn(key);
				break;
			default:
				result = this.GetValue(key);
				break;
			}
			return result;
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x0007417E File Offset: 0x0007237E
		public float GetValue(HeuristicKey key)
		{
			return this.Heuristics[(int)key];
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x0007418C File Offset: 0x0007238C
		public float GetAveragePerTurn(HeuristicKey key)
		{
			return this.GetValue(key) / this.GetValue(HeuristicKey.TurnsProcessed);
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x000741A0 File Offset: 0x000723A0
		public float GetAveragePerPlayer(HeuristicKey key)
		{
			float numPlayers = this.GetValue(HeuristicKey.ActivePlayerTurns) / this.GetValue(HeuristicKey.TurnsProcessed);
			return this.GetAveragePerPlayer(key, numPlayers);
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000741C5 File Offset: 0x000723C5
		public float GetAveragePerPlayer(HeuristicKey key, float numPlayers)
		{
			return this.GetValue(key) / numPlayers;
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000741D0 File Offset: 0x000723D0
		public float GetAveragePerPlayerPerTurn(HeuristicKey key)
		{
			return this.GetValue(key) / this.GetValue(HeuristicKey.ActivePlayerTurns);
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000741E1 File Offset: 0x000723E1
		public void SetValue(HeuristicKey key, float value)
		{
			this.Heuristics[(int)key] = value;
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000741F0 File Offset: 0x000723F0
		public void SetMax(HeuristicKey key, float value)
		{
			this.Heuristics[(int)key] = Math.Max(this.Heuristics[(int)key], value);
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x00074210 File Offset: 0x00072410
		public void SetMin(HeuristicKey key, float value)
		{
			this.Heuristics[(int)key] = Math.Min(this.Heuristics[(int)key], value);
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x00074230 File Offset: 0x00072430
		public void AddValue(HeuristicKey key, float value = 1f)
		{
			SparseArray<float> heuristics = this.Heuristics;
			heuristics[(int)key] = heuristics[(int)key] + value;
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x00074256 File Offset: 0x00072456
		public GameHeuristics()
		{
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x0007427D File Offset: 0x0007247D
		public GameHeuristics(GameState state, GameDatabase db, int start, int end)
		{
			this.Aggregate(state, db, start, end);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000742AF File Offset: 0x000724AF
		public void Aggregate(GameState state, GameDatabase db)
		{
			this.Aggregate(state.TurnHistory, db);
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000742BE File Offset: 0x000724BE
		public void Aggregate(GameState state, GameDatabase db, int start, int end)
		{
			this.Aggregate(state.TurnHistory.GetRange(start, end - start), db);
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000742D8 File Offset: 0x000724D8
		public void Aggregate(IEnumerable<TurnState> turns, GameDatabase db)
		{
			foreach (TurnState turn in turns)
			{
				this.Aggregate(turn, db);
			}
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x00074324 File Offset: 0x00072524
		public void Aggregate(TurnState turn, GameDatabase db)
		{
			this.AddValue(HeuristicKey.TurnsProcessed, 1f);
			this.AddValue(HeuristicKey.ActivePlayerTurns, (float)turn.EnumeratePlayerStates(false, false).Count<PlayerState>());
			foreach (GameEvent gameEvent in turn.GetGameEvents())
			{
				this.Aggregate(turn, gameEvent);
			}
			this.AggregatePlayers(turn, db);
			if (this.GetValue(HeuristicKey.VendettasCompleted) > 0f)
			{
				float value = this.GetValue(HeuristicKey.VendettasWon) / this.GetValue(HeuristicKey.VendettasCompleted);
				this.SetValue(HeuristicKey.VendettaSuccessRate, value);
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000743C8 File Offset: 0x000725C8
		public void AggregatePlayers(TurnState turn, GameDatabase db)
		{
			this.AggregatePlayerTurns(turn, db);
			foreach (PlayerState playerState in turn.PlayerStates)
			{
				this.SetMax(HeuristicKey.MaxCardsHeld, (float)playerState.Resources.Count);
				this.SetMax(HeuristicKey.MaxOrderSlotsReached, (float)playerState.OrderSlots.Value);
			}
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x00074444 File Offset: 0x00072644
		public void AggregatePlayerTurns(TurnState turn, GameDatabase db)
		{
			foreach (PlayerState playerState in turn.PlayerStates)
			{
				this.Aggregate(turn, playerState, db, playerState.PlayerTurn.Orders);
				this.Aggregate(turn, playerState, playerState.PlayerTurn.Decisions);
			}
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000744B8 File Offset: 0x000726B8
		public void Aggregate(TurnState turn, PlayerState player, GameDatabase db, IEnumerable<ActionableOrder> orders)
		{
			float num = 0f;
			foreach (ActionableOrder actionableOrder in orders)
			{
				this.Aggregate(actionableOrder, turn, db);
				if (!player.PlayerTurn.CheckConflicts(actionableOrder))
				{
					num += 1f;
				}
				OrderMoveLegion orderMoveLegion = actionableOrder as OrderMoveLegion;
				if (orderMoveLegion == null)
				{
					if (!(actionableOrder is OrderVileCalumny))
					{
						if (!(actionableOrder is OrderDeclareDraconicRazzia))
						{
							if (!(actionableOrder is OrderRequestChainsOfAvarice))
							{
								if (!(actionableOrder is OrderRequestLureOfExcess))
								{
									if (actionableOrder is OrderDeclareBloodFeud)
									{
										this.AddValue(HeuristicKey.BloodFeud, 1f);
									}
								}
								else
								{
									this.AddValue(HeuristicKey.LureOfExcess, 1f);
								}
							}
							else
							{
								this.AddValue(HeuristicKey.ChainsOfAvarice, 1f);
							}
						}
						else
						{
							this.AddValue(HeuristicKey.DraconicRazzia, 1f);
						}
					}
					else
					{
						this.AddValue(HeuristicKey.VileCalumny, 1f);
					}
				}
				else
				{
					switch (orderMoveLegion.MoveIntent)
					{
					case FlankIntent.FlankForAttack:
						this.AddValue(HeuristicKey.FlankToBuffAttack, 1f);
						break;
					case FlankIntent.Flank_Heal:
						this.AddValue(HeuristicKey.HealMove, 1f);
						break;
					case FlankIntent.Flank_ReinforceStronghold:
						this.AddValue(HeuristicKey.ReinforceStrongholdMove, 1f);
						break;
					case FlankIntent.Flank_Support_Battle:
						this.AddValue(HeuristicKey.SupportMove, 1f);
						break;
					case FlankIntent.ThreatenBorders:
						this.AddValue(HeuristicKey.ThreatenBorders, 1f);
						break;
					}
				}
			}
			this.AddValue(HeuristicKey.OrderConflicts, num / 2f);
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x00074650 File Offset: 0x00072850
		public void Aggregate(TurnState turn, PlayerState player, IEnumerable<DecisionResponse> decisions)
		{
			foreach (DecisionResponse decisionResponse in decisions)
			{
				this.Aggregate(player.GetDecisionRequest(decisionResponse.DecisionId), decisionResponse);
			}
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x000746A4 File Offset: 0x000728A4
		public void Aggregate(TurnState turn, GameEvent gameEvent)
		{
			GameItemOwnershipChanged gameItemOwnershipChanged = gameEvent as GameItemOwnershipChanged;
			HeuristicKey heuristicKey;
			if (gameItemOwnershipChanged == null)
			{
				CantonClaimedEvent cantonClaimedEvent = gameEvent as CantonClaimedEvent;
				if (cantonClaimedEvent == null)
				{
					BattleEvent battleEvent = gameEvent as BattleEvent;
					if (battleEvent == null)
					{
						if (!(gameEvent is VendettaStartedEvent))
						{
							if (!(gameEvent is VendettaCompletedEvent))
							{
								if (!(gameEvent is DuelEvent))
								{
									if (!(gameEvent is GameItemAttachedEvent))
									{
										PowerUpgradedEvent powerUpgradedEvent = gameEvent as PowerUpgradedEvent;
										if (powerUpgradedEvent == null)
										{
											BazaarBidEvent bazaarBidEvent = gameEvent as BazaarBidEvent;
											if (bazaarBidEvent == null)
											{
												ActionFailedEvent actionFailedEvent = gameEvent as ActionFailedEvent;
												if (actionFailedEvent == null)
												{
													RitualCastEvent ritualCastEvent = gameEvent as RitualCastEvent;
													if (ritualCastEvent == null)
													{
														OfferVassalageResponseEvent offerVassalageResponseEvent = gameEvent as OfferVassalageResponseEvent;
														if (offerVassalageResponseEvent == null)
														{
															OfferLordshipResponseEvent offerLordshipResponseEvent = gameEvent as OfferLordshipResponseEvent;
															if (offerLordshipResponseEvent == null)
															{
																SendEmissaryResponseEvent sendEmissaryResponseEvent = gameEvent as SendEmissaryResponseEvent;
																if (sendEmissaryResponseEvent == null)
																{
																	if (gameEvent is SendEmissaryEvent)
																	{
																		heuristicKey = HeuristicKey.ArmistaceSent;
																		goto IL_288;
																	}
																	OutOfCombatDamageEvent outOfCombatDamageEvent = gameEvent as OutOfCombatDamageEvent;
																	if (outOfCombatDamageEvent != null)
																	{
																		heuristicKey = this.DetermineHeuristic(outOfCombatDamageEvent);
																		goto IL_288;
																	}
																	if (!(gameEvent is PlayerEliminatedEvent))
																	{
																		RankIncreaseEvent rankIncreaseEvent = gameEvent as RankIncreaseEvent;
																		if (rankIncreaseEvent != null)
																		{
																			heuristicKey = this.DetermineHeuristic(rankIncreaseEvent);
																			goto IL_288;
																		}
																		InsultResponseEvent insultResponseEvent = gameEvent as InsultResponseEvent;
																		if (insultResponseEvent == null)
																		{
																			MakeDemandResponseEvent makeDemandResponseEvent = gameEvent as MakeDemandResponseEvent;
																			if (makeDemandResponseEvent == null)
																			{
																				GrandEventPlayed grandEventPlayed = gameEvent as GrandEventPlayed;
																				if (grandEventPlayed != null)
																				{
																					heuristicKey = this.DetermineHeuristic(grandEventPlayed, turn);
																					goto IL_288;
																				}
																			}
																			else if (makeDemandResponseEvent.Response == YesNo.Yes)
																			{
																				heuristicKey = HeuristicKey.DemandAccepted;
																				goto IL_288;
																			}
																		}
																		else if (insultResponseEvent.Response == YesNo.Yes)
																		{
																			heuristicKey = HeuristicKey.InsultAccepted;
																			goto IL_288;
																		}
																	}
																	else if (turn.TurnValue > 1)
																	{
																		heuristicKey = HeuristicKey.Elimination;
																		goto IL_288;
																	}
																}
																else if (sendEmissaryResponseEvent.Response == YesNo.Yes)
																{
																	heuristicKey = HeuristicKey.ArmisticeAccepted;
																	goto IL_288;
																}
															}
															else if (offerLordshipResponseEvent.Response == YesNo.Yes)
															{
																heuristicKey = HeuristicKey.VassalageAccepted;
																goto IL_288;
															}
														}
														else if (offerVassalageResponseEvent.Response == YesNo.Yes)
														{
															heuristicKey = HeuristicKey.VassalageAccepted;
															goto IL_288;
														}
														heuristicKey = HeuristicKey.Invalid;
													}
													else
													{
														heuristicKey = this.DetermineHeuristic(ritualCastEvent);
													}
												}
												else
												{
													heuristicKey = this.DetermineHeuristic(actionFailedEvent, turn);
												}
											}
											else
											{
												heuristicKey = this.DetermineHeuristic(bazaarBidEvent, turn);
											}
										}
										else
										{
											heuristicKey = this.DetermineHeuristic(powerUpgradedEvent);
										}
									}
									else
									{
										heuristicKey = HeuristicKey.GameItemsAttached;
									}
								}
								else
								{
									heuristicKey = HeuristicKey.DuelsDeclared;
								}
							}
							else
							{
								heuristicKey = HeuristicKey.VendettasCompleted;
							}
						}
						else
						{
							heuristicKey = HeuristicKey.VendettasDeclared;
						}
					}
					else
					{
						heuristicKey = this.DetermineHeuristic(battleEvent);
					}
				}
				else
				{
					heuristicKey = this.DetermineHeuristic(cantonClaimedEvent);
				}
			}
			else
			{
				heuristicKey = this.DetermineHeuristic(turn, gameItemOwnershipChanged);
			}
			IL_288:
			HeuristicKey heuristicKey2 = heuristicKey;
			if (heuristicKey2 == HeuristicKey.Invalid)
			{
				return;
			}
			this.AddValue(heuristicKey2, 1f);
			VendettaCompletedEvent vendettaCompletedEvent = gameEvent as VendettaCompletedEvent;
			if (vendettaCompletedEvent != null && vendettaCompletedEvent.Successful)
			{
				this.AddValue(HeuristicKey.VendettasWon, 1f);
			}
			VendettaStartedEvent vendettaStartedEvent = gameEvent as VendettaStartedEvent;
			if (vendettaStartedEvent != null)
			{
				this.AddValue(this.DetermineHeuristic(vendettaStartedEvent), 1f);
			}
			BattleEvent battleEvent2 = gameEvent as BattleEvent;
			if (battleEvent2 != null)
			{
				if (battleEvent2.BattleResult.AttackerSupportingPieces.Count > 0 || battleEvent2.BattleResult.DefenderSupportingPieces.Count > 0)
				{
					this.AddValue(HeuristicKey.BattlesWithSupport, 1f);
				}
				GamePiece attacker_StartState = battleEvent2.BattleResult.Attacker_StartState;
				GamePiece defender_StartState = battleEvent2.BattleResult.Defender_StartState;
				bool flag = attacker_StartState.Slots.Any(delegate(Identifier x)
				{
					Stratagem stratagem;
					return turn.TryFetchGameItem<Stratagem>(x, out stratagem);
				});
				bool flag2 = defender_StartState.Slots.Any(delegate(Identifier x)
				{
					Stratagem stratagem;
					return turn.TryFetchGameItem<Stratagem>(x, out stratagem);
				});
				if (flag || flag2)
				{
					this.AddValue(HeuristicKey.BattlesWithStratagemsAttached, 1f);
				}
			}
			GameItemAttachedEvent gameItemAttachedEvent = gameEvent as GameItemAttachedEvent;
			if (gameItemAttachedEvent != null)
			{
				GamePiece gamePiece = turn.FetchGameItem<GamePiece>(gameItemAttachedEvent.GamePieceId);
				GameItem gameItem = turn.FetchGameItem<GameItem>(gameItemAttachedEvent.GameItemId);
				if (gameItem is Praetor)
				{
					this.AddValue(gamePiece.IsFixture() ? HeuristicKey.PraetorAttachedToFixture : HeuristicKey.PraetorAttachedToLegion, 1f);
				}
				if (gameItem is Artifact)
				{
					this.AddValue(gamePiece.IsFixture() ? HeuristicKey.ArtifactAttachedToFixture : HeuristicKey.ArtifactAttachedToLegion, 1f);
				}
			}
			RitualCastEvent ritualCastEvent2 = gameEvent as RitualCastEvent;
			if (ritualCastEvent2 != null)
			{
				RitualMaskingMode maskingMode = ritualCastEvent2.MaskingContext.MaskingMode;
				if (maskingMode == RitualMaskingMode.Masked)
				{
					this.AddValue(HeuristicKey.MaskedRituals, 1f);
					return;
				}
				if (maskingMode != RitualMaskingMode.Framed)
				{
					return;
				}
				this.AddValue(HeuristicKey.FramedRituals, 1f);
			}
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x00074AE0 File Offset: 0x00072CE0
		public void Aggregate(ActionableOrder order, TurnState turn, GameDatabase db)
		{
			HeuristicKey heuristicKey;
			if (!(order is OrderDemandTribute))
			{
				if (!(order is OrderSeekManuscripts))
				{
					if (!(order is OrderInsult))
					{
						if (!(order is OrderMakeDemand))
						{
							if (!(order is PlayGrandEventOrder))
							{
								OrderExtort orderExtort = order as OrderExtort;
								if (orderExtort == null)
								{
									if (!(order is OrderHumiliate))
									{
										if (!(order is OrderAssertWeakness))
										{
											InvokeManuscriptOrder invokeManuscriptOrder = order as InvokeManuscriptOrder;
											if (invokeManuscriptOrder == null)
											{
												OrderMakeBid orderMakeBid = order as OrderMakeBid;
												if (orderMakeBid == null)
												{
													heuristicKey = HeuristicKey.Invalid;
												}
												else
												{
													heuristicKey = this.DetermineHeuristic(orderMakeBid, turn);
												}
											}
											else
											{
												heuristicKey = this.DetermineHeuristic(invokeManuscriptOrder, turn, db);
											}
										}
										else
										{
											heuristicKey = HeuristicKey.AssertionOfWeakness;
										}
									}
									else
									{
										heuristicKey = HeuristicKey.Humiliates;
									}
								}
								else
								{
									heuristicKey = this.DetermineHeuristic(orderExtort);
								}
							}
							else
							{
								heuristicKey = HeuristicKey.GrandEventsPlayed;
							}
						}
						else
						{
							heuristicKey = HeuristicKey.DemandsMade;
						}
					}
					else
					{
						heuristicKey = HeuristicKey.InsultsThrown;
					}
				}
				else
				{
					heuristicKey = HeuristicKey.Manuscript_Seek;
				}
			}
			else
			{
				heuristicKey = HeuristicKey.SeekTribute;
			}
			HeuristicKey heuristicKey2 = heuristicKey;
			if (heuristicKey2 != HeuristicKey.Invalid)
			{
				this.AddValue(heuristicKey2, 1f);
			}
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x00074BA7 File Offset: 0x00072DA7
		public void Aggregate(DecisionRequest request, DecisionResponse response)
		{
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x00074BAC File Offset: 0x00072DAC
		private HeuristicKey DetermineHeuristic(InvokeManuscriptOrder invokeManuscriptOrder, TurnState turn, GameDatabase database)
		{
			Manuscript manuscript = turn.FetchGameItem<Manuscript>(invokeManuscriptOrder.ManuscriptId);
			if (manuscript == null)
			{
				return HeuristicKey.Invalid;
			}
			HeuristicKey result;
			switch (manuscript.GetCategory(database))
			{
			case ManuscriptCategory.Manual:
				result = HeuristicKey.Manuscript_Invoked_Manual;
				break;
			case ManuscriptCategory.Primer:
				result = HeuristicKey.Manuscript_Invoked_Primer;
				break;
			case ManuscriptCategory.Treatise:
				result = HeuristicKey.Manuscript_Invoked_Treatise;
				break;
			case ManuscriptCategory.Schematic:
				result = HeuristicKey.Manuscript_Invoked_Schematic;
				break;
			default:
				result = HeuristicKey.Invalid;
				break;
			}
			return result;
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x00074C04 File Offset: 0x00072E04
		private HeuristicKey DetermineHeuristic(OrderMakeBid bid, TurnState turn)
		{
			GameItem gameItem = turn.FetchGameItem(bid.Item);
			if (gameItem != null)
			{
				if (gameItem is Manuscript)
				{
					return HeuristicKey.Bid_Manuscript;
				}
				if (gameItem is Praetor)
				{
					return HeuristicKey.Bid_Praetor;
				}
				if (gameItem is Artifact)
				{
					return HeuristicKey.Bid_Artifact;
				}
				if (gameItem is GamePiece)
				{
					return HeuristicKey.Bid_Legion;
				}
			}
			return HeuristicKey.Bid_Other;
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x00074C50 File Offset: 0x00072E50
		private HeuristicKey DetermineHeuristic(RankIncreaseEvent increaseRank)
		{
			HeuristicKey result;
			switch (increaseRank.NewRank)
			{
			case Rank.Marquis:
				result = HeuristicKey.RankPurchased_Marquis;
				break;
			case Rank.Duke:
				result = HeuristicKey.RankPurchased_Duke;
				break;
			case Rank.Prince:
				result = HeuristicKey.RankPurchased_Prince;
				break;
			default:
				result = HeuristicKey.Invalid;
				break;
			}
			return result;
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x00074C8C File Offset: 0x00072E8C
		private HeuristicKey DetermineHeuristic(OrderExtort extort)
		{
			if (extort.DemandOption == DemandOptions.SelectedPraetor)
			{
				return HeuristicKey.Extorts_Praetor;
			}
			if (extort.DemandOption == DemandOptions.SelectedArtifact)
			{
				return HeuristicKey.Extorts_Artifact;
			}
			return HeuristicKey.Invalid;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x00074CA8 File Offset: 0x00072EA8
		private HeuristicKey DetermineHeuristic(TurnState turn, GameItemOwnershipChanged changed)
		{
			GamePiece gamePiece;
			if (!turn.TryFetchGameItem<GamePiece>(changed.Item, out gamePiece))
			{
				return HeuristicKey.Invalid;
			}
			if (gamePiece.SubCategory != GamePieceCategory.PoP)
			{
				return HeuristicKey.Invalid;
			}
			if (changed.Owner == -1)
			{
				return HeuristicKey.Invalid;
			}
			return HeuristicKey.PoPsCapturedByPlayers;
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x00074CDF File Offset: 0x00072EDF
		private HeuristicKey DetermineHeuristic(CantonClaimedEvent canton)
		{
			if (canton.NewOwner == -1)
			{
				return HeuristicKey.Invalid;
			}
			if (canton.PreviousOwner != -1)
			{
				return HeuristicKey.CantonsCapturedFromOtherPlayers;
			}
			return HeuristicKey.CantonsCapturedFromForceMajeure;
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x00074CF8 File Offset: 0x00072EF8
		private HeuristicKey DetermineHeuristic(GrandEventPlayed eventPlayed, TurnState turnState)
		{
			string eventEffectId = eventPlayed.EventEffectId;
			if (eventEffectId == "EventEffect_AngelicHostIncursion")
			{
				return HeuristicKey.Event_AngelicHost;
			}
			if (eventEffectId == "EventEffect_WrathOfTheTyrant")
			{
				return HeuristicKey.Event_WrathOfTheTyrant;
			}
			if (eventEffectId == "EventEffect_LeviathanRising")
			{
				return HeuristicKey.Event_AbyssLeviathan;
			}
			if (eventEffectId == "EventEffect_OfConquestsPast")
			{
				return HeuristicKey.Event_OfConquestsPast;
			}
			return HeuristicKey.Event_Other;
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x00074D4E File Offset: 0x00072F4E
		private HeuristicKey DetermineHeuristic(BattleEvent battle)
		{
			if (!battle.IsAssociatedWith(-1))
			{
				return HeuristicKey.BattlesBetweenPlayers;
			}
			return HeuristicKey.BattlesWithForceMajeure;
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x00074D5C File Offset: 0x00072F5C
		private HeuristicKey DetermineHeuristic(RitualCastEvent ritualEvent)
		{
			string ritualId = ritualEvent.RitualId;
			if (ritualId.Contains("undying_vigor"))
			{
				return HeuristicKey.CastRitual_UndyingVigor;
			}
			if (ritualId.Contains("infernal_juggernaut"))
			{
				return HeuristicKey.CastRitual_InfernalJuggernaut;
			}
			if (ritualId.Contains("loot_the_vaults"))
			{
				return HeuristicKey.CastRitual_LootTheVaults;
			}
			if (ritualId.Contains("pilfer_artifacts"))
			{
				return HeuristicKey.CastRitual_PilferArtifacts;
			}
			if (ritualId.Contains("bribe_praetor"))
			{
				return HeuristicKey.CastRitual_BribePraetor;
			}
			if (ritualId.Contains("convert_legion"))
			{
				return HeuristicKey.CastRitual_ConvertLegion;
			}
			if (ritualId.Contains("raid_the_library"))
			{
				return HeuristicKey.CastRitual_RaidTheLibrary;
			}
			if (ritualId.Contains("enfeeble"))
			{
				return HeuristicKey.CastRitual_Enfeeble;
			}
			if (ritualId.Contains("infernal_affliction"))
			{
				return HeuristicKey.CastRitual_InfernalAffliction;
			}
			if (ritualId.Contains("corrupt_tribute"))
			{
				return HeuristicKey.CastRitual_CorruptTribute;
			}
			if (ritualId.Contains("dire_dissipation"))
			{
				return HeuristicKey.CastRitual_DireDissipation;
			}
			if (ritualId.Contains("banish_praetor"))
			{
				return HeuristicKey.CastRitual_BanishPraetor;
			}
			if (ritualId.Contains("planar_lock"))
			{
				return HeuristicKey.CastRitual_PlanarLock;
			}
			if (ritualId.Contains("blight_wisdom"))
			{
				return HeuristicKey.CastRitual_BlightWisdom;
			}
			if (ritualId.Contains("dark_augury"))
			{
				return HeuristicKey.CastRitual_DarkAugury;
			}
			if (ritualId.Contains("malediction_of_the_seer"))
			{
				return HeuristicKey.CastRitual_MaledictionOfTheSeer;
			}
			if (ritualId.Contains("demonic_interference"))
			{
				return HeuristicKey.CastRitual_DemonicInterference;
			}
			if (ritualId.Contains("procured_honour"))
			{
				return HeuristicKey.CastRitual_ProcuredHonour;
			}
			if (ritualId.Contains("demand_of_supplication"))
			{
				return HeuristicKey.CastRitual_DemandOfSupplication;
			}
			if (ritualId.Contains("burnt_offerings"))
			{
				return HeuristicKey.CastRitual_BurntOfferings;
			}
			HeuristicKey result;
			if (!(ritualId == "lilith_baleful_gaze"))
			{
				if (!(ritualId == "andromalius_vanitys_anointed"))
				{
					if (!(ritualId == "beelzebub_hells_maw"))
					{
						if (!(ritualId == "murmur_danse_macabre"))
						{
							result = HeuristicKey.Invalid;
						}
						else
						{
							result = HeuristicKey.DanseMacabre;
						}
					}
					else
					{
						result = HeuristicKey.HellsMaw;
					}
				}
				else
				{
					result = HeuristicKey.VanityAnointed;
				}
			}
			else
			{
				result = HeuristicKey.BalefulGaze;
			}
			return result;
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x00074F03 File Offset: 0x00073103
		private HeuristicKey DetermineHeuristic(OutOfCombatDamageEvent damageEvent)
		{
			if (damageEvent.DamageType == OutOfCombatDamageType.Lava)
			{
				return HeuristicKey.LavaDamage;
			}
			return HeuristicKey.Invalid;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x00074F14 File Offset: 0x00073114
		private HeuristicKey DetermineHeuristic(PowerUpgradedEvent powerUpgradedEvent)
		{
			PowerType powerType = powerUpgradedEvent.PowerType;
			int level = powerUpgradedEvent.Level;
			switch (powerType)
			{
			case PowerType.Wrath:
				switch (level)
				{
				case 1:
					return HeuristicKey.Upgrade_Wrath_1;
				case 2:
					return HeuristicKey.Upgrade_Wrath_2;
				case 3:
					return HeuristicKey.Upgrade_Wrath_3;
				case 4:
					return HeuristicKey.Upgrade_Wrath_4;
				case 5:
					return HeuristicKey.Upgrade_Wrath_5;
				case 6:
					return HeuristicKey.Upgrade_Wrath_6;
				}
				break;
			case PowerType.Deceit:
				switch (level)
				{
				case 1:
					return HeuristicKey.Upgrade_Deceit_1;
				case 2:
					return HeuristicKey.Upgrade_Deceit_2;
				case 3:
					return HeuristicKey.Upgrade_Deceit_3;
				case 4:
					return HeuristicKey.Upgrade_Deceit_4;
				case 5:
					return HeuristicKey.Upgrade_Deceit_5;
				case 6:
					return HeuristicKey.Upgrade_Deceit_6;
				}
				break;
			case PowerType.Prophecy:
				switch (level)
				{
				case 1:
					return HeuristicKey.Upgrade_Prophecy_1;
				case 2:
					return HeuristicKey.Upgrade_Prophecy_2;
				case 3:
					return HeuristicKey.Upgrade_Prophecy_3;
				case 4:
					return HeuristicKey.Upgrade_Prophecy_4;
				case 5:
					return HeuristicKey.Upgrade_Prophecy_5;
				case 6:
					return HeuristicKey.Upgrade_Prophecy_6;
				}
				break;
			case PowerType.Destruction:
				switch (level)
				{
				case 1:
					return HeuristicKey.Upgrade_Destruction_1;
				case 2:
					return HeuristicKey.Upgrade_Destruction_2;
				case 3:
					return HeuristicKey.Upgrade_Destruction_3;
				case 4:
					return HeuristicKey.Upgrade_Destruction_4;
				case 5:
					return HeuristicKey.Upgrade_Destruction_5;
				case 6:
					return HeuristicKey.Upgrade_Destruction_6;
				}
				break;
			case PowerType.Charisma:
				switch (level)
				{
				case 1:
					return HeuristicKey.Upgrade_Charisma_1;
				case 2:
					return HeuristicKey.Upgrade_Charisma_2;
				case 3:
					return HeuristicKey.Upgrade_Charisma_3;
				case 4:
					return HeuristicKey.Upgrade_Charisma_4;
				case 5:
					return HeuristicKey.Upgrade_Charisma_5;
				case 6:
					return HeuristicKey.Upgrade_Charisma_6;
				}
				break;
			}
			return HeuristicKey.Invalid;
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x0007505C File Offset: 0x0007325C
		private HeuristicKey DetermineHeuristic(ActionFailedEvent ev, TurnState turn)
		{
			Result result = ev.Result;
			if (result is LegionMovementProcessor.NotEnoughMovementPointsProblem || result is LegionMovementProcessor.OccupiedCantonProblem || result is LegionMovementProcessor.BanishedBeforeMovingProblem || result is LegionMovementProcessor.ConvertedBeforeMovingProblem || result is Result.OutRankedProblem || result is Result.OutInfluencedProblem)
			{
				return HeuristicKey.Invalid;
			}
			SimLogger logger = SimLogger.Logger;
			if (logger != null)
			{
				logger.Trace("[ActionFailed] " + ev.Result.DebugString);
			}
			return HeuristicKey.OrderFailed;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000750CC File Offset: 0x000732CC
		private HeuristicKey DetermineHeuristic(BazaarBidEvent bidEvent, TurnState turn)
		{
			GameItemCategory gameItemCategory = bidEvent.GameItemCategory;
			GamePiece gamePiece;
			if (gameItemCategory == GameItemCategory.GamePiece && turn.TryFetchGameItem<GamePiece>(bidEvent.GameItemId, out gamePiece))
			{
				if (gamePiece.SubCategory == GamePieceCategory.Legion)
				{
					return HeuristicKey.LegionPurchased;
				}
				if (gamePiece.SubCategory == GamePieceCategory.Titan)
				{
					return HeuristicKey.TitanPurchased;
				}
			}
			if (gameItemCategory == GameItemCategory.ManuscriptPiece)
			{
				return HeuristicKey.ManuscriptPurchased;
			}
			return HeuristicKey.Invalid;
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x00075114 File Offset: 0x00073314
		private HeuristicKey DetermineHeuristic(VendettaStartedEvent vendettaEvent)
		{
			string id = vendettaEvent.Objective.Id;
			if (id != null)
			{
				if (id.Contains("CaptureCanton"))
				{
					return HeuristicKey.VendettaDeclared_CaptureCantons;
				}
				if (id.Contains("DestroyLegion"))
				{
					return HeuristicKey.VendettaDeclared_DestroyLegion;
				}
				if (id.Contains("CapturePoP"))
				{
					return HeuristicKey.VendettaDeclared_CapturePoPs;
				}
			}
			else
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Vendetta with no objective was raised. Why?");
				}
			}
			return HeuristicKey.VendettasDeclared;
		}

		// Token: 0x04000EFB RID: 3835
		private static List<HeuristicKey> _keys;

		// Token: 0x04000EFC RID: 3836
		private SparseArray<float> Heuristics = new SparseArray<float>(Enum.GetValues(typeof(HeuristicKey)).Length);
	}
}
