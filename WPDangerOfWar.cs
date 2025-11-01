using System;

namespace LoG
{
	// Token: 0x02000160 RID: 352
	public class WPDangerOfWar : WorldProperty<WPDangerOfWar>
	{
		// Token: 0x060006E5 RID: 1765 RVA: 0x00021EA1 File Offset: 0x000200A1
		public WPDangerOfWar(int playerID)
		{
			this.EnemyID = playerID;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00021EBC File Offset: 0x000200BC
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			float num;
			if (!this.OwningPlanner.ArchfiendHeuristics.TryGetDiplomaticStress(this.OwningPlanner.PlayerId, out num))
			{
				return false;
			}
			PlayerState instigator = viewContext.CurrentTurn.FindPlayerState(this.EnemyID, null);
			float num2;
			float num3;
			this.OwningPlanner.GetGrievanceRisks(instigator, this.OwningPlanner.PlayerState, out num2, out num3);
			float num4 = (3f * num3 + num2) / 4f;
			return num * (1f - num4) >= this.Threshold;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00021F3E File Offset: 0x0002013E
		public override WPProvidesEffect ProvidesEffectInternal(WPDangerOfWar property)
		{
			if (property.EnemyID == this.EnemyID)
			{
				return WPProvidesEffect.Yes;
			}
			return WPProvidesEffect.No;
		}

		// Token: 0x0400031F RID: 799
		public int EnemyID;

		// Token: 0x04000320 RID: 800
		public float Threshold = 0.4f;
	}
}
