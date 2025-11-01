using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000509 RID: 1289
	public static class TurnProcessorExtensions
	{
		// Token: 0x060018CF RID: 6351 RVA: 0x000588A0 File Offset: 0x00056AA0
		public static void AddSiphonedTribute(this TurnProcessContext context, int receivingPlayer, ResourceNFT resource)
		{
			Dictionary<int, List<ResourceNFT>> siphonedTribute = context.ProcessContexts.SiphonedTribute;
			List<ResourceNFT> list;
			if (!siphonedTribute.TryGetValue(receivingPlayer, out list))
			{
				list = (siphonedTribute[receivingPlayer] = new List<ResourceNFT>());
			}
			list.Add(resource);
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x000588DC File Offset: 0x00056ADC
		public static void ProcessSiphonedTribute(this TurnProcessContext context)
		{
			TurnState currentTurn = context.CurrentTurn;
			foreach (KeyValuePair<int, List<ResourceNFT>> keyValuePair in context.ProcessContexts.SiphonedTribute)
			{
				int num;
				List<ResourceNFT> list;
				keyValuePair.Deconstruct(out num, out list);
				int num2 = num;
				List<ResourceNFT> nfts = list;
				PlayerState playerState = currentTurn.FindPlayerState(num2, null);
				ResourceNFT resourceNFT;
				if (playerState != null && context.ConsolidateTribute(nfts.Total(), out resourceNFT))
				{
					currentTurn.AddGameEvent<TributeSiphonedEvent>(new TributeSiphonedEvent(num2, new ResourceNFT[]
					{
						resourceNFT
					})).AddChildEvent<PaymentReceivedEvent>(playerState.GiveResources(new ResourceNFT[]
					{
						resourceNFT
					}));
				}
			}
		}
	}
}
