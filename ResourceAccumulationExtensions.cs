using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000482 RID: 1154
	public static class ResourceAccumulationExtensions
	{
		// Token: 0x060015A3 RID: 5539 RVA: 0x0005176B File Offset: 0x0004F96B
		public static IEnumerable<ResourceAccumulation> EnumerateAccumulations(this IEnumerable<ResourceNFT> nfts)
		{
			foreach (ResourceNFT resourceNFT in nfts)
			{
				yield return resourceNFT.Values;
			}
			IEnumerator<ResourceNFT> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0005177B File Offset: 0x0004F97B
		public static ResourceAccumulation Total(this IEnumerable<ResourceNFT> nfts)
		{
			return nfts.EnumerateAccumulations().Total();
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00051788 File Offset: 0x0004F988
		public static ResourceAccumulation Total(this IEnumerable<ResourceAccumulation> enumerable)
		{
			return new ResourceAccumulation(IEnumerableExtensions.ToArray<ResourceAccumulation>(enumerable));
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00051795 File Offset: 0x0004F995
		public static int CompareTo(this ResourceNFT a, ResourceNFT b, ResourceTypes startingType)
		{
			return a.Values.CompareTo(b.Values, startingType);
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x000517AC File Offset: 0x0004F9AC
		public static int CompareTo(this ResourceAccumulation a, ResourceAccumulation b, ResourceTypes startingType)
		{
			int[] array = new int[]
			{
				a._souls,
				a._ichor,
				a._hellfire,
				a._darkness,
				a._prestige
			};
			int[] array2 = new int[]
			{
				b._souls,
				b._ichor,
				b._hellfire,
				b._darkness,
				b._prestige
			};
			for (int i = 0; i < array.Length; i++)
			{
				int num = (int)((i + startingType) % (ResourceTypes)array.Length);
				int num2 = array[num];
				int num3 = array2[num];
				if (num2 > num3)
				{
					return -1;
				}
				if (num2 < num3)
				{
					return 1;
				}
			}
			return 0;
		}
	}
}
