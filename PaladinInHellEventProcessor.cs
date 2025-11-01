using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x02000609 RID: 1545
	public class PaladinInHellEventProcessor : GrandEventActionProcessor<PaladinInHellEventOrder, PaladinInHellEventStaticData>
	{
		// Token: 0x06001CBE RID: 7358 RVA: 0x000632A0 File Offset: 0x000614A0
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			Artifact artifact2 = IEnumerableExtensions.ToList<Artifact>(base._currentTurn.EnumerateActiveGameItems<Artifact>().Where(delegate(Artifact a)
			{
				if (base._currentTurn.BazaarState.IsForSale(a))
				{
					return false;
				}
				PlayerState playerState2 = base._currentTurn.FindControllingPlayer(a);
				return playerState2 != null && playerState2.Id != -1 && playerState2.Id != this._player.Id;
			})).WeightedRandom((Artifact artifact) => (float)artifact.Level, base._currentTurn.Random, false);
			PlayerState playerState = (artifact2 != null) ? base._currentTurn.FindControllingPlayer(artifact2) : (from candidate in base._currentTurn.EnumeratePlayerStates(false, false)
			where candidate.Id != this._player.Id
			select candidate).GetRandomOrDefault(base._random);
			if (playerState == null)
			{
				return new NoValidTargetsProblem();
			}
			Payment payment = new Payment();
			int prestige = (int)Math.Round((double)((float)playerState.SpendablePrestige * base.data.PrestigeLossPercent), MidpointRounding.AwayFromZero);
			payment.Prestige = prestige;
			if (artifact2 != null)
			{
				ItemBanishedEvent ev = this.TurnProcessContext.BanishGameItem(artifact2, this._player.Id);
				base.GameEvent.AddChildEvent<ItemBanishedEvent>(ev);
			}
			else
			{
				int count = base._random.Next(base.data.TributeLossMin, base.data.TributeLossMax);
				IEnumerable<ResourceNFT> random = playerState.Resources.GetRandom(base._random, count);
				payment.Resources = IEnumerableExtensions.ToList<ResourceNFT>(random);
			}
			PaymentRemovedEvent ev2 = this.TurnProcessContext.RemovePayment(playerState, payment, null);
			base.GameEvent.AddChildEvent<PaymentRemovedEvent>(ev2);
			base.GameEvent.AddAffectedPlayerId(playerState.Id);
			return Result.Success;
		}
	}
}
