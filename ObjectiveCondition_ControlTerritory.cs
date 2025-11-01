using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000569 RID: 1385
	public class ObjectiveCondition_ControlTerritory : ObjectiveCondition
	{
		// Token: 0x06001A9C RID: 6812 RVA: 0x0005CE19 File Offset: 0x0005B019
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return context.CurrentTurn.HexBoard.GetHexesControlledByPlayer(owner.Id).Count<Hex>();
		}
	}
}
