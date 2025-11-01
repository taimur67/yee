using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004E1 RID: 1249
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestToBeBloodLordDecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x06001786 RID: 6022 RVA: 0x0005563D File Offset: 0x0005383D
		protected override void Reset()
		{
			base.Reset();
			this.Payment.ClearPayment();
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00055650 File Offset: 0x00053850
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			RequestToBeBloodLordDecisionRequest request;
			if (!player.TryGetDecisionRequest<RequestToBeBloodLordDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x06001788 RID: 6024 RVA: 0x00055678 File Offset: 0x00053878
		public override void DeepClone(out DecisionResponse clone)
		{
			RequestToBeBloodLordDecisionResponse requestToBeBloodLordDecisionResponse = new RequestToBeBloodLordDecisionResponse
			{
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneDiplomaticDecisionResponseParts(requestToBeBloodLordDecisionResponse);
			clone = requestToBeBloodLordDecisionResponse;
		}

		// Token: 0x04000B56 RID: 2902
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
