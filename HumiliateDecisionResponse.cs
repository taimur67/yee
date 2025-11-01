using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004D5 RID: 1237
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HumiliateDecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x0600173D RID: 5949 RVA: 0x00054AC7 File Offset: 0x00052CC7
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

		// Token: 0x0600173E RID: 5950 RVA: 0x00054AD7 File Offset: 0x00052CD7
		protected override void Reset()
		{
			base.Reset();
			this.Payment.ClearPayment();
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x00054AEA File Offset: 0x00052CEA
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			HumiliateDecisionRequest request;
			if (!player.TryGetDecisionRequest<HumiliateDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x06001740 RID: 5952 RVA: 0x00054B10 File Offset: 0x00052D10
		public override void DeepClone(out DecisionResponse clone)
		{
			HumiliateDecisionResponse humiliateDecisionResponse = new HumiliateDecisionResponse
			{
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneDiplomaticDecisionResponseParts(humiliateDecisionResponse);
			clone = humiliateDecisionResponse;
		}

		// Token: 0x04000B4F RID: 2895
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
