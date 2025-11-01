using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020005C1 RID: 1473
	public static class AbilityOrderExtensions
	{
		// Token: 0x06001B7C RID: 7036 RVA: 0x0005F69A File Offset: 0x0005D89A
		public static IEnumerable<IUnlockProvider> UnlockProviders(this GameDatabase database, TurnState turn, PlayerState player)
		{
			foreach (IUnlockProvider unlockProvider in database.GetAllControlledStaticDataReferences(turn, player).OfType<IUnlockProvider>())
			{
				yield return unlockProvider;
			}
			IEnumerator<IUnlockProvider> enumerator = null;
			foreach (ArchfiendRankStaticData archfiendRankStaticData in database.GetArchfiendRanks(player.RankValue))
			{
				yield return archfiendRankStaticData;
			}
			IEnumerator<ArchfiendRankStaticData> enumerator2 = null;
			ArchFiendStaticData archFiendStaticData = database.Fetch<ArchFiendStaticData>(player.ArchfiendId);
			if (archFiendStaticData != null)
			{
				yield return archFiendStaticData;
			}
			foreach (PowerBaseStaticData powerBaseStaticData in database.GetUnlockedAndChosenPowers(player))
			{
				yield return powerBaseStaticData;
			}
			IEnumerator<PowerBaseStaticData> enumerator3 = null;
			yield break;
			yield break;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x0005F6B8 File Offset: 0x0005D8B8
		public static IEnumerable<string> GetUnlockedAbilityIds(this GameDatabase database, TurnState turn, PlayerState player)
		{
			return from x in database.UnlockProviders(turn, player).SelectMany((IUnlockProvider t) => t.Unlocks)
			select x.Id;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0005F715 File Offset: 0x0005D915
		public static bool IsUnlockedForPlayer<T>(this GameDatabase database, TurnState turn, PlayerState player, T data) where T : StaticDataEntity
		{
			return database.IsUnlockedForPlayer(database.UnlockProviders(turn, player), data);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0005F726 File Offset: 0x0005D926
		public static bool IsUnlockedForPlayer<T>(this GameDatabase database, IEnumerable<IUnlockProvider> playerProviders, T data) where T : StaticDataEntity
		{
			T t = data;
			return database.IsUnlockedForPlayer(playerProviders, (t != null) ? t.ConfigRef : null);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0005F744 File Offset: 0x0005D944
		public static bool IsUnlockedForPlayer(this GameDatabase database, IEnumerable<IUnlockProvider> playerProviders, ConfigRef cref)
		{
			HashSet<IUnlockProvider> potentialProviders = database.GetUnlockProvidersForAbility(cref).ToHashSet<IUnlockProvider>();
			return potentialProviders.Count == 0 || playerProviders.Any((IUnlockProvider t) => potentialProviders.Contains(t));
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0005F78C File Offset: 0x0005D98C
		public static IEnumerable<T> GetUnlockedAbilities<T>(this GameDatabase database, TurnState turn, PlayerState player, bool includePreviousLevels = false) where T : StaticDataEntity
		{
			List<T> list = new List<T>();
			foreach (ConfigRef<AbilityStaticData> configRef in database.UnlockProviders(turn, player).SelectMany((IUnlockProvider t) => t.Unlocks).ExcludeNull<ConfigRef<AbilityStaticData>>().Distinct<ConfigRef<AbilityStaticData>>())
			{
				ScalableAbility scalableAbility;
				T t2;
				if (database.TryFetch<ScalableAbility>(configRef.Id, out scalableAbility))
				{
					if (includePreviousLevels)
					{
						int num = player.Get(scalableAbility.Stat);
						for (int i = 0; i <= num; i++)
						{
							T level = scalableAbility.GetLevel(database, i);
							if (level != null)
							{
								list.Add(level);
							}
						}
					}
					else
					{
						T level2 = scalableAbility.GetLevel(database, player);
						if (level2 != null)
						{
							list.Add(level2);
						}
					}
				}
				else if (database.TryFetch<T>(configRef.Id, out t2))
				{
					T level3 = t2.GetLevel(database, player);
					if (level3 != null)
					{
						list.Add(level3);
					}
				}
			}
			return list;
		}
	}
}
