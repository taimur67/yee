using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F2 RID: 754
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class PlayerItemKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x06000EBB RID: 3771 RVA: 0x0003AAD1 File Offset: 0x00038CD1
		[JsonConstructor]
		protected PlayerItemKnowledgeModifier()
		{
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0003AAEF File Offset: 0x00038CEF
		protected PlayerItemKnowledgeModifier(int revealedPlayerId, List<Identifier> items)
		{
			this.RevealedPlayerId = revealedPlayerId;
			this.Items = items;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0003AB1C File Offset: 0x00038D1C
		public override void Process(TurnState view, in TurnState truth, int knowledgeOwnerId)
		{
			PlayerState revealedPlayer = view.FindPlayerState(this.RevealedPlayerId, null);
			foreach (Identifier item in IEnumerableExtensions.ToList<Identifier>(this.Items))
			{
				this.Expose(view, truth, knowledgeOwnerId, revealedPlayer, item);
			}
		}

		// Token: 0x06000EBE RID: 3774
		protected abstract void Expose(TurnState view, in TurnState truth, int knowledgeOwnerId, PlayerState revealedPlayer, Identifier item);

		// Token: 0x040006BC RID: 1724
		[DefaultValue(-2147483648)]
		[JsonProperty]
		public int RevealedPlayerId = int.MinValue;

		// Token: 0x040006BD RID: 1725
		[JsonProperty]
		public List<Identifier> Items = new List<Identifier>();
	}
}
