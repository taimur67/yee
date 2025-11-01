using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020006DE RID: 1758
	public static class BitMaskUtils
	{
		// Token: 0x06002132 RID: 8498 RVA: 0x00073545 File Offset: 0x00071745
		public static int Set(int index)
		{
			return 1 << index;
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x0007354D File Offset: 0x0007174D
		public static int Set(params int[] indices)
		{
			return BitMaskUtils.Set(indices.AsEnumerable<int>());
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x0007355C File Offset: 0x0007175C
		public static int Set(IEnumerable<int> indices)
		{
			int num = 0;
			foreach (int index in indices)
			{
				num |= BitMaskUtils.Set(index);
			}
			return num;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000735AC File Offset: 0x000717AC
		public static int AllBut(int index)
		{
			return ~BitMaskUtils.Set(index);
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000735B5 File Offset: 0x000717B5
		public static int AllBut(params int[] indices)
		{
			return ~BitMaskUtils.Set(indices);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000735BE File Offset: 0x000717BE
		public static int AllBut(IEnumerable<int> indices)
		{
			return ~BitMaskUtils.Set(indices);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000735C7 File Offset: 0x000717C7
		public static bool IsSet(int value, int index)
		{
			return (1 << index & value) != 0;
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000735D4 File Offset: 0x000717D4
		public static bool NotSet(int value, int index)
		{
			return !BitMaskUtils.IsSet(value, index);
		}
	}
}
