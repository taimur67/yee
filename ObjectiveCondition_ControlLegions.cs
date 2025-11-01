using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000567 RID: 1383
	public class ObjectiveCondition_ControlLegions : ObjectiveCondition
	{
		// Token: 0x06001A97 RID: 6807 RVA: 0x0005CD96 File Offset: 0x0005AF96
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return context.CurrentTurn.GetActiveGamePiecesForPlayer(owner.Id).Count((GamePiece x) => x.IsLegionOrTitan());
		}
	}
}
