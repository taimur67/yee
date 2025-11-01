using System;

namespace LoG
{
	// Token: 0x020003B9 RID: 953
	public class DuelParticipantInstance
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060012A7 RID: 4775 RVA: 0x00047003 File Offset: 0x00045203
		public bool TechniqueActive
		{
			get
			{
				return !this.MoveCountered && this.CombatMoveInstance != null;
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00047018 File Offset: 0x00045218
		public DuelParticipantInstance(PraetorDuelParticipantData data)
		{
			this.Data = data;
		}

		// Token: 0x040008AE RID: 2222
		public PraetorDuelParticipantData Data;

		// Token: 0x040008AF RID: 2223
		public PlayerState Player;

		// Token: 0x040008B0 RID: 2224
		public Praetor Praetor;

		// Token: 0x040008B1 RID: 2225
		public bool MoveCountered;

		// Token: 0x040008B2 RID: 2226
		public PraetorCombatMoveInstance CombatMoveInstance;

		// Token: 0x040008B3 RID: 2227
		public PraetorCombatMoveStaticData CombatMoveData;

		// Token: 0x040008B4 RID: 2228
		public ModifiableValue DamageGiven = 0;
	}
}
