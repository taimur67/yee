using System;

namespace LoG
{
	// Token: 0x02000541 RID: 1345
	public interface IDynamicObjective
	{
		// Token: 0x06001A16 RID: 6678
		ObjectiveDifficulty CalculateDifficulty(TurnContext context, PlayerState player);
	}
}
