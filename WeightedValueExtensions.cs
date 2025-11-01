using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x0200042D RID: 1069
	public static class WeightedValueExtensions
	{
		// Token: 0x060014C4 RID: 5316 RVA: 0x0004F3E8 File Offset: 0x0004D5E8
		public static T SelectRandom<T>(this IEnumerable<WeightedValue<T>> weights, SimulationRandom random)
		{
			return IEnumerableExtensions.ToList<WeightedValue<T>>(weights).SelectRandom(random);
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0004F3F8 File Offset: 0x0004D5F8
		public static T SelectRandom<T>(this List<WeightedValue<T>> weights, SimulationRandom random)
		{
			WeightedValue<T> weightedValue;
			if (weights.SelectRandom(random, out weightedValue))
			{
				return weightedValue.Value;
			}
			return default(T);
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0004F420 File Offset: 0x0004D620
		public static bool TrySelectRandom<T>(this IReadOnlyList<WeightedValue<T>> weights, Random random, out T value)
		{
			WeightedValue<T> weightedValue;
			if (weights.SelectRandom((float)random.NextDouble(), out weightedValue))
			{
				value = weightedValue.Value;
				return true;
			}
			value = default(T);
			return false;
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004F454 File Offset: 0x0004D654
		public static bool SelectRandom<T>(this IEnumerable<WeightedValue<T>> weights, SimulationRandom random, out WeightedValue<T> wv)
		{
			return IEnumerableExtensions.ToList<WeightedValue<T>>(weights).SelectRandom(random, out wv);
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004F463 File Offset: 0x0004D663
		public static bool SelectRandom<T>(this List<WeightedValue<T>> weights, SimulationRandom random, out WeightedValue<T> wv)
		{
			return weights.SelectRandom(random.NextFloat(), out wv);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004F474 File Offset: 0x0004D674
		private static bool SelectRandom<T>(this IReadOnlyList<WeightedValue<T>> weights, float randomInterval, out WeightedValue<T> wv)
		{
			wv = default(WeightedValue<T>);
			float num = IEnumerableExtensions.Accumulate<WeightedValue<T>>(weights, (WeightedValue<T> t) => t.Weight);
			num *= randomInterval;
			foreach (WeightedValue<T> weightedValue in weights)
			{
				num -= weightedValue.Weight;
				if (num <= 1E-45f)
				{
					wv = weightedValue;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0004F508 File Offset: 0x0004D708
		public static bool TryPopRandom<T>(this List<WeightedValue<T>> weights, SimulationRandom random, out T wv)
		{
			wv = default(T);
			float num = IEnumerableExtensions.Accumulate<WeightedValue<T>>(weights, (WeightedValue<T> t) => t.Weight);
			num *= random.NextFloat();
			for (int i = 0; i < weights.Count; i++)
			{
				WeightedValue<T> weightedValue = weights[i];
				num -= weightedValue.Weight;
				if (num <= 1E-45f)
				{
					wv = weightedValue.Value;
					weights.RemoveAt(i++);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0004F58F File Offset: 0x0004D78F
		public static IEnumerable<T> Values<T>(this List<WeightedValue<T>> weights)
		{
			return from t in weights
			select t.Value;
		}
	}
}
