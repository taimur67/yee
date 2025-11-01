using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020003EC RID: 1004
	public static class PaymentProcessor
	{
		// Token: 0x06001402 RID: 5122 RVA: 0x0004CFB3 File Offset: 0x0004B1B3
		public static Result AcceptPayment(this TurnState state, int playerId, Payment payment)
		{
			PlayerState playerState = state.FindPlayerState(playerId, null);
			return ((playerState != null) ? playerState.AcceptPayment(payment) : null) ?? Result.Failure;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0004CFD4 File Offset: 0x0004B1D4
		public static Result AcceptPayment(this PlayerState player, Payment payment)
		{
			Problem problem = player.ValidatePayment(payment) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return player.RemovePayment(payment);
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0004CFFA File Offset: 0x0004B1FA
		public static Result RemovePayment(this TurnState state, int playerId, Payment payment)
		{
			return state.FindPlayerState(playerId, null).RemovePayment(payment);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0004D00A File Offset: 0x0004B20A
		public static Result ConsolidateTribute(this TurnContext context, Payment payment, out ResourceNFT resource)
		{
			return context.ConsolidateTribute(payment.Total, out resource);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0004D019 File Offset: 0x0004B219
		public static bool ConsolidateTribute(this TurnContext context, ResourceAccumulation payment, out ResourceAccumulation consolidation)
		{
			if (payment.AnyGreaterThan(99, true))
			{
				consolidation = ResourceAccumulation.Empty;
				return false;
			}
			consolidation = new ResourceAccumulation(new ResourceAccumulation[]
			{
				payment
			});
			consolidation.Limit(context.Rules.MaximumValueOfIndividualResourceOnToken);
			return true;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0004D054 File Offset: 0x0004B254
		public static Result ConsolidateTribute(this TurnContext context, ResourceAccumulation payment, out ResourceNFT resource)
		{
			resource = null;
			ResourceAccumulation resourceAccumulation;
			if (context.ConsolidateTribute(payment, out resourceAccumulation))
			{
				resource = context.CurrentTurn.CreateNFT(new ResourceAccumulation[]
				{
					resourceAccumulation
				});
			}
			if (resource != null)
			{
				return Result.Success;
			}
			return Result.Failure;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004D095 File Offset: 0x0004B295
		public static Result CanAfford(this PlayerState player, Cost cost)
		{
			return new Payment(player.Resources, player.SpendablePrestige).IsSufficient(cost);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004D0AE File Offset: 0x0004B2AE
		public static bool IsFulfilledBy(this Cost cost, ResourceAccumulation accumulation, int numCards)
		{
			return accumulation.MeetsValueObligations(cost) && numCards >= cost.RequiredTokenCount;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004D0C7 File Offset: 0x0004B2C7
		public static Result IsSufficient(this Payment payment, Cost cost)
		{
			if (!cost.IsFulfilledBy(payment.Total, payment.Resources.Count))
			{
				return Result.CannotAfford(cost, payment);
			}
			return Result.Success;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0004D0EF File Offset: 0x0004B2EF
		public static bool MeetsValueObligations(this Payment payment, Cost cost)
		{
			return payment.Total.MeetsValueObligations(cost);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0004D100 File Offset: 0x0004B300
		public static bool MeetsValueObligations(this ResourceAccumulation paymentTotal, Cost cost)
		{
			foreach (ValueTuple<ResourceTypes, int> valueTuple in cost.EnumerateResourceValues())
			{
				ResourceTypes item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (item2 > 0 && paymentTotal[item] < item2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0004D168 File Offset: 0x0004B368
		public static Result ValidatePayment(this TurnState state, int playerId, Cost cost, Payment payment)
		{
			PlayerState player = state.FindPlayerState(playerId, null);
			return state.ValidatePayment(player, cost, payment);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0004D188 File Offset: 0x0004B388
		public static Result ValidatePayment(this TurnState state, PlayerState player, Cost cost, Payment payment)
		{
			Problem problem = player.ValidatePayment(payment) as Problem;
			if (problem != null)
			{
				return problem;
			}
			return payment.IsSufficient(cost);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0004D1AE File Offset: 0x0004B3AE
		public static Result ValidatePayment(this TurnState state, int playerId, Payment payment)
		{
			return state.FindPlayerState(playerId, null).ValidatePayment(payment);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0004D1C0 File Offset: 0x0004B3C0
		public static Result ValidatePayment(this PlayerState state, Payment payment)
		{
			if (payment == null)
			{
				return Result.Failure;
			}
			List<ResourceNFT> list = IEnumerableExtensions.ToList<ResourceNFT>(state.Resources);
			using (List<ResourceNFT>.Enumerator enumerator = payment.Resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ResourceNFT resource = enumerator.Current;
					ResourceNFT resourceNFT = list.FirstOrDefault((ResourceNFT x) => x.ValueHash == resource.ValueHash);
					if (resourceNFT == null)
					{
						return Result.CounterfeitTribute(state.Id, new int[]
						{
							resource.Id
						});
					}
					list.Remove(resourceNFT);
				}
			}
			return Result.Success;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0004D278 File Offset: 0x0004B478
		public static Result RemovePayment(this PlayerState state, Payment payment)
		{
			if (payment.Resources == null)
			{
				return Result.Failure;
			}
			for (int i = payment.Resources.Count - 1; i >= 0; i--)
			{
				ResourceNFT resource = payment.Resources[i];
				ResourceNFT resourceNFT = state.Resources.FirstOrDefault((ResourceNFT x) => x.Id == resource.Id) ?? state.Resources.FirstOrDefault((ResourceNFT x) => x.ValueHash == resource.ValueHash);
				if (resourceNFT == null)
				{
					return Result.Failure;
				}
				state.Resources.Remove(resourceNFT);
				payment.Resources[i] = resourceNFT;
			}
			state.RemovePrestige(payment.Prestige);
			return Result.Success;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0004D32C File Offset: 0x0004B52C
		public static bool HasResource(this PlayerState state, ResourceNFT resource)
		{
			return state.Resources.Any((ResourceNFT x) => x.ValueHash == resource.ValueHash);
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0004D35D File Offset: 0x0004B55D
		public static Result GivePayment(this PlayerState state, Payment payment)
		{
			if (payment == null)
			{
				return Result.Failure;
			}
			state.GivePrestige(payment.Prestige);
			state.GiveResources(payment.Resources.ToArray());
			return Result.Success;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0004D38C File Offset: 0x0004B58C
		public static void GivePrestige(this PlayerState state, int prestige)
		{
			state.SpendablePrestige += prestige;
			if (state.SpendablePrestige < 0)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Trace(string.Format("Prestige loss of {0} caused an overspend of {1}. Clamping value to 0.", -prestige, -state.SpendablePrestige));
				}
				state.SpendablePrestige = 0;
			}
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0004D3E4 File Offset: 0x0004B5E4
		public static void RemovePrestige(this PlayerState state, int prestige)
		{
			state.GivePrestige(-prestige);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0004D3F0 File Offset: 0x0004B5F0
		public static PaymentReceivedEvent GiveResources(this PlayerState state, params ResourceNFT[] nfts)
		{
			foreach (ResourceNFT item in nfts)
			{
				state.Resources.Add(item);
			}
			return new PaymentReceivedEvent(state.Id, state.Id, new Payment(nfts), null);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0004D435 File Offset: 0x0004B635
		public static PaymentReceivedEvent GiveResources(this PlayerState state, IEnumerable<ResourceNFT> nfts)
		{
			return state.GiveResources(IEnumerableExtensions.ToArray<ResourceNFT>(nfts));
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0004D443 File Offset: 0x0004B643
		public static void TransferResource(this PlayerState owner, ResourceNFT nft, PlayerState recipient)
		{
			owner.Resources.Remove(nft);
			recipient.Resources.Add(nft);
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0004D45E File Offset: 0x0004B65E
		public static void DestroyResource(this PlayerState state, ResourceNFT nft)
		{
			state.Resources.Remove(nft);
		}

		// Token: 0x0200098D RID: 2445
		[Serializable]
		public class CounterfeitTributeProblem : Problem
		{
			// Token: 0x17000669 RID: 1641
			// (get) Token: 0x06002CB7 RID: 11447 RVA: 0x00091333 File Offset: 0x0008F533
			public override string DebugString
			{
				get
				{
					return "Attempted to pay with cards you don't own";
				}
			}

			// Token: 0x1700066A RID: 1642
			// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x0009133A File Offset: 0x0008F53A
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CounterfeitTributeProblem";
				}
			}
		}

		// Token: 0x0200098E RID: 2446
		[Serializable]
		public class CannotAfford : Problem
		{
			// Token: 0x1700066B RID: 1643
			// (get) Token: 0x06002CBA RID: 11450 RVA: 0x00091354 File Offset: 0x0008F554
			public override string DebugString
			{
				get
				{
					return this._output;
				}
			}

			// Token: 0x06002CBB RID: 11451 RVA: 0x0009135C File Offset: 0x0008F55C
			public CannotAfford(string str)
			{
				this._output = str;
			}

			// Token: 0x1700066C RID: 1644
			// (get) Token: 0x06002CBC RID: 11452 RVA: 0x0009136B File Offset: 0x0008F56B
			public override string LocKey
			{
				get
				{
					return this.LocKeyScope + ".CannotAfford";
				}
			}

			// Token: 0x04001661 RID: 5729
			private string _output;
		}
	}
}
