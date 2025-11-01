using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004F9 RID: 1273
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestBribeEventDecisionResponse : GrandEventDecisionResponse
	{
		// Token: 0x06001825 RID: 6181 RVA: 0x00056BAE File Offset: 0x00054DAE
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

		// Token: 0x06001826 RID: 6182 RVA: 0x00056BBE File Offset: 0x00054DBE
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			RequestBribeEventDecisionRequest requestBribeEventDecisionRequest;
			if (!player.TryGetDecisionRequest<RequestBribeEventDecisionRequest>(base.DecisionId, out requestBribeEventDecisionRequest))
			{
				yield break;
			}
			yield return new ActionPhase_Tribute(new Action<Payment>(this.SetPayment), requestBribeEventDecisionRequest.Cost);
			yield break;
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00056BD5 File Offset: 0x00054DD5
		private void SetPayment(Payment payment)
		{
			this.Payment = payment;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x00056BE0 File Offset: 0x00054DE0
		public override void DeepClone(out DecisionResponse clone)
		{
			RequestBribeEventDecisionResponse requestBribeEventDecisionResponse = new RequestBribeEventDecisionResponse
			{
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneGrandEventDecisionResponseParts(requestBribeEventDecisionResponse);
			clone = requestBribeEventDecisionResponse;
		}

		// Token: 0x04000B79 RID: 2937
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
