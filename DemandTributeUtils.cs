using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000635 RID: 1589
	public static class DemandTributeUtils
	{
		// Token: 0x06001D5B RID: 7515 RVA: 0x000656DC File Offset: 0x000638DC
		public static DemandTributeUtils.TributeParameters GetDemandTributeParameters(this PlayerState player, TurnProcessContext context)
		{
			return player.GetDemandTributeParameters(context.Database, context.Rules);
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000656F0 File Offset: 0x000638F0
		public static DemandTributeUtils.TributeParameters GetDemandTributeParameters(this PlayerState player, GameDatabase database, GameRules rules)
		{
			if (player == null || rules == null)
			{
				return default(DemandTributeUtils.TributeParameters);
			}
			return new DemandTributeUtils.TributeParameters
			{
				IsAvailable = player.IsDrawTributeAvailable,
				Draw = player.NumTributeDraws,
				Pick = player.NumTributeSelections,
				Quality = player.TributeQuality,
				TypeQueue = player.ResourceQueue,
				PlayerTags = player.EnumerateTags<EntityTag>()
			};
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x00065778 File Offset: 0x00063978
		public static Result CreateAndAddDemandTributeDecisionToPlayer(TurnProcessContext context, PlayerState player, DemandTributeUtils.TributeParameters offeringParams)
		{
			SelectTributeDecisionRequest decisionRequest = DemandTributeUtils.CreateDemandTributeSelection(context, player, offeringParams);
			context.CurrentTurn.AddDecisionToAskPlayer(player.Id, decisionRequest);
			return Result.Success;
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000657A8 File Offset: 0x000639A8
		public static SelectTributeDecisionRequest CreateDemandTributeSelection(TurnProcessContext context, PlayerState player, DemandTributeUtils.TributeParameters offeringParams)
		{
			GameDatabase database = context.Database;
			TributeEconomyStaticData tributeEconomyStaticData = database.FetchSingle<TributeEconomyStaticData>();
			CardGenerationData generationData = database.Fetch(tributeEconomyStaticData.DemandTributeGenerationData);
			DemandPayload demandPayload = new DemandPayload();
			IEnumerable<ResourceNFT> enumerable = DemandTributeUtils.GenerateCandidateTribute(context, generationData, offeringParams);
			demandPayload.Tokens = IEnumerableExtensions.ToList<ResourceNFT>(enumerable);
			return new SelectTributeDecisionRequest(context.CurrentTurn, demandPayload, offeringParams.Pick, false);
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x00065804 File Offset: 0x00063A04
		public static DemandTributeUtils.TributeParameters GetTributeOfferingParameters(this PlayerState player)
		{
			int num = Math.Max(player.TributeOfferings_NumOptions, player.TributeOfferings_NumSelections);
			return new DemandTributeUtils.TributeParameters
			{
				Draw = num,
				Pick = num,
				Quality = player.TributeQuality,
				TypeQueue = player.ResourceQueue,
				PlayerTags = player.EnumerateTags<EntityTag>()
			};
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x00065874 File Offset: 0x00063A74
		public static Result CreateOfferingWithEvent(TurnProcessContext context, PlayerState player)
		{
			TributeOfferingEvent gameEvent = DemandTributeUtils.CreateTributeOffering(context, player);
			context.CurrentTurn.AddGameEvent<TributeOfferingEvent>(gameEvent);
			return Result.Success;
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0006589C File Offset: 0x00063A9C
		public static TributeOfferingEvent CreateTributeOffering(TurnProcessContext context, PlayerState player)
		{
			DemandTributeUtils.TributeParameters tributeOfferingParameters = player.GetTributeOfferingParameters();
			TributeEconomyStaticData tributeEconomyStaticData = context.Database.FetchSingle<TributeEconomyStaticData>();
			CardGenerationData generationData = context.Database.Fetch(tributeEconomyStaticData.OfferingTributeGenerationData);
			PaymentReceivedEvent paymentEvent = context.GivePayment(player, new Payment
			{
				Resources = IEnumerableExtensions.ToList<ResourceNFT>(DemandTributeUtils.GenerateCandidateTribute(context, generationData, tributeOfferingParameters))
			}, null);
			return new TributeOfferingEvent(player.Id, paymentEvent)
			{
				TurnsUntilNextOffering = context.Rules.OfferingInterval
			};
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x00065910 File Offset: 0x00063B10
		public static PaymentRemovedEvent RemovePayment(this TurnProcessContext turnContext, PlayerState affectedPlayer, Payment payment, List<Identifier> items = null)
		{
			return turnContext.RemovePayment(turnContext.CurrentTurn.ForceMajeurePlayer, affectedPlayer, payment, items);
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x00065928 File Offset: 0x00063B28
		public static PaymentRemovedEvent RemovePayment(this TurnProcessContext turnContext, PlayerState triggeringPlayer, PlayerState affectedPlayer, Payment payment, List<Identifier> items = null)
		{
			PaymentRemovedEvent result = new PaymentRemovedEvent(triggeringPlayer.Id, affectedPlayer.Id, payment, items);
			affectedPlayer.RemovePayment(payment);
			if (items != null)
			{
				foreach (Identifier itemId in items)
				{
					turnContext.RemoveItemFromAnySlotControlledByPlayer(affectedPlayer, itemId);
				}
			}
			return result;
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0006599C File Offset: 0x00063B9C
		public static PaymentReceivedEvent GivePayment(this TurnProcessContext turnContext, PlayerState toPlayer, Payment payment, List<Identifier> items = null)
		{
			return turnContext.GivePayment(turnContext.CurrentTurn.ForceMajeurePlayer, toPlayer, payment, items);
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x000659B4 File Offset: 0x00063BB4
		public static PaymentReceivedEvent GivePayment(this TurnProcessContext turnContext, PlayerState fromPlayer, PlayerState toPlayer, Payment payment, List<Identifier> items = null)
		{
			PaymentReceivedEvent paymentReceivedEvent = new PaymentReceivedEvent(fromPlayer.Id, toPlayer.Id, payment, items);
			PaymentRemovedEvent ev = new PaymentRemovedEvent(toPlayer.Id, fromPlayer.Id, payment, items);
			paymentReceivedEvent.AddChildEvent<PaymentRemovedEvent>(ev);
			bool flag = fromPlayer.Id == -1;
			if (!flag)
			{
				fromPlayer.RemovePayment(payment);
			}
			if (items != null)
			{
				foreach (Identifier identifier in items)
				{
					if (!flag)
					{
						turnContext.RemoveItemFromAnySlotControlledByPlayer(fromPlayer, identifier);
					}
					turnContext.RemoveItemFromPlayersKnowledge(toPlayer, identifier);
					toPlayer.AddToVault(identifier);
				}
			}
			toPlayer.GivePayment(payment);
			return paymentReceivedEvent;
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00065A70 File Offset: 0x00063C70
		public static PaymentReceivedEvent GivePrestige(this TurnProcessContext turnContext, PlayerState toPlayer, int prestige)
		{
			return turnContext.GivePrestige(turnContext.CurrentTurn.ForceMajeurePlayer, toPlayer, prestige);
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x00065A88 File Offset: 0x00063C88
		public static PaymentReceivedEvent GivePrestige(this TurnProcessContext turnContext, PlayerState fromPlayer, PlayerState toPlayer, int prestige)
		{
			Payment payment = new Payment
			{
				Prestige = prestige
			};
			return turnContext.GivePayment(fromPlayer, toPlayer, payment, null);
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x00065AAC File Offset: 0x00063CAC
		public static IEnumerable<ResourceNFT> GenerateCandidateTribute(TurnProcessContext context, CardGenerationData generationData, DemandTributeUtils.TributeParameters parameters)
		{
			return DemandTributeUtils.GenerateCandidateTribute(context.CurrentTurn, context.Rules, context.Database, generationData, parameters);
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x00065AC8 File Offset: 0x00063CC8
		public static IEnumerable<ResourceNFT> GenerateCandidateTribute(TurnState turn, GameRules rules, GameDatabase database, CardGenerationData generationData, DemandTributeUtils.TributeParameters parameters)
		{
			TributeQualityStaticData selection = generationData.GetSelection(database, parameters.Quality);
			return DemandTributeUtils.GenerateCandidateTribute(turn, rules, generationData, selection, parameters);
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x00065AEF File Offset: 0x00063CEF
		public static IEnumerable<ResourceNFT> GenerateCandidateTribute(TurnState turn, GameRules rules, CardGenerationData economy, TributeQualityStaticData quality, DemandTributeUtils.TributeParameters parameters)
		{
			int maxAmountPerResource = rules.MaximumValueOfIndividualResourceOnToken;
			ResourceTypeQueue typeQueue = parameters.TypeQueue;
			int numberOfTokensToGenerate = parameters.Draw;
			double num;
			if (!IEnumerableExtensions.Any<EntityTag_CheatLuckyTributeDraw>(parameters.PlayerTags.OfType<EntityTag_CheatLuckyTributeDraw>()))
			{
				num = (double)quality.ValueWeights.SelectRandom(turn.Random);
			}
			else
			{
				num = (double)quality.ValueWeights.Max((WeightedValue<float> valueWeight) => valueWeight.Value);
			}
			int num2 = (int)Math.Floor(num * (double)((float)numberOfTokensToGenerate));
			int i = 0;
			ResourceAccumulation[] tokenResourceAccumulations = new ResourceAccumulation[numberOfTokensToGenerate];
			for (int j = 0; j < numberOfTokensToGenerate; j++)
			{
				ResourceAccumulation resourceAccumulation4 = new ResourceAccumulation();
				ResourceTypes type = typeQueue.Draw(turn.Random);
				resourceAccumulation4[type] = 1;
				i++;
				tokenResourceAccumulations[j] = resourceAccumulation4;
			}
			Func<ResourceAccumulation, float> <>9__1;
			int num4;
			while (i < num2)
			{
				IList<ResourceAccumulation> list = tokenResourceAccumulations;
				Func<ResourceAccumulation, float> weightSelector;
				if ((weightSelector = <>9__1) == null)
				{
					weightSelector = (<>9__1 = delegate(ResourceAccumulation resourceAccumulation)
					{
						if (!resourceAccumulation.HasSpaceForMoreResources(maxAmountPerResource))
						{
							return 0f;
						}
						return 1f / (float)resourceAccumulation.ValueSum;
					});
				}
				ResourceAccumulation resourceAccumulation2 = list.WeightedRandom(weightSelector, turn.Random, true);
				int num3 = resourceAccumulation2.GetAllNonNullResources().Count<ResourceTypes>();
				bool flag = resourceAccumulation2.HasSpaceForResourcesOnExistingTypes(maxAmountPerResource);
				bool flag2 = num3 == ResourceNFT.ResourceKeys.Count;
				bool flag3 = !flag || (!flag2 && economy.CardCombinations.SelectRandom(turn.Random) > num3);
				ResourceTypes resourceTypes = ResourceTypes.Souls;
				if (flag3)
				{
					if (!typeQueue.TryDrawFirst(turn.Random, resourceAccumulation2.GetAllNullResources(), out resourceTypes))
					{
						SimLogger logger = SimLogger.Logger;
						if (logger == null)
						{
							break;
						}
						logger.Error("ResourceTypeQueue must be corrupted, as it should always contain every ResourceTypes except Prestige");
						break;
					}
				}
				else if (!typeQueue.TryDrawFirst(turn.Random, resourceAccumulation2.GetAllNonNullNotFullResources(maxAmountPerResource), out resourceTypes))
				{
					SimLogger logger2 = SimLogger.Logger;
					if (logger2 == null)
					{
						break;
					}
					logger2.Error("ResourceTypeQueue must be corrupted, as it should always contain every ResourceTypes except Prestige");
					break;
				}
				ResourceAccumulation resourceAccumulation3 = resourceAccumulation2;
				ResourceTypes type2 = resourceTypes;
				num4 = resourceAccumulation3[type2];
				resourceAccumulation3[type2] = num4 + 1;
				i++;
			}
			for (int tokenIndex = 0; tokenIndex < numberOfTokensToGenerate; tokenIndex = num4)
			{
				ResourceNFT resourceNFT = turn.CreateNFT(new ResourceAccumulation[]
				{
					tokenResourceAccumulations[tokenIndex]
				});
				yield return resourceNFT;
				num4 = tokenIndex + 1;
			}
			yield break;
		}

		// Token: 0x02000A5A RID: 2650
		public struct TributeParameters
		{
			// Token: 0x0400197D RID: 6525
			public bool IsAvailable;

			// Token: 0x0400197E RID: 6526
			public int Draw;

			// Token: 0x0400197F RID: 6527
			public int Pick;

			// Token: 0x04001980 RID: 6528
			public int Quality;

			// Token: 0x04001981 RID: 6529
			public ResourceTypeQueue TypeQueue;

			// Token: 0x04001982 RID: 6530
			public IEnumerable<EntityTag> PlayerTags;
		}
	}
}
