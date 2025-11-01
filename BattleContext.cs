using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020001C6 RID: 454
	public class BattleContext
	{
		// Token: 0x06000881 RID: 2177 RVA: 0x000282F0 File Offset: 0x000264F0
		[return: TupleElementNames(new string[]
		{
			"stronger",
			"weaker"
		})]
		public ValueTuple<GamePiece, GamePiece> DeduceStrength(int attackPower, int defencePower)
		{
			if (attackPower >= defencePower)
			{
				return new ValueTuple<GamePiece, GamePiece>(this.Attacker, this.Defender);
			}
			return new ValueTuple<GamePiece, GamePiece>(this.Defender, this.Attacker);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00028319 File Offset: 0x00026519
		public void ClearSupport()
		{
			List<GamePiece> attackerSupport = this.AttackerSupport;
			if (attackerSupport != null)
			{
				attackerSupport.Clear();
			}
			List<GamePiece> defenderSupport = this.DefenderSupport;
			if (defenderSupport == null)
			{
				return;
			}
			defenderSupport.Clear();
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0002833C File Offset: 0x0002653C
		public bool CombatantsSet
		{
			get
			{
				return this.Attacker != null && this.Defender != null;
			}
		}

		// Token: 0x04000418 RID: 1048
		public HexCoord Location;

		// Token: 0x04000419 RID: 1049
		public AttackOutcomeIntent AttackOutcomeIntent;

		// Token: 0x0400041A RID: 1050
		public GamePiece Attacker;

		// Token: 0x0400041B RID: 1051
		public List<GamePiece> AttackerSupport;

		// Token: 0x0400041C RID: 1052
		public GamePiece Defender;

		// Token: 0x0400041D RID: 1053
		public List<GamePiece> DefenderSupport;

		// Token: 0x0400041E RID: 1054
		public readonly List<BattlePhaseModification> PhaseModifications = new List<BattlePhaseModification>();
	}
}
