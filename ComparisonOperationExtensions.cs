using System;

namespace LoG
{
	// Token: 0x020001E0 RID: 480
	public static class ComparisonOperationExtensions
	{
		// Token: 0x0600096F RID: 2415 RVA: 0x0002C8D4 File Offset: 0x0002AAD4
		public static bool Process(this ComparisonOperation operation, int target, int value)
		{
			bool result;
			switch (operation)
			{
			case ComparisonOperation.EqualTo:
				result = (target == value);
				break;
			case ComparisonOperation.LessThan:
				result = (target < value);
				break;
			case ComparisonOperation.GreaterThan:
				result = (target > value);
				break;
			case ComparisonOperation.LessThanOrEqualTo:
				result = (target <= value);
				break;
			case ComparisonOperation.GreaterThanOrEqualTo:
				result = (target >= value);
				break;
			case ComparisonOperation.NotEqualTo:
				result = (target != value);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}
	}
}
