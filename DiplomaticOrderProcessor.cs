using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020003E7 RID: 999
	public static class DiplomaticOrderProcessor
	{
		// Token: 0x060013BC RID: 5052 RVA: 0x0004B04C File Offset: 0x0004924C
		public static void ProcessDiplomaticActions(TurnState turn, Dictionary<PlayerPair, List<PlayerDiplomaticAction>> diplomaticActions)
		{
			foreach (KeyValuePair<PlayerPair, List<PlayerDiplomaticAction>> keyValuePair in diplomaticActions)
			{
				PlayerPair playerPair;
				List<PlayerDiplomaticAction> list;
				keyValuePair.Deconstruct(out playerPair, out list);
				List<PlayerDiplomaticAction> diplomaticActions2 = list;
				DiplomaticOrderProcessor.ProcessDiplomaticActions(turn, diplomaticActions2);
			}
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0004B0A8 File Offset: 0x000492A8
		public static void ProcessDiplomaticActions(TurnState turn, List<PlayerDiplomaticAction> diplomaticActions)
		{
			diplomaticActions.Sort((PlayerDiplomaticAction lhs, PlayerDiplomaticAction rhs) => DiplomaticOrderProcessor.CompareDiplomaticMight(turn, lhs, rhs));
			diplomaticActions.Reverse();
			DiplomaticOrderProcessor.ProcessSortedDiplomaticActions(turn, diplomaticActions);
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0004B0E8 File Offset: 0x000492E8
		private static void ProcessSortedDiplomaticActions(TurnState turn, List<PlayerDiplomaticAction> diplomaticActions)
		{
			if (diplomaticActions.Count == 0)
			{
				return;
			}
			PlayerDiplomaticAction playerDiplomaticAction = diplomaticActions[0];
			Result result = DiplomaticStateProcessor.ValidateAction(turn, playerDiplomaticAction);
			Result result2;
			if (!result)
			{
				result2 = result;
			}
			else
			{
				PlayerDiplomaticOrder playerDiplomaticOrder = playerDiplomaticAction as PlayerDiplomaticOrder;
				Result result3;
				if (playerDiplomaticOrder == null)
				{
					PlayerDiplomaticDecision playerDiplomaticDecision = playerDiplomaticAction as PlayerDiplomaticDecision;
					if (playerDiplomaticDecision == null)
					{
						result3 = Result.SimulationError(string.Format("Unknown PlayerDiplomaticAction subclass {0}", playerDiplomaticAction));
					}
					else
					{
						result3 = playerDiplomaticDecision.Processor.Enact(playerDiplomaticDecision.Response);
					}
				}
				else
				{
					result3 = playerDiplomaticOrder.Processor.Enact(playerDiplomaticOrder.Request);
				}
				result2 = result3;
			}
			Result result4 = result2;
			if (!result4)
			{
				DiplomaticOrderProcessor.FinalizeDiplomaticAction(turn, playerDiplomaticAction, result);
				diplomaticActions.Remove(playerDiplomaticAction);
				DiplomaticOrderProcessor.ProcessSortedDiplomaticActions(turn, diplomaticActions);
				return;
			}
			DiplomaticOrderProcessor.FinalizeDiplomaticAction(turn, playerDiplomaticAction, result4);
			int rank = (int)playerDiplomaticAction.Player.Rank;
			turn.GetDiplomaticStatus(playerDiplomaticAction.ActorId, playerDiplomaticAction.TargetId);
			for (int i = 1; i < diplomaticActions.Count; i++)
			{
				PlayerDiplomaticAction playerDiplomaticAction2 = diplomaticActions[i];
				Result.OutInfluencedProblem outInfluencedProblem;
				if (playerDiplomaticAction2.Player.Rank >= (Rank)rank)
				{
					(outInfluencedProblem = new Result.OutInfluencedProblem(playerDiplomaticAction2.TargetId, playerDiplomaticAction2.OrderType)).IsDecisionResponse = playerDiplomaticAction2.IsDecision;
				}
				else
				{
					outInfluencedProblem = new Result.OutRankedProblem(playerDiplomaticAction2.TargetId, playerDiplomaticAction2.OrderType);
				}
				Result.DiplomacyProblem result5 = outInfluencedProblem;
				DiplomaticOrderProcessor.FinalizeDiplomaticAction(turn, playerDiplomaticAction2, result5);
			}
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0004B238 File Offset: 0x00049438
		private static int CompareDiplomaticMight(TurnState turn, PlayerDiplomaticAction lhs, PlayerDiplomaticAction rhs)
		{
			int num = lhs.Player.Rank.CompareTo(rhs.Player.Rank);
			if (num != 0)
			{
				return num;
			}
			num = lhs.OrderSlotIndex.CompareTo(rhs.OrderSlotIndex);
			if (num != 0)
			{
				return -num;
			}
			int num2 = turn.TurnsUntilRegency(lhs.Player);
			int value = turn.TurnsUntilRegency(rhs.Player);
			num = num2.CompareTo(value);
			return -num;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0004B2B4 File Offset: 0x000494B4
		public static void FinalizeDiplomaticAction(TurnState turn, in PlayerDiplomaticAction diplomaticAction, Result result)
		{
			if (!result.successful)
			{
				PlayerDiplomaticOrder playerDiplomaticOrder = diplomaticAction as PlayerDiplomaticOrder;
				if (playerDiplomaticOrder != null)
				{
					DiplomaticPairStatus diplomaticStatus = turn.GetDiplomaticStatus(playerDiplomaticOrder.ActorId, playerDiplomaticOrder.TargetId);
					DiplomaticOrderProcessor.RefundPayment(turn, diplomaticAction.Player, diplomaticAction.Payment);
					turn.AddGameEvent<ActionFailedEvent>(new ActionFailedEvent(playerDiplomaticOrder.Request, result, playerDiplomaticOrder.Player.Id));
					diplomaticStatus.ClearCooldownCount(playerDiplomaticOrder.Request.OrderType);
					foreach (OrderTypes orderType in playerDiplomaticOrder.Request.GetRelatedOrderTypes())
					{
						diplomaticStatus.ClearCooldownCount(orderType);
					}
				}
			}
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0004B378 File Offset: 0x00049578
		public static void RefundPayment(TurnState turn, PlayerState player, Payment payment)
		{
			player.GivePayment(payment);
		}
	}
}
