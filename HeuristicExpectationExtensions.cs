using System;

namespace LoG
{
	// Token: 0x020006E4 RID: 1764
	public static class HeuristicExpectationExtensions
	{
		// Token: 0x0600216A RID: 8554 RVA: 0x000740C8 File Offset: 0x000722C8
		public static bool MeetsExpectation(this GameHeuristics heuristics, in HeuristicExpectation expectation)
		{
			float value = heuristics.GetValue(expectation);
			return value >= expectation.MinValue && value <= expectation.MaxValue;
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000740F4 File Offset: 0x000722F4
		public static float GetValue(this GameHeuristics heuristics, in HeuristicExpectation expectation)
		{
			return heuristics.GetValue(expectation.Key, expectation.ValueType);
		}
	}
}
