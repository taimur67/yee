using System;

namespace LoG
{
	// Token: 0x0200036E RID: 878
	public interface IRevealsKnowledge
	{
		// Token: 0x060010AF RID: 4271
		KnowledgeModifier GenerateKnowledgeModifier(TurnState view, PlayerState player, PlayerState targetPlayer);
	}
}
