using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004DA RID: 1242
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EnterLureOfExcessDecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x06001762 RID: 5986 RVA: 0x000550D2 File Offset: 0x000532D2
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			EnterLureOfExcessDecisionRequest request;
			if (!player.TryGetDecisionRequest<EnterLureOfExcessDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x06001763 RID: 5987 RVA: 0x000550F8 File Offset: 0x000532F8
		public override void DeepClone(out DecisionResponse clone)
		{
			EnterLureOfExcessDecisionResponse enterLureOfExcessDecisionResponse = new EnterLureOfExcessDecisionResponse();
			base.DeepCloneDiplomaticDecisionResponseParts(enterLureOfExcessDecisionResponse);
			clone = enterLureOfExcessDecisionResponse;
		}
	}
}
