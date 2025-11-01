using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004DE RID: 1246
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MakeDemandDecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x06001774 RID: 6004 RVA: 0x000552FE File Offset: 0x000534FE
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

		// Token: 0x06001775 RID: 6005 RVA: 0x0005530E File Offset: 0x0005350E
		protected override void Reset()
		{
			base.Reset();
			this.Payment.ClearPayment();
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00055321 File Offset: 0x00053521
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			MakeDemandDecisionRequest request;
			if (!player.TryGetDecisionRequest<MakeDemandDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x06001777 RID: 6007 RVA: 0x00055348 File Offset: 0x00053548
		public override void DeepClone(out DecisionResponse clone)
		{
			MakeDemandDecisionResponse makeDemandDecisionResponse = new MakeDemandDecisionResponse
			{
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneDiplomaticDecisionResponseParts(makeDemandDecisionResponse);
			clone = makeDemandDecisionResponse;
		}

		// Token: 0x04000B55 RID: 2901
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
