using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004E7 RID: 1255
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SendEmissaryDecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x060017A7 RID: 6055 RVA: 0x00055B43 File Offset: 0x00053D43
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

		// Token: 0x060017A8 RID: 6056 RVA: 0x00055B53 File Offset: 0x00053D53
		protected override void Reset()
		{
			base.Reset();
			this.Payment.ClearPayment();
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00055B66 File Offset: 0x00053D66
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			SendEmissaryDecisionRequest request;
			if (!player.TryGetDecisionRequest<SendEmissaryDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x060017AA RID: 6058 RVA: 0x00055B8C File Offset: 0x00053D8C
		public override void DeepClone(out DecisionResponse clone)
		{
			SendEmissaryDecisionResponse sendEmissaryDecisionResponse = new SendEmissaryDecisionResponse
			{
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneDiplomaticDecisionResponseParts(sendEmissaryDecisionResponse);
			clone = sendEmissaryDecisionResponse;
		}

		// Token: 0x04000B5D RID: 2909
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
