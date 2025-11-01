using System;

namespace LoG
{
	// Token: 0x020006D2 RID: 1746
	public static class TurnExtensions
	{
		// Token: 0x06001FEE RID: 8174 RVA: 0x0006DC2C File Offset: 0x0006BE2C
		public static bool IsOfferingTurn(this TurnProcessContext context)
		{
			if (context.Rules.OfferingInterval == 0)
			{
				return false;
			}
			int num = context.CurrentTurn.TurnValue + 1;
			return num >= context.Rules.OfferingFirstTurn && (num - context.Rules.OfferingFirstTurn) % context.Rules.OfferingInterval == 0;
		}
	}
}
