using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004E4 RID: 1252
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RequestToBeVassalizedDecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x06001793 RID: 6035 RVA: 0x0005588D File Offset: 0x00053A8D
		protected override void Reset()
		{
			base.Reset();
			this.Payment.ClearPayment();
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000558A0 File Offset: 0x00053AA0
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			RequestToBeVassalizedDecisionRequest request;
			if (!player.TryGetDecisionRequest<RequestToBeVassalizedDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x06001795 RID: 6037 RVA: 0x000558C8 File Offset: 0x00053AC8
		public override void DeepClone(out DecisionResponse clone)
		{
			RequestToBeVassalizedDecisionResponse requestToBeVassalizedDecisionResponse = new RequestToBeVassalizedDecisionResponse
			{
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneDiplomaticDecisionResponseParts(requestToBeVassalizedDecisionResponse);
			clone = requestToBeVassalizedDecisionResponse;
		}

		// Token: 0x04000B57 RID: 2903
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
