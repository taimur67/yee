using System;

namespace LoG
{
	// Token: 0x0200062C RID: 1580
	public class ConsolidateTributeActionProcessor : ActionProcessor<OrderConsolidateTribute>
	{
		// Token: 0x06001D30 RID: 7472 RVA: 0x00064D64 File Offset: 0x00062F64
		public override Result Validate()
		{
			Problem problem = this._player.ValidatePayment(base.request.Payment) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (base.request.Payment.Resources.Count > base._rules.NumResourceSlots)
			{
				return Result.Failure;
			}
			if (base.request.Payment.IsEmpty)
			{
				return Result.Failure;
			}
			if (base.request.Payment.Resources.Count <= 1)
			{
				return Result.Failure;
			}
			if (base.request.Payment.Total.AnyGreaterThan(99, true))
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x00064E18 File Offset: 0x00063018
		public override Result Process(ActionProcessContext context)
		{
			if (base.request.Payment.IsEmpty)
			{
				return Result.Failure;
			}
			ConsolidateTributeEvent consolidateTributeEvent = new ConsolidateTributeEvent(this._player.Id);
			ResourceNFT resourceNFT;
			Problem problem = this.TurnProcessContext.ConsolidateTribute(base.request.Payment, out resourceNFT) as Problem;
			if (problem != null)
			{
				this._player.GiveResources(base.request.Payment.Resources);
				return problem;
			}
			consolidateTributeEvent.ConsolidatedResources = base.request.Payment.Resources;
			consolidateTributeEvent.ResultResource = resourceNFT;
			this._player.GiveResources(new ResourceNFT[]
			{
				resourceNFT
			});
			base._currentTurn.AddGameEvent<ConsolidateTributeEvent>(consolidateTributeEvent);
			return Result.Success;
		}
	}
}
