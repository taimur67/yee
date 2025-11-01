using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004CF RID: 1231
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EnterChainsOfAvariceDecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x06001715 RID: 5909 RVA: 0x0005453B File Offset: 0x0005273B
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			EnterChainsOfAvariceDecisionRequest request;
			if (!player.TryGetDecisionRequest<EnterChainsOfAvariceDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x06001716 RID: 5910 RVA: 0x00054560 File Offset: 0x00052760
		public override void DeepClone(out DecisionResponse clone)
		{
			EnterChainsOfAvariceDecisionResponse enterChainsOfAvariceDecisionResponse = new EnterChainsOfAvariceDecisionResponse();
			base.DeepCloneDiplomaticDecisionResponseParts(enterChainsOfAvariceDecisionResponse);
			clone = enterChainsOfAvariceDecisionResponse;
		}
	}
}
