using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004F4 RID: 1268
	[Serializable]
	public class DiplomaticDecisionResponse : DecisionResponse
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001809 RID: 6153 RVA: 0x00056AC8 File Offset: 0x00054CC8
		// (set) Token: 0x0600180A RID: 6154 RVA: 0x00056AD0 File Offset: 0x00054CD0
		[JsonProperty]
		public virtual YesNo Choice { get; set; }

		// Token: 0x0600180B RID: 6155 RVA: 0x00056AD9 File Offset: 0x00054CD9
		protected void SetChoice(YesNo choice)
		{
			this.Choice = choice;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00056AE2 File Offset: 0x00054CE2
		public override IEnumerable<ActionPhase> GetActionPhaseSteps(PlayerState player, TurnState turn, GameDatabase database)
		{
			if (this.Choice != YesNo.Undefined)
			{
				yield return new ActionPhase_MessageBox(ActionMessageType.ResolvedAction, new Action(this.Reset), null);
			}
			yield break;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x00056AF2 File Offset: 0x00054CF2
		protected virtual void Reset()
		{
			this.Choice = YesNo.Undefined;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00056AFB File Offset: 0x00054CFB
		protected void DeepCloneDiplomaticDecisionResponseParts(DiplomaticDecisionResponse diplomaticDecisionResponse)
		{
			diplomaticDecisionResponse.Choice = this.Choice;
			base.DeepCloneDecisionResponseParts(diplomaticDecisionResponse);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x00056B10 File Offset: 0x00054D10
		public override void DeepClone(out DecisionResponse clone)
		{
			DiplomaticDecisionResponse diplomaticDecisionResponse = new DiplomaticDecisionResponse();
			this.DeepCloneDiplomaticDecisionResponseParts(diplomaticDecisionResponse);
			clone = diplomaticDecisionResponse;
		}
	}
}
