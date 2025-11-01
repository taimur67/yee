using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002FB RID: 763
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RelicsKnowledgeModifier : PlayerItemKnowledgeModifier
	{
		// Token: 0x06000ED8 RID: 3800 RVA: 0x0003AEA1 File Offset: 0x000390A1
		[JsonConstructor]
		public RelicsKnowledgeModifier()
		{
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0003AEA9 File Offset: 0x000390A9
		public RelicsKnowledgeModifier(int revealedPlayerId, List<Identifier> relics) : base(revealedPlayerId, relics)
		{
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0003AEB3 File Offset: 0x000390B3
		protected override void Expose(TurnState view, in TurnState truth, int knowledgeOwnerId, PlayerState revealedPlayer, Identifier item)
		{
			revealedPlayer.GiveRelic(item);
		}
	}
}
