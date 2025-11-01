using System;

namespace LoG
{
	// Token: 0x02000137 RID: 311
	[Serializable]
	public class GoalRelevance
	{
		// Token: 0x06000637 RID: 1591 RVA: 0x0001F0B4 File Offset: 0x0001D2B4
		public GoalRelevance(float relevance, float pathCost, float finalScore)
		{
			this.Relevance = relevance;
			this.PathCost = pathCost;
			this.FinalScore = finalScore;
		}

		// Token: 0x040002F2 RID: 754
		public float Relevance;

		// Token: 0x040002F3 RID: 755
		public float PathCost;

		// Token: 0x040002F4 RID: 756
		public float FinalScore;
	}
}
