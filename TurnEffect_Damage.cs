using System;

namespace LoG
{
	// Token: 0x0200034C RID: 844
	[Serializable]
	public class TurnEffect_Damage : TurnAbilityEffect
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x0003F6E6 File Offset: 0x0003D8E6
		public override TurnProcessStage HasEffectInStage
		{
			get
			{
				return this.DamageStage;
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0003F6F0 File Offset: 0x0003D8F0
		protected override void OnStageOfTurnIfActive(Ability ability, TurnProcessContext context, GamePiece piece)
		{
			if (base.IsNullified(context, piece))
			{
				return;
			}
			bool flag = context.CurrentTurn.TurnValue == piece.SpawnTurn;
			if (!this.DamageFirstTurn && flag)
			{
				return;
			}
			int num = this.DamageAmount;
			if (this.MultiplyDamageByLevel)
			{
				num *= piece.Level;
			}
			if (!this.CanKill && piece.HP <= num)
			{
				return;
			}
			BattleProcessor.DamageEvent ev = context.DealDamage(piece, piece.ControllingPlayerId, num, LoG.DamageType.True, false, AttackOutcomeIntent.Default);
			OutOfCombatDamageEvent outOfCombatDamageEvent = new OutOfCombatDamageEvent
			{
				DamageType = this.DamageType
			};
			outOfCombatDamageEvent.AddChildEvent<BattleProcessor.DamageEvent>(ev);
			if (piece.IsPandaemonium() && !piece.IsOwned())
			{
				GameItem item = context.CurrentTurn.FetchGameItem(ability.ProviderId);
				PlayerState playerState = context.CurrentTurn.FindControllingPlayer(item);
				context.Diplomacy.SetPlayerAsExcommunicated(context, playerState, ExcommunicationReason.Unknown, playerState.Id);
			}
			context.CurrentTurn.AddGameEvent<OutOfCombatDamageEvent>(outOfCombatDamageEvent);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003F7D8 File Offset: 0x0003D9D8
		public override void DeepClone(out AbilityEffect clone)
		{
			TurnEffect_Damage turnEffect_Damage = new TurnEffect_Damage
			{
				DamageAmount = this.DamageAmount,
				DamageType = this.DamageType,
				CanKill = this.CanKill,
				DamageFirstTurn = this.DamageFirstTurn,
				DamageStage = this.DamageStage,
				MultiplyDamageByLevel = this.MultiplyDamageByLevel
			};
			base.DeepCloneTurnAbilityEffectParts(turnEffect_Damage);
			clone = turnEffect_Damage;
		}

		// Token: 0x04000772 RID: 1906
		public bool CanKill;

		// Token: 0x04000773 RID: 1907
		[BindableValue(null, BindingOption.None)]
		public int DamageAmount;

		// Token: 0x04000774 RID: 1908
		public OutOfCombatDamageType DamageType;

		// Token: 0x04000775 RID: 1909
		public bool DamageFirstTurn;

		// Token: 0x04000776 RID: 1910
		public TurnProcessStage DamageStage;

		// Token: 0x04000777 RID: 1911
		public bool MultiplyDamageByLevel;
	}
}
