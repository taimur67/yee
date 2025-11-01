using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002FD RID: 765
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SchemesKnowledgeModifier : PlayerItemKnowledgeModifier
	{
		// Token: 0x06000EDE RID: 3806 RVA: 0x0003AEE5 File Offset: 0x000390E5
		[JsonConstructor]
		public SchemesKnowledgeModifier()
		{
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0003AEED File Offset: 0x000390ED
		public SchemesKnowledgeModifier(int revealedPlayerId, List<Identifier> schemeCards) : base(revealedPlayerId, schemeCards)
		{
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0003AEF7 File Offset: 0x000390F7
		protected override void Expose(TurnState view, in TurnState truth, int knowledgeOwnerId, PlayerState revealedPlayer, Identifier item)
		{
			revealedPlayer.AddSchemeCard(item);
		}
	}
}
