using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000620 RID: 1568
	public class ManuscriptArchfiendBuffProcessor : ManuscriptActionProcessor<ManuscriptArchfiendOrder, ModifyArchfiendData>
	{
		// Token: 0x06001CFE RID: 7422 RVA: 0x000641FE File Offset: 0x000623FE
		public IEnumerable<PlayerState> GetTargetPlayers()
		{
			if (base.data.SelfTarget)
			{
				return IEnumerableExtensions.ToEnumerable<PlayerState>(this._player);
			}
			return IEnumerableExtensions.ExceptFor<PlayerState>(base._currentTurn.EnumeratePlayerStates(false, false), new PlayerState[]
			{
				this._player
			});
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x0006423C File Offset: 0x0006243C
		public override Result Process(ActionProcessContext context)
		{
			Problem problem = this.Validate() as Problem;
			if (problem != null)
			{
				return problem;
			}
			Manuscript manuscript = base._currentTurn.FetchGameItem<Manuscript>(base.request.ManuscriptId);
			ManuscriptStaticData manuscriptStaticData = base._database.Fetch<ManuscriptStaticData>(manuscript.StaticDataId);
			Problem problem2 = base.ConsumeManuscriptFragments(manuscriptStaticData.Id, manuscriptStaticData.FragmentCount) as Problem;
			if (problem2 != null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Validate method passed, but unable to consume manuscript fragments");
				}
				return problem2;
			}
			ManuscriptEvent manuscriptEvent = new ManuscriptEvent(this._player.Id, manuscript.GetCategory(base._database), Array.Empty<int>());
			List<PlayerState> list = IEnumerableExtensions.ToList<PlayerState>(this.GetTargetPlayers());
			foreach (PlayerState playerState in list)
			{
				IEnumerable<GameEvent> events = this.TurnProcessContext.InstallModifierAsPermanentAdjustment(this.TurnProcessContext.CurrentTurn.ForceMajeurePlayer, playerState, base.data.Modifiers);
				manuscriptEvent.AddChildEvent(events);
				this.TurnProcessContext.RecalculateModifiers(playerState);
			}
			manuscriptEvent.AddAffectedPlayerIds(from t in list
			select t.Id);
			manuscriptEvent.AddAffectedPlayerId(this._player.Id);
			manuscriptEvent.ManuscriptId = manuscript.Id;
			if (base.data.SelfTarget)
			{
				manuscriptEvent.PowerType = base.data.Modifiers.GetHighestPowerValue();
			}
			else
			{
				manuscriptEvent.PowerType = base.data.Modifiers.GetLowestPowerValue();
			}
			base._currentTurn.AddGameEvent<ManuscriptEvent>(manuscriptEvent);
			return Result.Success;
		}
	}
}
