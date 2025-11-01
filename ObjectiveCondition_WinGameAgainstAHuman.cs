using System;

namespace LoG
{
	// Token: 0x0200058F RID: 1423
	[Serializable]
	public class ObjectiveCondition_WinGameAgainstAHuman : ObjectiveCondition_WinGame
	{
		// Token: 0x06001B04 RID: 6916 RVA: 0x0005E50A File Offset: 0x0005C70A
		public override bool CheckCompleteStatus(TurnContext context, PlayerState owner, GameVictory victory)
		{
			return context.IsMultiplayer() && base.CheckCompleteStatus(context, owner, victory);
		}
	}
}
