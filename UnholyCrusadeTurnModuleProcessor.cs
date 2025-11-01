using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020006D8 RID: 1752
	public class UnholyCrusadeTurnModuleProcessor : TurnModuleProcessor<UnholyCrusadeTurnModuleInstance, UnholyCrusadeEventStaticData>
	{
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x0006E29E File Offset: 0x0006C49E
		private bool FinalTurn
		{
			get
			{
				return base._currentTurn.TurnValue >= base.Instance.StartTurn + base.Instance.Duration;
			}
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0006E2C7 File Offset: 0x0006C4C7
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_Events, new TurnModuleProcessor.ProcessEvent(this.Process));
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0006E2E4 File Offset: 0x0006C4E4
		public override void OnAdded()
		{
			int duration = this.TurnProcessContext.Random.Next(this._staticData.MinDuration, this._staticData.MaxDuration);
			base.Instance.Duration = duration;
			base.Instance.StartTurn = base._currentTurn.TurnValue + 1;
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0006E33C File Offset: 0x0006C53C
		private void ProcessSubmittedLegions()
		{
			foreach (KeyValuePair<int, Identifier> keyValuePair in base.Instance.SubmittedLegions)
			{
				int num;
				Identifier identifier;
				keyValuePair.Deconstruct(out num, out identifier);
				int num2 = num;
				Identifier id = identifier;
				GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(id);
				if (gamePiece.Status != GameItemStatus.InPlay)
				{
					base.Instance.NoSubmissionPlayers.Add(num2);
				}
				else if (!gamePiece.IsAlive())
				{
					base.Instance.NoSubmissionPlayers.Add(num2);
				}
				else if (gamePiece.ControllingPlayerId != num2)
				{
					base.Instance.NoSubmissionPlayers.Add(num2);
				}
				else
				{
					gamePiece.Status = GameItemStatus.Unavailable;
					gamePiece.Location = HexCoord.Invalid;
					this.TurnProcessContext.RecalculateAurasFromGamePiece(gamePiece);
					gamePiece.Heal(gamePiece.TotalHP);
				}
			}
			foreach (int key in base.Instance.NoSubmissionPlayers)
			{
				base.Instance.SubmittedLegions.Remove(key);
			}
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0006E48C File Offset: 0x0006C68C
		private void Process()
		{
			if (base._currentTurn.TurnValue == base.Instance.StartTurn)
			{
				this.ProcessSubmittedLegions();
				if (base.Instance.NoSubmissionPlayers.Count > 0)
				{
					UnholyCrusadeNoLegionSentEvent gameEvent = new UnholyCrusadeNoLegionSentEvent(this._staticData.NoSubmissionPrestigePenalty, base.Instance.NoSubmissionPlayers.ToArray());
					base._currentTurn.AddGameEvent<UnholyCrusadeNoLegionSentEvent>(gameEvent);
					return;
				}
				base._currentTurn.AddGameEvent<UnholyCrusadeStartEvent>(new UnholyCrusadeStartEvent(-1));
				return;
			}
			else
			{
				if (base._currentTurn.TurnValue <= base.Instance.StartTurn)
				{
					return;
				}
				IEnumerable<Identifier> enumerable;
				if (this.FinalTurn)
				{
					enumerable = base.Instance.UnProcessedLegions;
				}
				else
				{
					int minValue = (int)Math.Floor((double)((float)base.Instance.SubmittedLegions.Count / (float)base.Instance.Duration));
					int maxValue = (int)Math.Ceiling((double)((float)base.Instance.SubmittedLegions.Count / (float)base.Instance.Duration));
					int count = this.TurnProcessContext.Random.Next(minValue, maxValue);
					enumerable = base.Instance.UnProcessedLegions.GetRandom(this.TurnProcessContext.Random, count);
				}
				bool flag = true;
				foreach (Identifier legionId in enumerable)
				{
					if (!this.ProcessLegion(legionId))
					{
						flag = false;
					}
				}
				if (!flag && !this.FinalTurn)
				{
					base._currentTurn.AddGameEvent<UnholyCrusadeLegionDefeatEvent>(new UnholyCrusadeLegionDefeatEvent(-1));
				}
				if (this.FinalTurn)
				{
					this.EndCrusade();
				}
				return;
			}
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0006E634 File Offset: 0x0006C834
		private void EndCrusade()
		{
			foreach (Identifier identifier in IEnumerableExtensions.ToList<Identifier>(base.Instance.VictoriousLegions))
			{
				GamePiece gamePiece = base._currentTurn.FetchGameItem<GamePiece>(identifier);
				PlayerState player = base._currentTurn.FindPlayerState(gamePiece.ControllingPlayerId, null);
				HexCoord location;
				if (!LegionMovementProcessor.TryFindSpawnPointFor(this.TurnProcessContext, player, gamePiece, out location))
				{
					base.Instance.VictoriousLegions.Remove(identifier);
					base.Instance.DefeatedLegions.Add(identifier);
				}
				else
				{
					gamePiece.Location = location;
					gamePiece.Status = GameItemStatus.InPlay;
					this.TurnProcessContext.RecalculateAurasFromGamePiece(gamePiece);
					LegionLevelTable levelTable = base._database.Fetch(this._staticData.LevelTable);
					gamePiece.ProcessLevelUp(this.TurnProcessContext, levelTable, this._staticData.RewardLevels);
					UnholyCrusadeLegionReturnedEvent unholyCrusadeLegionReturnedEvent = new UnholyCrusadeLegionReturnedEvent(-1, gamePiece.Id, true);
					unholyCrusadeLegionReturnedEvent.AddAffectedPlayerId(gamePiece.ControllingPlayerId);
					base._currentTurn.AddGameEvent<UnholyCrusadeLegionReturnedEvent>(unholyCrusadeLegionReturnedEvent);
				}
			}
			bool flag = base.Instance.VictoriousLegions.Count <= 0;
			GameEvent gameEvent = null;
			if (flag)
			{
				gameEvent = base._currentTurn.AddGameEvent<UnholyCrusadeFailureEvent>(new UnholyCrusadeFailureEvent(-1));
			}
			foreach (Identifier id in base.Instance.DefeatedLegions)
			{
				GamePiece gamePiece2 = base._currentTurn.FetchGameItem<GamePiece>(id);
				this.TurnProcessContext.KillGamePieceWithEvent(base._currentTurn.ForceMajeurePlayer, gamePiece2);
				UnholyCrusadeLegionReturnedEvent unholyCrusadeLegionReturnedEvent2 = new UnholyCrusadeLegionReturnedEvent(-1, gamePiece2.Id, false);
				unholyCrusadeLegionReturnedEvent2.AddAffectedPlayerId(gamePiece2.ControllingPlayerId);
				if (((gameEvent != null) ? gameEvent.AddChildEvent<UnholyCrusadeLegionReturnedEvent>(unholyCrusadeLegionReturnedEvent2) : null) == null)
				{
					base._currentTurn.AddGameEvent<UnholyCrusadeLegionReturnedEvent>(unholyCrusadeLegionReturnedEvent2);
				}
			}
			base.RemoveSelf();
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0006E834 File Offset: 0x0006CA34
		private bool ProcessLegion(Identifier legionId)
		{
			float num = (float)(from x in base.Instance.UnProcessedLegions
			select base._currentTurn.FetchGameItem<GamePiece>(x)).Sum((GamePiece x) => x.Level) * this._staticData.SurvivalChanceFactor;
			if (this.TurnProcessContext.Random.NextFloat() > num)
			{
				base.Instance.SetLegionDefeated(legionId);
				return false;
			}
			base.Instance.SetLegionVictorious(legionId);
			return true;
		}
	}
}
