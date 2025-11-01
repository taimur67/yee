using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000215 RID: 533
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VoteProclamationEvent : VoteEvent
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x0002E310 File Offset: 0x0002C510
		[JsonConstructor]
		private VoteProclamationEvent()
		{
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0002E318 File Offset: 0x0002C518
		public VoteProclamationEvent(string edictId) : base(edictId)
		{
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0002E321 File Offset: 0x0002C521
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.VotingEdictProclaimed;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0002E328 File Offset: 0x0002C528
		public override string GetDebugName(TurnContext context)
		{
			return "The conclave has proclaimed that an edict must be voted on.";
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0002E330 File Offset: 0x0002C530
		public override void DeepClone(out GameEvent clone)
		{
			VoteProclamationEvent voteProclamationEvent = new VoteProclamationEvent
			{
				DeliberationTurns = this.DeliberationTurns
			};
			base.DeepCloneVoteEventParts(voteProclamationEvent);
			clone = voteProclamationEvent;
		}

		// Token: 0x040004D5 RID: 1237
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int DeliberationTurns;
	}
}
