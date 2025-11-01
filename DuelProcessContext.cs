using System;

namespace LoG
{
	// Token: 0x020003B8 RID: 952
	public class DuelProcessContext
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060012A3 RID: 4771 RVA: 0x00046F21 File Offset: 0x00045121
		public DuelProcessDamageTally DamageTally
		{
			get
			{
				return new DuelProcessDamageTally
				{
					ChallengerDamageDealt = this.ChallengerInstance.DamageGiven,
					DefenderDamageDealt = this.DefenderInstance.DamageGiven
				};
			}
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00046F4A File Offset: 0x0004514A
		public DuelProcessContext(PraetorDuelData data)
		{
			this.DuelData = data;
			this.ChallengerInstance = new DuelParticipantInstance(data.Challenger);
			this.DefenderInstance = new DuelParticipantInstance(data.Defender);
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00046F7B File Offset: 0x0004517B
		public bool TryGetOther(DuelParticipantInstance instance, out DuelParticipantInstance other)
		{
			other = ((this.ChallengerInstance == instance) ? this.DefenderInstance : this.ChallengerInstance);
			return true;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00046F98 File Offset: 0x00045198
		public bool DetermineWinner<T>(T challenger, T defender, out T winner)
		{
			if (this.DamageTally.ChallengerDamageDealt > this.DamageTally.DefenderDamageDealt)
			{
				winner = challenger;
				return true;
			}
			if (this.DamageTally.DefenderDamageDealt > this.DamageTally.ChallengerDamageDealt)
			{
				winner = defender;
				return true;
			}
			winner = default(T);
			return false;
		}

		// Token: 0x040008AB RID: 2219
		public PraetorDuelData DuelData;

		// Token: 0x040008AC RID: 2220
		public DuelParticipantInstance ChallengerInstance;

		// Token: 0x040008AD RID: 2221
		public DuelParticipantInstance DefenderInstance;
	}
}
