using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200035A RID: 858
	[Serializable]
	public abstract class CombatAbilityEffect : AbilityEffect
	{
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x0004045B File Offset: 0x0003E65B
		[JsonIgnore]
		public virtual bool CanBeCancelled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x0004045E File Offset: 0x0003E65E
		[JsonIgnore]
		public string TypeName
		{
			get
			{
				return StringExtensions.RemoveStart(base.GetType().Name, "CombatEffect_");
			}
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00040478 File Offset: 0x0003E678
		public bool IsActiveForContext(BattleRole userRole, GamePiece opponent)
		{
			int role = (int)this.Role;
			return (userRole & (BattleRole)role) != (BattleRole)0 && (this.AffectsCategory.Count <= 0 || this.AffectsCategory.Contains(opponent.SubCategory));
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x000404B7 File Offset: 0x0003E6B7
		public bool IsActiveForContext(CombatAbilityContext context)
		{
			this.ActorId = context.Actor;
			this.OpponentId = context.Opponent;
			return this.IsActiveForContext(context.BattleRole, context.Opponent);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x000404F0 File Offset: 0x0003E6F0
		public virtual GameEvent ProcessAbilityState(CombatAbilityStage combatAbilityStage, Ability source, CombatAbilityContext abilityContext, BattleContext battleContext, BattleEvent battleEvent, BattlePhase battlePhase = BattlePhase.Undefined)
		{
			this.CurrentAbilityStage = combatAbilityStage;
			if (!this.IsActiveForContext(abilityContext))
			{
				return null;
			}
			GameEvent result;
			switch (combatAbilityStage)
			{
			case CombatAbilityStage.BlockAbilities:
				result = this.OnBlockAbilities(source, abilityContext, battleEvent);
				break;
			case CombatAbilityStage.PreBattle:
				result = this.OnPreBattle(source, abilityContext, battleEvent, battleContext);
				break;
			case CombatAbilityStage.BlockStratagems:
				result = this.OnBlockStratagems(source, abilityContext, battleEvent);
				break;
			case CombatAbilityStage.Stratagems:
				result = this.OnStratagems(source, abilityContext, battleEvent, battleContext);
				break;
			default:
				if (combatAbilityStage != CombatAbilityStage.PostBattle)
				{
					throw new ArgumentOutOfRangeException("combatAbilityStage", combatAbilityStage, null);
				}
				result = this.OnPostBattle(source, abilityContext, battleEvent);
				break;
			}
			return result;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00040584 File Offset: 0x0003E784
		protected virtual GameEvent OnBlockAbilities(Ability source, CombatAbilityContext context, BattleEvent battleEvent)
		{
			return null;
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00040587 File Offset: 0x0003E787
		protected virtual GameEvent OnPreBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext)
		{
			return null;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0004058A File Offset: 0x0003E78A
		protected virtual GameEvent OnBlockStratagems(Ability source, CombatAbilityContext context, BattleEvent battleEvent)
		{
			return null;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0004058D File Offset: 0x0003E78D
		protected virtual GameEvent OnPreCombatPhase(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext, BattlePhase phase)
		{
			return null;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00040590 File Offset: 0x0003E790
		protected virtual GameEvent OnStratagems(Ability source, CombatAbilityContext context, BattleEvent battleEvent, BattleContext battleContext)
		{
			return null;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00040593 File Offset: 0x0003E793
		protected virtual GameEvent OnPostBattle(Ability source, CombatAbilityContext context, BattleEvent battleEvent)
		{
			return null;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00040596 File Offset: 0x0003E796
		protected void DeepCloneCombatAbilityEffectParts(CombatAbilityEffect combatAbilityEffect)
		{
			combatAbilityEffect.NullifiedByOpponentAbilities = this.NullifiedByOpponentAbilities.DeepClone<ItemAbilityStaticData>();
			combatAbilityEffect.Role = this.Role;
			combatAbilityEffect.ActorId = this.ActorId;
			combatAbilityEffect.OpponentId = this.OpponentId;
			base.DeepCloneAbilityEffectParts(combatAbilityEffect);
		}

		// Token: 0x06001058 RID: 4184
		public abstract override void DeepClone(out AbilityEffect clone);

		// Token: 0x04000794 RID: 1940
		[JsonProperty]
		public List<ConfigRef<ItemAbilityStaticData>> NullifiedByOpponentAbilities = new List<ConfigRef<ItemAbilityStaticData>>();

		// Token: 0x04000795 RID: 1941
		[JsonProperty]
		[DefaultValue(BattleRole.All)]
		public BattleRole Role = BattleRole.All;

		// Token: 0x04000796 RID: 1942
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public Identifier ActorId;

		// Token: 0x04000797 RID: 1943
		[JsonProperty]
		[BindableValue("target", BindingOption.None)]
		public Identifier OpponentId;

		// Token: 0x04000798 RID: 1944
		[JsonIgnore]
		protected CombatAbilityStage CurrentAbilityStage;
	}
}
