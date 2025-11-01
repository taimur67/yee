using System;
using LoG.Simulation;

namespace LoG
{
	// Token: 0x0200012B RID: 299
	public class RespondToUpkeep : DecisionResponseGOAPNode<UpkeepDecisionRequest, UpkeepDecisionResponse>
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001A604 File Offset: 0x00018804
		public override ActionID ID
		{
			get
			{
				return ActionID.Pay_Upkeep;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001A608 File Offset: 0x00018808
		public override string ActionName
		{
			get
			{
				return "Pay Upkeep";
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001A60F File Offset: 0x0001880F
		public RespondToUpkeep(UpkeepDecisionRequest request)
		{
			this.Request = request;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001A61E File Offset: 0x0001881E
		public override void Prepare()
		{
			base.AddPrecondition(new WPTribute(this.Request.RequiredPayment));
			base.AddEffect(new WPLegionUpkeep(this.Request.GameItemId));
			base.Prepare();
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001A654 File Offset: 0x00018854
		protected override UpkeepDecisionResponse GenerateDecision()
		{
			UpkeepDecisionResponse upkeepDecisionResponse = base.GenerateDecision();
			upkeepDecisionResponse.Choice = YesNo.No;
			PlayerState aipreviewPlayerState = this.OwningPlanner.AIPreviewPlayerState;
			if (!aipreviewPlayerState.CanAfford(this.Request.RequiredPayment))
			{
				return upkeepDecisionResponse;
			}
			Payment payment = PaymentUtils.DeducePayment(aipreviewPlayerState, this.Request.RequiredPayment, PaymentUtils.AutoPayMethods.Optimal, 8);
			if (payment == null)
			{
				return upkeepDecisionResponse;
			}
			upkeepDecisionResponse.SetPayment(payment);
			upkeepDecisionResponse.Choice = YesNo.Yes;
			return upkeepDecisionResponse;
		}
	}
}
