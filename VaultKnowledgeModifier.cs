using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002FF RID: 767
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VaultKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x06000EE4 RID: 3812 RVA: 0x0003AF56 File Offset: 0x00039156
		public VaultKnowledgeModifier()
		{
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0003AF69 File Offset: 0x00039169
		public VaultKnowledgeModifier(int revealedPlayerId, List<Identifier> vaultItems)
		{
			this.VaultItems = vaultItems;
			this.RevealedPlayerId = revealedPlayerId;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0003AF8C File Offset: 0x0003918C
		public override void Process(TurnState view, in TurnState truth, int knowledgeOwnerId)
		{
			PlayerState playerState = view.FindPlayerState(this.RevealedPlayerId, null);
			foreach (Identifier identifier in this.VaultItems)
			{
				if (view.FindControllingPlayer(identifier) == null)
				{
					playerState.AddToVault(identifier);
				}
			}
		}

		// Token: 0x040006CC RID: 1740
		[JsonProperty]
		public List<Identifier> VaultItems = new List<Identifier>();

		// Token: 0x040006CD RID: 1741
		[JsonProperty]
		public int RevealedPlayerId;
	}
}
