using System;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020001B1 RID: 433
	public class MoveVsMoveData
	{
		// Token: 0x0600080C RID: 2060 RVA: 0x000253C4 File Offset: 0x000235C4
		public static bool DoesPraetorMoveCounter(TurnContext context, PraetorCombatMoveInstance attacker, PraetorCombatMoveInstance defender)
		{
			GameDatabase database = context.Database;
			PraetorCombatMoveStaticData praetorCombatMoveStaticData;
			if (!database.TryFetch<PraetorCombatMoveStaticData>(attacker.CombatMoveReference, out praetorCombatMoveStaticData))
			{
				return false;
			}
			PraetorCombatMoveStaticData praetorCombatMoveStaticData2;
			if (!database.TryFetch<PraetorCombatMoveStaticData>(defender.CombatMoveReference, out praetorCombatMoveStaticData2))
			{
				return true;
			}
			foreach (PraetorCombatMoveEffectData_Counter praetorCombatMoveEffectData_Counter in praetorCombatMoveStaticData.Effects.OfType<PraetorCombatMoveEffectData_Counter>())
			{
				if (praetorCombatMoveEffectData_Counter.StyleRestriction.IsEmpty())
				{
					return true;
				}
				if (praetorCombatMoveEffectData_Counter.StyleRestriction.Equals(praetorCombatMoveStaticData2.TechniqueType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002546C File Offset: 0x0002366C
		private static float ModifyAttack(float attack, PraetorCombatMoveStaticData ourMove, PraetorCombatMoveStaticData theirMove)
		{
			foreach (PraetorCombatMoveEffectData praetorCombatMoveEffectData in ourMove.Effects)
			{
				if (praetorCombatMoveEffectData.StyleRestriction.IsEmpty() || !(praetorCombatMoveEffectData.StyleRestriction.Id != theirMove.TechniqueType.Id))
				{
					float num = attack;
					PraetorCombatMoveEffectData_DamageModifier praetorCombatMoveEffectData_DamageModifier = praetorCombatMoveEffectData as PraetorCombatMoveEffectData_DamageModifier;
					float num2;
					if (praetorCombatMoveEffectData_DamageModifier == null)
					{
						PraetorCombatMoveEffectData_RandomDamageModifier praetorCombatMoveEffectData_RandomDamageModifier = praetorCombatMoveEffectData as PraetorCombatMoveEffectData_RandomDamageModifier;
						if (praetorCombatMoveEffectData_RandomDamageModifier == null)
						{
							num2 = 0f;
						}
						else
						{
							num2 = praetorCombatMoveEffectData_RandomDamageModifier.CalculateExpectedMultiplier() * attack;
						}
					}
					else
					{
						num2 = praetorCombatMoveEffectData_DamageModifier.Value * attack;
					}
					attack = num + num2;
				}
			}
			return attack;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00025520 File Offset: 0x00023720
		private static float CalcMoveSideEffectPower(PraetorCombatMoveStaticData ourMove, PraetorCombatMoveStaticData theirMove, float normalisedPower)
		{
			float num = 0f;
			foreach (PraetorCombatMoveEffectData praetorCombatMoveEffectData in ourMove.Effects)
			{
				if (praetorCombatMoveEffectData.StyleRestriction.IsEmpty() || !(praetorCombatMoveEffectData.StyleRestriction.Id != theirMove.TechniqueType.Id))
				{
					if (!(praetorCombatMoveEffectData is PraetorCombatMoveEffectData_KillOpponentOnDeath))
					{
						if (!(praetorCombatMoveEffectData is PraetorCombatMoveEffectData_Unkillable))
						{
							if (!(praetorCombatMoveEffectData is PraetorCombatMoveEffectData_CaptureOpponent))
							{
								if (!(praetorCombatMoveEffectData is PraetorCombatMoveEffectData_ArbiterCancellation))
								{
									if (praetorCombatMoveEffectData is PraetorCombatMoveEffectData_PrestigeRewardMultiplier)
									{
										num += 0.5f * normalisedPower;
									}
								}
								else
								{
									num += 0.8f;
								}
							}
							else
							{
								num += normalisedPower;
							}
						}
						else
						{
							num += 0.7f * (1f - normalisedPower);
						}
					}
					else
					{
						num += 0.9f * (1f - normalisedPower);
					}
				}
			}
			return num;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0002560C File Offset: 0x0002380C
		public MoveVsMoveData(PraetorCombatMoveInstance move, PraetorCombatMoveInstance otherMove, Praetor praetor, Praetor otherPraetor, TurnContext turnContext)
		{
			PraetorCombatMoveStaticData praetorCombatMoveStaticData;
			if (!turnContext.Database.TryFetch<PraetorCombatMoveStaticData>(move.CombatMoveReference, out praetorCombatMoveStaticData))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Error("Could not retrieve static data for " + move.CombatMoveReference.Id);
				return;
			}
			else
			{
				PraetorCombatMoveStaticData praetorCombatMoveStaticData2;
				if (turnContext.Database.TryFetch<PraetorCombatMoveStaticData>(otherMove.CombatMoveReference, out praetorCombatMoveStaticData2))
				{
					bool flag = MoveVsMoveData.DoesPraetorMoveCounter(turnContext, move, otherMove);
					bool flag2 = MoveVsMoveData.DoesPraetorMoveCounter(turnContext, otherMove, move);
					float num = (float)(praetor.Level - otherPraetor.Level);
					if (!flag2)
					{
						num += MoveVsMoveData.ModifyAttack((float)move.Power, praetorCombatMoveStaticData, praetorCombatMoveStaticData2);
					}
					if (!flag)
					{
						num -= MoveVsMoveData.ModifyAttack((float)otherMove.Power, praetorCombatMoveStaticData2, praetorCombatMoveStaticData);
					}
					this.Overkill = num;
					float num2 = Math.Clamp((num + 10f) / 20f, 0f, 1f);
					this.NormalisedOverkill = num2;
					float num3 = flag2 ? 0f : MoveVsMoveData.CalcMoveSideEffectPower(praetorCombatMoveStaticData, praetorCombatMoveStaticData2, num2);
					this.SideEffectPower = num3;
					float overallPower = 0.6f * num2 + 0.4f * num3;
					this.OverallPower = overallPower;
					return;
				}
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 == null)
				{
					return;
				}
				logger2.Error("Could not retrieve static data for " + otherMove.CombatMoveReference.Id);
				return;
			}
		}

		// Token: 0x040003B9 RID: 953
		public float Overkill;

		// Token: 0x040003BA RID: 954
		public float NormalisedOverkill;

		// Token: 0x040003BB RID: 955
		public float SideEffectPower;

		// Token: 0x040003BC RID: 956
		public float OverallPower;
	}
}
