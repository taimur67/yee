using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200061C RID: 1564
	public class UnholyCrusadeEventProcessor : GrandEventActionProcessor<UnholyCrusadeEventOrder, UnholyCrusadeEventStaticData>
	{
		// Token: 0x06001CEF RID: 7407 RVA: 0x00063D2C File Offset: 0x00061F2C
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			UnholyCrusadeTurnModuleInstance unholyCrusadeTurnModuleInstance = (UnholyCrusadeTurnModuleInstance)TurnModuleInstanceFactory.CreateInstance(base._currentTurn, base.data);
			UnholyCrusadeEvent unholyCrusadeEvent = new UnholyCrusadeEvent(this._player.Id, this.AbilityData.Id);
			foreach (PlayerState playerState in base._currentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Excommunicated)
				{
					unholyCrusadeEvent.AddAffectedPlayerId(playerState.Id);
				}
				else
				{
					UnholyCrusadeEventDecisionRequest unholyCrusadeEventDecisionRequest = new UnholyCrusadeEventDecisionRequest(base._currentTurn, this.AbilityData.Id);
					unholyCrusadeEventDecisionRequest.TurnModuleInstanceId = unholyCrusadeTurnModuleInstance.Id;
					base._currentTurn.AddDecisionToAskPlayer(playerState.Id, unholyCrusadeEventDecisionRequest);
				}
			}
			base._currentTurn.AddGameEvent<UnholyCrusadeEvent>(unholyCrusadeEvent);
			base._currentTurn.AddActiveTurnModule(this.TurnProcessContext, unholyCrusadeTurnModuleInstance);
			return Result.Success;
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x00063E24 File Offset: 0x00062024
		protected override GrandEventPlayed ProcessGameEvent()
		{
			return null;
		}
	}
}
