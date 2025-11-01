using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003A7 RID: 935
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerState : GameEntity, IDeepClone<PlayerState>
	{
		// Token: 0x060011FC RID: 4604 RVA: 0x000449F4 File Offset: 0x00042BF4
		public override void ConfigureFrom(IdentifiableStaticData idata)
		{
			base.ConfigureFrom(idata);
			ArchFiendStaticData archFiendStaticData = idata as ArchFiendStaticData;
			if (archFiendStaticData == null)
			{
				return;
			}
			this.SetPowerLevels(archFiendStaticData);
			this.TributeOfferings_NumOptions.SetBase((float)archFiendStaticData.TributeOfferings_NumOptions);
			this.TributeOfferings_NumSelections.SetBase((float)archFiendStaticData.TributeOfferings_NumSelections);
			this.CommandRating.SetBase((float)archFiendStaticData.CommandRating);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00044A50 File Offset: 0x00042C50
		public void SetPowerLevels(ArchFiendStaticData archfiend)
		{
			this.PowersLevels[PowerType.Wrath].SetLevel(archfiend.Wrath);
			this.PowersLevels[PowerType.Deceit].SetLevel(archfiend.Deceit);
			this.PowersLevels[PowerType.Prophecy].SetLevel(archfiend.Prophecy);
			this.PowersLevels[PowerType.Destruction].SetLevel(archfiend.Destruction);
			this.PowersLevels[PowerType.Charisma].SetLevel(archfiend.Charisma);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00044AD0 File Offset: 0x00042CD0
		public Result IsRitualFramingAvailable()
		{
			if (this.Excommunicated)
			{
				return new Result.RitualMaskingExcommunicatedProblem();
			}
			if (!base.HasTag<EntityTag_CanFrameRituals>())
			{
				return new Result.RitualMaskingLockedProblem();
			}
			return Result.Success;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00044AF3 File Offset: 0x00042CF3
		public Result IsRitualMaskingAvailable()
		{
			if (this.Excommunicated)
			{
				return new Result.RitualMaskingExcommunicatedProblem();
			}
			if (!base.HasTag<EntityTag_CanMaskRituals>())
			{
				return new Result.RitualMaskingLockedProblem();
			}
			return Result.Success;
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x00044B16 File Offset: 0x00042D16
		[JsonIgnore]
		public ResourceAccumulation TotalResourcesIncludingPrestige
		{
			get
			{
				return this.TotalTributeSum.Add(ResourceTypes.Prestige, this.SpendablePrestige);
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x00044B2A File Offset: 0x00042D2A
		[JsonIgnore]
		public ResourceAccumulation TotalTributeSum
		{
			get
			{
				return this.Resources.EnumerateAccumulations().Total();
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x00044B3C File Offset: 0x00042D3C
		[JsonIgnore]
		public string PlayerName
		{
			get
			{
				return this.PlatformDisplayName ?? ("Player " + this.Id.ToString());
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x00044B5D File Offset: 0x00042D5D
		// (set) Token: 0x06001204 RID: 4612 RVA: 0x00044B6A File Offset: 0x00042D6A
		[JsonIgnore]
		public Rank Rank
		{
			get
			{
				return (Rank)this.RankValue.Value;
			}
			set
			{
				this.RankValue.SetBase((float)value);
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00044B79 File Offset: 0x00042D79
		public static implicit operator PlayerIndex(PlayerState player)
		{
			if (player == null)
			{
				return PlayerIndex.Invalid;
			}
			return (PlayerIndex)player.Id;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x00044B8A File Offset: 0x00042D8A
		// (set) Token: 0x06001207 RID: 4615 RVA: 0x00044BA4 File Offset: 0x00042DA4
		[JsonIgnore]
		public ModifiableValue TributeQuality
		{
			get
			{
				this._baseTributeQuality.SetBase((float)this.CalculateTributeBaseQualityLevel());
				return this._baseTributeQuality;
			}
			set
			{
				this._baseTributeQuality = value;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x00044BAD File Offset: 0x00042DAD
		[JsonIgnore]
		public int TotalSchemeOptions
		{
			get
			{
				return this.NumBasicSchemeOptions + this.NumGrandSchemeOptions;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x00044BC6 File Offset: 0x00042DC6
		[JsonIgnore]
		public IReadOnlyList<Identifier> ActiveSchemeCards
		{
			get
			{
				return this._activeSchemeCards;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x00044BCE File Offset: 0x00042DCE
		[JsonIgnore]
		public IReadOnlyList<Identifier> PreviousSchemeCards
		{
			get
			{
				return this._previousSchemeCards;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x00044BD8 File Offset: 0x00042DD8
		[JsonIgnore]
		public int AvailableSchemeSlots
		{
			get
			{
				int numSchemes = this.NumSchemes;
				return Math.Max(this.SchemeSlots - numSchemes, 0);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x00044BFF File Offset: 0x00042DFF
		[JsonIgnore]
		public int NumSchemes
		{
			get
			{
				return this.ActiveSchemeCards.Count;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x00044C0C File Offset: 0x00042E0C
		[JsonIgnore]
		public int NumVaultedItems
		{
			get
			{
				return this._vaultedItems.Count;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x00044C19 File Offset: 0x00042E19
		[JsonIgnore]
		public IEnumerable<Identifier> VaultedItems
		{
			get
			{
				return this._vaultedItems;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x00044C21 File Offset: 0x00042E21
		[JsonIgnore]
		public IEnumerable<Identifier> ActiveRelics
		{
			get
			{
				return this._activeRelics;
			}
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00044C2C File Offset: 0x00042E2C
		[JsonConstructor]
		public PlayerState()
		{
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00044FC4 File Offset: 0x000431C4
		public PlayerState(int id, int orderSlots)
		{
			this.Id = id;
			this.OrderSlots.SetBase((float)orderSlots);
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x00045370 File Offset: 0x00043570
		[JsonIgnore]
		public IReadOnlyList<KnowledgeModifier> AllKnowledgeModifiers
		{
			get
			{
				return this.KnowledgeModifiers;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x00045378 File Offset: 0x00043578
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x00045385 File Offset: 0x00043585
		public string ArchfiendId
		{
			get
			{
				return this.StaticDataReference.Id;
			}
			set
			{
				this.StaticDataReference = new ConfigRef("", value);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00045398 File Offset: 0x00043598
		public void RecordPlayerWasRegent(int turn)
		{
			this._turnWasLastRegent = turn;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x000453A1 File Offset: 0x000435A1
		public int GetTurnPlayerWasLastRegent()
		{
			return this._turnWasLastRegent;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x000453A9 File Offset: 0x000435A9
		public bool IsVaultOverflowing(int maxVaultSize)
		{
			return this.Resources.Count > maxVaultSize;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x000453B9 File Offset: 0x000435B9
		public bool IsVaultOverflowing(int maxVaultSize, out int excessCount)
		{
			excessCount = 0;
			bool flag = this.IsVaultOverflowing(maxVaultSize);
			if (flag)
			{
				excessCount = this.Resources.Count - maxVaultSize;
			}
			return flag;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x000453D7 File Offset: 0x000435D7
		public AIPersistentData GetPersistentData()
		{
			return this.AIPersistentData;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x000453DF File Offset: 0x000435DF
		public void InitializePersistentData(int seed)
		{
			if (this.AIPersistentData == null)
			{
				this.AIPersistentData = new AIPersistentData(new SimulationRandom(seed), 4);
			}
			if (this.AIPersistentData.SimRandom == null)
			{
				this.AIPersistentData.SimRandom = new SimulationRandom(seed);
			}
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00045419 File Offset: 0x00043619
		public IEnumerable<ObjectiveCondition> EnumerateActiveObjectives(TurnState turnState)
		{
			foreach (Identifier id in this._activeSchemeCards)
			{
				SchemeCard schemeCard = turnState.FetchGameItem<SchemeCard>(id);
				if (schemeCard != null)
				{
					foreach (ObjectiveCondition objectiveCondition in schemeCard.Scheme.Conditions)
					{
						yield return objectiveCondition;
					}
					IEnumerator<ObjectiveCondition> enumerator2 = null;
				}
			}
			List<Identifier>.Enumerator enumerator = default(List<Identifier>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00045430 File Offset: 0x00043630
		private int CalculateTributeBaseQualityLevel()
		{
			return this.PowersLevels.Powers.Max((PlayerPowerLevel t) => t.CurrentLevel);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00045464 File Offset: 0x00043664
		public DecisionRequest GetDecisionRequest(DecisionId id)
		{
			return this.DecisionRequests.FirstOrDefault((DecisionRequest x) => x.DecisionId == id);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00045495 File Offset: 0x00043695
		public bool TryGetDecisionRequest<T>(DecisionId id, out T request) where T : DecisionRequest
		{
			request = (this.GetDecisionRequest(id) as T);
			return request != null;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x000454BC File Offset: 0x000436BC
		public T GetDecisionRequest<T>() where T : DecisionRequest
		{
			return IEnumerableExtensions.FirstOrDefault<T>(this.GetDecisionRequests<T>());
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x000454C9 File Offset: 0x000436C9
		public IEnumerable<T> GetDecisionRequests<T>() where T : DecisionRequest
		{
			return this.DecisionRequests.OfType<T>();
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x000454D6 File Offset: 0x000436D6
		public Result AddSchemeCard(SchemeCard schemeCard)
		{
			return this.AddSchemeCard(schemeCard.Id);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x000454E4 File Offset: 0x000436E4
		public Result AddSchemeCard(Identifier schemeCardId)
		{
			this.RemoveSchemeCard(schemeCardId);
			if (this.NumSchemes >= this.SchemeSlots)
			{
				return Result.NotEnoughSlots();
			}
			this._activeSchemeCards.Add(schemeCardId);
			return Result.Success;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00045517 File Offset: 0x00043717
		public bool HasActiveScheme()
		{
			return this._activeSchemeCards != null && this._activeSchemeCards.Count > 0;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00045531 File Offset: 0x00043731
		public List<Identifier> ActiveSchemes()
		{
			return this._activeSchemeCards;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00045539 File Offset: 0x00043739
		public void SetSchemeComplete(SchemeCard schemeCard)
		{
			if (!this._previousSchemeCards.Contains(schemeCard.Id))
			{
				this._previousSchemeCards.Add(schemeCard);
			}
			schemeCard.Status = GameItemStatus.Banished;
			this.RemoveSchemeCard(schemeCard);
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0004556D File Offset: 0x0004376D
		public void RemoveSchemeCard(SchemeCard schemeCard)
		{
			this.RemoveSchemeCard(schemeCard.Id);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0004557C File Offset: 0x0004377C
		public void RemoveSchemeCard(Identifier id)
		{
			this._activeSchemeCards.RemoveAll((Identifier t) => t == id);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x000455AE File Offset: 0x000437AE
		public void SetAnimosity(ArchfiendAnimosity animosity)
		{
			this.Animosity = animosity;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x000455B8 File Offset: 0x000437B8
		public void SetAnimosity(List<AnimosityData> animosityData)
		{
			this.Animosity = new ArchfiendAnimosity();
			foreach (AnimosityData animosityData2 in animosityData)
			{
				this.Animosity.SetValue(animosityData2.OtherID, animosityData2.Animosity);
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00045624 File Offset: 0x00043824
		public void AddAITag(AITag tag)
		{
			if (tag == AITag.Undefined)
			{
				return;
			}
			if (!this.AITags.Contains(tag))
			{
				this.AITags.Add(tag);
			}
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00045645 File Offset: 0x00043845
		public void RemoveAITag(AITag tag)
		{
			if (this.AITags.Contains(tag))
			{
				this.AITags.Remove(tag);
			}
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00045662 File Offset: 0x00043862
		public void SetAITags(List<AITag> tags)
		{
			this.AITags = tags;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0004566B File Offset: 0x0004386B
		public void ClearAITags()
		{
			this.AITags.Clear();
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00045678 File Offset: 0x00043878
		public void AddDecisionRequest(DecisionRequest decisionRequest)
		{
			this.RemoveDecisionRequest(decisionRequest);
			this.DecisionRequests.Add(decisionRequest);
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00045690 File Offset: 0x00043890
		public void RemoveDecisionRequest(DecisionRequest decisionRequest)
		{
			this.DecisionRequests.RemoveAll((DecisionRequest t) => t.DecisionId == decisionRequest.DecisionId);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x000456C2 File Offset: 0x000438C2
		public void AddToVault(Identifier item)
		{
			this.RemoveFromVault(item);
			this._vaultedItems.Add(item);
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x000456D8 File Offset: 0x000438D8
		public bool RemoveFromVault(Identifier item)
		{
			return this._vaultedItems.RemoveAll((Identifier t) => t == item) > 0;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0004570C File Offset: 0x0004390C
		public override void ClearModifiers()
		{
			this.RitualState.ClearModifiers();
			this.PowersLevels.ClearModifiers();
			base.ClearModifiers();
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0004572A File Offset: 0x0004392A
		public override ModifierContext CreateContext()
		{
			return new PlayerContext
			{
				PlayerId = this.Id
			};
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00045740 File Offset: 0x00043940
		public PlayerKnowledgeContext GetKnowledgeContext(int playerId)
		{
			PlayerKnowledgeContext result;
			if (!this.PlayerKnowledgeContexts.TryGetValue(playerId, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00045760 File Offset: 0x00043960
		public PlayerKnowledgeContext GetOrCreateKnowledgeContext(int playerId)
		{
			PlayerKnowledgeContext playerKnowledgeContext;
			if (this.PlayerKnowledgeContexts.TryGetValue(playerId, out playerKnowledgeContext))
			{
				return playerKnowledgeContext;
			}
			playerKnowledgeContext = new PlayerKnowledgeContext();
			this.PlayerKnowledgeContexts.Add(playerId, playerKnowledgeContext);
			return playerKnowledgeContext;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00045793 File Offset: 0x00043993
		public void GiveRelic(Relic relic)
		{
			this.GiveRelic(relic.Id);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x000457A1 File Offset: 0x000439A1
		public void GiveRelic(Identifier relicId)
		{
			this._activeRelics.Add(relicId);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000457B0 File Offset: 0x000439B0
		public void RemoveRelic(Identifier relicId)
		{
			this._activeRelics.RemoveAll((Identifier t) => t == relicId);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x000457E4 File Offset: 0x000439E4
		public void Eliminate(TurnProcessContext context)
		{
			if (!this.Eliminated)
			{
				PlayerTargetGroup modifiableTargetGroup = new PlayerTargetGroup(new ArchfiendModifier(new ArchfiendModifierStaticData
				{
					ValidRegent = false
				})
				{
					Source = new PlayerContext(this.Id, this.ArchfiendId)
				}, new int[]
				{
					this.Id
				});
				this._eliminatedModifierId = context.CurrentTurn.PushGlobalModifier(modifiableTargetGroup);
				this.Eliminated = true;
				this.EliminatedTurn = context.CurrentTurn.TurnValue;
				return;
			}
			SimLogger logger = SimLogger.Logger;
			if (logger == null)
			{
				return;
			}
			logger.Error(string.Format("Eliminating already Eliminated player id:{0} name:{1}", this.Id, this.PlayerName));
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0004588C File Offset: 0x00043A8C
		public void Excommunicate(TurnProcessContext context, ExcommunicationReason reason)
		{
			if (!this.Excommunicated)
			{
				PlayerTargetGroup modifiableTargetGroup = new PlayerTargetGroup(new ArchfiendModifier(new ArchfiendModifierStaticData
				{
					ValidRegent = false,
					BlockBazaarAccess = true
				})
				{
					Source = new PlayerContext(this.Id, this.ArchfiendId)
				}, new int[]
				{
					this.Id
				});
				this._excommunicatedModifierId = context.CurrentTurn.PushGlobalModifier(modifiableTargetGroup);
				this.Excommunicated = true;
				this.ExcommunicationReasons.Set((int)reason);
				this.ExcommunicatedTurn = context.CurrentTurn.TurnValue;
				return;
			}
			SimLogger logger = SimLogger.Logger;
			if (logger == null)
			{
				return;
			}
			logger.Error(string.Format("Excommunicating already excommunicated player id:{0} name:{1}", this.Id, this.PlayerName));
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00045948 File Offset: 0x00043B48
		public void ReinstateFromExcommunication(TurnProcessContext context)
		{
			if (this.Excommunicated)
			{
				context.CurrentTurn.PopGlobalModifier(this._excommunicatedModifierId);
				this._excommunicatedModifierId = Guid.Empty;
				this.Excommunicated = false;
				return;
			}
			SimLogger logger = SimLogger.Logger;
			if (logger == null)
			{
				return;
			}
			logger.Error(string.Format("Reinstating non-excommunicated player id:{0} name:{1}", this.Id, this.PlayerName));
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x000459AB File Offset: 0x00043BAB
		public bool HasBeenExcommunicatedFor(ExcommunicationReason reason)
		{
			return this.ExcommunicationReasons.IsSet((int)reason);
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x000459BC File Offset: 0x00043BBC
		public void DeepClone(out PlayerState clone)
		{
			using (SimProfilerBlock.ProfilerBlock("PlayerState.DeepClone"))
			{
				clone = new PlayerState
				{
					Role = this.Role,
					AIDifficulty = this.AIDifficulty,
					PlayFabId = this.PlayFabId.DeepClone(),
					PlatformDisplayName = this.PlatformDisplayName.DeepClone(),
					MultiplayerIdleCounter = this.MultiplayerIdleCounter,
					KnowledgeModifiers = this.KnowledgeModifiers.DeepClone(CloneFunction.FastClone),
					Id = this.Id,
					SpendablePrestige = this.SpendablePrestige,
					RankValue = this.RankValue.DeepClone<ModifiableValue>(),
					StrongholdId = this.StrongholdId,
					PersonalGuardId = this.PersonalGuardId,
					Eliminated = this.Eliminated,
					EliminatedTurn = this.EliminatedTurn,
					ExcommunicatedTurn = this.ExcommunicatedTurn,
					_eliminatedModifierId = this._eliminatedModifierId,
					Excommunicated = this.Excommunicated,
					_excommunicatedModifierId = this._excommunicatedModifierId,
					ExcommunicationReasons = this.ExcommunicationReasons,
					KingmakerPuppetId = this.KingmakerPuppetId,
					IsKingmaker = this.IsKingmaker.DeepClone<ModifiableBool>(),
					IsPowerBehindTheThrone = this.IsPowerBehindTheThrone.DeepClone<ModifiableBool>(),
					HasBazaarPurchaseKnowledge = this.HasBazaarPurchaseKnowledge.DeepClone<ModifiableBool>(),
					HasVendettaKnowledge = this.HasVendettaKnowledge.DeepClone<ModifiableBool>(),
					BlockEventCardUse = this.BlockEventCardUse.DeepClone<ModifiableBool>(),
					BlockStratagemUse = this.BlockStratagemUse.DeepClone<ModifiableBool>(),
					BlockBazaarAccess = this.BlockBazaarAccess.DeepClone<ModifiableBool>(),
					BlockRitualTable = this.BlockRitualTable.DeepClone<ModifiableBool>(),
					BlockOfferings = this.BlockOfferings.DeepClone<ModifiableBool>(),
					CanReplaceAnyCombatCard = this.CanReplaceAnyCombatCard.DeepClone<ModifiableBool>(),
					CanBribeArbiter = this.CanBribeArbiter.DeepClone<ModifiableBool>(),
					DiplomaticImmunity = this.DiplomaticImmunity.DeepClone<ModifiableBool>(),
					HiddenPrestige = this.HiddenPrestige.DeepClone<ModifiableBool>(),
					ValidRegent = this.ValidRegent.DeepClone<ModifiableBool>(),
					VotingPower = this.VotingPower.DeepClone<ModifiableValue>(),
					HealingRate = this.HealingRate.DeepClone<ModifiableValue>(),
					PassivePrestige = this.PassivePrestige.DeepClone<ModifiableValue>(),
					CommandRating = this.CommandRating.DeepClone<ModifiableValue>(),
					OrderSlots = this.OrderSlots.DeepClone<ModifiableValue>(),
					CombatRewardMultiplier = this.CombatRewardMultiplier.DeepClone<ModifiableValue>(),
					RitualDestructionRewardMultiplier = this.RitualDestructionRewardMultiplier.DeepClone<ModifiableValue>(),
					RitualCostMultiplier = this.RitualCostMultiplier.DeepClone<ModifiableValue>(),
					RitualCostOffset = this.RitualCostOffset.DeepClone<ModifiableValue>(),
					VendettasForBloodFeudOffset = this.VendettasForBloodFeudOffset.DeepClone<ModifiableValue>(),
					BazaarBackroomAccess = this.BazaarBackroomAccess.DeepClone<ModifiableBool>(),
					DiplomacyPrestigeBonus = this.DiplomacyPrestigeBonus.DeepClone<ModifiableValue>(),
					SelfDemandCostReduction = this.SelfDemandCostReduction.DeepClone<ModifiableValue>(),
					OtherDemandCostIncrease = this.OtherDemandCostIncrease.DeepClone<ModifiableValue>(),
					StratagemTacticSlots = this.StratagemTacticSlots.DeepClone<ModifiableValue>(),
					SchemeSlots = this.SchemeSlots.DeepClone<ModifiableValue>(),
					NumBasicSchemeOptions = this.NumBasicSchemeOptions.DeepClone<ModifiableValue>(),
					NumGrandSchemeOptions = this.NumGrandSchemeOptions.DeepClone<ModifiableValue>(),
					NumSchemeSelections = this.NumSchemeSelections.DeepClone<ModifiableValue>(),
					IsDrawTributeAvailable = this.IsDrawTributeAvailable.DeepClone<ModifiableBool>(),
					_baseTributeQuality = this._baseTributeQuality.DeepClone<ModifiableValue>(),
					NumTributeDraws = this.NumTributeDraws.DeepClone<ModifiableValue>(),
					NumTributeSelections = this.NumTributeSelections.DeepClone<ModifiableValue>(),
					TributeOfferings_NumOptions = this.TributeOfferings_NumOptions.DeepClone<ModifiableValue>(),
					TributeOfferings_NumSelections = this.TributeOfferings_NumSelections.DeepClone<ModifiableValue>(),
					MaxEventCards = this.MaxEventCards.DeepClone<ModifiableValue>(),
					EventCardDraw = this.EventCardDraw.DeepClone<ModifiableValue>(),
					ManuscriptSeekChance = this.ManuscriptSeekChance.DeepClone<ModifiableValue>(),
					ManuscriptSeekNumDrawn = this.ManuscriptSeekNumDrawn.DeepClone<ModifiableValue>(),
					ManuscriptSeekNumKept = this.ManuscriptSeekNumKept.DeepClone<ModifiableValue>(),
					MaxReviveDuration = this.MaxReviveDuration.DeepClone<ModifiableValue>(),
					Animosity = this.Animosity.DeepClone<ArchfiendAnimosity>(),
					PlayerTurn = this.PlayerTurn.DeepClone<PlayerTurn>(),
					PowersLevels = this.PowersLevels.DeepClone<PlayerPowersLevels>(),
					PlayerKnowledgeContexts = this.PlayerKnowledgeContexts.DeepClone<PlayerKnowledgeContext>(),
					Resources = this.Resources.DeepClone<ResourceNFT>(),
					DecisionRequests = this.DecisionRequests.DeepClone(CloneFunction.FastClone),
					AbilityCooldowns = this.AbilityCooldowns.DeepClone(),
					_activeSchemeCards = this._activeSchemeCards.DeepClone(),
					_previousSchemeCards = this._previousSchemeCards.DeepClone(),
					RitualState = this.RitualState.DeepClone<PlayerRitualState>(),
					_vaultedItems = this._vaultedItems.DeepClone(),
					_activeRelics = this._activeRelics.DeepClone(),
					AIPersistentData = this.AIPersistentData.DeepClone<AIPersistentData>(),
					AITags = this.AITags.DeepClone(),
					DuelRewardMultiplier = this.DuelRewardMultiplier.DeepClone<ModifiableValue>(),
					MessageLog = this.MessageLog.DeepClone<MessageLog>(),
					GameStatistics = this.GameStatistics.DeepClone<PlayerGameStatistics>(),
					MessageTriggers = this.MessageTriggers.DeepClone<CannedMessageTrigger>(),
					RankCostPercent = this.RankCostPercent.DeepClone<ModifiableValue>(),
					ResourceQueue = this.ResourceQueue.DeepClone<ResourceTypeQueue>(),
					ManuscriptQueue = this.ManuscriptQueue.DeepClone<ManuscriptTypeQueue>(),
					_turnWasLastRegent = this._turnWasLastRegent
				};
				base.DeepCloneParts(clone);
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00045F74 File Offset: 0x00044174
		public void RemoveKnowledgeModifier(KnowledgeModifier modifier)
		{
			this.RemoveKnowledgeModifier(modifier.Guid);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00045F84 File Offset: 0x00044184
		public void RemoveKnowledgeModifier(Guid guid)
		{
			this.KnowledgeModifiers.RemoveAll((KnowledgeModifier x) => x.Guid == guid);
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00045FB6 File Offset: 0x000441B6
		public void ClearKnowledgeList()
		{
			this.KnowledgeModifiers.Clear();
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00045FC3 File Offset: 0x000441C3
		public void AddKnowledgeModifier(KnowledgeModifier modifier, bool isUnique)
		{
			if (isUnique)
			{
				this.RemoveKnowledgeModifier(modifier.Guid);
			}
			this.KnowledgeModifiers.Add(modifier);
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00045FE0 File Offset: 0x000441E0
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.PlatformDisplayName))
			{
				return this.PlatformDisplayName;
			}
			return base.ToString();
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00045FFC File Offset: 0x000441FC
		public void RegenerateAITags(GameDatabase database)
		{
			IEnumerable<AITag> enumerable = Enumerable.Empty<AITag>();
			AIArchfiendDefaultTags aiarchfiendDefaultTags = database.FetchSingle<AIArchfiendDefaultTags>();
			if (aiarchfiendDefaultTags != null)
			{
				enumerable = enumerable.Concat(aiarchfiendDefaultTags.GetTagsForArchfiend(this.ArchfiendId));
			}
			AIDifficultyStaticData aidifficultyStaticData;
			if (database.TryGetDifficultyData(this.AIDifficulty, out aidifficultyStaticData))
			{
				enumerable = enumerable.Concat(aidifficultyStaticData.AppliedBehaviourTags);
			}
			this.SetAITags(IEnumerableExtensions.ToList<AITag>(enumerable));
		}

		// Token: 0x0400082E RID: 2094
		public const int ForceMajeurePlayerId = -1;

		// Token: 0x0400082F RID: 2095
		public const int InvalidPlayerId = -2147483648;

		// Token: 0x04000830 RID: 2096
		public const int MinOrderSlots = 1;

		// Token: 0x04000831 RID: 2097
		public const int MaxOrderSlots = 6;

		// Token: 0x04000832 RID: 2098
		public const int MaxSchemeSlots = 3;

		// Token: 0x04000833 RID: 2099
		[JsonProperty]
		public PlayerGameStatistics GameStatistics = new PlayerGameStatistics();

		// Token: 0x04000834 RID: 2100
		[JsonProperty]
		private List<KnowledgeModifier> KnowledgeModifiers = new List<KnowledgeModifier>();

		// Token: 0x04000835 RID: 2101
		[JsonProperty]
		[PublicKnowledge]
		public PlayerRole Role;

		// Token: 0x04000836 RID: 2102
		[JsonProperty]
		[PublicKnowledge]
		public AIDifficulty AIDifficulty;

		// Token: 0x04000837 RID: 2103
		[JsonProperty]
		[PublicKnowledge]
		public string PlayFabId;

		// Token: 0x04000838 RID: 2104
		[JsonProperty]
		[PublicKnowledge]
		public string PlatformDisplayName;

		// Token: 0x04000839 RID: 2105
		[JsonProperty]
		[PublicKnowledge]
		public int MultiplayerIdleCounter;

		// Token: 0x0400083A RID: 2106
		[JsonProperty]
		[PublicKnowledge]
		[DefaultValue(-2147483648)]
		public int Id = int.MinValue;

		// Token: 0x0400083B RID: 2107
		[JsonProperty]
		[PublicKnowledge]
		public int SpendablePrestige;

		// Token: 0x0400083C RID: 2108
		[JsonProperty]
		[PublicKnowledge]
		public ModifiableValue RankValue = new ModifiableValue(0f, 0, 3, RoundingMode.RoundDown);

		// Token: 0x0400083D RID: 2109
		[JsonProperty]
		[PublicKnowledge]
		public Identifier StrongholdId;

		// Token: 0x0400083E RID: 2110
		[JsonProperty]
		[PublicKnowledge]
		public Identifier PersonalGuardId;

		// Token: 0x0400083F RID: 2111
		[JsonProperty]
		[PublicKnowledge]
		public bool Eliminated;

		// Token: 0x04000840 RID: 2112
		[JsonProperty]
		[PublicKnowledge]
		public int EliminatedTurn;

		// Token: 0x04000841 RID: 2113
		[JsonProperty]
		[PublicKnowledge]
		private Guid _eliminatedModifierId;

		// Token: 0x04000842 RID: 2114
		[JsonProperty]
		[PublicKnowledge]
		public bool Excommunicated;

		// Token: 0x04000843 RID: 2115
		[JsonProperty]
		[PublicKnowledge]
		public int ExcommunicatedTurn;

		// Token: 0x04000844 RID: 2116
		[JsonProperty]
		[PublicKnowledge]
		private Guid _excommunicatedModifierId;

		// Token: 0x04000845 RID: 2117
		[JsonProperty]
		[PublicKnowledge]
		private BitMask ExcommunicationReasons;

		// Token: 0x04000846 RID: 2118
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int KingmakerPuppetId = int.MinValue;

		// Token: 0x04000847 RID: 2119
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool IsKingmaker = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000848 RID: 2120
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool IsPowerBehindTheThrone = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000849 RID: 2121
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool HasBazaarPurchaseKnowledge = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x0400084A RID: 2122
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool HasVendettaKnowledge = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x0400084B RID: 2123
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool BlockEventCardUse = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x0400084C RID: 2124
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool BlockStratagemUse = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x0400084D RID: 2125
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool BlockBazaarAccess = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x0400084E RID: 2126
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool BlockRitualTable = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x0400084F RID: 2127
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool BlockOfferings = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000850 RID: 2128
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool CanReplaceAnyCombatCard = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000851 RID: 2129
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool CanBribeArbiter = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000852 RID: 2130
		[PublicKnowledge]
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool DiplomaticImmunity = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000853 RID: 2131
		[PublicKnowledge]
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool HiddenPrestige = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000854 RID: 2132
		[JsonProperty]
		[DefaultValue(2)]
		public ModifiableBool ValidRegent = new ModifiableBool(true, LogicOperation.And);

		// Token: 0x04000855 RID: 2133
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue VotingPower = 0;

		// Token: 0x04000856 RID: 2134
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue HealingRate = 1;

		// Token: 0x04000857 RID: 2135
		[JsonProperty]
		public ModifiableValue PassivePrestige = new ModifiableValue(0f, 0, int.MaxValue, RoundingMode.RoundUp);

		// Token: 0x04000858 RID: 2136
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue CommandRating = 0;

		// Token: 0x04000859 RID: 2137
		[JsonProperty]
		public ModifiableValue OrderSlots = new ModifiableValue(0f, 1, 6, RoundingMode.RoundDown);

		// Token: 0x0400085A RID: 2138
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue CombatRewardMultiplier = 1;

		// Token: 0x0400085B RID: 2139
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue DuelRewardMultiplier = 1;

		// Token: 0x0400085C RID: 2140
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue RitualDestructionRewardMultiplier = 1;

		// Token: 0x0400085D RID: 2141
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue RitualCostMultiplier = 1;

		// Token: 0x0400085E RID: 2142
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue RitualCostOffset = 0;

		// Token: 0x0400085F RID: 2143
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue VendettasForBloodFeudOffset = new ModifiableValue(0f, int.MinValue, int.MaxValue, RoundingMode.RoundDown);

		// Token: 0x04000860 RID: 2144
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool BazaarBackroomAccess = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000861 RID: 2145
		[JsonProperty]
		[DefaultValue(100)]
		public ModifiableValue RankCostPercent = 100;

		// Token: 0x04000862 RID: 2146
		[JsonProperty]
		[DefaultValue(100)]
		public ModifiableValue MaxReviveDuration = 100;

		// Token: 0x04000863 RID: 2147
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue DiplomacyPrestigeBonus = 0;

		// Token: 0x04000864 RID: 2148
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue SelfDemandCostReduction = 0;

		// Token: 0x04000865 RID: 2149
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue OtherDemandCostIncrease = 0;

		// Token: 0x04000866 RID: 2150
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue StratagemTacticSlots = 1;

		// Token: 0x04000867 RID: 2151
		[JsonProperty]
		[PublicKnowledge]
		public ModifiableValue SchemeSlots = new ModifiableValue(2f, 0, 3, RoundingMode.RoundDown);

		// Token: 0x04000868 RID: 2152
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue NumBasicSchemeOptions = 1;

		// Token: 0x04000869 RID: 2153
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue NumGrandSchemeOptions = 1;

		// Token: 0x0400086A RID: 2154
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue NumSchemeSelections = 1;

		// Token: 0x0400086B RID: 2155
		[JsonProperty]
		[DefaultValue(2)]
		public ModifiableBool IsDrawTributeAvailable = new ModifiableBool(true, LogicOperation.And);

		// Token: 0x0400086C RID: 2156
		[JsonProperty]
		[DefaultValue(0)]
		private ModifiableValue _baseTributeQuality = 0;

		// Token: 0x0400086D RID: 2157
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue NumTributeDraws = new ModifiableValue(0f, 1, int.MaxValue, RoundingMode.RoundDown);

		// Token: 0x0400086E RID: 2158
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue NumTributeSelections = new ModifiableValue(0f, 1, int.MaxValue, RoundingMode.RoundDown);

		// Token: 0x0400086F RID: 2159
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue TributeOfferings_NumOptions = 0;

		// Token: 0x04000870 RID: 2160
		[JsonProperty]
		[DefaultValue(0)]
		public ModifiableValue TributeOfferings_NumSelections = 0;

		// Token: 0x04000871 RID: 2161
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue MaxEventCards = 1;

		// Token: 0x04000872 RID: 2162
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue EventCardDraw = 1;

		// Token: 0x04000873 RID: 2163
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue ManuscriptSeekChance = 1;

		// Token: 0x04000874 RID: 2164
		[JsonProperty]
		[DefaultValue(4)]
		public ModifiableValue ManuscriptSeekNumDrawn = 4;

		// Token: 0x04000875 RID: 2165
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue ManuscriptSeekNumKept = 1;

		// Token: 0x04000876 RID: 2166
		[JsonProperty]
		[PublicKnowledge]
		public ArchfiendAnimosity Animosity = new ArchfiendAnimosity();

		// Token: 0x04000877 RID: 2167
		[JsonProperty]
		public PlayerTurn PlayerTurn = new PlayerTurn();

		// Token: 0x04000878 RID: 2168
		[JsonProperty]
		public PlayerPowersLevels PowersLevels = new PlayerPowersLevels();

		// Token: 0x04000879 RID: 2169
		[JsonProperty]
		public Dictionary<int, PlayerKnowledgeContext> PlayerKnowledgeContexts = new Dictionary<int, PlayerKnowledgeContext>();

		// Token: 0x0400087A RID: 2170
		[JsonProperty]
		public List<ResourceNFT> Resources = new List<ResourceNFT>();

		// Token: 0x0400087B RID: 2171
		[JsonProperty]
		public List<DecisionRequest> DecisionRequests = new List<DecisionRequest>();

		// Token: 0x0400087C RID: 2172
		[JsonProperty]
		public Dictionary<string, int> AbilityCooldowns = new Dictionary<string, int>();

		// Token: 0x0400087D RID: 2173
		[JsonProperty]
		private List<Identifier> _activeSchemeCards = new List<Identifier>();

		// Token: 0x0400087E RID: 2174
		[JsonProperty]
		private List<Identifier> _previousSchemeCards = new List<Identifier>();

		// Token: 0x0400087F RID: 2175
		[JsonProperty]
		public PlayerRitualState RitualState = new PlayerRitualState();

		// Token: 0x04000880 RID: 2176
		[JsonProperty]
		private List<Identifier> _vaultedItems = new List<Identifier>();

		// Token: 0x04000881 RID: 2177
		[JsonProperty]
		private List<Identifier> _activeRelics = new List<Identifier>();

		// Token: 0x04000882 RID: 2178
		[JsonProperty]
		public MessageLog MessageLog = new MessageLog();

		// Token: 0x04000883 RID: 2179
		[JsonProperty]
		[DefaultValue(-1)]
		private int _turnWasLastRegent = -1;

		// Token: 0x04000884 RID: 2180
		[JsonProperty]
		public AIPersistentData AIPersistentData;

		// Token: 0x04000885 RID: 2181
		[JsonProperty]
		public List<AITag> AITags = new List<AITag>();

		// Token: 0x04000886 RID: 2182
		[JsonProperty]
		public List<CannedMessageTrigger> MessageTriggers = new List<CannedMessageTrigger>();

		// Token: 0x04000887 RID: 2183
		[JsonProperty]
		public ResourceTypeQueue ResourceQueue = new ResourceTypeQueue();

		// Token: 0x04000888 RID: 2184
		[JsonProperty]
		public ManuscriptTypeQueue ManuscriptQueue = new ManuscriptTypeQueue();
	}
}
