using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200011D RID: 285
	public static class ActionMovement
	{
		// Token: 0x06000525 RID: 1317 RVA: 0x00017DD0 File Offset: 0x00015FD0
		public static void RegisterBlockages(GOAPPlanner planner, GamePiece movingPiece, HexCoord targetLocation, bool destinationAlwaysValid = true, bool destinationOwnersCantonsAlwaysValid = false, bool allowRedeployToDestination = true)
		{
			List<HexCoord> list = planner.FindTerrainPath(movingPiece, movingPiece.Location, targetLocation, ~(GamePieceAvoidance.FriendlyLegion | GamePieceAvoidance.FriendlyFixture), destinationAlwaysValid, destinationOwnersCantonsAlwaysValid, allowRedeployToDestination);
			if (list.Count <= 0)
			{
				return;
			}
			list.RemoveAt(0);
			for (int i = 0; i < list.Count; i++)
			{
				GamePiece gamePiece;
				if (planner.PristineTurn.TryGetActiveGamePieceAt(list[i], out gamePiece) && gamePiece.ControllingPlayerId == planner.PlayerId)
				{
					planner.AIPersistentData.RegisterBlockingPiece(gamePiece, gamePiece.Location);
				}
			}
		}
	}
}
