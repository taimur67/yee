using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020006EA RID: 1770
	public static class RelicUtils
	{
		// Token: 0x06003015 RID: 12309 RVA: 0x000C5ABB File Offset: 0x000C3CBB
		public static int SlotValue(this RelicType relicType)
		{
			switch (relicType)
			{
			case RelicType.Ring:
				return 1;
			case RelicType.Amulet:
				return 2;
			case RelicType.Crown:
				return 3;
			default:
				return 0;
			}
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000C5ADA File Offset: 0x000C3CDA
		public static List<Pair<RelicStaticData, GameItemClientDataComponent>> GetRelicSetInfo(this RelicSetStaticData relicSet, ClientDataAccessor dataAccessor, GameDatabase gameDatabase)
		{
			return RelicUtils.GetRelicSetInfo(relicSet.Relics, dataAccessor, gameDatabase);
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000C5AEC File Offset: 0x000C3CEC
		public static List<Pair<RelicStaticData, GameItemClientDataComponent>> GetRelicSetInfo(List<ConfigRef> relicSet, ClientDataAccessor dataAccessor, GameDatabase gameDatabase)
		{
			List<Pair<RelicStaticData, GameItemClientDataComponent>> list = new List<Pair<RelicStaticData, GameItemClientDataComponent>>();
			foreach (ConfigRef relicConfig in relicSet)
			{
				list.Add(relicConfig.GetRelicInfo(dataAccessor, gameDatabase));
			}
			return list;
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000C5B48 File Offset: 0x000C3D48
		public static Pair<RelicStaticData, GameItemClientDataComponent> GetRelicInfo(this RelicStaticData relicStaticData, ClientDataAccessor dataAccessor)
		{
			GameItemClientDataComponent gameItemClientDataComponent = dataAccessor.GameItemClientDataComponent(relicStaticData.Id);
			return new Pair<RelicStaticData, GameItemClientDataComponent>(relicStaticData, gameItemClientDataComponent);
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x000C5B6C File Offset: 0x000C3D6C
		public static Pair<RelicStaticData, GameItemClientDataComponent> GetRelicInfo(this ConfigRef relicConfig, ClientDataAccessor dataAccessor, GameDatabase gameDatabase)
		{
			RelicStaticData relicStaticData = gameDatabase.Fetch<RelicStaticData>(relicConfig.Id);
			GameItemClientDataComponent gameItemClientDataComponent = dataAccessor.GameItemClientDataComponent(relicStaticData.Id);
			return new Pair<RelicStaticData, GameItemClientDataComponent>(relicStaticData, gameItemClientDataComponent);
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x000C5B9A File Offset: 0x000C3D9A
		public static IEnumerable<RelicStaticData> GetAllRelics(this GameDatabase gameDatabase)
		{
			return gameDatabase.Enumerate<RelicStaticData>();
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x000C5BA2 File Offset: 0x000C3DA2
		public static IEnumerable<RelicStaticData> GetAllRings(this GameDatabase gameDatabase)
		{
			return from x in gameDatabase.GetAllRelics()
			where x.Type == RelicType.Ring
			select x;
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x000C5BCE File Offset: 0x000C3DCE
		public static IEnumerable<RelicStaticData> GetAllAmulets(this GameDatabase gameDatabase)
		{
			return from x in gameDatabase.GetAllRelics()
			where x.Type == RelicType.Amulet
			select x;
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x000C5BFA File Offset: 0x000C3DFA
		public static IEnumerable<RelicStaticData> GetAllCrowns(this GameDatabase gameDatabase)
		{
			return from x in gameDatabase.GetAllRelics()
			where x.Type == RelicType.Crown
			select x;
		}
	}
}
