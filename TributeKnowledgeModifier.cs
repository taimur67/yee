using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002FE RID: 766
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TributeKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x06000EE1 RID: 3809 RVA: 0x0003AF03 File Offset: 0x00039103
		public TributeKnowledgeModifier()
		{
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0003AF16 File Offset: 0x00039116
		public TributeKnowledgeModifier(int revealedPlayerId, List<ResourceNFT> tributeResources)
		{
			this.TributeResources = tributeResources;
			this.RevealedPlayerId = revealedPlayerId;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0003AF37 File Offset: 0x00039137
		public override void Process(TurnState view, in TurnState truth, int knowledgeOwnerId)
		{
			view.FindPlayerState(this.RevealedPlayerId, null).Resources = IEnumerableExtensions.ToList<ResourceNFT>(this.TributeResources);
		}

		// Token: 0x040006CA RID: 1738
		[JsonProperty]
		public List<ResourceNFT> TributeResources = new List<ResourceNFT>();

		// Token: 0x040006CB RID: 1739
		[JsonProperty]
		public int RevealedPlayerId;
	}
}
