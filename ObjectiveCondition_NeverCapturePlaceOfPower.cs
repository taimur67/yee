using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000598 RID: 1432
	[Serializable]
	public class ObjectiveCondition_NeverCapturePlaceOfPower : ObjectiveCondition
	{
		// Token: 0x06001B19 RID: 6937 RVA: 0x0005E72C File Offset: 0x0005C92C
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			foreach (GameItemOwnershipChanged gameItemOwnershipChanged in context.CurrentTurn.GetGameEvents<GameItemOwnershipChanged>())
			{
				GamePiece gamePiece;
				if (gameItemOwnershipChanged.TriggeringPlayerID == owner.Id && context.CurrentTurn.TryFetchGameItem<GamePiece>(gameItemOwnershipChanged.Item, out gamePiece) && gamePiece.IsFixture())
				{
					this.HasCaptured = true;
				}
			}
			if (!this.HasCaptured)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x04000C4A RID: 3146
		[JsonProperty]
		private bool HasCaptured;
	}
}
