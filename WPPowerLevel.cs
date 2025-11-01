using System;

namespace LoG
{
	// Token: 0x02000188 RID: 392
	public class WPPowerLevel : WorldProperty<WPPowerLevel>
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x00022D38 File Offset: 0x00020F38
		public override bool MustBeSuccessfullFulfilledAsPrecondition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00022D3B File Offset: 0x00020F3B
		public WPPowerLevel(PowerType power, int level)
		{
			this.Power = power;
			this.Level = level;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00022D51 File Offset: 0x00020F51
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return playerState.PowersLevels[this.Power] >= this.Level;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00022D74 File Offset: 0x00020F74
		public override WPProvidesEffect ProvidesEffectInternal(WPPowerLevel powerLevelPrecondition)
		{
			if (this.Power != powerLevelPrecondition.Power || this.Level > powerLevelPrecondition.Level)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x04000358 RID: 856
		public PowerType Power;

		// Token: 0x04000359 RID: 857
		public int Level;
	}
}
