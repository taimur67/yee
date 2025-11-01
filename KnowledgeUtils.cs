using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020002F0 RID: 752
	public static class KnowledgeUtils
	{
		// Token: 0x06000EB4 RID: 3764 RVA: 0x0003AA08 File Offset: 0x00038C08
		public static int GetLastRevealed(this PlayerKnowledgeContext knowledgeContext, GameItem item)
		{
			int lastRevealed;
			if (item is Relic)
			{
				lastRevealed = knowledgeContext.GetLastRevealed(KnowledgeCategory.Relics, PowerType.None);
			}
			else
			{
				lastRevealed = knowledgeContext.GetLastRevealed(KnowledgeCategory.Vault, PowerType.None);
			}
			return lastRevealed;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0003AA34 File Offset: 0x00038C34
		public static int GetLastRevealed(this PlayerKnowledgeContext knowledgeContext, KnowledgeCategory category, PowerType powerType = PowerType.None)
		{
			if (knowledgeContext == null)
			{
				return -1;
			}
			switch (category)
			{
			case KnowledgeCategory.Vault:
				return knowledgeContext.LastRevealedVault;
			case KnowledgeCategory.Rituals:
				return knowledgeContext.LastRevealedRituals;
			case KnowledgeCategory.Schemes:
				return knowledgeContext.LastRevealedSchemes;
			case KnowledgeCategory.Relics:
				return knowledgeContext.LastRevealedRelics;
			case KnowledgeCategory.Powers:
				if (powerType == PowerType.None)
				{
					return -1;
				}
				return knowledgeContext.LastRevealedPowers.GetValueOrDefault(powerType, -1);
			default:
				return -1;
			}
		}
	}
}
