using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002FC RID: 764
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RitualsKnowledgeModifier : PlayerItemKnowledgeModifier
	{
		// Token: 0x06000EDB RID: 3803 RVA: 0x0003AEBE File Offset: 0x000390BE
		public RitualsKnowledgeModifier()
		{
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0003AEC6 File Offset: 0x000390C6
		public RitualsKnowledgeModifier(int revealedPlayerId, List<Identifier> rituals) : base(revealedPlayerId, rituals)
		{
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0003AED0 File Offset: 0x000390D0
		protected override void Expose(TurnState view, in TurnState truth, int knowledgeOwnerId, PlayerState revealedPlayer, Identifier item)
		{
			revealedPlayer.RitualState.SlottedItems.Add(item);
		}
	}
}
