using System;
using System.Linq;

namespace LoG
{
	// Token: 0x0200016A RID: 362
	public class WPGamePieceHasPraetor : WorldProperty
	{
		// Token: 0x060006FF RID: 1791 RVA: 0x00022365 File Offset: 0x00020565
		public WPGamePieceHasPraetor(GamePiece gamePiece, bool val)
		{
			this.GamePiece = gamePiece;
			this.Value = val;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002237B File Offset: 0x0002057B
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return IEnumerableExtensions.Any<Praetor>(this.GamePiece.GetAttachedAndPendingItems(viewContext.CurrentTurn).OfType<Praetor>()) == this.Value;
		}

		// Token: 0x0400033C RID: 828
		private readonly GamePiece GamePiece;

		// Token: 0x0400033D RID: 829
		public bool Value;
	}
}
