using System;

namespace LoG
{
	// Token: 0x02000710 RID: 1808
	public class DiscountBazaarProcessor : EdictEffectModuleProcessor<DiscountBazaarInstance, DiscountBazaarEffectStaticData>
	{
		// Token: 0x0600228D RID: 8845 RVA: 0x0007847C File Offset: 0x0007667C
		public override void OnAdded()
		{
			foreach (GameItem item in base._currentTurn.GetAllBazaarItems())
			{
				item.ReduceItemCost(this._staticData.DiscountAmount);
			}
			base.RemoveSelf();
		}
	}
}
