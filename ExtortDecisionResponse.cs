using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004D2 RID: 1234
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ExtortDecisionResponse : DiplomaticDecisionResponse
	{
		// Token: 0x06001722 RID: 5922 RVA: 0x000546B7 File Offset: 0x000528B7
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

		// Token: 0x06001723 RID: 5923 RVA: 0x000546C7 File Offset: 0x000528C7
		protected override void Reset()
		{
			base.Reset();
			this.Payment.ClearPayment();
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x000546DA File Offset: 0x000528DA
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			foreach (ActionPhase actionPhase in base.GetActionPhaseSteps(player, turn, database))
			{
				yield return actionPhase;
			}
			IEnumerator<ActionPhase> enumerator = null;
			ExtortDecisionRequest request;
			if (!player.TryGetDecisionRequest<ExtortDecisionRequest>(base.DecisionId, out request))
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

		// Token: 0x06001725 RID: 5925 RVA: 0x00054700 File Offset: 0x00052900
		public override void DeepClone(out DecisionResponse clone)
		{
			ExtortDecisionResponse extortDecisionResponse = new ExtortDecisionResponse
			{
				ExtortedItem = this.ExtortedItem,
				Payment = this.Payment.DeepClone<Payment>()
			};
			base.DeepCloneDiplomaticDecisionResponseParts(extortDecisionResponse);
			clone = extortDecisionResponse;
		}

		// Token: 0x04000B4B RID: 2891
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier ExtortedItem = Identifier.Invalid;

		// Token: 0x04000B4C RID: 2892
		[JsonProperty]
		public Payment Payment = new Payment();
	}
}
