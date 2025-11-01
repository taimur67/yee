using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004EC RID: 1260
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GrievanceDecisionResponse : DiplomaticDecisionResponse, IGrievanceAccessor, ISelectionAccessor, IGrievanceResponse
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060017CA RID: 6090 RVA: 0x00055FCA File Offset: 0x000541CA
		// (set) Token: 0x060017CB RID: 6091 RVA: 0x00055FD2 File Offset: 0x000541D2
		[JsonProperty]
		public Payment Payment { get; set; } = new Payment();

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060017CC RID: 6092 RVA: 0x00055FDB File Offset: 0x000541DB
		// (set) Token: 0x060017CD RID: 6093 RVA: 0x00055FE3 File Offset: 0x000541E3
		[JsonProperty]
		public GrievanceContext GrievanceResponse { get; set; }

		// Token: 0x060017CE RID: 6094 RVA: 0x00055FEC File Offset: 0x000541EC
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

		// Token: 0x060017CF RID: 6095 RVA: 0x00055FFC File Offset: 0x000541FC
		protected override void Reset()
		{
			base.Reset();
			this.Payment.ClearPayment();
			this.GrievanceResponse = null;
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00056016 File Offset: 0x00054216
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			GrievanceDecisionRequest request;
			if (!player.TryGetDecisionRequest<GrievanceDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x060017D1 RID: 6097 RVA: 0x0005603C File Offset: 0x0005423C
		public override void DeepClone(out DecisionResponse clone)
		{
			GrievanceDecisionResponse grievanceDecisionResponse = new GrievanceDecisionResponse
			{
				Payment = this.Payment.DeepClone<Payment>(),
				GrievanceResponse = this.GrievanceResponse.DeepClone<GrievanceContext>()
			};
			base.DeepCloneDiplomaticDecisionResponseParts(grievanceDecisionResponse);
			clone = grievanceDecisionResponse;
		}
	}
}
