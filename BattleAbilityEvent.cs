using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001EE RID: 494
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BattleAbilityEvent : GameEvent
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0002CE69 File Offset: 0x0002B069
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0002CE6C File Offset: 0x0002B06C
		[JsonConstructor]
		protected BattleAbilityEvent()
		{
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0002CE7F File Offset: 0x0002B07F
		public BattleAbilityEvent(CombatAbilityStage combatAbilityStage, Ability ability, CombatAbilityContext combatAbilityContext, string effectId) : this(combatAbilityStage, ability, combatAbilityContext.Actor.ControllingPlayerId, combatAbilityContext.Opponent.Id, combatAbilityContext.Actor.Id, effectId)
		{
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0002CEAC File Offset: 0x0002B0AC
		public BattleAbilityEvent(CombatAbilityStage combatAbilityStage, Ability ability, int controllingPlayerId, Identifier opponentId, Identifier actorId, string effectId) : base(controllingPlayerId)
		{
			this.CombatAbilityStage = combatAbilityStage;
			this.AbilityContext = ability;
			this.AbilityId = ability.SourceId;
			this.EffectId = effectId;
			this.OpponentId = opponentId;
			this.ActorId = actorId;
			base.AddAffectedPlayerId(controllingPlayerId);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0002CF09 File Offset: 0x0002B109
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} triggered in stage {1}", this.AbilityContext.SourceId, this.CombatAbilityStage);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0002CF2C File Offset: 0x0002B12C
		public virtual void AddAppliedModifier(Identifier affectedGamePiece, GamePieceModifier modifier)
		{
			List<GamePieceModifier> list;
			if (this.AppliedModifiers.TryGetValue(affectedGamePiece, out list))
			{
				list.Add(modifier);
				return;
			}
			this.AppliedModifiers.Add(affectedGamePiece, new List<GamePieceModifier>
			{
				modifier
			});
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0002CF6C File Offset: 0x0002B16C
		protected void DeepCloneBattleAbilityEventParts(BattleAbilityEvent battleAbilityEvent)
		{
			battleAbilityEvent.AbilityContext = this.AbilityContext.DeepClone(CloneFunction.FastClone);
			battleAbilityEvent.CombatAbilityStage = this.CombatAbilityStage;
			battleAbilityEvent.EffectId = this.EffectId;
			battleAbilityEvent.ActorId = this.ActorId;
			battleAbilityEvent.OpponentId = this.OpponentId;
			battleAbilityEvent.AbilityId = this.AbilityId;
			battleAbilityEvent.AppliedModifiers = this.AppliedModifiers.DeepClone(CloneFunction.FastClone);
			base.DeepCloneGameEventParts<BattleAbilityEvent>(battleAbilityEvent);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0002CFE4 File Offset: 0x0002B1E4
		public override void DeepClone(out GameEvent clone)
		{
			BattleAbilityEvent battleAbilityEvent = new BattleAbilityEvent();
			this.DeepCloneBattleAbilityEventParts(battleAbilityEvent);
			clone = battleAbilityEvent;
		}

		// Token: 0x0400049C RID: 1180
		[BindableValue("ability", BindingOption.StaticDataId)]
		[JsonProperty]
		public string AbilityId;

		// Token: 0x0400049D RID: 1181
		[JsonProperty]
		public AbilityContext AbilityContext;

		// Token: 0x0400049E RID: 1182
		[JsonProperty]
		public CombatAbilityStage CombatAbilityStage;

		// Token: 0x0400049F RID: 1183
		[JsonProperty]
		public string EffectId;

		// Token: 0x040004A0 RID: 1184
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier ActorId;

		// Token: 0x040004A1 RID: 1185
		[BindableValue("target", BindingOption.None)]
		[JsonProperty]
		public Identifier OpponentId;

		// Token: 0x040004A2 RID: 1186
		[JsonProperty]
		public Dictionary<Identifier, List<GamePieceModifier>> AppliedModifiers = new Dictionary<Identifier, List<GamePieceModifier>>();
	}
}
