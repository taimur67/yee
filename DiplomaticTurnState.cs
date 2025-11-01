using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006DA RID: 1754
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DiplomaticTurnState : IDeepClone<DiplomaticTurnState>
	{
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600206B RID: 8299 RVA: 0x0007148A File Offset: 0x0006F68A
		[JsonIgnore]
		public IEnumerable<DiplomaticPairStatus> Standings
		{
			get
			{
				return this._diplomaticStandings.Values;
			}
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x00071497 File Offset: 0x0006F697
		public DiplomaticPairStatus GetDiplomaticStatus(PlayerState firstPlayer, PlayerState secondPlayer)
		{
			return this.GetDiplomaticStatus(firstPlayer.Id, secondPlayer.Id);
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x000714AB File Offset: 0x0006F6AB
		public DiplomaticPairStatus GetDiplomaticStatus(int firstPlayerId, int secondPlayerId)
		{
			return this.GetDiplomaticStatus(new PlayerPair(firstPlayerId, secondPlayerId));
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000714BC File Offset: 0x0006F6BC
		public DiplomaticPairStatus GetDiplomaticStatus(PlayerPair playerPair)
		{
			DiplomaticPairStatus diplomaticPairStatus;
			if (!this._diplomaticStandings.TryGetValue(playerPair.GetHashCode(), out diplomaticPairStatus))
			{
				diplomaticPairStatus = new DiplomaticPairStatus(playerPair);
				if (playerPair.First == -1 || playerPair.Second == -1)
				{
					return new DiplomaticPairStatus(playerPair, new ForceMajeureState());
				}
				if (playerPair.PlayersAreEqual())
				{
					return new DiplomaticPairStatus(playerPair, new SelfDiplomaticState());
				}
				this.SetDiplomaticStatus(playerPair, diplomaticPairStatus);
			}
			return diplomaticPairStatus;
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x00071528 File Offset: 0x0006F728
		public bool TryGetDiplomaticState<T>(int firstPlayerId, int secondPlayerId, out T state) where T : DiplomaticState
		{
			return this.TryGetDiplomaticState<T>(new PlayerPair(firstPlayerId, secondPlayerId), out state);
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00071538 File Offset: 0x0006F738
		public bool TryGetDiplomaticState<T>(PlayerPair pair, out T state) where T : DiplomaticState
		{
			state = (this.GetDiplomaticState(pair) as T);
			return state != null;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x0007155F File Offset: 0x0006F75F
		public DiplomaticState GetDiplomaticState(PlayerPair pair)
		{
			return this.GetDiplomaticStatus(pair).DiplomaticState;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x00071570 File Offset: 0x0006F770
		public void SetPlayerAsExcommunicated(TurnProcessContext context, PlayerState excommunicatedPlayer, ExcommunicationReason reason = ExcommunicationReason.Unknown, int triggeringPlayerId = -1)
		{
			if (excommunicatedPlayer.Eliminated)
			{
				return;
			}
			if (excommunicatedPlayer.Excommunicated)
			{
				return;
			}
			IEnumerable<DiplomaticPairStatus> enumerable = from x in this.Standings
			where x.IsDiplomacyStatusOfPlayer(excommunicatedPlayer.Id)
			select x;
			excommunicatedPlayer.Excommunicate(context, reason);
			context.RecalculateAllModifiersFor(excommunicatedPlayer);
			foreach (DiplomaticPairStatus diplomaticPairStatus in enumerable)
			{
				if (!(diplomaticPairStatus.DiplomaticState is EliminatedState) && !diplomaticPairStatus.IsDiplomacyStatusOfPlayer(-1))
				{
					diplomaticPairStatus.SetExcommunicated(context, excommunicatedPlayer.Id);
				}
			}
			PlayerExcommunicatedEvent gameEvent = new PlayerExcommunicatedEvent(triggeringPlayerId, excommunicatedPlayer.Id, reason);
			context.CurrentTurn.AddGameEvent<PlayerExcommunicatedEvent>(gameEvent);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x00071654 File Offset: 0x0006F854
		public GameEvent ReinstateExcommunicatedPlayer(TurnProcessContext context, PlayerState player)
		{
			IEnumerable<DiplomaticPairStatus> enumerable = from x in this.Standings
			where x.IsDiplomacyStatusOfPlayer(player.Id)
			select x;
			player.ReinstateFromExcommunication(context);
			foreach (DiplomaticPairStatus diplomaticPairStatus in enumerable)
			{
				int playerId;
				if (!diplomaticPairStatus.IsDiplomacyStatusOfPlayer(-1) && diplomaticPairStatus.TryGetOtherPlayer(player.Id, out playerId))
				{
					PlayerState playerState = context.CurrentTurn.FindPlayerState(playerId, null);
					if (!playerState.Excommunicated && !playerState.Eliminated)
					{
						diplomaticPairStatus.SetNeutral(context, false);
					}
				}
			}
			player.Excommunicated = false;
			return new ExcommunicatedPlayerReinstatedEvent(player.Id);
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x00071728 File Offset: 0x0006F928
		public void SetDiplomaticStatus(PlayerPair pair, DiplomaticPairStatus pairStatus)
		{
			this._diplomaticStandings[pair.GetHashCode()] = pairStatus;
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x00071743 File Offset: 0x0006F943
		public IEnumerable<DiplomaticPairStatus> GetAllDiplomaticStatesOfPlayer(PlayerState playerState)
		{
			return this.GetAllDiplomaticStatesOfPlayer(playerState.Id);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x00071751 File Offset: 0x0006F951
		public IEnumerable<DiplomaticPairStatus> GetAllDiplomaticStatesOfPlayer(int playerId)
		{
			foreach (DiplomaticPairStatus diplomaticPairStatus in this.Standings)
			{
				if (diplomaticPairStatus.IsDiplomacyStatusOfPlayer(playerId))
				{
					yield return diplomaticPairStatus;
				}
			}
			IEnumerator<DiplomaticPairStatus> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x00071768 File Offset: 0x0006F968
		public bool HasDiplomaticStateWithTarget(TurnProcessContext context, PlayerState player, PlayerState target, DiplomaticStateValue state)
		{
			return this.HasDiplomaticStateWithTarget(player.Id, target.Id, state);
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x0007177E File Offset: 0x0006F97E
		public bool HasDiplomaticStateWithTarget(int playerId, int targetId, DiplomaticStateValue state)
		{
			return this.GetDiplomaticStatus(playerId, targetId).DiplomaticState.Type == state;
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x00071798 File Offset: 0x0006F998
		public bool HasDiplomaticStateWithAny(int playerId, DiplomaticStateValue state)
		{
			foreach (DiplomaticPairStatus diplomaticPairStatus in this.Standings)
			{
				if (diplomaticPairStatus.IsDiplomacyStatusOfPlayer(playerId) && diplomaticPairStatus.DiplomaticState.Type == state)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000717FC File Offset: 0x0006F9FC
		public bool IsVassalOfAny(int playerId, out int bloodLordId)
		{
			bloodLordId = int.MinValue;
			foreach (DiplomaticPairStatus diplomaticPairStatus in this.Standings)
			{
				if (diplomaticPairStatus.IsDiplomacyStatusOfPlayer(playerId))
				{
					BloodVassalageState bloodVassalageState = diplomaticPairStatus.DiplomaticState as BloodVassalageState;
					if (bloodVassalageState != null && bloodVassalageState.VassalId == playerId)
					{
						bloodLordId = bloodVassalageState.BloodLordId;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x0007187C File Offset: 0x0006FA7C
		public bool IsBloodLordOfAny(int playerId, out int vassalId)
		{
			vassalId = int.MinValue;
			foreach (DiplomaticPairStatus diplomaticPairStatus in this.Standings)
			{
				if (diplomaticPairStatus.IsDiplomacyStatusOfPlayer(playerId))
				{
					BloodVassalageState bloodVassalageState = diplomaticPairStatus.DiplomaticState as BloodVassalageState;
					if (bloodVassalageState != null && bloodVassalageState.BloodLordId == playerId)
					{
						vassalId = bloodVassalageState.VassalId;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000718FC File Offset: 0x0006FAFC
		public IEnumerable<T> DiplomaticStatesOfType<T>(int playerId) where T : DiplomaticState
		{
			foreach (DiplomaticPairStatus diplomaticPairStatus in this.Standings)
			{
				if (diplomaticPairStatus.PlayerPair.First == playerId || diplomaticPairStatus.PlayerPair.Second == playerId)
				{
					T t = diplomaticPairStatus.DiplomaticState as T;
					if (t != null)
					{
						yield return t;
					}
				}
			}
			IEnumerator<DiplomaticPairStatus> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x1700046F RID: 1135
		public DiplomaticPairStatus this[PlayerPair pair]
		{
			get
			{
				return this.GetDiplomaticStatus(pair);
			}
			set
			{
				this.SetDiplomaticStatus(pair, value);
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x00071926 File Offset: 0x0006FB26
		public void DeepClone(out DiplomaticTurnState clone)
		{
			clone = new DiplomaticTurnState
			{
				_diplomaticStandings = this._diplomaticStandings.DeepClone<DiplomaticPairStatus>()
			};
		}

		// Token: 0x04000E45 RID: 3653
		[JsonProperty]
		private Dictionary<int, DiplomaticPairStatus> _diplomaticStandings = new Dictionary<int, DiplomaticPairStatus>();
	}
}
