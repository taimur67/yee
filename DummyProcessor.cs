using System;

namespace LoG
{
	// Token: 0x0200049C RID: 1180
	public class DummyProcessor : ActionProcessor
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x000521D5 File Offset: 0x000503D5
		public override Type GetOrderType()
		{
			return this.request.GetType();
		}
	}
}
