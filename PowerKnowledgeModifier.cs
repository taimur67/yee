using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002FA RID: 762
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PowerKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x06000ED5 RID: 3797 RVA: 0x0003AE38 File Offset: 0x00039038
		public PowerKnowledgeModifier()
		{
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0003AE4D File Offset: 0x0003904D
		public PowerKnowledgeModifier(int revealedPlayerId, PowerType powerType, PlayerPowerLevel powerLevel)
		{
			this.PowerType = powerType;
			this.PowerLevel = powerLevel;
			this.RevealedPlayerId = revealedPlayerId;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0003AE77 File Offset: 0x00039077
		public override void Process(TurnState view, in TurnState truth, int knowledgeOwnerId)
		{
			view.FindPlayerState(this.RevealedPlayerId, null).PowersLevels[this.PowerType] = this.PowerLevel.DeepClone<PlayerPowerLevel>();
		}

		// Token: 0x040006C7 RID: 1735
		[JsonProperty]
		public PowerType PowerType;

		// Token: 0x040006C8 RID: 1736
		[JsonProperty]
		public PlayerPowerLevel PowerLevel = new PlayerPowerLevel(0, 6);

		// Token: 0x040006C9 RID: 1737
		[JsonProperty]
		public int RevealedPlayerId;
	}
}
