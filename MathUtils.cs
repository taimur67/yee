using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020006E9 RID: 1769
	public static class MathUtils
	{
		// Token: 0x060021AF RID: 8623 RVA: 0x000757B4 File Offset: 0x000739B4
		public static float RoundToNearest(float val, float nearest)
		{
			if (nearest == 0f)
			{
				return val;
			}
			return (float)Math.Round((double)(val / nearest)) * nearest;
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000757CC File Offset: 0x000739CC
		public static bool IsMagnitudinallyApproximate(float val, float other, float precision = 1E-45f)
		{
			float num = MathF.Abs(val - other);
			float y = MathF.Abs(val);
			float num2 = MathF.Max(MathF.Abs(other), y);
			return num <= num2 * precision;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x00075800 File Offset: 0x00073A00
		public static float LerpTo01(this float input, float amount)
		{
			amount = Math.Clamp(amount, -1f, 1f);
			float num;
			if (amount > 0f)
			{
				num = 1f - input;
			}
			else
			{
				num = input;
			}
			input += num * amount;
			input = Math.Clamp(input, 0f, 1f);
			return input;
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00075854 File Offset: 0x00073A54
		public static IEnumerable<IEnumerable<T>> GetPermutationsWithRepeats<T>(IEnumerable<T> list, int length)
		{
			if (length < 1)
			{
				return Enumerable.Empty<IEnumerable<T>>();
			}
			if (length == 1)
			{
				return from t in list
				select new T[]
				{
					t
				};
			}
			return MathUtils.GetPermutationsWithRepeats<T>(list, length - 1).SelectMany((IEnumerable<T> t) => list, (IEnumerable<T> t1, T t2) => t1.Concat(new T[]
			{
				t2
			}));
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000758E8 File Offset: 0x00073AE8
		public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
		{
			if (length < 1)
			{
				return Enumerable.Empty<IEnumerable<T>>();
			}
			if (length == 1)
			{
				return from t in list
				select new T[]
				{
					t
				};
			}
			return MathUtils.GetPermutations<T>(list, length - 1).SelectMany((IEnumerable<T> t) => from o in list
			where !IEnumerableExtensions.Contains<T>(t, o)
			select o, (IEnumerable<T> t1, T t2) => t1.Concat(new T[]
			{
				t2
			}));
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x0007597C File Offset: 0x00073B7C
		public static IEnumerable<IEnumerable<T>> GetCombinationsWithRepeats<T>(IEnumerable<T> list, int length) where T : IComparable
		{
			if (length < 1)
			{
				return Enumerable.Empty<IEnumerable<T>>();
			}
			if (length == 1)
			{
				return from t in list
				select new T[]
				{
					t
				};
			}
			return MathUtils.GetCombinationsWithRepeats<T>(list, length - 1).SelectMany((IEnumerable<T> t) => from o in list
			where o.CompareTo(t.Last<T>()) >= 0
			select o, (IEnumerable<T> t1, T t2) => t1.Concat(new T[]
			{
				t2
			}));
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x00075A10 File Offset: 0x00073C10
		public static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> list, int length) where T : IComparable
		{
			if (length < 1)
			{
				return Enumerable.Empty<IEnumerable<T>>();
			}
			if (length == 1)
			{
				return from t in list
				select new T[]
				{
					t
				};
			}
			return MathUtils.GetCombinations<T>(list, length - 1).SelectMany((IEnumerable<T> t) => from o in list
			where o.CompareTo(t.Last<T>()) > 0
			select o, (IEnumerable<T> t1, T t2) => t1.Concat(new T[]
			{
				t2
			}));
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00075AA1 File Offset: 0x00073CA1
		public static float Percentile(int numValuesBelowScore, int numScores)
		{
			if (numScores == 0)
			{
				return 0f;
			}
			return (float)numValuesBelowScore / (float)numScores * 100f;
		}

		// Token: 0x04000EFE RID: 3838
		public const float Rad2Deg = 57.29578f;
	}
}
