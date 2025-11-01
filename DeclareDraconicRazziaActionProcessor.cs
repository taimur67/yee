using System;

namespace LoG
{
	// Token: 0x020005D9 RID: 1497
	public class DeclareDraconicRazziaActionProcessor : DiplomaticActionProcessor<OrderDeclareDraconicRazzia, DiplomaticAbility_DeclareDraconicRazzia>
	{
		// Token: 0x06001C1C RID: 7196 RVA: 0x000611FC File Offset: 0x0005F3FC
		public override Result Enact(OrderDeclareDraconicRazzia order)
		{
			DiplomaticPairStatus diplomaticStatus = base._currentTurn.GetDiplomaticStatus(this._player.Id, order.TargetID);
			if (base.data.TurnDelay > 0)
			{
				diplomaticStatus.SetDiplomacyPending(this.TurnProcessContext, new PendingDiplomacy_DraconicRazzia(this._player.Id, base.data.TurnDelay, base.data.Duration));
			}
			else
			{
				diplomaticStatus.ChangeState<DiplomaticState_DraconicRazzia>(this.TurnProcessContext, new DiplomaticState_DraconicRazzia(this._player.Id, base.data.Duration + 1), false);
			}
			return Result.Success;
		}
	}
}
