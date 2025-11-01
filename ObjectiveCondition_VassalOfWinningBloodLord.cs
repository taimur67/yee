using System;

namespace LoG
{
	// Token: 0x020005AE RID: 1454
	public class ObjectiveCondition_VassalOfWinningBloodLord : ObjectiveCondition_GameOver
	{
		// Token: 0x06001B46 RID: 6982 RVA: 0x0005EC70 File Offset: 0x0005CE70
		public override bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			int num;
			return context.Diplomacy.IsVassalOfAny(owner.Id, out num) && victory.WinningPlayerID == num && base.CheckCompleteStatus(context, owner, victory);
		}
	}
}
