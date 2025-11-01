using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000673 RID: 1651
	public class GainResourcesItemRitualProcessor : TargetedRitualActionProcessor<GainResourcesRitualOrder, GainResourcesRitualData, RitualCastEvent>
	{
		// Token: 0x06001E6E RID: 7790 RVA: 0x00068F18 File Offset: 0x00067118
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (!base.data.GainedResources.IsZero)
			{
				TurnState currentTurn = base._currentTurn;
				ResourceAccumulation[] array = new ResourceAccumulation[1];
				int num = 0;
				ResourceAccumulation resourceAccumulation = new ResourceAccumulation();
				resourceAccumulation[ResourceTypes.Souls] = base.data.GainedResources.Soul;
				resourceAccumulation[ResourceTypes.Ichor] = base.data.GainedResources.Ichor;
				resourceAccumulation[ResourceTypes.Hellfire] = base.data.GainedResources.Hellfire;
				resourceAccumulation[ResourceTypes.Darkness] = base.data.GainedResources.Darkness;
				array[num] = resourceAccumulation;
				ResourceNFT resourceNFT = currentTurn.CreateNFT(array);
				Payment payment = new Payment(new ResourceNFT[]
				{
					resourceNFT
				});
				ritualCastEvent.AddChildEvent<PaymentReceivedEvent>(this.TurnProcessContext.GivePayment(this._player, payment, null));
			}
			int randomRoll = base._currentTurn.GetRandomRoll(base.data.MinGainedPrestige, base.data.MaxGainedPrestige, this._player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>());
			ritualCastEvent.AddChildEvent<PaymentReceivedEvent>(this.TurnProcessContext.GivePrestige(this._player, randomRoll));
			return Result.Success;
		}
	}
}
