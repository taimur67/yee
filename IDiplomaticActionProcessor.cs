using System;

namespace LoG
{
	// Token: 0x020005CE RID: 1486
	public interface IDiplomaticActionProcessor
	{
		// Token: 0x06001BF0 RID: 7152
		Result Enact(DiplomaticOrder request);

		// Token: 0x06001BF1 RID: 7153
		Result Validate();
	}
}
