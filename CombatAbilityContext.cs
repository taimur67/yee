using System;

namespace LoG
{
	// Token: 0x02000359 RID: 857
	public class CombatAbilityContext
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x00040446 File Offset: 0x0003E646
		public TurnState Turn
		{
			get
			{
				return this.TurnContext.CurrentTurn;
			}
		}

		// Token: 0x04000790 RID: 1936
		public TurnProcessContext TurnContext;

		// Token: 0x04000791 RID: 1937
		public GamePiece Actor;

		// Token: 0x04000792 RID: 1938
		public GamePiece Opponent;

		// Token: 0x04000793 RID: 1939
		public BattleRole BattleRole;
	}
}
