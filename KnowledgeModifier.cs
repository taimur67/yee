using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F1 RID: 753
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class KnowledgeModifier
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0003AA93 File Offset: 0x00038C93
		public virtual int Priority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0003AA96 File Offset: 0x00038C96
		public KnowledgeModifier()
		{
			this.Guid = Guid.NewGuid();
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0003AAB7 File Offset: 0x00038CB7
		public PlayerKnowledgeContext GetKnowledgeContext(TurnState view, int knowledgeOwnerId, int targetPlayerId)
		{
			return view.FindPlayerState(knowledgeOwnerId, null).GetOrCreateKnowledgeContext(targetPlayerId);
		}

		// Token: 0x06000EB9 RID: 3769
		public abstract void Process(TurnState playerView, in TurnState truth, int knowledgeOwnerId);

		// Token: 0x06000EBA RID: 3770 RVA: 0x0003AAC7 File Offset: 0x00038CC7
		public KnowledgeModifier SetExpiry(int turn)
		{
			this.ExpiryTurn = turn;
			return this;
		}

		// Token: 0x040006B9 RID: 1721
		[JsonProperty]
		[DefaultValue(-1)]
		public int ExpiryTurn = -1;

		// Token: 0x040006BA RID: 1722
		[JsonProperty]
		[DefaultValue(-1)]
		public int StartTurn = -1;

		// Token: 0x040006BB RID: 1723
		[JsonProperty]
		public Guid Guid;
	}
}
