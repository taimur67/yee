using System;

namespace LoG
{
	// Token: 0x0200073A RID: 1850
	public class RefreshBazaarProcessor : EdictEffectModuleProcessor<RefreshBazaarInstance, RefreshBazaarEffectStaticData>
	{
		// Token: 0x060022E2 RID: 8930 RVA: 0x000792A5 File Offset: 0x000774A5
		public override void OnAdded()
		{
			this.TurnProcessContext.RePopulateBazaar(true);
			base.RemoveSelf();
		}
	}
}
