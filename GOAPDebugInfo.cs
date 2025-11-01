using System;

namespace LoG
{
	// Token: 0x02000133 RID: 307
	[Serializable]
	public class GOAPDebugInfo
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x0001D81D File Offset: 0x0001BA1D
		public GOAPDebugInfo(string actionName, float g, float h, bool isGoal, GOAPNodeType nodeType)
		{
			this.ActionName = actionName;
			this.GScore = g;
			this.HScore = h;
			this.IsGoal = isGoal;
			this.NodeType = nodeType;
		}

		// Token: 0x040002D3 RID: 723
		public string ActionName;

		// Token: 0x040002D4 RID: 724
		public float GScore;

		// Token: 0x040002D5 RID: 725
		public float HScore;

		// Token: 0x040002D6 RID: 726
		public bool IsGoal;

		// Token: 0x040002D7 RID: 727
		public GOAPNodeType NodeType;
	}
}
