using System;

namespace LoG
{
	// Token: 0x0200017F RID: 383
	public class WPMilitarySuperiority : WorldProperty
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x00022B18 File Offset: 0x00020D18
		public WPMilitarySuperiority(int aggressorPlayerID, int targetPlayerID, float maxVendettaRiskForSuperiority = 0.5f)
		{
			this.AggressorPlayerID = aggressorPlayerID;
			this.TargetPlayerID = targetPlayerID;
			this.MaxVendettaRiskForSuperiority = maxVendettaRiskForSuperiority;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00022B38 File Offset: 0x00020D38
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			VendettaHeuristics.VendettaParameters vendettaParameters;
			return planner.ArchfiendHeuristics.TryGetLeastRiskyVendettaType(this.AggressorPlayerID, this.TargetPlayerID, out vendettaParameters) && vendettaParameters.TypeRisk <= this.MaxVendettaRiskForSuperiority;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00022B74 File Offset: 0x00020D74
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPMilitarySuperiority wpmilitarySuperiority = precondition as WPMilitarySuperiority;
			if (wpmilitarySuperiority == null)
			{
				return WPProvidesEffect.No;
			}
			if (wpmilitarySuperiority.TargetPlayerID != this.TargetPlayerID)
			{
				return WPProvidesEffect.No;
			}
			if (wpmilitarySuperiority.AggressorPlayerID != this.AggressorPlayerID)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x04000351 RID: 849
		public readonly int AggressorPlayerID;

		// Token: 0x04000352 RID: 850
		public readonly int TargetPlayerID;

		// Token: 0x04000353 RID: 851
		public readonly float MaxVendettaRiskForSuperiority;
	}
}
