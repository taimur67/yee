using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001B3 RID: 435
	public class MoveData
	{
		// Token: 0x06000811 RID: 2065 RVA: 0x0002575C File Offset: 0x0002395C
		public MoveData(PraetorCombatMoveInstance move, Praetor praetor, List<Praetor> allPraetors, TurnContext turnContext)
		{
			PraetorCombatMoveStaticData praetorCombatMoveStaticData;
			if (turnContext.Database.TryFetch<PraetorCombatMoveStaticData>(move.CombatMoveReference, out praetorCombatMoveStaticData))
			{
				foreach (Praetor praetor2 in allPraetors)
				{
					if (praetor2.Id != praetor.Id)
					{
						int num = 0;
						float num2 = 0f;
						float num3 = float.MinValue;
						foreach (PraetorCombatMoveInstance praetorCombatMoveInstance in praetor2.CombatMoves)
						{
							MoveVsMoveData moveVsMoveData = new MoveVsMoveData(move, praetorCombatMoveInstance, praetor, praetor2, turnContext);
							this.MoveVsMove[praetorCombatMoveInstance] = moveVsMoveData;
							if (moveVsMoveData.Overkill > num3)
							{
								num3 = moveVsMoveData.Overkill;
							}
							num2 += moveVsMoveData.OverallPower;
							num++;
						}
						float averagePower = (num == 0) ? 0f : (num2 / (float)num);
						this.MoveVsPraetor[praetor2] = new MoveVsPraetorData(averagePower, num3);
					}
				}
				return;
			}
			SimLogger logger = SimLogger.Logger;
			if (logger == null)
			{
				return;
			}
			logger.Error("Could not retrieve static data for " + move.CombatMoveReference.Id);
		}

		// Token: 0x040003BF RID: 959
		public Dictionary<PraetorCombatMoveInstance, MoveVsMoveData> MoveVsMove = new Dictionary<PraetorCombatMoveInstance, MoveVsMoveData>();

		// Token: 0x040003C0 RID: 960
		public Dictionary<Praetor, MoveVsPraetorData> MoveVsPraetor = new Dictionary<Praetor, MoveVsPraetorData>();
	}
}
