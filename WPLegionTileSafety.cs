using System;
using System.Numerics;

namespace LoG
{
	// Token: 0x0200017C RID: 380
	public class WPLegionTileSafety : WorldProperty<WPLegionTileSafety>
	{
		// Token: 0x06000730 RID: 1840 RVA: 0x00022988 File Offset: 0x00020B88
		public static WPLegionTileSafety SafetyFromMovement(GOAPPlanner planner, GamePiece gamePiece, HexCoord location)
		{
			Vector2 safetyInDirection;
			if (planner.AIPreviewTurn.HexBoard.TryGetGeneralDirection(gamePiece.Location, location, out safetyInDirection))
			{
				return new WPLegionTileSafety(gamePiece)
				{
					SafetyInDirection = safetyInDirection
				};
			}
			return null;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000229C4 File Offset: 0x00020BC4
		public WPLegionTileSafety(Identifier legionID)
		{
			this.LegionID = legionID;
			this.SafetyInDirection = Vector2.Zero;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x000229E0 File Offset: 0x00020BE0
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			GamePiece gamePiece = viewContext.CurrentTurn.TryFetchGameItem<GamePiece>(this.LegionID);
			return gamePiece == null || planner.TileIsSafeForLegion(gamePiece, gamePiece.Location);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00022A14 File Offset: 0x00020C14
		public override WPProvidesEffect ProvidesEffectInternal(WPLegionTileSafety property)
		{
			if (property.LegionID != this.LegionID)
			{
				return WPProvidesEffect.No;
			}
			if (this.SafetyInDirection.LengthSquared() <= 0f)
			{
				return WPProvidesEffect.Yes;
			}
			Vector2 value;
			if (!this.OwningPlanner.AIPersistentData.TryGetLegionHistoricMovement(property.LegionID, this.OwningPlanner.AIPreviewTurn.HexBoard, out value))
			{
				return WPProvidesEffect.Yes;
			}
			if (Vector2.Dot(value, this.SafetyInDirection) < 0f)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400034C RID: 844
		public Identifier LegionID;

		// Token: 0x0400034D RID: 845
		public Vector2 SafetyInDirection;
	}
}
