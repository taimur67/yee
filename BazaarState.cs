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
	// Token: 0x020001D0 RID: 464
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BazaarState : IDeepClone<BazaarState>
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00029EB2 File Offset: 0x000280B2
		public IEnumerable<BazaarState.BazaarSlot> AllSlots
		{
			get
			{
				return this.BazaarSlots.Values.SelectMany((List<BazaarState.BazaarSlot> t) => t);
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00029EE4 File Offset: 0x000280E4
		public IEnumerable<BazaarState.BazaarSlot> GetSlotsForCategory(GameItemCategory category)
		{
			List<BazaarState.BazaarSlot> result;
			if (this.BazaarSlots.TryGetValue(category, out result))
			{
				return result;
			}
			return Enumerable.Empty<BazaarState.BazaarSlot>();
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00029F08 File Offset: 0x00028108
		public IEnumerable<GameItemCategory> AvailableCategories
		{
			get
			{
				return this.BazaarSlots.Keys.Where(new Func<GameItemCategory, bool>(this.AnyAvailable));
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00029F26 File Offset: 0x00028126
		public IEnumerable<Identifier> GetAllBazaarItems()
		{
			return this.BazaarSlots.SelectMany((KeyValuePair<GameItemCategory, List<BazaarState.BazaarSlot>> category) => from x in category.Value
			select x.CurrentItem);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00029F52 File Offset: 0x00028152
		private bool AnyAvailable(GameItemCategory category)
		{
			return this.GetNumAvailable(category) > 0;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00029F60 File Offset: 0x00028160
		public int GetNumAvailable(GameItemCategory category)
		{
			List<BazaarState.BazaarSlot> source;
			if (!this.BazaarSlots.TryGetValue(category, out source))
			{
				return 0;
			}
			return source.Count((BazaarState.BazaarSlot x) => x.CurrentItem != Identifier.Invalid);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00029FA4 File Offset: 0x000281A4
		public IReadOnlyList<Identifier> GetAllItemsForCategory(GameItemCategory category, bool includeEarlyAccess)
		{
			List<BazaarState.BazaarSlot> source;
			if (this.BazaarSlots.TryGetValue(category, out source))
			{
				return IEnumerableExtensions.ToList<Identifier>(from x in source
				where x.CurrentItem != Identifier.Invalid
				where !x.RequireEarlyAccess | includeEarlyAccess
				select x.CurrentItem);
			}
			return Array.Empty<Identifier>();
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002A034 File Offset: 0x00028234
		public IReadOnlyList<Identifier> GetEarlyAccessBazaarItemsForCategory(GameItemCategory category)
		{
			List<BazaarState.BazaarSlot> source;
			if (this.BazaarSlots.TryGetValue(category, out source))
			{
				return IEnumerableExtensions.ToList<Identifier>(from x in source
				where x.CurrentItem != Identifier.Invalid
				where x.RequireEarlyAccess
				select x.CurrentItem);
			}
			return Array.Empty<Identifier>();
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0002A0CC File Offset: 0x000282CC
		public bool IsForSale(GameItem item)
		{
			foreach (KeyValuePair<GameItemCategory, List<BazaarState.BazaarSlot>> keyValuePair in this.BazaarSlots)
			{
				GameItemCategory gameItemCategory;
				List<BazaarState.BazaarSlot> list;
				keyValuePair.Deconstruct(out gameItemCategory, out list);
				using (List<BazaarState.BazaarSlot>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.CurrentItem == item.Id)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0002A170 File Offset: 0x00028370
		public bool IsInstanceForSale(TurnState turn, GameItemStaticData itemData)
		{
			foreach (KeyValuePair<GameItemCategory, List<BazaarState.BazaarSlot>> keyValuePair in this.BazaarSlots)
			{
				GameItemCategory gameItemCategory;
				List<BazaarState.BazaarSlot> list;
				keyValuePair.Deconstruct(out gameItemCategory, out list);
				foreach (BazaarState.BazaarSlot bazaarSlot in list)
				{
					GameItem gameItem = turn.FetchGameItem(bazaarSlot.CurrentItem);
					if (gameItem != null && gameItem.StaticDataId == itemData.Id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002A230 File Offset: 0x00028430
		public GameItem AddItemToBazaar<TGameItem>(TurnProcessContext context, GameItemStaticData data, BazaarState.BazaarSlot slot) where TGameItem : GameItem, new()
		{
			TGameItem tgameItem = BazaarState.CreateItem<TGameItem>(context, data);
			if (slot == null)
			{
				return this.AddItemToBazaar(tgameItem);
			}
			slot.CurrentItem = tgameItem;
			return tgameItem;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002A26C File Offset: 0x0002846C
		public GameItem AddItemToBazaar(GameItem item)
		{
			List<BazaarState.BazaarSlot> list;
			if (!this.BazaarSlots.TryGetValue(item.Category, out list))
			{
				list = new List<BazaarState.BazaarSlot>();
				this.BazaarSlots.Add(item.Category, list);
			}
			BazaarState.BazaarSlot bazaarSlot = list.FirstOrDefault((BazaarState.BazaarSlot x) => x.CurrentItem == Identifier.Invalid);
			if (bazaarSlot != null)
			{
				bazaarSlot.CurrentItem = item.Id;
				return item;
			}
			this.BazaarSlots[item.Category].Add(new BazaarState.BazaarSlot
			{
				CurrentItem = item.Id
			});
			return item;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0002A308 File Offset: 0x00028508
		public bool RemoveItemFromBazaar(Identifier itemId)
		{
			foreach (BazaarState.BazaarSlot bazaarSlot in this.BazaarSlots.SelectMany((KeyValuePair<GameItemCategory, List<BazaarState.BazaarSlot>> category) => category.Value))
			{
				if (bazaarSlot.CurrentItem == itemId)
				{
					bazaarSlot.CurrentItem = Identifier.Invalid;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002A38C File Offset: 0x0002858C
		public void ClearSlots()
		{
			foreach (BazaarState.BazaarSlot bazaarSlot in this.BazaarSlots.SelectMany((KeyValuePair<GameItemCategory, List<BazaarState.BazaarSlot>> category) => category.Value))
			{
				bazaarSlot.CurrentItem = Identifier.Invalid;
			}
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0002A3FC File Offset: 0x000285FC
		public bool RemoveSlot(BazaarState.BazaarSlot slot)
		{
			return this.BazaarSlots.Values.Any((List<BazaarState.BazaarSlot> t) => t.Remove(slot));
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0002A434 File Offset: 0x00028634
		public void AddSlot(GameItemCategory category, BazaarState.BazaarSlot slot)
		{
			List<BazaarState.BazaarSlot> list;
			if (!this.BazaarSlots.TryGetValue(category, out list))
			{
				list = (this.BazaarSlots[category] = new List<BazaarState.BazaarSlot>());
			}
			list.Add(slot);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0002A46C File Offset: 0x0002866C
		public void InitSlots(TurnProcessContext context, GameItemCategory category, List<BazaarSlotData> slots)
		{
			List<GameItemStaticData> list = IEnumerableExtensions.ToList<GameItemStaticData>(context.GetPurchasableGameItems<GameItemStaticData>());
			List<BazaarState.BazaarSlot> list2 = new List<BazaarState.BazaarSlot>(slots.Count);
			foreach (BazaarSlotData bazaarSlotData in slots)
			{
				GameItem slotItem = this.CreateItemMatchingFilter(context, list, context.Database.Fetch(bazaarSlotData.Filter));
				List<BazaarState.BazaarSlot> list3 = list2;
				BazaarState.BazaarSlot bazaarSlot = new BazaarState.BazaarSlot();
				bazaarSlot.RequireEarlyAccess = bazaarSlotData.RequireEarlyAccess;
				bazaarSlot.FilterId = bazaarSlotData.Filter;
				GameItem slotItem2 = slotItem;
				bazaarSlot.CurrentItem = ((slotItem2 != null) ? slotItem2.Id : Identifier.Invalid);
				list3.Add(bazaarSlot);
				if (slotItem != null)
				{
					list.RemoveAll((GameItemStaticData x) => x.Id == slotItem.StaticDataId);
				}
			}
			this.BazaarSlots[category] = list2;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0002A560 File Offset: 0x00028760
		public List<GameItemStaticData> FilterCandidates(TurnProcessContext context, IReadOnlyList<GameItemStaticData> candidates, BazaarFilter filter)
		{
			List<GameItemStaticData> list = new List<GameItemStaticData>();
			if (filter == null)
			{
				return null;
			}
			foreach (GameItemStaticData item in candidates)
			{
				if (filter.Filter(context, item))
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				return list;
			}
			if (filter.FallbackFilter.IsEmpty())
			{
				return null;
			}
			filter = context.Database.Fetch(filter.FallbackFilter);
			return this.FilterCandidates(context, candidates, filter);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0002A5F4 File Offset: 0x000287F4
		private GameItem CreateItemMatchingFilter(TurnProcessContext context, IReadOnlyList<GameItemStaticData> candidates, BazaarFilter filter)
		{
			List<GameItemStaticData> list = this.FilterCandidates(context, candidates, filter);
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			return BazaarState.CreateItem(context, list.GetRandom(context.Random));
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0002A62C File Offset: 0x0002882C
		public static GameItem CreateItem(TurnProcessContext context, GameItemStaticData itemStaticData)
		{
			GameItem result;
			if (!(itemStaticData is GamePieceStaticData))
			{
				if (!(itemStaticData is ArtifactStaticData))
				{
					if (!(itemStaticData is PraetorStaticData))
					{
						if (!(itemStaticData is ManuscriptStaticData))
						{
							throw new ArgumentOutOfRangeException();
						}
						result = BazaarState.CreateItem<Manuscript>(context, itemStaticData);
					}
					else
					{
						result = BazaarState.CreateItem<Praetor>(context, itemStaticData);
					}
				}
				else
				{
					result = BazaarState.CreateItem<Artifact>(context, itemStaticData);
				}
			}
			else
			{
				result = BazaarState.CreateItem<GamePiece>(context, itemStaticData);
			}
			return result;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002A68C File Offset: 0x0002888C
		private static TGameItem CreateItem<TGameItem>(TurnProcessContext context, GameItemStaticData itemStaticData) where TGameItem : GameItem, new()
		{
			TGameItem tgameItem = context.CurrentTurn.SpawnGameItem(itemStaticData, null, false);
			tgameItem.Status = GameItemStatus.Unavailable;
			GamePiece gamePiece = tgameItem as GamePiece;
			if (gamePiece == null)
			{
				return tgameItem;
			}
			gamePiece.Location = HexCoord.Invalid;
			gamePiece.ControllingPlayerId = -1;
			context.RecalculateSupportModifiers(gamePiece.ControllingPlayerId);
			return tgameItem;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0002A6E4 File Offset: 0x000288E4
		public bool RefillSlot(BazaarState.BazaarSlot slot, GameItemCategory category, TurnProcessContext context)
		{
			if (slot.FilterId.IsEmpty())
			{
				this.BazaarSlots[category].Remove(slot);
				return false;
			}
			GameItem gameItem;
			if (this.TryFill(context, slot, out gameItem))
			{
				return true;
			}
			List<ConfigRef> list = IEnumerableExtensions.ToList<ConfigRef>(from t in context.GetStockableGameItems<GameItemStaticData>()
			where t.GameItemCategory == category && t.CanBeShuffledBackIntoGame
			select t.ConfigRef);
			if (list.Count == 0)
			{
				return false;
			}
			CollectionExtensions.Remove<ConfigRef>(context.CurrentTurn.DeadItemReferences, list);
			return this.TryFill(context, slot, out gameItem);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002A79C File Offset: 0x0002899C
		private bool TryFill(TurnProcessContext context, BazaarState.BazaarSlot slot, out GameItem item)
		{
			List<GameItemStaticData> candidates = IEnumerableExtensions.ToList<GameItemStaticData>(context.GetPurchasableGameItems<GameItemStaticData>());
			BazaarFilter filter = context.Database.Fetch(slot.FilterId);
			item = this.CreateItemMatchingFilter(context, candidates, filter);
			GameItem gameItem = item;
			slot.CurrentItem = ((gameItem != null) ? gameItem.Id : Identifier.Invalid);
			return item != null;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0002A7EC File Offset: 0x000289EC
		public void DeepClone(out BazaarState clone)
		{
			clone = new BazaarState
			{
				BazaarSlots = new Dictionary<GameItemCategory, List<BazaarState.BazaarSlot>>(this.BazaarSlots.Count)
			};
			foreach (KeyValuePair<GameItemCategory, List<BazaarState.BazaarSlot>> keyValuePair in this.BazaarSlots)
			{
				GameItemCategory gameItemCategory;
				List<BazaarState.BazaarSlot> list;
				keyValuePair.Deconstruct(out gameItemCategory, out list);
				GameItemCategory key = gameItemCategory;
				List<BazaarState.BazaarSlot> originalList = list;
				clone.BazaarSlots.Add(key, originalList.DeepClone<BazaarState.BazaarSlot>());
			}
		}

		// Token: 0x04000452 RID: 1106
		[JsonProperty]
		public Dictionary<GameItemCategory, List<BazaarState.BazaarSlot>> BazaarSlots = new Dictionary<GameItemCategory, List<BazaarState.BazaarSlot>>();

		// Token: 0x02000889 RID: 2185
		[JsonObject(MemberSerialization.OptIn)]
		[Serializable]
		public class BazaarSlot : IDeepClone<BazaarState.BazaarSlot>
		{
			// Token: 0x06002864 RID: 10340 RVA: 0x000860C8 File Offset: 0x000842C8
			public void DeepClone(out BazaarState.BazaarSlot clone)
			{
				clone = new BazaarState.BazaarSlot
				{
					RequireEarlyAccess = this.RequireEarlyAccess,
					FilterId = this.FilterId.DeepClone<BazaarFilter>(),
					CurrentItem = this.CurrentItem
				};
			}

			// Token: 0x0400124F RID: 4687
			[JsonProperty]
			public bool RequireEarlyAccess;

			// Token: 0x04001250 RID: 4688
			[JsonProperty]
			public ConfigRef<BazaarFilter> FilterId;

			// Token: 0x04001251 RID: 4689
			[JsonProperty]
			[DefaultValue(Identifier.Invalid)]
			public Identifier CurrentItem = Identifier.Invalid;
		}
	}
}
