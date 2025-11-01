using System;

namespace LoG
{
	// Token: 0x02000491 RID: 1169
	[Serializable]
	public class PowerUpConflict : ActionConflict<PowerUpConflict>
	{
		// Token: 0x060015DE RID: 5598 RVA: 0x00051DF2 File Offset: 0x0004FFF2
		public PowerUpConflict()
		{
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x00051DFA File Offset: 0x0004FFFA
		public PowerUpConflict(PowerType power)
		{
			this.Power = power;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x00051E09 File Offset: 0x00050009
		protected override bool ConflictsWith(PowerUpConflict other)
		{
			return other.Power == this.Power;
		}

		// Token: 0x04000B01 RID: 2817
		public PowerType Power;
	}
}
