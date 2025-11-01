using System;

namespace LoG
{
	// Token: 0x02000177 RID: 375
	public class WPLegionBadlyDamaged : WorldProperty<WPLegionBadlyDamaged>
	{
		// Token: 0x06000724 RID: 1828 RVA: 0x00022848 File Offset: 0x00020A48
		public WPLegionBadlyDamaged(Identifier legionID, float threshold = 0.5f)
		{
			this.LegionID = legionID;
			this.Threshold = threshold;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00022860 File Offset: 0x00020A60
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			GamePiece gamePiece = viewContext.CurrentTurn.FetchGameItem<GamePiece>(this.LegionID);
			return gamePiece != null && (float)gamePiece.HP <= (float)gamePiece.TotalHP * this.Threshold;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000228A3 File Offset: 0x00020AA3
		public override WPProvidesEffect ProvidesEffectInternal(WPLegionBadlyDamaged property)
		{
			if (property.LegionID == this.LegionID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x04000346 RID: 838
		public float Threshold;

		// Token: 0x04000347 RID: 839
		public Identifier LegionID;
	}
}
