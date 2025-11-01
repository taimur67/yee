using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000362 RID: 866
	[Serializable]
	public abstract class TurnAbilityEffect : AbilityEffect
	{
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600107E RID: 4222
		[JsonIgnore]
		public abstract TurnProcessStage HasEffectInStage { get; }

		// Token: 0x0600107F RID: 4223 RVA: 0x00040E88 File Offset: 0x0003F088
		private bool IsActiveForContext(TurnProcessContext context, GamePiece gamePieceUsingTheAbility)
		{
			return (this.AffectsInactiveGamePieces || gamePieceUsingTheAbility.IsActive) && (this.AffectsCategory.Count <= 0 || this.AffectsCategory.Contains(gamePieceUsingTheAbility.SubCategory));
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00040EC0 File Offset: 0x0003F0C0
		public void OnStageOfTurn(TurnProcessStage stage, Ability ability, TurnProcessContext context, GamePiece gamePieceUsingTheAbility)
		{
			if (!this.IsActiveForContext(context, gamePieceUsingTheAbility))
			{
				return;
			}
			this.OnStageOfTurnIfActive(ability, context, gamePieceUsingTheAbility);
		}

		// Token: 0x06001081 RID: 4225
		protected abstract void OnStageOfTurnIfActive(Ability ability, TurnProcessContext context, GamePiece gamePieceUsingTheAbility);

		// Token: 0x06001082 RID: 4226 RVA: 0x00040ED8 File Offset: 0x0003F0D8
		protected void DeepCloneTurnAbilityEffectParts(TurnAbilityEffect turnAbilityEffect)
		{
			turnAbilityEffect.AffectsInactiveGamePieces = this.AffectsInactiveGamePieces;
			base.DeepCloneAbilityEffectParts(turnAbilityEffect);
		}

		// Token: 0x06001083 RID: 4227
		public abstract override void DeepClone(out AbilityEffect clone);

		// Token: 0x040007B1 RID: 1969
		[JsonProperty]
		[DefaultValue(false)]
		public bool AffectsInactiveGamePieces;
	}
}
