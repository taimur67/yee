using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200030F RID: 783
	public class MessageTrigger_OnPoPCaptured : MessageTriggerCondition
	{
		// Token: 0x06000F4D RID: 3917 RVA: 0x0003CAF6 File Offset: 0x0003ACF6
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when <{1}> is captured by anyone", this.RecipientArchfiendId, this.TargetPoP);
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0003CB10 File Offset: 0x0003AD10
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			foreach (GameItemOwnershipChanged gameItemOwnershipChanged in context.CurrentTurn.GetGameEvents().OfType<GameItemOwnershipChanged>())
			{
				GamePiece gamePiece;
				if (context.CurrentTurn.TryFetchGameItem<GamePiece>(gameItemOwnershipChanged.Item, out gamePiece) && gamePiece.SubCategory.IsPlaceOfPower(true, true) && gamePiece.Id == this.TargetPoP.Id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400072B RID: 1835
		public GamePiece TargetPoP;
	}
}
