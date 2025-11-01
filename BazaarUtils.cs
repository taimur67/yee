using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020003DE RID: 990
	public static class BazaarUtils
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x00049F47 File Offset: 0x00048147
		public static IList<GameItemCategory> AllBazaarCategories
		{
			get
			{
				return BazaarUtils._bazaarCategories;
			}
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00049F4E File Offset: 0x0004814E
		public static bool IsBackroomItem(this TurnProcessContext context, GameItem item)
		{
			return IEnumerableExtensions.Contains<GameItem>(context.CurrentTurn.GetEarlyAccessBazaarItemsForCategory(item.Category), item);
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00049F68 File Offset: 0x00048168
		public static void ResetBazaar(this TurnProcessContext context)
		{
			BazaarState bazaarState = context.CurrentTurn.BazaarState;
			foreach (BazaarState.BazaarSlot bazaarSlot in bazaarState.AllSlots)
			{
				context.RemoveGameItemFromGameNoRecord(bazaarSlot.CurrentItem);
			}
			bazaarState.BazaarSlots.Clear();
			context.InitializeBazaarSlots();
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00049FD8 File Offset: 0x000481D8
		public static void InitializeBazaarSlots(this TurnProcessContext context)
		{
			BazaarEconomyData bazaarEconomyData = context.Database.FetchSingle<BazaarEconomyData>();
			if (bazaarEconomyData == null)
			{
				return;
			}
			foreach (GameItemCategory category in BazaarUtils._bazaarCategories)
			{
				List<BazaarSlotData> slots = IEnumerableExtensions.ToList<BazaarSlotData>(bazaarEconomyData.GetSlots(category, context.CurrentTurn.GetNumberOfPlayers(false, false)));
				context.CurrentTurn.BazaarState.InitSlots(context, category, slots);
			}
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0004A03C File Offset: 0x0004823C
		public static void RePopulateBazaar(this TurnProcessContext context, bool clearExisting = false)
		{
			BazaarState bazaarState = context.CurrentTurn.BazaarState;
			if (clearExisting)
			{
				bazaarState.ClearSlots();
			}
			if (context.Database.FetchSingle<BazaarEconomyData>() == null)
			{
				return;
			}
			foreach (KeyValuePair<GameItemCategory, List<BazaarState.BazaarSlot>> keyValuePair in bazaarState.BazaarSlots)
			{
				foreach (BazaarState.BazaarSlot bazaarSlot in IEnumerableExtensions.ToList<BazaarState.BazaarSlot>(keyValuePair.Value))
				{
					if (bazaarSlot.CurrentItem == Identifier.Invalid)
					{
						bazaarState.RefillSlot(bazaarSlot, keyValuePair.Key, context);
					}
				}
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0004A108 File Offset: 0x00048308
		public static void ForceRepopulateBazaar(this TurnProcessContext context)
		{
			foreach (GameItemCategory category in BazaarUtils.AllBazaarCategories)
			{
				context.ForceRepopulateBazaar(category);
			}
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0004A154 File Offset: 0x00048354
		public static void ForceRepopulateBazaar(this TurnProcessContext context, GameItemCategory category)
		{
			BazaarState bazaarState = context.CurrentTurn.BazaarState;
			if (context.Database.FetchSingle<BazaarEconomyData>() == null)
			{
				return;
			}
			foreach (BazaarState.BazaarSlot bazaarSlot in bazaarState.GetSlotsForCategory(category))
			{
				context.RemoveGameItemFromGameNoRecord(bazaarSlot.CurrentItem);
				bazaarSlot.CurrentItem = Identifier.Invalid;
				bazaarState.RefillSlot(bazaarSlot, category, context);
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0004A1D4 File Offset: 0x000483D4
		public static void ReduceItemCost(this GameItem item, int discountVal)
		{
			ResourceTypes resourceTypes = ResourceTypes.Souls;
			while (discountVal > 0)
			{
				if (item.Cost[resourceTypes] <= 0)
				{
					resourceTypes++;
					if (resourceTypes >= ResourceTypes.Prestige)
					{
						return;
					}
				}
				else
				{
					Cost cost = item.Cost;
					ResourceTypes type = resourceTypes;
					int value = cost[type] - 1;
					cost[type] = value;
					discountVal--;
				}
			}
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0004A220 File Offset: 0x00048420
		public static GamePiece AddLegionToBazaar(TurnProcessContext context, GameDatabase database, string legionId, BazaarState.BazaarSlot slot = null)
		{
			GamePieceStaticData legionData = database.GetLegionData(legionId);
			if (legionData == null)
			{
				return null;
			}
			return (GamePiece)context.CurrentTurn.BazaarState.AddItemToBazaar<GamePiece>(context, legionData, slot);
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0004A258 File Offset: 0x00048458
		public static Artifact AddArtifactToBazaar(TurnProcessContext context, GameDatabase database, string itemId, BazaarState.BazaarSlot slot = null)
		{
			ArtifactStaticData artifactStaticData = database.Fetch<ArtifactStaticData>(itemId);
			if (artifactStaticData == null)
			{
				return null;
			}
			return (Artifact)context.CurrentTurn.BazaarState.AddItemToBazaar<Artifact>(context, artifactStaticData, slot);
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0004A290 File Offset: 0x00048490
		public static Manuscript AddManuscriptToBazaar(TurnProcessContext context, GameDatabase database, string itemId, BazaarState.BazaarSlot slot = null)
		{
			ManuscriptStaticData manuscriptStaticData = database.Fetch<ManuscriptStaticData>(itemId);
			if (manuscriptStaticData == null)
			{
				return null;
			}
			return (Manuscript)context.CurrentTurn.BazaarState.AddItemToBazaar<Manuscript>(context, manuscriptStaticData, slot);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0004A2C4 File Offset: 0x000484C4
		public static Praetor AddPraetorToBazaar(TurnProcessContext context, GameDatabase database, string itemId, BazaarState.BazaarSlot slot = null)
		{
			PraetorStaticData praetorStaticData = database.Fetch<PraetorStaticData>(itemId);
			if (praetorStaticData == null)
			{
				return null;
			}
			return (Praetor)context.CurrentTurn.BazaarState.AddItemToBazaar<Praetor>(context, praetorStaticData, slot);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0004A2FC File Offset: 0x000484FC
		public static bool RemoveGameItemFromBazaar(TurnState turn, string itemId)
		{
			GameItem gameItem = turn.FetchGameItem(itemId);
			return gameItem != null && turn.RemoveGameItem<GameItem>(gameItem) && turn.BazaarState.RemoveItemFromBazaar(gameItem);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0004A332 File Offset: 0x00048532
		public static IEnumerable<T> GetStockableGameItems<T>(this TurnContext context) where T : GameItemStaticData
		{
			foreach (T t in context.Database.Enumerate<T>())
			{
				if (t.CanSpawnInBazaar && !t.Cost.IsZero && !context.Rules.BlacklistedEntities.Contains(t.ConfigRef))
				{
					yield return t;
				}
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0004A342 File Offset: 0x00048542
		public static IEnumerable<T> GetPurchasableGameItems<T>(this TurnContext context) where T : GameItemStaticData
		{
			TurnState turn = context.CurrentTurn;
			using (IEnumerator<T> enumerator = context.GetStockableGameItems<T>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T candidate = enumerator.Current;
					if ((from x in turn.AllGameItems
					where x.StaticDataId == candidate.Id
					select x).Sum((GameItem x) => x.ItemCount) + turn.DeadItemReferences.Count((ConfigRef t) => t.Id == candidate.Id) < candidate.SpawnableItemLimit)
					{
						yield return candidate;
					}
				}
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0004A354 File Offset: 0x00048554
		public static T GetRandomPurchasableItem<T>(this TurnContext context) where T : GameItemStaticData
		{
			List<T> list = IEnumerableExtensions.ToList<T>(context.GetPurchasableGameItems<T>());
			if (list.Count != 0)
			{
				return list.Random(context.Random);
			}
			return default(T);
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0004A38C File Offset: 0x0004858C
		public static ManuscriptStaticData GetWeightedPurchasableManuscript(this TurnContext context, params ManuscriptCategory[] types)
		{
			IEnumerable<ManuscriptStaticData> enumerable = context.GetPurchasableGameItems<ManuscriptStaticData>();
			if (types != null && types.Length > 0)
			{
				enumerable = from x in enumerable
				where IEnumerableExtensions.Contains<ManuscriptCategory>(types, x.ManuscriptCategory)
				select x;
			}
			List<ManuscriptStaticData> list = IEnumerableExtensions.ToList<ManuscriptStaticData>(enumerable);
			if (list.Count != 0)
			{
				return list.WeightedRandom((ManuscriptStaticData x) => (float)x.RarityWeighting, context.Random, false);
			}
			return null;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0004A414 File Offset: 0x00048614
		private static float GetManuscriptFragmentWeight(TurnContext context, PlayerState player, ManuscriptStaticData fragment, bool forceLuckyRoll = false)
		{
			float num = (float)context.GetManuscriptCurrentFragmentCount(player.Id, fragment.Id) / (float)fragment.FragmentCount;
			if (num > 0f && num < 1f)
			{
				if (forceLuckyRoll)
				{
					return num * float.MaxValue;
				}
			}
			else if (context.CurrentTurn.BazaarState.IsInstanceForSale(context.CurrentTurn, fragment))
			{
				if (fragment.FragmentCount > 1)
				{
					return (float)fragment.RarityWeighting * 3f;
				}
				return (float)fragment.RarityWeighting / 3f;
			}
			return (float)fragment.RarityWeighting;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0004A4A4 File Offset: 0x000486A4
		public static ManuscriptStaticData GetWeightedManuscriptForPlayer(this TurnContext context, PlayerState player, ManuscriptCategory manuscriptType, bool forceLuckyRoll = false)
		{
			TurnState currentTurn = context.CurrentTurn;
			List<ManuscriptStaticData> list = IEnumerableExtensions.ToList<ManuscriptStaticData>(from fragment in context.GetPurchasableGameItems<ManuscriptStaticData>()
			where fragment.ManuscriptCategory == manuscriptType
			select fragment);
			if (list.Count == 0)
			{
				return null;
			}
			Dictionary<ManuscriptStaticData, float> candidateWeights = new Dictionary<ManuscriptStaticData, float>();
			foreach (ManuscriptStaticData manuscriptStaticData in list)
			{
				candidateWeights[manuscriptStaticData] = BazaarUtils.GetManuscriptFragmentWeight(context, player, manuscriptStaticData, forceLuckyRoll);
			}
			return list.WeightedRandom((ManuscriptStaticData fragment) => candidateWeights[fragment], currentTurn.Random, false);
		}

		// Token: 0x040008DF RID: 2271
		private static readonly GameItemCategory[] _bazaarCategories = new GameItemCategory[]
		{
			GameItemCategory.GamePiece,
			GameItemCategory.Praetor,
			GameItemCategory.Artifact,
			GameItemCategory.ManuscriptPiece
		};
	}
}
