using System;

namespace LoG
{
	// Token: 0x0200048D RID: 1165
	[Serializable]
	public class ConductDiplomacyConflict : ActionConflict<ConductDiplomacyConflict>
	{
		// Token: 0x060015D3 RID: 5587 RVA: 0x00051D72 File Offset: 0x0004FF72
		public ConductDiplomacyConflict()
		{
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00051D7A File Offset: 0x0004FF7A
		public ConductDiplomacyConflict(int playerIndex)
		{
			this.PlayerIndex = playerIndex;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00051D89 File Offset: 0x0004FF89
		protected override bool ConflictsWith(ConductDiplomacyConflict other)
		{
			return other.PlayerIndex == this.PlayerIndex;
		}

		// Token: 0x04000AFE RID: 2814
		public int PlayerIndex;
	}
}
