using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200064F RID: 1615
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerTurn : IDeepClone<PlayerTurn>
	{
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x00066E7B File Offset: 0x0006507B
		public bool IsEmpty
		{
			get
			{
				return this.Orders.Count + this.Decisions.Count + this.OutgoingMessages.Count == 0;
			}
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x00066EA4 File Offset: 0x000650A4
		public PlayerTurn AddOrReplaceOrders(params ActionableOrder[] orders)
		{
			foreach (ActionableOrder actionableOrder in orders)
			{
				int orderIndex = this.GetOrderIndex(actionableOrder);
				if (orderIndex != -1)
				{
					this.Orders[orderIndex] = actionableOrder;
				}
				else
				{
					this.Orders.Add(actionableOrder);
				}
			}
			return this;
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x00066EED File Offset: 0x000650ED
		public PlayerTurn RemoveOrders(params ActionableOrder[] orders)
		{
			return this.RemoveOrders(IEnumerableExtensions.ToArray<Guid>(from t in orders
			select t.ActionInstanceId));
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x00066F20 File Offset: 0x00065120
		public PlayerTurn RemoveOrders(params Guid[] guids)
		{
			this.Orders.RemoveAll((ActionableOrder t) => IEnumerableExtensions.Contains<Guid>(guids, t.ActionInstanceId));
			return this;
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x00066F54 File Offset: 0x00065154
		public ActionableOrder GetOrder(Guid actionableInstanceId)
		{
			return this.Orders.FirstOrDefault((ActionableOrder t) => t.ActionInstanceId == actionableInstanceId);
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x00066F88 File Offset: 0x00065188
		private int GetOrderIndex(ActionableOrder order)
		{
			for (int i = 0; i < this.Orders.Count; i++)
			{
				if (this.Orders[i].ActionInstanceId == order.ActionInstanceId)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x00066FCC File Offset: 0x000651CC
		public PlayerTurn SetOrders(params ActionableOrder[] orders)
		{
			return this.SetOrders(IEnumerableExtensions.ToList<ActionableOrder>(orders));
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00066FDA File Offset: 0x000651DA
		public PlayerTurn SetOrders(IEnumerable<ActionableOrder> orders)
		{
			return this.SetOrders(IEnumerableExtensions.ToList<ActionableOrder>(orders));
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x00066FE8 File Offset: 0x000651E8
		public PlayerTurn SetOrders(List<ActionableOrder> orders)
		{
			this.Orders = orders;
			return this;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x00066FF2 File Offset: 0x000651F2
		public T AddDecision<T>(T response) where T : DecisionResponse
		{
			this.AddDecisions(new DecisionResponse[]
			{
				response
			});
			return response;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0006700C File Offset: 0x0006520C
		public PlayerTurn AddDecisions(params DecisionResponse[] decisions)
		{
			foreach (DecisionResponse decisionResponse in decisions)
			{
				this.RemoveDecision(decisionResponse.DecisionId);
				this.Decisions.Add(decisionResponse);
			}
			return this;
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x00067046 File Offset: 0x00065246
		public PlayerTurn SetDecisions(IEnumerable<DecisionResponse> decisions)
		{
			return this.SetDecisions(IEnumerableExtensions.ToList<DecisionResponse>(decisions));
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x00067054 File Offset: 0x00065254
		public PlayerTurn SetDecisions(List<DecisionResponse> decisions)
		{
			this.Decisions = decisions;
			return this;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x00067060 File Offset: 0x00065260
		public bool HasQueuedDiplomaticOrderVsPlayer(int playerID)
		{
			foreach (ActionableOrder actionableOrder in this.Orders)
			{
				DiplomaticOrder diplomaticOrder = actionableOrder as DiplomaticOrder;
				if (diplomaticOrder != null && diplomaticOrder.TargetID == playerID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000670C4 File Offset: 0x000652C4
		public DecisionResponse GetDecision(DecisionId id)
		{
			return this.Decisions.FirstOrDefault((DecisionResponse t) => t.DecisionId == id);
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000670F8 File Offset: 0x000652F8
		private void RemoveDecision(DecisionId id)
		{
			this.Decisions.RemoveAll((DecisionResponse t) => t.DecisionId == id);
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0006712C File Offset: 0x0006532C
		public DecisionResponse GetOrCreateDecision(DecisionRequest request)
		{
			DecisionResponse decisionResponse = this.GetDecision(request.DecisionId);
			if (decisionResponse == null)
			{
				decisionResponse = request.GenerateResponse();
				this.AddDecisions(new DecisionResponse[]
				{
					decisionResponse
				});
			}
			return decisionResponse;
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x00067162 File Offset: 0x00065362
		public T GetOrCreateDecision<T>(DecisionRequest request) where T : DecisionResponse
		{
			return (T)((object)this.GetOrCreateDecision(request));
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x00067170 File Offset: 0x00065370
		public void AddMessage(Message message)
		{
			PlayerTurn.<>c__DisplayClass25_0 CS$<>8__locals1 = new PlayerTurn.<>c__DisplayClass25_0();
			CS$<>8__locals1.message = message;
			if (this.OutgoingMessages.Any(new Func<Message, bool>(CS$<>8__locals1.<AddMessage>g__Predicate|0)))
			{
				return;
			}
			this.OutgoingMessages.Add(CS$<>8__locals1.message);
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000671B8 File Offset: 0x000653B8
		public bool TryGetMessage(int turnId, int toPlayerId, out Message message)
		{
			message = this.OutgoingMessages.Find((Message x) => x.TurnId == turnId && x.ToPlayerId == toPlayerId);
			return message != null;
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000671F7 File Offset: 0x000653F7
		public void RemoveMessage(Message message)
		{
			if (this.OutgoingMessages.Contains(message))
			{
				this.OutgoingMessages.Remove(message);
			}
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x00067214 File Offset: 0x00065414
		public void RemoveMessage(int turnId, int toPlayerId)
		{
			Message message = this.OutgoingMessages.Find((Message x) => x.TurnId == turnId && x.ToPlayerId == toPlayerId);
			if (message != null)
			{
				this.OutgoingMessages.Remove(message);
			}
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x00067260 File Offset: 0x00065460
		public void DeepClone(out PlayerTurn clone)
		{
			clone = new PlayerTurn
			{
				Decisions = this.Decisions.DeepClone<DecisionResponse>(),
				HasBeenPlayerSubmitted = this.HasBeenPlayerSubmitted,
				Orders = this.Orders.DeepClone(CloneFunction.FastClone),
				OutgoingMessages = this.OutgoingMessages.DeepClone<Message>(),
				DataChanges = this.DataChanges.DeepClone<AIPersistentData>()
			};
		}

		// Token: 0x04000CC1 RID: 3265
		public const int InvalidOrderIndex = -1;

		// Token: 0x04000CC2 RID: 3266
		[JsonProperty]
		public bool HasBeenPlayerSubmitted;

		// Token: 0x04000CC3 RID: 3267
		[JsonProperty]
		public List<ActionableOrder> Orders = new List<ActionableOrder>();

		// Token: 0x04000CC4 RID: 3268
		[JsonProperty]
		public List<DecisionResponse> Decisions = new List<DecisionResponse>();

		// Token: 0x04000CC5 RID: 3269
		[JsonProperty]
		public List<Message> OutgoingMessages = new List<Message>();

		// Token: 0x04000CC6 RID: 3270
		[JsonProperty]
		public AIPersistentData DataChanges;
	}
}
