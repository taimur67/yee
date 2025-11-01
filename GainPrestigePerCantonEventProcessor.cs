using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005F9 RID: 1529
	public class GainPrestigePerCantonEventProcessor : GrandEventActionProcessor<GainPrestigePerCantonEventOrder, GainPrestigePerCantonEventStaticData>
	{
		// Token: 0x06001C96 RID: 7318 RVA: 0x00062918 File Offset: 0x00060B18
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			foreach (PlayerState playerState in this.TurnProcessContext.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (!playerState.Excommunicated)
				{
					int prestige = this.TurnProcessContext.HexBoard.GetHexesControlledByPlayer(playerState.Id).Count<Hex>();
					PaymentReceivedEvent ev = this.TurnProcessContext.GivePrestige(playerState, prestige);
					base.GameEvent.AddChildEvent<PaymentReceivedEvent>(ev);
					base.GameEvent.AddAffectedPlayerId(playerState.Id);
				}
			}
			return Result.Success;
		}
	}
}
