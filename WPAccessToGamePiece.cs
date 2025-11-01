using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000151 RID: 337
	public class WPAccessToGamePiece : WorldProperty
	{
		// Token: 0x060006AB RID: 1707 RVA: 0x0002153D File Offset: 0x0001F73D
		public WPAccessToGamePiece(GamePiece movingLegion, GamePiece gamePiece)
		{
			this.MovingLegion = movingLegion;
			this.GamePiece = gamePiece;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00021554 File Offset: 0x0001F754
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			if (this.MovingLegion == null || this.GamePiece == null)
			{
				return false;
			}
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>(planner.FindTerrainPath(this.MovingLegion, this.MovingLegion.Location, this.GamePiece.Location, ~GamePieceAvoidance.FriendlyFixture, true, false, true));
			if (list.Count > 0)
			{
				list.RemoveAt(0);
			}
			return list.Count > 0;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000215BC File Offset: 0x0001F7BC
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPAccessToGamePiece wpaccessToGamePiece = precondition as WPAccessToGamePiece;
			if (wpaccessToGamePiece != null && wpaccessToGamePiece.GamePiece == this.GamePiece && wpaccessToGamePiece.MovingLegion == this.MovingLegion)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000306 RID: 774
		private readonly GamePiece MovingLegion;

		// Token: 0x04000307 RID: 775
		private readonly GamePiece GamePiece;
	}
}
