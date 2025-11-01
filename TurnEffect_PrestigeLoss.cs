using System;

namespace LoG
{
	// Token: 0x02000350 RID: 848
	public class TurnEffect_PrestigeLoss : TurnAbilityEffect
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001021 RID: 4129 RVA: 0x0003FBB7 File Offset: 0x0003DDB7
		public override TurnProcessStage HasEffectInStage
		{
			get
			{
				return TurnProcessStage.TurnModule_Prestige;
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0003FBBC File Offset: 0x0003DDBC
		protected override void OnStageOfTurnIfActive(Ability ability, TurnProcessContext context, GamePiece piece)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != piece.ControllingPlayerId && !playerState.Excommunicated)
				{
					playerState.RemovePrestige(this.PrestigeLost);
				}
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0003FC2C File Offset: 0x0003DE2C
		public override void DeepClone(out AbilityEffect clone)
		{
			TurnEffect_PrestigeLoss turnEffect_PrestigeLoss = new TurnEffect_PrestigeLoss
			{
				PrestigeLost = this.PrestigeLost
			};
			base.DeepCloneTurnAbilityEffectParts(turnEffect_PrestigeLoss);
			clone = turnEffect_PrestigeLoss;
		}

		// Token: 0x0400077D RID: 1917
		public int PrestigeLost;
	}
}
