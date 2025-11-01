using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x0200066E RID: 1646
	public class DiscardSchemeRitualProcessor : TargetedRitualActionProcessor<DiscardSchemeRitualOrder, DiscardSchemeRitualData, RitualCastEvent>
	{
		// Token: 0x06001E60 RID: 7776 RVA: 0x00068BF4 File Offset: 0x00066DF4
		public override Result Validate()
		{
			Problem problem = base.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			SchemeCard gameItem;
			if (!base._currentTurn.TryFetchGameItem<SchemeCard>(base.request.SchemeCardId, out gameItem))
			{
				return new SimulationError(string.Format("Could not find SchemeCard {0}", base.request.SchemeCardId));
			}
			PlayerState playerState = base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null);
			if (IEnumerableExtensions.Contains<Identifier>(playerState.PreviousSchemeCards, base.request.SchemeCardId))
			{
				return new Result.RitualSchemeAlreadyCompletedProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId, gameItem);
			}
			if (!IEnumerableExtensions.Contains<Identifier>(playerState.ActiveSchemeCards, base.request.SchemeCardId))
			{
				return new Result.RitualSchemeAlreadyDiscardedProblem(this.AbilityData.ConfigRef, base.request.TargetPlayerId, gameItem);
			}
			return Result.Success;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x00068CD4 File Offset: 0x00066ED4
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			SchemeCard schemeCard;
			if (!this.TurnProcessContext.CurrentTurn.TryFetchGameItem<SchemeCard>(base.request.SchemeCardId, out schemeCard))
			{
				return new SimulationError(string.Format("Could not find SchemeCard {0}", base.request.SchemeCardId));
			}
			PlayerState playerState = base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null);
			ritualCastEvent.AddChildEvent<SchemeDiscardedEvent>(new SchemeDiscardedEvent(this._player.Id, playerState.Id, base.request.SchemeCardId, schemeCard.Scheme.IsPrivate));
			base._currentTurn.RemoveScheme(playerState, base.request.SchemeCardId);
			this.TurnProcessContext.RemoveItemFromPlayersKnowledge(this._player, base.request.SchemeCardId);
			return Result.Success;
		}
	}
}
