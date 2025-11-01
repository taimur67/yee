using System;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020004D0 RID: 1232
	public class EnterChainsOfAvariceDecisionProcessor : DiplomaticDecisionProcessor<EnterChainsOfAvariceDecisionRequest, EnterChainsOfAvariceDecisionResponse>
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x00054590 File Offset: 0x00052790
		protected override Result Enact(EnterChainsOfAvariceDecisionResponse response)
		{
			YesNo choice = response.Choice;
			DiplomaticPairStatus diplomaticStatus = base._currentDiplomacy.GetDiplomaticStatus(this._player.Id, base.request.RequestingPlayerId);
			int additionalTokenDraw = 1;
			DiplomaticAbility_ChainsOfAvarice diplomaticAbility_ChainsOfAvarice;
			ArchfiendModifierStaticData modifier;
			StatModificationBinding<ArchfiendStat> statModificationBinding;
			if (this.TurnProcessContext.Database.TryFetchSingle(out diplomaticAbility_ChainsOfAvarice) && diplomaticAbility_ChainsOfAvarice.TryGetComponent<ArchfiendModifierStaticData>(out modifier) && modifier.TryGetBinding(ArchfiendStat.Tribute_NumOptions, out statModificationBinding))
			{
				additionalTokenDraw = (int)Math.Round((double)statModificationBinding.Value);
			}
			int duration = base.request.Duration;
			base._currentTurn.AddGameEvent<ChainsOfAvariceResponseEvent>(new ChainsOfAvariceResponseEvent(base.request.RequestingPlayerId, this._player.Id, choice, 0, additionalTokenDraw, duration)).OrderType = OrderTypes.ChainsOfAvarice;
			if (choice == YesNo.Yes)
			{
				diplomaticStatus.SetChainsOfAvarice(this.TurnProcessContext, base.request.RequestingPlayerId, base.request.Duration, false);
			}
			else
			{
				diplomaticStatus.SetNeutral(this.TurnProcessContext, false);
			}
			return Result.Success;
		}
	}
}
