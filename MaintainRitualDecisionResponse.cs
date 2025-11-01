using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004A8 RID: 1192
	[Serializable]
	public class MaintainRitualDecisionResponse : DecisionResponse
	{
		// Token: 0x0600165C RID: 5724 RVA: 0x00052AAA File Offset: 0x00050CAA
		public override IEnumerable<ResourceNFT> GetReservedResources()
		{
			foreach (ResourceNFT resourceNFT in base.GetReservedResources())
			{
				yield return resourceNFT;
			}
			IEnumerator<ResourceNFT> enumerator = null;
			foreach (ResourceNFT resourceNFT2 in this.Payment.Resources)
			{
				yield return resourceNFT2;
			}
			List<ResourceNFT>.Enumerator enumerator2 = default(List<ResourceNFT>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00052ABA File Offset: 0x00050CBA
		public void SetPayment(Payment payment)
		{
			this.Payment = payment;
			this.Choice = YesNo.Yes;
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00052ACA File Offset: 0x00050CCA
		private Result ValidatePayment(TurnContext context, Payment target, int castingPlayerId)
		{
			return Result.Success;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00052AD1 File Offset: 0x00050CD1
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			MaintainRitualDecisionRequest maintainRitualDecisionRequest = (MaintainRitualDecisionRequest)player.DecisionRequests.First((DecisionRequest t) => t.DecisionId == base.DecisionId);
			if (maintainRitualDecisionRequest.RequiredPayment.IsZero)
			{
				yield break;
			}
			ActionPhase_Tribute actionPhase_Tribute = new ActionPhase_Tribute(new Action<Payment>(this.SetPayment), new ActionPhase_SingleTarget<Payment>.IsValidFunc(this.ValidatePayment))
			{
				AllowIncompletePayments = true,
				Cost = maintainRitualDecisionRequest.RequiredPayment,
				ExistingPayment = this.Payment
			};
			yield return actionPhase_Tribute;
			yield break;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00052AE8 File Offset: 0x00050CE8
		public override void DeepClone(out DecisionResponse clone)
		{
			MaintainRitualDecisionResponse maintainRitualDecisionResponse = new MaintainRitualDecisionResponse
			{
				Choice = this.Choice,
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneDecisionResponseParts(maintainRitualDecisionResponse);
			clone = maintainRitualDecisionResponse;
		}

		// Token: 0x04000B1B RID: 2843
		[JsonProperty]
		public YesNo Choice;

		// Token: 0x04000B1C RID: 2844
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
