using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200012A RID: 298
	public class RespondToTributeSelect : DecisionResponseGOAPNode<SelectTributeDecisionRequest, SelectTributeDecisionResponse>
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001A502 File Offset: 0x00018702
		public override ActionID ID
		{
			get
			{
				return ActionID.Decision_RespondToTributeSelect;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001A506 File Offset: 0x00018706
		public override string ActionName
		{
			get
			{
				return "Respond to Tribute Select";
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001A50D File Offset: 0x0001870D
		public RespondToTributeSelect(SelectTributeDecisionRequest request, IEnumerable<int> selection)
		{
			this.Request = request;
			this.Selection = IEnumerableExtensions.ToList<int>(selection);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001A528 File Offset: 0x00018728
		public override void Prepare()
		{
			Cost cost = new Cost();
			foreach (ResourceNFT resourceNFT in this.Request.Candidates.Tokens)
			{
				cost.Accumulate(new ResourceAccumulation[]
				{
					resourceNFT.Values
				});
			}
			base.AddEffect(new WPTribute(cost));
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001A5A8 File Offset: 0x000187A8
		protected override SelectTributeDecisionResponse GenerateDecision()
		{
			SelectTributeDecisionResponse selectTributeDecisionResponse = base.GenerateDecision();
			foreach (int selectedId in this.Selection)
			{
				selectTributeDecisionResponse.Select(selectedId);
			}
			return selectTributeDecisionResponse;
		}

		// Token: 0x040002B6 RID: 694
		public List<int> Selection;
	}
}
