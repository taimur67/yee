using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000205 RID: 517
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public abstract class VoteEvent : GameEvent
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0002DB4F File Offset: 0x0002BD4F
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0002DB52 File Offset: 0x0002BD52
		protected VoteEvent()
		{
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0002DB5A File Offset: 0x0002BD5A
		protected VoteEvent(string edictId) : base(-1)
		{
			this.EdictId = edictId;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0002DB6A File Offset: 0x0002BD6A
		protected void DeepCloneVoteEventParts(VoteEvent voteEvent)
		{
			voteEvent.EdictId = this.EdictId.DeepClone();
			base.DeepCloneGameEventParts<VoteEvent>(voteEvent);
		}

		// Token: 0x06000A18 RID: 2584
		public abstract override void DeepClone(out GameEvent clone);

		// Token: 0x040004C3 RID: 1219
		[BindableValue("edict", BindingOption.StaticDataId)]
		[JsonProperty]
		public string EdictId;
	}
}
