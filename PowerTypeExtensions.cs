using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200045A RID: 1114
	public static class PowerTypeExtensions
	{
		// Token: 0x06001504 RID: 5380 RVA: 0x0004FB8C File Offset: 0x0004DD8C
		public static ArchfiendStat ToArchfiendStat(this PowerType power)
		{
			ArchfiendStat result;
			switch (power)
			{
			case PowerType.Wrath:
				result = ArchfiendStat.Wrath;
				break;
			case PowerType.Deceit:
				result = ArchfiendStat.Deceit;
				break;
			case PowerType.Prophecy:
				result = ArchfiendStat.Prophecy;
				break;
			case PowerType.Destruction:
				result = ArchfiendStat.Destruction;
				break;
			case PowerType.Charisma:
				result = ArchfiendStat.Charisma;
				break;
			default:
				result = ArchfiendStat.None;
				break;
			}
			return result;
		}

		// Token: 0x04000A84 RID: 2692
		public static readonly PowerType[] ValidTypes = IEnumerableExtensions.ToArray<PowerType>(IEnumerableExtensions.ExceptFor<PowerType>(EnumUtility.GetValues<PowerType>(), new PowerType[]
		{
			PowerType.None
		}));
	}
}
