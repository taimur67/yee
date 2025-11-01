using System;

namespace LoG
{
	// Token: 0x0200057E RID: 1406
	public class ObjectiveCondition_Eliminated : BooleanStateObjectiveCondition
	{
		// Token: 0x06001ACD RID: 6861 RVA: 0x0005DA03 File Offset: 0x0005BC03
		protected override bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return owner.Eliminated;
		}
	}
}
