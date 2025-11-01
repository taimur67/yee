using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004C5 RID: 1221
	[Serializable]
	public class SelectSchemeDecisionRequest : DecisionRequest<SelectSchemeDecisionResponse>
	{
		// Token: 0x060016D2 RID: 5842 RVA: 0x00053A60 File Offset: 0x00051C60
		[JsonConstructor]
		protected SelectSchemeDecisionRequest()
		{
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00053A7A File Offset: 0x00051C7A
		public SelectSchemeDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00053A95 File Offset: 0x00051C95
		public SelectSchemeDecisionRequest(DecisionId decisionId, params SchemeObjective[] schemes) : this(decisionId, schemes.AsEnumerable<SchemeObjective>())
		{
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00053AA4 File Offset: 0x00051CA4
		public SelectSchemeDecisionRequest(DecisionId decisionId, IEnumerable<SchemeObjective> schemes) : base(decisionId)
		{
			this.Options.AddRange(schemes);
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00053ACB File Offset: 0x00051CCB
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.SchemeOption;
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00053AD0 File Offset: 0x00051CD0
		private IEnumerable<SchemeObjective> GetSelectedSchemes(SelectSchemeDecisionResponse response)
		{
			return from t in this.Options
			where response.Selected.Contains(t.Id)
			select t;
		}

		// Token: 0x04000B3C RID: 2876
		[JsonProperty]
		public List<SchemeObjective> Options = new List<SchemeObjective>();

		// Token: 0x04000B3D RID: 2877
		[JsonProperty]
		[DefaultValue(1)]
		public int NumSelections = 1;
	}
}
