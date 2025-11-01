using System;

namespace LoG
{
	// Token: 0x020003C0 RID: 960
	public class PraetorDuel_SelectChampion_DecisionProcessor : DecisionProcessor<PraetorDuel_SelectChampion_DecisionRequest, PraetorDuel_SelectionChampion_DecisionResponse>
	{
		// Token: 0x060012D7 RID: 4823 RVA: 0x00047FBC File Offset: 0x000461BC
		protected override Result Preview(PraetorDuel_SelectionChampion_DecisionResponse response)
		{
			PraetorDuelData praetorDuelData;
			if (response.Choice == YesNo.Yes && this.TurnProcessContext.TryGetPraetorDuel(base.request.Contestants, out praetorDuelData))
			{
				praetorDuelData.Defender.SetPraetor(response.Praetor, -1, null);
			}
			return Result.Success;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00048004 File Offset: 0x00046204
		protected override Result Process(PraetorDuel_SelectionChampion_DecisionResponse response)
		{
			PraetorDuelData praetorDuelData;
			if (!this.TurnProcessContext.TryGetPraetorDuel(base.request.Contestants, out praetorDuelData))
			{
				return Result.Failure;
			}
			if (response.Choice != YesNo.Yes)
			{
				return Result.Success;
			}
			PraetorDuelParticipantData praetorDuelParticipantData;
			if (!praetorDuelData.TryGetAssociated(this._player.Id, out praetorDuelParticipantData))
			{
				return Result.Failure;
			}
			praetorDuelParticipantData.SetPraetor(response.Praetor, -1, null);
			return base.Process(response);
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00048070 File Offset: 0x00046270
		protected override Result Validate(PraetorDuel_SelectionChampion_DecisionResponse response)
		{
			YesNo choice = response.Choice;
			Result result;
			if (choice != YesNo.Undefined)
			{
				if (choice != YesNo.Yes)
				{
					result = Result.Success;
				}
				else
				{
					result = ((response.Praetor != Identifier.Invalid) ? Result.Success : Result.Failure);
				}
			}
			else
			{
				result = Result.Failure;
			}
			return result;
		}
	}
}
