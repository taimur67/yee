using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x0200060E RID: 1550
	public class ReducePowerEventProcessor : GrandEventActionProcessor<ReducePowerEventOrder, ReducePowerEventStaticData>
	{
		// Token: 0x06001CCB RID: 7371 RVA: 0x000635A0 File Offset: 0x000617A0
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			if (base.data.AllArchfiends)
			{
				using (IEnumerator<PlayerState> enumerator = base._currentTurn.EnumeratePlayerStates(false, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlayerState targetPlayer = enumerator.Current;
						this.ReducePower(targetPlayer, -1);
					}
					goto IL_A5;
				}
			}
			PlayerState playerState = IEnumerableExtensions.ToList<PlayerState>(from x in base._currentTurn.EnumeratePlayerStates(false, false)
			where x.Id != this._player.Id
			select x).WeightedRandom((PlayerState player) => (float)player.OrderSlots, base._random, false);
			if (playerState == null)
			{
				return new NoValidTargetsProblem();
			}
			this.ReducePower(playerState, -1);
			IL_A5:
			return Result.Success;
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x00063668 File Offset: 0x00061868
		private bool PowerLevelIsAboveZero([TupleElementNames(new string[]
		{
			"_",
			"powerLevel"
		})] ValueTuple<PowerType, PlayerPowerLevel> tuple)
		{
			return tuple.Item2.CurrentLevel.Value > 0;
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00063680 File Offset: 0x00061880
		private ValueTuple<PowerType, PlayerPowerLevel> GetPowerToAffect(PlayerState targetPlayer)
		{
			if (!targetPlayer.PowersLevels.PowerTypeLevels.Any(new Func<ValueTuple<PowerType, PlayerPowerLevel>, bool>(this.PowerLevelIsAboveZero)))
			{
				return targetPlayer.PowersLevels.PowerTypeLevels.GetRandom(base._random);
			}
			return targetPlayer.PowersLevels.PowerTypeLevels.Where(new Func<ValueTuple<PowerType, PlayerPowerLevel>, bool>(this.PowerLevelIsAboveZero)).GetRandom(base._random);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000636E9 File Offset: 0x000618E9
		private void ReducePower(PlayerState targetPlayer)
		{
			this.ReducePower(targetPlayer, this._player.Id);
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x00063700 File Offset: 0x00061900
		private void ReducePower(PlayerState targetPlayer, int overrideInstigatorId)
		{
			ValueTuple<PowerType, PlayerPowerLevel> powerToAffect = this.GetPowerToAffect(targetPlayer);
			PowerType item = powerToAffect.Item1;
			int num = Math.Min(powerToAffect.Item2.CurrentLevel.Value, base.data.NumberOfLevels);
			GameEvent gameEvent;
			this.TurnProcessContext.TryPermanentlyAdjustArchfiendStat(this._player, targetPlayer, item, -base.data.NumberOfLevels, -num, out gameEvent);
			if (gameEvent != null)
			{
				gameEvent.TriggeringPlayerID = overrideInstigatorId;
				base.GameEvent.AddAffectedPlayerId(targetPlayer.Id);
				base.GameEvent.AddChildEvent(gameEvent);
			}
		}
	}
}
