using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005A1 RID: 1441
	[Serializable]
	public class ObjectiveCondition_Reveal : ObjectiveCondition_EventFilter<InformationRevealedEvent>
	{
		// Token: 0x06001B28 RID: 6952 RVA: 0x0005E90B File Offset: 0x0005CB0B
		[JsonConstructor]
		public ObjectiveCondition_Reveal()
		{
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0005E913 File Offset: 0x0005CB13
		public ObjectiveCondition_Reveal(int playerId)
		{
			base.TargetingPlayer = new int?(playerId);
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0005E927 File Offset: 0x0005CB27
		protected override bool Filter(TurnContext context, InformationRevealedEvent revealEvent, PlayerState owner, PlayerState target)
		{
			return (revealEvent.Revealed.Flags & this.Reveal) != (RevealedDataFlags)0 && base.Filter(context, revealEvent, owner, target);
		}

		// Token: 0x04000C4D RID: 3149
		[JsonProperty]
		public RevealedDataFlags Reveal;
	}
}
