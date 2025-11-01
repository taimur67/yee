using System;

namespace LoG
{
	// Token: 0x020004A1 RID: 1185
	public abstract class DecisionProcessor : ProcessorBase
	{
		// Token: 0x06001634 RID: 5684 RVA: 0x000526FF File Offset: 0x000508FF
		public DecisionProcessor Configure(TurnProcessContext context, PlayerState player, DecisionRequest req)
		{
			this.TurnProcessContext = context;
			this._player = player;
			this.request = req;
			return this;
		}

		// Token: 0x06001635 RID: 5685
		public abstract Type GetRequestType();

		// Token: 0x06001636 RID: 5686
		public abstract Type GetResponseType();

		// Token: 0x06001637 RID: 5687 RVA: 0x00052717 File Offset: 0x00050917
		public virtual Result Process(DecisionResponse response)
		{
			return Result.Success;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0005271E File Offset: 0x0005091E
		public virtual Result Preview(DecisionResponse response)
		{
			return Result.Success;
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00052725 File Offset: 0x00050925
		public virtual Result Validate(DecisionResponse response)
		{
			return Result.Success;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0005272C File Offset: 0x0005092C
		public virtual DecisionResponse GenerateFallbackResponse()
		{
			return this.request.GenerateResponse();
		}

		// Token: 0x04000B10 RID: 2832
		protected PlayerState _player;

		// Token: 0x04000B11 RID: 2833
		protected DecisionRequest request;
	}
}
