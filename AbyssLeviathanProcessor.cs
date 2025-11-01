using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020003FF RID: 1023
	public class AbyssLeviathanProcessor : HostileForceProcessor<AbyssLeviathanTurnModuleInstance, AbyssLeviathanTurnModuleStaticData>
	{
		// Token: 0x0600145B RID: 5211 RVA: 0x0004DA48 File Offset: 0x0004BC48
		protected override List<int> CalculatePlayerPriorityList(List<PlayerState> playerStates)
		{
			List<int> list = new List<int>();
			playerStates.SortOnValueDescending((PlayerState p) => this.GetPlayerPriority(base._currentTurn, p));
			if (base._currentTurn.ConclaveFavouriteId != -2147483648)
			{
				list.Add(base._currentTurn.ConclaveFavouriteId);
			}
			foreach (PlayerState playerState in playerStates)
			{
				if (!list.Contains(playerState.Id))
				{
					list.Add(playerState.Id);
				}
			}
			return list;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0004DAE8 File Offset: 0x0004BCE8
		protected override List<GamePiece> GetPrioritisedTargetList(int playerId)
		{
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(base._currentTurn.GetAllActivePoPsForPlayer(playerId, false));
			ListExtensions.OrderDescending<GamePiece, float>(list, new Func<GamePiece, float>(this.GetFixturePriority));
			GamePiece pandaemonium = base._currentTurn.GetPandaemonium();
			if (pandaemonium.ControllingPlayerId == playerId)
			{
				list.Insert(0, pandaemonium);
			}
			List<GamePiece> list2 = new List<GamePiece>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				GamePiece gamePiece = list[i];
				if (GamePiece.CalcCombatAdvantageAtPosition(this.TurnProcessContext, base.NeutralForce, gamePiece) <= 0.5f)
				{
					list.RemoveAt(i);
					list2.Add(gamePiece);
				}
			}
			list.AddRange(list2);
			return list;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0004DB8C File Offset: 0x0004BD8C
		private float GetPlayerPriority(TurnState turn, PlayerState player)
		{
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(turn.GetAllActivePoPsForPlayer(player.Id, false));
			if (list.Count > 0)
			{
				return list.Max(new Func<GamePiece, float>(this.GetFixturePriority));
			}
			return float.MinValue;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0004DBD0 File Offset: 0x0004BDD0
		private float GetFixturePriority(GamePiece fixture)
		{
			return (float)fixture.Level * 1000f + (float)fixture.PassivePrestige.Value * 500f + (float)fixture.CombatStats.Ranged.Value + (float)fixture.CombatStats.Melee.Value + (float)fixture.CombatStats.Infernal.Value + (float)fixture.GroundMoveDistance.Value;
		}
	}
}
