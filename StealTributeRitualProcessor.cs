using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Simulation.StaticData;
using Game.Simulation.Utils;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200069F RID: 1695
	public class StealTributeRitualProcessor : TargetedRitualActionProcessor<StealTributeRitualOrder, StealTributeRitualData, RitualCastEvent>
	{
		// Token: 0x06001F25 RID: 7973 RVA: 0x0006B5B8 File Offset: 0x000697B8
		private StealTributeEvent StealTributeFrom(PlayerState targetPlayer)
		{
			List<ResourceNFT> vulnerableTribute = this.TurnProcessContext.GetVulnerableTribute(targetPlayer.Id);
			int count = vulnerableTribute.Count;
			int randomRoll = base._currentTurn.GetRandomRoll(base.data.MinTributeStolen, base.data.MaxTributeStolen, this._player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>());
			bool flag = this.ShouldReduceTributeValue(targetPlayer);
			Payment payment = new Payment();
			int num = 0;
			while (num < randomRoll && vulnerableTribute.Count > 0)
			{
				ResourceNFT random = vulnerableTribute.GetRandom(base._currentTurn.Random);
				if (flag)
				{
					random.ReduceValue(this.TurnProcessContext.Random, 2, 2);
				}
				payment.AddResources(new ResourceNFT[]
				{
					random
				});
				vulnerableTribute.Remove(random);
				num++;
			}
			StealTributeEvent stealTributeEvent = new StealTributeEvent(this._player.Id, targetPlayer, count, payment.Resources.Count, false);
			PaymentRemovedEvent ev = this.TurnProcessContext.RemovePayment(targetPlayer, payment, null);
			PaymentReceivedEvent ev2 = this.TurnProcessContext.GivePayment(this._player, payment, null);
			stealTributeEvent.AddChildEvent<PaymentRemovedEvent>(ev);
			stealTributeEvent.AddChildEvent<PaymentReceivedEvent>(ev2);
			return stealTributeEvent;
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x0006B6D4 File Offset: 0x000698D4
		private bool ShouldReduceTributeValue(PlayerState targetPlayer)
		{
			if (targetPlayer.Role != PlayerRole.GOAP)
			{
				return false;
			}
			AIDifficultyStaticData entity;
			if (!base._database.TryGetDifficultyData(targetPlayer.AIDifficulty, out entity))
			{
				return false;
			}
			foreach (ModifierStaticData modifierStaticData in entity.GetModifiers())
			{
				using (IEnumerator<StatModificationBinding> enumerator2 = modifierStaticData.Bindings.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (StealTributeRitualProcessor.<ShouldReduceTributeValue>g__IsTributeBoost|1_0(enumerator2.Current))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0006B77C File Offset: 0x0006997C
		private StealTributeEvent StealManuscriptsFrom(PlayerState targetPlayer)
		{
			List<Manuscript> list = IEnumerableExtensions.ToList<Manuscript>(this.TurnProcessContext.CurrentTurn.GetGameItemsControlledBy<Manuscript>(targetPlayer.Id));
			int count = list.Count;
			int num = base._currentTurn.Random.Next(base.data.MinManuscriptsStolen, base.data.MaxManuscriptsStolen);
			if (num <= 0)
			{
				return null;
			}
			List<Identifier> list2 = new List<Identifier>();
			int num2 = 0;
			Manuscript manuscript;
			while (num2 < num && list.TryGetRandom(base._currentTurn.Random, out manuscript))
			{
				list2.Add(manuscript.Id);
				list.Remove(manuscript);
				num2++;
			}
			Payment payment = new Payment();
			PaymentRemovedEvent ev = this.TurnProcessContext.RemovePayment(targetPlayer, payment, list2);
			PaymentReceivedEvent ev2 = this.TurnProcessContext.GivePayment(this._player, payment, list2);
			StealTributeEvent stealTributeEvent = new StealTributeEvent(this._player.Id, targetPlayer, count, list2.Count, true);
			stealTributeEvent.AddChildEvent<PaymentRemovedEvent>(ev);
			stealTributeEvent.AddChildEvent<PaymentReceivedEvent>(ev2);
			return stealTributeEvent;
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0006B874 File Offset: 0x00069A74
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
			if (base.data.MaxTributeStolen > 0)
			{
				StealTributeEvent ev = this.StealTributeFrom(playerState);
				ritualCastEvent.AddChildEvent<StealTributeEvent>(ev);
			}
			if (base.data.MaxManuscriptsStolen > 0)
			{
				StealTributeEvent stealTributeEvent = this.StealManuscriptsFrom(playerState);
				if (stealTributeEvent != null)
				{
					ritualCastEvent.AddChildEvent<StealTributeEvent>(stealTributeEvent);
				}
			}
			return Result.Success;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0006B928 File Offset: 0x00069B28
		[CompilerGenerated]
		internal static bool <ShouldReduceTributeValue>g__IsTributeBoost|1_0(StatModificationBinding binding)
		{
			StatModificationBinding<ArchfiendStat> statModificationBinding = binding as StatModificationBinding<ArchfiendStat>;
			return statModificationBinding != null && statModificationBinding.StatKey == ArchfiendStat.TributeQuality && binding.Value > 0f;
		}
	}
}
