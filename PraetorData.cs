using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020001B5 RID: 437
	public class PraetorData
	{
		// Token: 0x06000813 RID: 2067 RVA: 0x000258DC File Offset: 0x00023ADC
		public PraetorData(Praetor praetor, List<Praetor> allPraetors, TurnContext turnContext)
		{
			PlayerState playerState = turnContext.CurrentTurn.FindControllingPlayer(praetor);
			this.CurrentOwnerId = ((playerState != null) ? playerState.Id : int.MinValue);
			foreach (PraetorCombatMoveInstance praetorCombatMoveInstance in praetor.CombatMoves)
			{
				this.Moves[praetorCombatMoveInstance] = new MoveData(praetorCombatMoveInstance, praetor, allPraetors, turnContext);
			}
			foreach (Praetor praetor2 in allPraetors)
			{
				if (praetor2.Id != praetor.Id)
				{
					float num = float.MinValue;
					float num2 = 0f;
					int num3 = 0;
					foreach (MoveData moveData in this.Moves.Values)
					{
						MoveVsPraetorData moveVsPraetorData = moveData.MoveVsPraetor[praetor2];
						num2 += moveVsPraetorData.AveragePower;
						num3++;
						if (moveVsPraetorData.MaxOverkill > num)
						{
							num = moveVsPraetorData.MaxOverkill;
						}
					}
					this.PraetorVsPraetor[praetor2] = new PraetorVsPraetorData(num2 / (float)num3, num);
				}
			}
			foreach (PlayerState playerState2 in turnContext.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState2.Id != this.CurrentOwnerId)
				{
					float num4 = 1f;
					foreach (Praetor key in turnContext.CurrentTurn.GetGameItemsControlledBy<Praetor>(playerState2.Id))
					{
						PraetorVsPraetorData praetorVsPraetorData = this.PraetorVsPraetor[key];
						if (praetorVsPraetorData.MaxAdvantage < num4)
						{
							num4 = praetorVsPraetorData.MaxAdvantage;
						}
					}
					this.PraetorVsArchfiend[playerState2.Id] = num4;
				}
			}
		}

		// Token: 0x040003C3 RID: 963
		public int CurrentOwnerId = int.MinValue;

		// Token: 0x040003C4 RID: 964
		public Dictionary<PraetorCombatMoveInstance, MoveData> Moves = new Dictionary<PraetorCombatMoveInstance, MoveData>();

		// Token: 0x040003C5 RID: 965
		public Dictionary<Praetor, PraetorVsPraetorData> PraetorVsPraetor = new Dictionary<Praetor, PraetorVsPraetorData>();

		// Token: 0x040003C6 RID: 966
		public Dictionary<int, float> PraetorVsArchfiend = new Dictionary<int, float>();
	}
}
