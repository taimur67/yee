using System;
using Core.StaticData;

namespace LoG
{
	// Token: 0x0200053A RID: 1338
	public class LifetimeTurnModuleProcessor<TModuleInstance, TModuleStaticData> : TurnModuleProcessor<TModuleInstance, TModuleStaticData> where TModuleInstance : TurnModuleInstance where TModuleStaticData : IdentifiableStaticData
	{
		// Token: 0x060019F3 RID: 6643 RVA: 0x0005AB34 File Offset: 0x00058D34
		protected virtual void Process()
		{
			if (base.Instance.Lifetime == -1)
			{
				return;
			}
			if (base._currentTurn.TurnValue >= base.Instance.CreatedTurn + base.Instance.Lifetime)
			{
				base.RemoveSelf();
			}
		}
	}
}
