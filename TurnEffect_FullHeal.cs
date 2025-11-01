using System;

namespace LoG
{
	// Token: 0x0200034D RID: 845
	public class TurnEffect_FullHeal : TurnAbilityEffect
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x0003F845 File Offset: 0x0003DA45
		public override TurnProcessStage HasEffectInStage
		{
			get
			{
				return TurnProcessStage.TurnModule_Healing;
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0003F848 File Offset: 0x0003DA48
		protected override void OnStageOfTurnIfActive(Ability ability, TurnProcessContext context, GamePiece piece)
		{
			piece.Heal(piece.TotalHP);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0003F85C File Offset: 0x0003DA5C
		public override void DeepClone(out AbilityEffect clone)
		{
			TurnEffect_FullHeal turnEffect_FullHeal = new TurnEffect_FullHeal();
			base.DeepCloneTurnAbilityEffectParts(turnEffect_FullHeal);
			clone = turnEffect_FullHeal;
		}
	}
}
