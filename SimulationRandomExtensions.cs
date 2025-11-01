using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020006ED RID: 1773
	public static class SimulationRandomExtensions
	{
		// Token: 0x060021DA RID: 8666 RVA: 0x0007631A File Offset: 0x0007451A
		public static int NextIndex(this SimulationRandom random, int count)
		{
			if (count == 0)
			{
				return -1;
			}
			return random.Next() % count;
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x00076329 File Offset: 0x00074529
		public static int NextIndex(this SimulationRandom random, IList list)
		{
			return random.NextIndex(list.Count);
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x00076338 File Offset: 0x00074538
		public static T Random<T>(this IList<T> list, SimulationRandom random)
		{
			if (list.Count == 0)
			{
				return default(T);
			}
			return list[random.NextIndex(list.Count)];
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00076369 File Offset: 0x00074569
		public static IEnumerable<T> SelectRandom<T>(this IList<T> list, SimulationRandom random, int count)
		{
			return list.GetRandom(random, count);
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x00076374 File Offset: 0x00074574
		public static IEnumerable<T> SelectRandom<T>(this IList<T> list, SimulationRandom random, int min, int max)
		{
			max = Math.Clamp(max, 0, list.Count);
			int num = max - min;
			int count = min + random.Next() % (num + 1);
			return list.SelectRandom(random, count);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000763A9 File Offset: 0x000745A9
		public static bool TryGetRandom<T>(this IReadOnlyList<T> list, SimulationRandom random, out T value)
		{
			if (list.Count != 0)
			{
				value = list[random.NextIndex(list.Count)];
				return true;
			}
			value = default(T);
			return false;
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000763D5 File Offset: 0x000745D5
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, SimulationRandom random)
		{
			List<T> buffer = IEnumerableExtensions.ToList<T>(source);
			int num;
			for (int i = 0; i < buffer.Count; i = num + 1)
			{
				int j = random.Next(i, buffer.Count);
				yield return buffer[j];
				buffer[j] = buffer[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000763EC File Offset: 0x000745EC
		public static T PopRandom<T>(this IList<T> list, SimulationRandom random)
		{
			if (list.Count == 0)
			{
				return default(T);
			}
			return ListExtensions.PopAt<T>(list, random.NextIndex(list.Count));
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0007641D File Offset: 0x0007461D
		public static bool TryPopRandom<T>(this IList<T> list, SimulationRandom random, out T value)
		{
			value = list.PopRandom(random);
			return value != null;
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0007643C File Offset: 0x0007463C
		public static T WeightedRandom<T>(this IList<T> list, Func<T, float> weightSelector, SimulationRandom random, bool ignoreZeroAndBelow = false)
		{
			float num = list.Sum(weightSelector);
			float num2 = random.NextFloat() * num;
			float num3 = 0f;
			foreach (T t in list)
			{
				float num4 = weightSelector(t);
				if (!ignoreZeroAndBelow || num4 > 0f)
				{
					num3 += num4;
					if (num3 >= num2)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000764C8 File Offset: 0x000746C8
		public static float NextFloat(this SimulationRandom random)
		{
			return (float)random.NextDouble();
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x000764D1 File Offset: 0x000746D1
		public static float NextFloat(this SimulationRandom random, float min, float max)
		{
			return (float)(random.NextDouble() * (double)(max - min) + (double)min);
		}
	}
}
