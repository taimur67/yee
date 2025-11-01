using System;

namespace LoG
{
	// Token: 0x020001A8 RID: 424
	public interface IPathDestinationChecker<TNode> where TNode : PFNode
	{
		// Token: 0x060007DD RID: 2013
		bool IsDestination(TNode node);
	}
}
