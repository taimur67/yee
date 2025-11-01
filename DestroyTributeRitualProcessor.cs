using System;
using System.Collections.Generic;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200066B RID: 1643
	public class DestroyTributeRitualProcessor : TargetedRitualActionProcessor<DestroyTributeRitualOrder, DestroyTributeRitualData, RitualCastEvent>
	{
		// Token: 0x06001E50 RID: 7760 RVA: 0x000687F4 File Offset: 0x000669F4
		private DestroyTributeEvent DestroyTributeOf(PlayerState targetPlayer)
		{
			List<ResourceNFT> list = IEnumerableExtensions.ToList<ResourceNFT>(targetPlayer.Resources);
			int count = list.Count;
			int randomRoll = base._currentTurn.GetRandomRoll(base.data.MinTributeDestroyed, base.data.MaxTributeDestroyed, this._player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>());
			Payment payment = new Payment();
			int num = 0;
			while (num < randomRoll && list.Count > 0)
			{
				ResourceNFT random = list.GetRandom(base._currentTurn.Random);
				payment.AddResources(new ResourceNFT[]
				{
					random
				});
				list.Remove(random);
				num++;
			}
			DestroyTributeEvent destroyTributeEvent = new DestroyTributeEvent(this._player.Id, targetPlayer, count, payment.Resources.Count, false);
			PaymentRemovedEvent paymentRemovedEvent = this.TurnProcessContext.RemovePayment(targetPlayer, payment, null);
			paymentRemovedEvent.TriggeringPlayerID = this._player.Id;
			destroyTributeEvent.AddChildEvent<PaymentRemovedEvent>(paymentRemovedEvent);
			return destroyTributeEvent;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000688D8 File Offset: 0x00066AD8
		private DestroyTributeEvent DestroyManuscriptsOf(PlayerState targetPlayer)
		{
			List<Manuscript> list = IEnumerableExtensions.ToList<Manuscript>(this.TurnProcessContext.CurrentTurn.GetGameItemsControlledBy<Manuscript>(targetPlayer.Id));
			int count = list.Count;
			int num = base._currentTurn.Random.Next(base.data.MinManuscriptsDestroyed, base.data.MaxManuscriptsDestroyed);
			if (num <= 0)
			{
				return null;
			}
			List<Identifier> list2 = new List<Identifier>();
			for (int i = 0; i < num; i++)
			{
				Manuscript random = list.GetRandom(base._currentTurn.Random);
				if (random != null && random.Id != Identifier.Invalid)
				{
					list2.Add(random);
					list.Remove(random);
				}
			}
			Payment payment = new Payment();
			DestroyTributeEvent destroyTributeEvent = new DestroyTributeEvent(this._player.Id, targetPlayer, count, list2.Count, true);
			PaymentRemovedEvent paymentRemovedEvent = this.TurnProcessContext.RemovePayment(targetPlayer, payment, list2);
			paymentRemovedEvent.TriggeringPlayerID = this._player.Id;
			destroyTributeEvent.AddChildEvent<PaymentRemovedEvent>(paymentRemovedEvent);
			return destroyTributeEvent;
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000689D0 File Offset: 0x00066BD0
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			PlayerState playerState = base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null);
			if (playerState == null)
			{
				return new Result.CastRitualOnPlayerProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId);
			}
			if (base.data.MaxTributeDestroyed > 0)
			{
				DestroyTributeEvent ev = this.DestroyTributeOf(playerState);
				ritualCastEvent.AddChildEvent<DestroyTributeEvent>(ev);
			}
			if (base.data.MaxManuscriptsDestroyed > 0)
			{
				DestroyTributeEvent destroyTributeEvent = this.DestroyManuscriptsOf(playerState);
				if (destroyTributeEvent != null)
				{
					ritualCastEvent.AddChildEvent<DestroyTributeEvent>(destroyTributeEvent);
				}
			}
			return Result.Success;
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x00068A7C File Offset: 0x00066C7C
		protected override int GetPrestigeReward()
		{
			RitualCastEvent gameEvent = base.GameEvent;
			DestroyTributeEvent destroyTributeEvent = (gameEvent != null) ? IEnumerableExtensions.FirstOrDefault<DestroyTributeEvent>(gameEvent.Enumerate<DestroyTributeEvent>()) : null;
			if (destroyTributeEvent == null)
			{
				return 0;
			}
			return destroyTributeEvent.NumberOfTokensDestroyed;
		}
	}
}
