using System;

namespace LoG
{
	// Token: 0x020001A7 RID: 423
	[Serializable]
	public class PFAgent
	{
		// Token: 0x060007DB RID: 2011 RVA: 0x00023EE4 File Offset: 0x000220E4
		public virtual float GenerateCostModifierForNode(Pathfinder pathfinder, PFNode node)
		{
			return 0f;
		}
	}
}
