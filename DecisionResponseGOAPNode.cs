using System;

namespace LoG
{
	// Token: 0x02000124 RID: 292
	public abstract class DecisionResponseGOAPNode<TRequest, TResponse> : GOAPNode where TRequest : DecisionRequest where TResponse : DecisionResponse
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0001A009 File Offset: 0x00018209
		public override GOAPNodeType NodeType
		{
			get
			{
				return GOAPNodeType.Decision;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001A00C File Offset: 0x0001820C
		protected TResponse Decision
		{
			get
			{
				TResponse result;
				if ((result = this.Response) == null)
				{
					result = (this.Response = this.GenerateDecision());
				}
				return result;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001A037 File Offset: 0x00018237
		public override bool ConsumesActionSlot
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A042 File Offset: 0x00018242
		protected virtual TResponse GenerateDecision()
		{
			return this.Request.GenerateResponse() as TResponse;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A05E File Offset: 0x0001825E
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			this.OwningPlanner.SetDecisionHandledByPlan(this.Request, this.Decision);
			return Result.Success;
		}

		// Token: 0x040002AB RID: 683
		public TRequest Request;

		// Token: 0x040002AC RID: 684
		public TResponse Response;
	}
}
