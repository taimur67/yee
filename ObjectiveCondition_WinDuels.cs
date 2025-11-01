using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005BA RID: 1466
	[Serializable]
	public class ObjectiveCondition_WinDuels : ObjectiveCondition_EventFilter<PraetorDuelOutcomeEvent>
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001B68 RID: 7016 RVA: 0x0005F326 File Offset: 0x0005D526
		public override string LocalizationKey
		{
			get
			{
				if (!this.WinnerMustWinBribe)
				{
					return "WinDuels";
				}
				if (this.WithExactBribeAmount <= 0)
				{
					return "WinDuelsWithBribes";
				}
				return "WinDuelWithExactBribeAmount";
			}
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0005F34C File Offset: 0x0005D54C
		protected override bool Filter(TurnContext context, PraetorDuelOutcomeEvent @event, PlayerState owner, PlayerState target)
		{
			if (@event.Winner == null)
			{
				return false;
			}
			if (@event.Winner.PlayerId != owner.Id)
			{
				return false;
			}
			if (target != null && @event.Loser.PlayerId != target.Id)
			{
				return false;
			}
			if (owner.Id == @event.Challenger.PlayerId && !this.WinnerMustHaveRole.HasFlag(BattleRole.Attacker))
			{
				return false;
			}
			if (owner.Id == @event.Defender.PlayerId && !this.WinnerMustHaveRole.HasFlag(BattleRole.Defender))
			{
				return false;
			}
			if (this.WinnerMustBeLowerLevel)
			{
				Praetor praetor;
				Praetor praetor2;
				if (!context.CurrentTurn.TryFetchGameItem<Praetor>(@event.Winner.Praetor, out praetor) || !context.CurrentTurn.TryFetchGameItem<Praetor>(@event.Loser.Praetor, out praetor2))
				{
					return false;
				}
				if (praetor.Level >= praetor2.Level)
				{
					return false;
				}
			}
			if (this.WinnerMustWinBribe)
			{
				ArbiterInterventionEvent arbiterInterventionEvent;
				if (!@event.TryGet<ArbiterInterventionEvent>(out arbiterInterventionEvent))
				{
					return false;
				}
				if (arbiterInterventionEvent.FavoredPlayer != owner.Id)
				{
					return false;
				}
				if (this.WithExactBribeAmount > 0 && arbiterInterventionEvent.WinningBribeAmount != this.WithExactBribeAmount)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0005F478 File Offset: 0x0005D678
		public override int GetHashCode()
		{
			return (int)(base.GetHashCode() * 23 + this.WinnerMustHaveRole * (BattleRole)29 + (this.WinnerMustBeLowerLevel ? 31 : 0) + (this.WinnerMustWinBribe ? 37 : 0) + this.WithExactBribeAmount * 41);
		}

		// Token: 0x04000C5B RID: 3163
		[JsonProperty]
		[DefaultValue(BattleRole.All)]
		public BattleRole WinnerMustHaveRole = BattleRole.All;

		// Token: 0x04000C5C RID: 3164
		[JsonProperty]
		[DefaultValue(false)]
		public bool WinnerMustBeLowerLevel;

		// Token: 0x04000C5D RID: 3165
		[JsonProperty]
		[DefaultValue(false)]
		public bool WinnerMustWinBribe;

		// Token: 0x04000C5E RID: 3166
		[JsonProperty]
		[DefaultValue(0)]
		public int WithExactBribeAmount;
	}
}
