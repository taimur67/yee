using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004F2 RID: 1266
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VileCalumnyResponse : InsultDecisionResponse, IGrievanceResponse
	{
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x000566CC File Offset: 0x000548CC
		// (set) Token: 0x060017FB RID: 6139 RVA: 0x000566D4 File Offset: 0x000548D4
		[JsonProperty]
		public Payment Payment { get; set; } = new Payment();

		// Token: 0x060017FC RID: 6140 RVA: 0x000566DD File Offset: 0x000548DD
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

		// Token: 0x060017FD RID: 6141 RVA: 0x000566ED File Offset: 0x000548ED
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			VileCalumnyRequest request;
			if (!player.TryGetDecisionRequest<VileCalumnyRequest>(base.DecisionId, out request))
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

		// Token: 0x060017FE RID: 6142 RVA: 0x00056714 File Offset: 0x00054914
		public override void DeepClone(out DecisionResponse clone)
		{
			VileCalumnyResponse vileCalumnyResponse = new VileCalumnyResponse();
			base.DeepCloneInsultDecisionResponseParts(vileCalumnyResponse);
			vileCalumnyResponse.Payment = this.Payment;
			clone = vileCalumnyResponse;
		}
	}
}
