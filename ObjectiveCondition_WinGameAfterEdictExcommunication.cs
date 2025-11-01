using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000590 RID: 1424
	[Serializable]
	public class ObjectiveCondition_WinGameAfterEdictExcommunication : ObjectiveCondition_WinGame
	{
		// Token: 0x06001B06 RID: 6918 RVA: 0x0005E527 File Offset: 0x0005C727
		public override bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			return (this.Reason == ExcommunicationReason.Unknown || owner.HasBeenExcommunicatedFor(this.Reason)) && base.CheckCompleteStatus(context, owner, victory);
		}

		// Token: 0x04000C46 RID: 3142
		[JsonProperty]
		public ExcommunicationReason Reason;
	}
}
