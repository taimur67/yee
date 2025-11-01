using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003BF RID: 959
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorDuel_SelectionChampion_DecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x060012CE RID: 4814 RVA: 0x00047EFA File Offset: 0x000460FA
		protected override void Reset()
		{
			base.Reset();
			this.Praetor = Identifier.Invalid;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00047F09 File Offset: 0x00046109
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			PraetorDuel_SelectChampion_DecisionRequest request;
			if (!player.TryGetDecisionRequest<PraetorDuel_SelectChampion_DecisionRequest>(base.DecisionId, out request))
			{
				yield break;
			}
			yield return new ActionPhase_DiplomaticResponse
			{
				Request = request,
				Response = this
			};
			yield break;
			yield break;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00047F2E File Offset: 0x0004612E
		private Result IsValidPraetor(TurnContext context, List<Identifier> selected, Identifier target, int castingplayerid)
		{
			if (context.CurrentTurn.FindControllingPlayer(target).Id == castingplayerid)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00047F50 File Offset: 0x00046150
		public void SetPraetor(Identifier id)
		{
			this.Praetor = id;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00047F59 File Offset: 0x00046159
		private Result IsValidOption(TurnContext context, YesNo target, int castingplayerid)
		{
			if (target == YesNo.Yes || target == YesNo.No)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00047F6E File Offset: 0x0004616E
		public void SetResponse(YesNo response)
		{
			this.Choice = response;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00047F78 File Offset: 0x00046178
		public override void DeepClone(out DecisionResponse clone)
		{
			PraetorDuel_SelectionChampion_DecisionResponse praetorDuel_SelectionChampion_DecisionResponse = new PraetorDuel_SelectionChampion_DecisionResponse
			{
				Praetor = this.Praetor
			};
			base.DeepCloneDiplomaticDecisionResponseParts(praetorDuel_SelectionChampion_DecisionResponse);
			clone = praetorDuel_SelectionChampion_DecisionResponse;
		}

		// Token: 0x040008C0 RID: 2240
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier Praetor = Identifier.Invalid;
	}
}
