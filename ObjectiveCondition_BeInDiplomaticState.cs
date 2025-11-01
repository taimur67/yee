using System;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200054E RID: 1358
	public class ObjectiveCondition_BeInDiplomaticState : BooleanStateObjectiveCondition
	{
		// Token: 0x06001A4C RID: 6732 RVA: 0x0005BB88 File Offset: 0x00059D88
		protected override bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return !isInitialProgress && context.Diplomacy.GetAllDiplomaticStatesOfPlayer(owner).Any((DiplomaticPairStatus t) => t.DiplomaticState.Type == this.State);
		}

		// Token: 0x04000BE5 RID: 3045
		[JsonProperty]
		public DiplomaticStateValue State;
	}
}
