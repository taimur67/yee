using System;

namespace LoG
{
	// Token: 0x020005B3 RID: 1459
	public class ObjectiveCondition_WinAsBloodLord : ObjectiveCondition_GameOver
	{
		// Token: 0x06001B51 RID: 6993 RVA: 0x0005ED94 File Offset: 0x0005CF94
		public override bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			int num;
			return owner.Id == victory.WinningPlayerID && context.Diplomacy.IsBloodLordOfAny(owner.Id, out num) && base.CheckCompleteStatus(context, owner, victory);
		}
	}
}
