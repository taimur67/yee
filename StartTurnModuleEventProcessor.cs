using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200061A RID: 1562
	public class StartTurnModuleEventProcessor : GrandEventActionProcessor<StartTurnModuleEventOrder, StartTurnModuleEventStaticData>
	{
		// Token: 0x06001CEB RID: 7403 RVA: 0x00063CB4 File Offset: 0x00061EB4
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			TurnModuleStaticData data = base._database.Fetch(base.data.TurnModuleData);
			HostileForceTurnModuleInstance hostileForceTurnModuleInstance = (HostileForceTurnModuleInstance)TurnModuleInstanceFactory.CreateInstance(base._currentTurn, data);
			base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, hostileForceTurnModuleInstance);
			hostileForceTurnModuleInstance.EventEffectId = base.data.Id;
			return Result.Success;
		}
	}
}
