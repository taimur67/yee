using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004BC RID: 1212
	[Serializable]
	public class SelectEventCardRequest : DecisionRequest<SelectEventCardResponse>
	{
		// Token: 0x060016AA RID: 5802 RVA: 0x000532B9 File Offset: 0x000514B9
		[JsonConstructor]
		public SelectEventCardRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060016AB RID: 5803 RVA: 0x000532C4 File Offset: 0x000514C4
		[JsonIgnore]
		public IEnumerable<Identifier> CardOptions
		{
			get
			{
				if (this.ExistingCards != null)
				{
					return this.CandidateCards.Concat(this.ExistingCards);
				}
				return this.CandidateCards;
			}
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x000532F3 File Offset: 0x000514F3
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.None;
		}

		// Token: 0x04000B34 RID: 2868
		public List<Identifier> ExistingCards;

		// Token: 0x04000B35 RID: 2869
		public List<Identifier> CandidateCards;
	}
}
