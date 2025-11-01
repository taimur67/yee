using System;

namespace LoG
{
	// Token: 0x020001C3 RID: 451
	public static class ComparableExtensions
	{
		// Token: 0x06000870 RID: 2160 RVA: 0x00027DE5 File Offset: 0x00025FE5
		public static bool TryCompareTo(this IComparable @this, object other, out int comparison)
		{
			comparison = @this.CompareTo(other);
			return comparison != 0;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00027DF5 File Offset: 0x00025FF5
		public static bool TryCompareTo<T>(this IComparable<T> @this, T other, out int comparison)
		{
			comparison = @this.CompareTo(other);
			return comparison != 0;
		}
	}
}
