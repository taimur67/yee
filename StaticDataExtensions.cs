using System;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000465 RID: 1125
	public static class StaticDataExtensions
	{
		// Token: 0x0600150F RID: 5391 RVA: 0x0004FC48 File Offset: 0x0004DE48
		public static T GetLevel<T>(this StaticDataEntity entity, GameDatabase database, int level) where T : StaticDataEntity
		{
			ScalableAbility scalableAbility = entity as ScalableAbility;
			if (scalableAbility == null)
			{
				return (T)((object)entity);
			}
			ConfigRef<StaticDataEntity> ability = scalableAbility.Levels.SelectClosestUnderOrEqual((LevelValue x) => x.Level, level).Ability;
			return database.Fetch(ability) as T;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0004FCA8 File Offset: 0x0004DEA8
		public static T GetLevel<T>(this StaticDataEntity entity, GameDatabase database, PlayerState player) where T : StaticDataEntity
		{
			ScalableAbility scalableAbility = entity as ScalableAbility;
			if (scalableAbility == null)
			{
				return (T)((object)entity);
			}
			ModifiableValue val = player.Get(scalableAbility.Stat);
			return entity.GetLevel(database, val);
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0004FCE0 File Offset: 0x0004DEE0
		public static bool IsLevelable(this StaticDataEntity entity)
		{
			ScalableAbility scalableAbility = entity as ScalableAbility;
			return scalableAbility != null && scalableAbility.Levels.Count > 0;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0004FD07 File Offset: 0x0004DF07
		public static bool TryGetComponent<T>(this StaticDataEntity entity, out T component) where T : IStaticData
		{
			component = default(T);
			if (entity == null)
			{
				return false;
			}
			component = IEnumerableExtensions.FirstOrDefault<T>(entity.Components.OfType<T>());
			return component != null;
		}
	}
}
