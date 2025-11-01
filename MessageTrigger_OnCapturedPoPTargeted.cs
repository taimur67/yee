using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000310 RID: 784
	public class MessageTrigger_OnCapturedPoPTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F50 RID: 3920 RVA: 0x0003CBA8 File Offset: 0x0003ADA8
		public override string GetDescription()
		{
			return string.Format("Message <{0}> When <{1}> captures <{2}>", this.RecipientArchfiendId, this.TargetArchfiendId, this.TargetPoP);
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0003CBC8 File Offset: 0x0003ADC8
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			foreach (GameItemOwnershipChanged gameItemOwnershipChanged in context.CurrentTurn.GetGameEvents().OfType<GameItemOwnershipChanged>())
			{
				GamePiece gamePiece;
				if (gameItemOwnershipChanged.TriggeringPlayerID == playerState.Id && context.CurrentTurn.TryFetchGameItem<GamePiece>(gameItemOwnershipChanged.Item, out gamePiece) && gamePiece.SubCategory.IsPlaceOfPower(true, true) && gamePiece.Id == this.TargetPoP.Id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400072C RID: 1836
		public string TargetArchfiendId;

		// Token: 0x0400072D RID: 1837
		public GamePiece TargetPoP;
	}
}
