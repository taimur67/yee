using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F6 RID: 758
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerTargetedKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x06000EC7 RID: 3783 RVA: 0x0003AC6E File Offset: 0x00038E6E
		public PlayerTargetedKnowledgeModifier TargetPlayer(int playerId)
		{
			return this.TargetPlayers(BitMask.From(playerId));
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0003AC7C File Offset: 0x00038E7C
		public PlayerTargetedKnowledgeModifier TargetPlayers(BitMask mask)
		{
			this.TargetPlayerMask = mask;
			return this;
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0003AC88 File Offset: 0x00038E88
		public override void Process(TurnState view, in TurnState truth, int knowledgeOwnerId)
		{
			foreach (PlayerState playerState in view.EnumeratePlayerStates(false, false))
			{
				if (this.TargetPlayerMask.IsSet(playerState.Id))
				{
					this.Process(view, playerState);
				}
			}
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0003ACEC File Offset: 0x00038EEC
		public virtual void Process(TurnState view, PlayerState playerState)
		{
		}

		// Token: 0x040006C1 RID: 1729
		[JsonProperty]
		protected BitMask TargetPlayerMask = BitMask.All;
	}
}
