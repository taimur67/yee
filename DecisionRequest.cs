using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004AB RID: 1195
	[Serializable]
	public abstract class DecisionRequest
	{
		// Token: 0x06001669 RID: 5737 RVA: 0x00052CD8 File Offset: 0x00050ED8
		protected DecisionRequest()
		{
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00052CE7 File Offset: 0x00050EE7
		protected DecisionRequest(DecisionId decisionId)
		{
			this.DecisionId = decisionId;
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00052CFD File Offset: 0x00050EFD
		public virtual bool DecisionRequired(TurnContext context)
		{
			return true;
		}

		// Token: 0x0600166C RID: 5740
		public abstract DecisionResponse GenerateResponse();

		// Token: 0x0600166D RID: 5741
		public abstract TurnLogEntryType GetTurnLogEntryType();

		// Token: 0x04000B1F RID: 2847
		[JsonProperty]
		[DefaultValue(DecisionId.Invalid)]
		public DecisionId DecisionId = DecisionId.Invalid;
	}
}
