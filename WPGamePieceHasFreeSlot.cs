using System;

namespace LoG
{
	// Token: 0x02000169 RID: 361
	public class WPGamePieceHasFreeSlot : WorldProperty
	{
		// Token: 0x060006FC RID: 1788 RVA: 0x000222B2 File Offset: 0x000204B2
		public WPGamePieceHasFreeSlot(Identifier gamePieceID, FreeSlotsMode mode = FreeSlotsMode.Any)
		{
			this.GamePieceID = gamePieceID;
			this.SlotsMode = mode;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000222C8 File Offset: 0x000204C8
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPGamePieceHasFreeSlot wpgamePieceHasFreeSlot = precondition as WPGamePieceHasFreeSlot;
			if (wpgamePieceHasFreeSlot == null)
			{
				return WPProvidesEffect.No;
			}
			if (wpgamePieceHasFreeSlot.GamePieceID != this.GamePieceID)
			{
				return WPProvidesEffect.No;
			}
			if (wpgamePieceHasFreeSlot.SlotsMode == FreeSlotsMode.Half && this.SlotsMode == FreeSlotsMode.Any)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00022308 File Offset: 0x00020508
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			GamePiece gamePiece = viewContext.CurrentTurn.FetchGameItem<GamePiece>(this.GamePieceID);
			if (gamePiece != null)
			{
				if (this.SlotsMode == FreeSlotsMode.Any)
				{
					return gamePiece.RemainingSlots > 0;
				}
				int num = gamePiece.LookupMaxSlots();
				if (num > 0)
				{
					double num2 = Math.Ceiling((double)((float)num * 0.5f));
					return (double)gamePiece.RemainingSlots >= num2;
				}
			}
			return false;
		}

		// Token: 0x0400033A RID: 826
		public Identifier GamePieceID;

		// Token: 0x0400033B RID: 827
		public FreeSlotsMode SlotsMode;
	}
}
