using System;

namespace LoG
{
	// Token: 0x020004AC RID: 1196
	[Serializable]
	public abstract class DecisionRequest<T> : DecisionRequest where T : DecisionResponse, new()
	{
		// Token: 0x0600166E RID: 5742 RVA: 0x00052D00 File Offset: 0x00050F00
		protected DecisionRequest()
		{
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00052D08 File Offset: 0x00050F08
		protected DecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00052D11 File Offset: 0x00050F11
		public override DecisionResponse GenerateResponse()
		{
			return this.GenerateTypedResponse();
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00052D1E File Offset: 0x00050F1E
		public T GenerateTypedResponse()
		{
			T t = Activator.CreateInstance<T>();
			t.DecisionId = this.DecisionId;
			return t;
		}
	}
}
