using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001B6 RID: 438
	public class PraetorHeuristics
	{
		// Token: 0x06000814 RID: 2068 RVA: 0x00025B58 File Offset: 0x00023D58
		public void Populate(TurnContext turnContext)
		{
			List<Praetor> list = IEnumerableExtensions.ToList<Praetor>(turnContext.CurrentTurn.EnumerateGameItems<Praetor>());
			foreach (Praetor praetor in list)
			{
				this.Data[praetor] = new PraetorData(praetor, list, turnContext);
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00025BC4 File Offset: 0x00023DC4
		public bool TryGetPraetorAdvantage(Praetor praetor, PlayerState targetPlayer, out float advantage)
		{
			advantage = 0f;
			PraetorData praetorData;
			return this.Data.TryGetValue(praetor, out praetorData) && praetorData.PraetorVsArchfiend.TryGetValue(targetPlayer.Id, out advantage);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00025BFC File Offset: 0x00023DFC
		public bool TryGetBestPraetor(PlayerState us, Praetor themPraetor, out Praetor best, out float bestAdvantage, bool alreadyOwned = true)
		{
			best = null;
			bestAdvantage = 0f;
			foreach (KeyValuePair<Praetor, PraetorData> keyValuePair in this.Data)
			{
				Praetor praetor;
				PraetorData praetorData;
				keyValuePair.Deconstruct(out praetor, out praetorData);
				Praetor praetor2 = praetor;
				PraetorData praetorData2 = praetorData;
				if (alreadyOwned && praetorData2.CurrentOwnerId == us.Id)
				{
					PraetorVsPraetorData praetorVsPraetorData;
					if (!praetorData2.PraetorVsPraetor.TryGetValue(themPraetor, out praetorVsPraetorData))
					{
						return false;
					}
					if (praetorVsPraetorData.MaxAdvantage > bestAdvantage)
					{
						bestAdvantage = praetorVsPraetorData.MaxAdvantage;
						best = praetor2;
					}
				}
			}
			return best != null;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00025CAC File Offset: 0x00023EAC
		public bool TryGetBestPraetor(PlayerState us, PlayerState them, out Praetor best, out float bestAdvantage, bool alreadyOwned = true)
		{
			best = null;
			bestAdvantage = 0f;
			foreach (KeyValuePair<Praetor, PraetorData> keyValuePair in this.Data)
			{
				Praetor praetor;
				PraetorData praetorData;
				keyValuePair.Deconstruct(out praetor, out praetorData);
				Praetor praetor2 = praetor;
				PraetorData praetorData2 = praetorData;
				if (alreadyOwned && praetorData2.CurrentOwnerId == us.Id)
				{
					float num;
					if (!praetorData2.PraetorVsArchfiend.TryGetValue(them.Id, out num))
					{
						return false;
					}
					if (num > bestAdvantage)
					{
						bestAdvantage = num;
						best = praetor2;
					}
				}
			}
			return best != null;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00025D58 File Offset: 0x00023F58
		public bool TryGetDuelAdvantage(PlayerState us, PlayerState them, out float advantage)
		{
			advantage = 0.5f;
			Praetor key;
			float num;
			if (!this.TryGetBestPraetor(us, them, out key, out num, true))
			{
				advantage = 0f;
				return true;
			}
			Praetor key2;
			if (!this.TryGetBestPraetor(them, us, out key2, out num, true))
			{
				advantage = 1f;
				return true;
			}
			PraetorData praetorData;
			if (!this.Data.TryGetValue(key, out praetorData))
			{
				return false;
			}
			PraetorVsPraetorData praetorVsPraetorData;
			if (!praetorData.PraetorVsPraetor.TryGetValue(key2, out praetorVsPraetorData))
			{
				return false;
			}
			advantage = praetorVsPraetorData.MaxAdvantage;
			return true;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00025DCC File Offset: 0x00023FCC
		public bool TryGetBestMove(PlayerState us, Praetor ourPraetor, Praetor theirPraetor, out PraetorCombatMoveInstance best, out float bestAveragePower, out bool isBestViable)
		{
			float num = (float)us.PowersLevels[PowerType.Charisma].CurrentLevel.Value;
			best = null;
			bestAveragePower = 0f;
			isBestViable = false;
			PraetorData praetorData;
			if (!this.Data.TryGetValue(ourPraetor, out praetorData))
			{
				return false;
			}
			foreach (KeyValuePair<PraetorCombatMoveInstance, MoveData> keyValuePair in praetorData.Moves)
			{
				PraetorCombatMoveInstance praetorCombatMoveInstance;
				MoveData moveData;
				keyValuePair.Deconstruct(out praetorCombatMoveInstance, out moveData);
				PraetorCombatMoveInstance praetorCombatMoveInstance2 = praetorCombatMoveInstance;
				MoveVsPraetorData moveVsPraetorData;
				if (!moveData.MoveVsPraetor.TryGetValue(theirPraetor, out moveVsPraetorData))
				{
					return false;
				}
				bool flag = moveVsPraetorData.MaxOverkill + num > 0f;
				if (!(!flag & isBestViable) && moveVsPraetorData.AveragePower > bestAveragePower)
				{
					best = praetorCombatMoveInstance2;
					bestAveragePower = moveVsPraetorData.AveragePower;
					isBestViable = flag;
				}
			}
			return best != null;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00025EBC File Offset: 0x000240BC
		public bool TryGetCounterToBestMove(PlayerState us, PlayerState them, Praetor ourPraetor, Praetor theirPraetor, out PraetorCombatMoveInstance bestCounter, out float bestCounterPower, out bool isBestCounterViable)
		{
			bestCounter = null;
			bestCounterPower = 0f;
			isBestCounterViable = false;
			PraetorCombatMoveInstance praetorCombatMoveInstance;
			float num;
			bool flag;
			if (!this.TryGetBestMove(them, theirPraetor, ourPraetor, out praetorCombatMoveInstance, out num, out flag))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Could not retrieve best move for {0}'s {1} versus {2}", them, theirPraetor, ourPraetor));
				}
				return false;
			}
			if (!this.TryGetBestMove(us, ourPraetor, praetorCombatMoveInstance, out bestCounter, out bestCounterPower, out isBestCounterViable))
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error(string.Format("Could not retrieve best move for {0}'s {1} versus {2}", us, ourPraetor, praetorCombatMoveInstance));
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00025F3C File Offset: 0x0002413C
		public bool TryGetBestMove(PlayerState us, Praetor ourPraetor, PraetorCombatMoveInstance theirMove, out PraetorCombatMoveInstance best, out float bestAveragePower, out bool isBestViable)
		{
			float num = (float)us.PowersLevels[PowerType.Charisma].CurrentLevel.Value;
			best = null;
			bestAveragePower = 0f;
			isBestViable = false;
			PraetorData praetorData;
			if (!this.Data.TryGetValue(ourPraetor, out praetorData))
			{
				return false;
			}
			foreach (KeyValuePair<PraetorCombatMoveInstance, MoveData> keyValuePair in praetorData.Moves)
			{
				PraetorCombatMoveInstance praetorCombatMoveInstance;
				MoveData moveData;
				keyValuePair.Deconstruct(out praetorCombatMoveInstance, out moveData);
				PraetorCombatMoveInstance praetorCombatMoveInstance2 = praetorCombatMoveInstance;
				MoveVsMoveData moveVsMoveData;
				if (!moveData.MoveVsMove.TryGetValue(theirMove, out moveVsMoveData))
				{
					return false;
				}
				bool flag = moveVsMoveData.Overkill + num > 0f;
				if (!(!flag & isBestViable) && moveVsMoveData.OverallPower > bestAveragePower)
				{
					best = praetorCombatMoveInstance2;
					bestAveragePower = moveVsMoveData.OverallPower;
					isBestViable = flag;
				}
			}
			return best != null;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002602C File Offset: 0x0002422C
		public bool TryGetDuelResultCertainty(PlayerState us, PlayerState them, Praetor ourPraetor, Praetor theirPraetor, out float certainty)
		{
			certainty = 0f;
			PraetorData praetorData;
			if (!this.Data.TryGetValue(ourPraetor, out praetorData))
			{
				return false;
			}
			PraetorVsPraetorData praetorVsPraetorData;
			if (!praetorData.PraetorVsPraetor.TryGetValue(theirPraetor, out praetorVsPraetorData))
			{
				return false;
			}
			float maxAdvantage = praetorVsPraetorData.MaxAdvantage;
			float num = (float)us.PowersLevels[PowerType.Charisma].CurrentLevel.Value;
			if (praetorVsPraetorData.MaxOverkill + num <= 0f)
			{
				certainty = 1f;
				return true;
			}
			PraetorData praetorData2;
			if (!this.Data.TryGetValue(theirPraetor, out praetorData2))
			{
				return false;
			}
			PraetorVsPraetorData praetorVsPraetorData2;
			if (!praetorData2.PraetorVsPraetor.TryGetValue(ourPraetor, out praetorVsPraetorData2))
			{
				return false;
			}
			float num2 = (float)them.PowersLevels[PowerType.Charisma].CurrentLevel.Value;
			if (praetorVsPraetorData2.MaxOverkill + num2 <= 0f)
			{
				certainty = 1f;
				return true;
			}
			certainty = (float)Math.Abs(Math.Pow((double)(maxAdvantage * 2f - 1f), 2.0));
			return true;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00026128 File Offset: 0x00024328
		public bool TryGetBribeImpactOnDuel(PlayerState us, PlayerState them, Praetor ourPraetor, Praetor theirPraetor, out float importance)
		{
			float num = (float)us.PowersLevels[PowerType.Charisma].CurrentLevel.Value / 6f;
			float num2 = (float)them.PowersLevels[PowerType.Charisma].CurrentLevel.Value / 6f;
			importance = (num + num2) / 2f;
			return true;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00026180 File Offset: 0x00024380
		public bool TryGetDuelImportance(PlayerState us, PlayerState them, Praetor ourPraetor, Praetor theirPraetor, int wager, out float importance)
		{
			float num = Math.Clamp((float)wager / 20f, 0f, 1f);
			float num2 = Math.Clamp((float)ourPraetor.Level / 5f, 0f, 1f);
			importance = (num + num2) / 3f;
			return true;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000261D0 File Offset: 0x000243D0
		public bool TryGetDesiredBribeRatio(PlayerState us, PlayerState them, Praetor ourPraetor, Praetor theirPraetor, int wager, out float bribeRatio)
		{
			bribeRatio = 0.5f;
			float num;
			if (!this.TryGetDuelResultCertainty(us, them, ourPraetor, theirPraetor, out num))
			{
				return false;
			}
			float num2;
			if (!this.TryGetBribeImpactOnDuel(us, them, ourPraetor, theirPraetor, out num2))
			{
				return false;
			}
			float num3;
			if (!this.TryGetDuelImportance(us, them, ourPraetor, theirPraetor, wager, out num3))
			{
				return false;
			}
			bribeRatio = 0.1f + 0.9f * (1f - num) * num2 * num3;
			return true;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00026238 File Offset: 0x00024438
		public bool TryGetDesiredBribeValue(PlayerState us, PlayerState them, Praetor ourPraetor, Praetor theirPraetor, int wager, out int value)
		{
			value = 0;
			float num;
			if (!this.TryGetDesiredBribeRatio(us, them, ourPraetor, theirPraetor, wager, out num))
			{
				return false;
			}
			int valueSum = us.Resources.Total().ValueSum;
			value = (int)((float)valueSum * num);
			return true;
		}

		// Token: 0x040003C7 RID: 967
		public Dictionary<Praetor, PraetorData> Data = new Dictionary<Praetor, PraetorData>();
	}
}
