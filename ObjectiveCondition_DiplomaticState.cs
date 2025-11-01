using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200057B RID: 1403
	public class ObjectiveCondition_DiplomaticState : ObjectiveCondition_DiplomaticFilter
	{
		// Token: 0x06001AC7 RID: 6855 RVA: 0x0005D84E File Offset: 0x0005BA4E
		public override bool CheckCompleteStatus(TurnState state, DiplomaticPairStatus status)
		{
			return status.DiplomaticState.Type == this.Value;
		}

		// Token: 0x04000C29 RID: 3113
		[JsonProperty]
		public DiplomaticStateValue Value;
	}
}
