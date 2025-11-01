using System;

namespace LoG
{
	// Token: 0x02000420 RID: 1056
	public static class EvaluationExtensions
	{
		// Token: 0x060014B6 RID: 5302 RVA: 0x0004F342 File Offset: 0x0004D542
		public static bool IsBetter(this Evaluation eval, int value, int other)
		{
			if (eval != Evaluation.Highest)
			{
				return value < other;
			}
			return value > other;
		}
	}
}
