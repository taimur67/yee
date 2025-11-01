using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020003FC RID: 1020
	public static class RelicTransactions
	{
		// Token: 0x06001447 RID: 5191 RVA: 0x0004D61D File Offset: 0x0004B81D
		public static bool IsValidRelicConfigRefs(this GameDatabase database, RelicSetStaticData relicLoadout, out List<ConfigRef> validSet)
		{
			return database.IsValidRelicConfigRefs((relicLoadout != null) ? relicLoadout.Relics : null, out validSet);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0004D634 File Offset: 0x0004B834
		public static bool IsValidRelicConfigRefs(this GameDatabase database, List<ConfigRef> relicLoadout, out List<ConfigRef> validSet)
		{
			validSet = new List<ConfigRef>();
			if (relicLoadout == null)
			{
				return false;
			}
			List<string> validList = database.ReturnMaxValueValidatedRelicIds(relicLoadout);
			bool flag = relicLoadout.All((ConfigRef x) => validList.Contains(x.Id));
			validSet = (flag ? relicLoadout : IEnumerableExtensions.ToList<ConfigRef>(from x in relicLoadout
			where validList.Contains(x.Id)
			select x));
			return flag;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0004D694 File Offset: 0x0004B894
		public static List<string> ReturnMaxValueValidatedRelicIds(this GameDatabase database, RelicSetStaticData relicLoadout)
		{
			List<string> relicLoadout2;
			if (relicLoadout == null)
			{
				relicLoadout2 = null;
			}
			else
			{
				List<ConfigRef> relics = relicLoadout.Relics;
				if (relics == null)
				{
					relicLoadout2 = null;
				}
				else
				{
					relicLoadout2 = IEnumerableExtensions.ToList<string>(from x in relics
					select x.Id);
				}
			}
			return database.ReturnMaxValueValidatedRelicIds(relicLoadout2);
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0004D6E3 File Offset: 0x0004B8E3
		public static List<string> ReturnMaxValueValidatedRelicIds(this GameDatabase database, List<ConfigRef> relicLoadout)
		{
			List<string> relicLoadout2;
			if (relicLoadout == null)
			{
				relicLoadout2 = null;
			}
			else
			{
				relicLoadout2 = IEnumerableExtensions.ToList<string>(from x in relicLoadout
				select x.Id);
			}
			return database.ReturnMaxValueValidatedRelicIds(relicLoadout2);
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0004D71C File Offset: 0x0004B91C
		public static List<string> ReturnMaxValueValidatedRelicIds(this GameDatabase database, List<string> relicLoadout)
		{
			if (relicLoadout == null)
			{
				return new List<string>();
			}
			int num = 0;
			int maxRelicsValue = database.FetchSingle<RelicsEconomyData>().MaxRelicsValue;
			List<string> list = new List<string>();
			foreach (string text in relicLoadout)
			{
				RelicStaticData relicStaticData;
				if (database.TryFetch<RelicStaticData>(text, out relicStaticData))
				{
					num += relicStaticData.RelicValue;
					if (num > maxRelicsValue)
					{
						num -= relicStaticData.RelicValue;
					}
					else
					{
						list.Add(text);
					}
				}
			}
			return list;
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0004D7B0 File Offset: 0x0004B9B0
		public static void AddRelicSet(this TurnContext context, PlayerState player, RelicSetStaticData set)
		{
			context.AddRelicList(player, set.Relics);
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0004D7C0 File Offset: 0x0004B9C0
		public static void AddRelicList(this TurnContext context, PlayerState player, IEnumerable<ConfigRef> relicList)
		{
			relicList = from x in relicList
			where x != null
			select x;
			foreach (ConfigRef configRef in relicList)
			{
				RelicStaticData relic;
				if (context.Database.TryFetch<RelicStaticData>(configRef.Id, out relic))
				{
					context.AddRelic(player, relic);
				}
			}
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0004D848 File Offset: 0x0004BA48
		public static void AddRelicList(this TurnContext context, PlayerState player, IEnumerable<RelicStaticData> relicList)
		{
			relicList = from x in relicList
			where x != null
			select x;
			foreach (RelicStaticData relic in relicList)
			{
				context.AddRelic(player, relic);
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0004D8B8 File Offset: 0x0004BAB8
		public static void AddRelic(this TurnContext context, PlayerState player, RelicStaticData relic)
		{
			Relic relic2 = context.CurrentTurn.SpawnRelic(relic, player);
			player.GiveRelic(relic2);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0004D8DC File Offset: 0x0004BADC
		public static void ClearRelics(this TurnContext context, PlayerState player)
		{
			foreach (Identifier id in IEnumerableExtensions.ToList<Identifier>(player.ActiveRelics))
			{
				context.RemoveRelic(player, id);
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0004D938 File Offset: 0x0004BB38
		public static void RemoveRelic(this TurnContext context, PlayerState player, Identifier id)
		{
			context.CurrentTurn.RemoveGameItem(id);
			player.RemoveRelic(id);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0004D94E File Offset: 0x0004BB4E
		public static IEnumerable<ValueTuple<Relic, RelicStaticData>> GetRelicDataPairs(this TurnState turn, GameDatabase db, PlayerState player)
		{
			foreach (Relic relic in turn.EnumerateGameItems<Relic>(player.ActiveRelics))
			{
				RelicStaticData item;
				if (db.TryFetch<RelicStaticData>(relic.StaticDataId, out item))
				{
					yield return new ValueTuple<Relic, RelicStaticData>(relic, item);
				}
			}
			IEnumerator<Relic> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0004D96C File Offset: 0x0004BB6C
		public static IEnumerable<ValueTuple<Relic, RelicStaticData>> GetRelicDataPairs(this TurnContext context, PlayerState player)
		{
			return context.CurrentTurn.GetRelicDataPairs(context.Database, player);
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0004D980 File Offset: 0x0004BB80
		public static int CalculateTotalRelicCost(this TurnContext context, PlayerState player)
		{
			int num = 0;
			foreach (ValueTuple<Relic, RelicStaticData> valueTuple in context.GetRelicDataPairs(player))
			{
				RelicStaticData item = valueTuple.Item2;
				num += item.RelicValue;
			}
			return num;
		}
	}
}
