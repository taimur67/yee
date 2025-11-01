using System;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020003E8 RID: 1000
	public static class DiplomaticStateProcessor
	{
		// Token: 0x060013C2 RID: 5058 RVA: 0x0004B384 File Offset: 0x00049584
		public static void UpdateDiplomaticState(TurnProcessContext context)
		{
			foreach (DiplomaticPairStatus diplomaticPairStatus in context.CurrentTurn.CurrentDiplomaticTurn.Standings)
			{
				diplomaticPairStatus.UpdateDiplomaticState(context);
			}
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0004B3DC File Offset: 0x000495DC
		public static Result ValidateAction(TurnState turn, PlayerDiplomaticAction action)
		{
			int actorId = action.ActorId;
			int targetId = action.TargetId;
			Result result = DiplomaticStateProcessor.ValidateOrderType(turn, actorId, targetId, action.OrderType, action.IsDecision);
			Result.DiplomacyProblem diplomacyProblem = result as Result.DiplomacyProblem;
			if (diplomacyProblem != null)
			{
				diplomacyProblem.IsDecisionResponse = action.IsDecision;
			}
			PlayerDiplomaticOrder playerDiplomaticOrder = action as PlayerDiplomaticOrder;
			if (playerDiplomaticOrder != null)
			{
				Result.DiplomacyProblem diplomacyProblem2 = playerDiplomaticOrder.Processor.Validate() as Result.DiplomacyProblem;
				if (diplomacyProblem2 != null)
				{
					diplomacyProblem2.IsDecisionResponse = action.IsDecision;
					return diplomacyProblem2;
				}
			}
			return result;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0004B458 File Offset: 0x00049658
		public static Result ValidateOrderType(TurnState turn, int actorId, int targetId, OrderTypes orderType, bool isResponse = false)
		{
			DiplomaticPairStatus diplomaticStatus = turn.GetDiplomaticStatus(actorId, targetId);
			if (!isResponse)
			{
				return diplomaticStatus.IsOrderAllowed(turn, orderType, actorId);
			}
			return diplomaticStatus.IsResponseAllowed(turn, orderType, actorId);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0004B485 File Offset: 0x00049685
		[return: TupleElementNames(new string[]
		{
			"first",
			"second"
		})]
		public static ValueTuple<PlayerState, PlayerState> FindPlayerStates(this TurnState turn, PlayerPair players)
		{
			return new ValueTuple<PlayerState, PlayerState>(turn.FindPlayerState(players.First, null), turn.FindPlayerState(players.Second, null));
		}
	}
}
