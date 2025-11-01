using System;

namespace LoG
{
	// Token: 0x0200062A RID: 1578
	public class OrderCancelSchemeProcessor : ActionProcessor<OrderCancelScheme>
	{
		// Token: 0x06001D2C RID: 7468 RVA: 0x00064D31 File Offset: 0x00062F31
		public override Result Process(ActionProcessContext context)
		{
			return base._currentTurn.RemoveScheme(this._player, base.request.SchemeCardId);
		}
	}
}
