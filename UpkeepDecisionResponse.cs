using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004CC RID: 1228
	[Serializable]
	public class UpkeepDecisionResponse : DecisionResponse
	{
		// Token: 0x06001705 RID: 5893 RVA: 0x000543AF File Offset: 0x000525AF
		public override IEnumerable<ResourceNFT> GetReservedResources()
		{
			foreach (ResourceNFT resourceNFT in base.GetReservedResources())
			{
				yield return resourceNFT;
			}
			IEnumerator<ResourceNFT> enumerator = null;
			foreach (ResourceNFT resourceNFT2 in this.UpkeepPayment.Resources)
			{
				yield return resourceNFT2;
			}
			List<ResourceNFT>.Enumerator enumerator2 = default(List<ResourceNFT>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000543BF File Offset: 0x000525BF
		public void SetPayment(Payment payment)
		{
			this.UpkeepPayment = payment;
			this.Choice = YesNo.Yes;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x000543CF File Offset: 0x000525CF
		private Result ValidatePayment(TurnContext context, Payment target, int castingPlayerId)
		{
			return Result.Success;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x000543D6 File Offset: 0x000525D6
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			UpkeepDecisionRequest upkeepDecisionRequest = (UpkeepDecisionRequest)player.DecisionRequests.First((DecisionRequest t) => t.DecisionId == base.DecisionId);
			ActionPhase_Tribute actionPhase_Tribute = new ActionPhase_Tribute(new Action<Payment>(this.SetPayment), new ActionPhase_SingleTarget<Payment>.IsValidFunc(this.ValidatePayment))
			{
				AllowIncompletePayments = true,
				Cost = upkeepDecisionRequest.RequiredPayment,
				ExistingPayment = this.UpkeepPayment
			};
			yield return actionPhase_Tribute;
			yield break;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x000543F0 File Offset: 0x000525F0
		public override void DeepClone(out DecisionResponse clone)
		{
			UpkeepDecisionResponse upkeepDecisionResponse = new UpkeepDecisionResponse
			{
				Choice = this.Choice,
				UpkeepPayment = this.UpkeepPayment.DeepClone<Payment>()
			};
			base.DeepCloneDecisionResponseParts(upkeepDecisionResponse);
			clone = upkeepDecisionResponse;
		}

		// Token: 0x04000B46 RID: 2886
		[JsonProperty]
		public YesNo Choice;

		// Token: 0x04000B47 RID: 2887
		[JsonProperty]
		public Payment UpkeepPayment = new Payment();
	}
}
