using System;
using System.Collections.Generic;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x0200015A RID: 346
	public class WPCombatAdvantage : WorldProperty
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x000218C5 File Offset: 0x0001FAC5
		public static WPCombatAdvantage MeetsThreshold(Identifier attacker, Identifier target, float threshold = 0.5f)
		{
			return new WPCombatAdvantage
			{
				AdvantageRequired = threshold,
				AttackerID = attacker,
				TargetID = target,
				StatsBoostedByEffect = null,
				HealAttackerRatio = 0f
			};
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000218F3 File Offset: 0x0001FAF3
		public static WPCombatAdvantage MustOneShot(Identifier attacker, Identifier target)
		{
			return new WPCombatAdvantage
			{
				AdvantageRequired = 0.5f,
				IsOneShotKillRequired = true,
				AttackerID = attacker,
				TargetID = target,
				StatsBoostedByEffect = null,
				HealAttackerRatio = 0f
			};
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002192C File Offset: 0x0001FB2C
		public static WPCombatAdvantage BonusAgainst(Identifier target, CombatStats bonus)
		{
			return new WPCombatAdvantage
			{
				AdvantageRequired = 0f,
				AttackerID = Identifier.Invalid,
				TargetID = target,
				StatsBoostedByEffect = bonus,
				HealAttackerRatio = 0f
			};
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002195E File Offset: 0x0001FB5E
		public static WPCombatAdvantage BonusFor(Identifier attackerId, CombatStats bonus)
		{
			return new WPCombatAdvantage
			{
				AdvantageRequired = 0f,
				AttackerID = attackerId,
				TargetID = Identifier.Invalid,
				StatsBoostedByEffect = bonus,
				HealAttackerRatio = 0f
			};
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00021990 File Offset: 0x0001FB90
		public static WPCombatAdvantage FromHealing(Identifier attackerId, float healAttackerRatio = 1f)
		{
			return new WPCombatAdvantage
			{
				AdvantageRequired = 0f,
				AttackerID = attackerId,
				TargetID = Identifier.Invalid,
				StatsBoostedByEffect = new CombatStats(),
				HealAttackerRatio = healAttackerRatio
			};
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000219C2 File Offset: 0x0001FBC2
		private WPCombatAdvantage()
		{
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000219E4 File Offset: 0x0001FBE4
		public float CalcCombatAdvantage(TurnContext context, out bool attackerWillDie, out bool defenderWillDie)
		{
			TurnState currentTurn = context.CurrentTurn;
			GamePiece attacker = currentTurn.FetchGameItem<GamePiece>(this.AttackerID);
			GamePiece target = currentTurn.FetchGameItem<GamePiece>(this.TargetID);
			return GamePiece.CalcCombatAdvantageAtPosition(context, attacker, target, out attackerWillDie, out defenderWillDie);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00021A1A File Offset: 0x0001FC1A
		public float TrueAdvantage(TurnContext context, out bool attackerWillDie, out bool defenderWillDie)
		{
			if (float.IsNaN(this._advantage))
			{
				this._advantage = this.CalcCombatAdvantage(context, out this._attackerWillDie, out this._defenderWillDie);
			}
			attackerWillDie = this._attackerWillDie;
			defenderWillDie = this._defenderWillDie;
			return this._advantage;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00021A58 File Offset: 0x0001FC58
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			bool flag;
			bool flag2;
			float num = this.CalcCombatAdvantage(viewContext, out flag, out flag2);
			return !flag && (!this.IsOneShotKillRequired || flag2) && num > this.AdvantageRequired;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00021A8C File Offset: 0x0001FC8C
		public override void OnAddedAsPrecondition(GOAPNode owner, TurnState turnState, PlayerState playerState)
		{
			if (this.AttackerID == Identifier.Invalid || this.TargetID == Identifier.Invalid)
			{
				throw new Exception("Please use MeetsThreshold for WPCombatAdvantage preconditions");
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00021AAC File Offset: 0x0001FCAC
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPCombatAdvantage wpcombatAdvantage = precondition as WPCombatAdvantage;
			if (wpcombatAdvantage == null)
			{
				return WPProvidesEffect.No;
			}
			bool flag = wpcombatAdvantage.AttackerID != Identifier.Invalid;
			bool flag2 = this.AttackerID != Identifier.Invalid;
			bool flag3 = this.AttackerID == wpcombatAdvantage.AttackerID;
			if (flag && flag2 && !flag3)
			{
				return WPProvidesEffect.No;
			}
			bool flag4 = wpcombatAdvantage.TargetID != Identifier.Invalid;
			bool flag5 = this.TargetID != Identifier.Invalid;
			bool flag6 = this.TargetID == wpcombatAdvantage.TargetID;
			if (flag4 && flag5 && !flag6)
			{
				return WPProvidesEffect.No;
			}
			if (flag && flag4)
			{
				bool flag7;
				bool flag8;
				float num = wpcombatAdvantage.TrueAdvantage(this.OwningPlanner.TrueContext, out flag7, out flag8);
				if (wpcombatAdvantage.IsOneShotKillRequired && !flag7 && flag8)
				{
					return WPProvidesEffect.Redundant;
				}
				if (num > wpcombatAdvantage.AdvantageRequired)
				{
					return WPProvidesEffect.Redundant;
				}
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00021B74 File Offset: 0x0001FD74
		public bool TryCalcBoostedAdvantage(WPCombatAdvantage otherEffect, TurnContext context, out float combatAdvantage)
		{
			CombatStats statsBoostedByEffect = otherEffect.StatsBoostedByEffect;
			float healAttackerRatio = otherEffect.HealAttackerRatio;
			if (statsBoostedByEffect == null && healAttackerRatio == 0f)
			{
				combatAdvantage = -1f;
				return false;
			}
			TurnState currentTurn = context.CurrentTurn;
			GamePiece item = currentTurn.FetchGameItem<GamePiece>(this.AttackerID);
			GamePiece target = currentTurn.FetchGameItem<GamePiece>(this.TargetID);
			GamePiece gamePiece = item.DeepClone<GamePiece>();
			if (statsBoostedByEffect != null)
			{
				gamePiece.CombatStats.Ranged.AddModifier(new StatModifier(statsBoostedByEffect.Ranged, null, ModifierTarget.ValueOffset));
				gamePiece.CombatStats.Melee.AddModifier(new StatModifier(statsBoostedByEffect.Melee, null, ModifierTarget.ValueOffset));
				gamePiece.CombatStats.Infernal.AddModifier(new StatModifier(statsBoostedByEffect.Infernal, null, ModifierTarget.ValueOffset));
			}
			int num = gamePiece.TotalHP - gamePiece.HP;
			gamePiece.HP += (int)((float)num * healAttackerRatio);
			combatAdvantage = GamePiece.CalcCombatAdvantageAtPosition(context, gamePiece, target);
			return true;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00021C6C File Offset: 0x0001FE6C
		public override float CalculateDynamicCostScalar(GOAPPlanner planner, IReadOnlyList<WorldProperty> effects)
		{
			WPCombatAdvantage otherEffect;
			if (!effects.FirstOrDefault(out otherEffect))
			{
				return 1f;
			}
			float num;
			if (!this.TryCalcBoostedAdvantage(otherEffect, planner.TrueContext, out num))
			{
				return 1f;
			}
			if (num > this.AdvantageRequired)
			{
				return GoapNodeCosts.Discount_Scalar_FullyFufilled;
			}
			if (num > this.AdvantageRequired * 0.5f)
			{
				return GoapNodeCosts.Discount_Scalar_PartialFufilled;
			}
			return GoapNodeCosts.Penalty_Scalar_NotFulfilled;
		}

		// Token: 0x04000310 RID: 784
		public float AdvantageRequired;

		// Token: 0x04000311 RID: 785
		public bool IsOneShotKillRequired;

		// Token: 0x04000312 RID: 786
		public Identifier AttackerID = Identifier.Invalid;

		// Token: 0x04000313 RID: 787
		public Identifier TargetID = Identifier.Invalid;

		// Token: 0x04000314 RID: 788
		public CombatStats StatsBoostedByEffect;

		// Token: 0x04000315 RID: 789
		public float HealAttackerRatio;

		// Token: 0x04000316 RID: 790
		private float _advantage = float.NaN;

		// Token: 0x04000317 RID: 791
		private bool _attackerWillDie;

		// Token: 0x04000318 RID: 792
		private bool _defenderWillDie;
	}
}
