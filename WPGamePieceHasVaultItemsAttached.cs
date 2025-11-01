using System;
using System.Linq;

namespace LoG
{
	// Token: 0x0200016B RID: 363
	public class WPGamePieceHasVaultItemsAttached : WorldProperty<WPGamePieceHasVaultItemsAttached>
	{
		// Token: 0x06000701 RID: 1793 RVA: 0x000223A0 File Offset: 0x000205A0
		public WPGamePieceHasVaultItemsAttached(GamePiece gamePiece)
		{
			this.GamePiece = gamePiece;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000223AF File Offset: 0x000205AF
		public override WPProvidesEffect ProvidesEffectInternal(WPGamePieceHasVaultItemsAttached vaultItemsPrecondition)
		{
			if (vaultItemsPrecondition.GamePiece.Id != this.GamePiece.Id)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000223CC File Offset: 0x000205CC
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return (from itemId in this.GamePiece.Slots
			select viewContext.CurrentTurn.FetchGameItem(itemId)).Any((GameItem gameItem) => gameItem.IsActive && gameItem.CanBePlacedInVault);
		}

		// Token: 0x0400033E RID: 830
		public GamePiece GamePiece;
	}
}
