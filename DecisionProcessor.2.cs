using System;

namespace LoG
{
	// Token: 0x020004A2 RID: 1186
	public abstract class DecisionProcessor<Request, Response> : DecisionProcessor where Request : DecisionRequest where Response : DecisionResponse
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x00052741 File Offset: 0x00050941
		protected new Request request
		{
			get
			{
				return (Request)((object)this.request);
			}
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0005274E File Offset: 0x0005094E
		public sealed override Type GetRequestType()
		{
			return typeof(Request);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0005275A File Offset: 0x0005095A
		public sealed override Type GetResponseType()
		{
			return typeof(Response);
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00052766 File Offset: 0x00050966
		public sealed override Result Process(DecisionResponse response)
		{
			return this.Process((Response)((object)response));
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00052774 File Offset: 0x00050974
		public sealed override Result Preview(DecisionResponse response)
		{
			return this.Preview((Response)((object)response));
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00052782 File Offset: 0x00050982
		public sealed override Result Validate(DecisionResponse response)
		{
			return this.Validate((Response)((object)response));
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00052790 File Offset: 0x00050990
		protected virtual Result Process(Response response)
		{
			return Result.Success;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00052797 File Offset: 0x00050997
		protected virtual Result Preview(Response response)
		{
			return Result.Success;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0005279E File Offset: 0x0005099E
		protected virtual Result Validate(Response response)
		{
			return Result.Success;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x000527A5 File Offset: 0x000509A5
		public sealed override DecisionResponse GenerateFallbackResponse()
		{
			return this.GenerateTypedFallbackResponse();
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x000527B2 File Offset: 0x000509B2
		protected virtual Response GenerateTypedFallbackResponse()
		{
			return (Response)((object)this.request.GenerateResponse());
		}
	}
}
