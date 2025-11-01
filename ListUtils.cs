using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020006E8 RID: 1768
	public static class ListUtils
	{
		// Token: 0x060021A4 RID: 8612 RVA: 0x000755C8 File Offset: 0x000737C8
		public static T GetRandom<T>(this IEnumerable<T> list, SimulationRandom rand)
		{
			return list.ElementAt(rand.Next(0, list.Count<T>()));
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000755E0 File Offset: 0x000737E0
		public static T GetRandomOrDefault<T>(this IEnumerable<T> list, SimulationRandom rand)
		{
			if (!IEnumerableExtensions.Any<T>(list))
			{
				return default(T);
			}
			return list.GetRandom(rand);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00075608 File Offset: 0x00073808
		public static T GetRandom<T>(this List<T> list, Random rand = null)
		{
			if (rand == null)
			{
				Random random;
				if ((random = ListUtils._rand) == null)
				{
					random = (ListUtils._rand = new Random(Guid.NewGuid().GetHashCode()));
				}
				rand = random;
			}
			return list[rand.Next(0, list.Count)];
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x00075654 File Offset: 0x00073854
		public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> enumerable, SimulationRandom rand, int count)
		{
			return IEnumerableExtensions.ToList<T>(enumerable).GetRandom(rand, count);
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00075663 File Offset: 0x00073863
		public static IEnumerable<T> GetRandom<T>(this IList<T> list, SimulationRandom rand, int count)
		{
			List<T> buffer = IEnumerableExtensions.ToList<T>(list);
			int num;
			for (int i = 0; i < Math.Min(count, buffer.Count); i = num + 1)
			{
				int j = rand.Next(i, buffer.Count);
				yield return buffer[j];
				buffer[j] = buffer[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x00075684 File Offset: 0x00073884
		public static T GetRandom<T>(this IList<T> list, SimulationRandom rand)
		{
			if (list.Count == 0)
			{
				return default(T);
			}
			return list[rand.Next() % list.Count];
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000756B6 File Offset: 0x000738B6
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.Shuffle(new Random());
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000756C3 File Offset: 0x000738C3
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (rng == null)
			{
				throw new ArgumentNullException("rng");
			}
			return source.ShuffleIterator(rng);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000756E8 File Offset: 0x000738E8
		private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
		{
			List<T> buffer = IEnumerableExtensions.ToList<T>(source);
			int num;
			for (int i = 0; i < buffer.Count; i = num + 1)
			{
				int j = rng.Next(i, buffer.Count);
				yield return buffer[j];
				buffer[j] = buffer[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00075700 File Offset: 0x00073900
		public static List<T> GetWrappingRangeByOffset<T>(this List<T> list, int listOffset, int count, out bool isWrapping)
		{
			isWrapping = false;
			if (list.Count == 0)
			{
				return null;
			}
			int index = (list.Count + listOffset) % list.Count;
			return list.GetWrappingRange(index, count, out isWrapping);
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x00075734 File Offset: 0x00073934
		public static List<T> GetWrappingRange<T>(this List<T> list, int index, int count, out bool isWrapping)
		{
			isWrapping = false;
			if (list.Count == 0)
			{
				return null;
			}
			int num = Math.Min(count, list.Count);
			int num2 = (index + num) % list.Count;
			isWrapping = ((float)(index + num) / (float)list.Count > 1f);
			List<T> list2 = new List<T>();
			if (isWrapping)
			{
				list2.AddRange(list.GetRange(index, num - num2));
				list2.AddRange(list.GetRange(0, num2));
			}
			else
			{
				list2.AddRange(list.GetRange(index, num));
			}
			return list2;
		}

		// Token: 0x04000EFD RID: 3837
		private static Random _rand;
	}
}
