using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005FB RID: 1531
	public class GainTributeEventProcessor : GrandEventActionProcessor<GainTributeEventOrder, GainTributeEventStaticData>
	{
		// Token: 0x06001C9A RID: 7322 RVA: 0x000629D8 File Offset: 0x00060BD8
		protected override Result ProcessInternal(PlayGrandEventOrder order)
		{
			TributeEconomyStaticData tributeEconomyStaticData = base._database.FetchSingle<TributeEconomyStaticData>();
			CardGenerationData generationData = base._database.Fetch(tributeEconomyStaticData.DemandTributeGenerationData);
			DemandTributeUtils.TributeParameters parameters = new DemandTributeUtils.TributeParameters
			{
				Draw = base.data.TokenCount,
				Pick = base.data.TokenCount,
				Quality = base.data.TributeQuality,
				TypeQueue = this._player.ResourceQueue,
				PlayerTags = this._player.EnumerateTags<EntityTag>(),
				IsAvailable = true
			};
			IEnumerable<ResourceNFT> resources = DemandTributeUtils.GenerateCandidateTribute(this.TurnProcessContext, generationData, parameters);
			PaymentReceivedEvent ev = this.TurnProcessContext.GivePayment(this._player, new Payment(resources, 0), null);
			base.GameEvent.AddChildEvent<PaymentReceivedEvent>(ev);
			base.GameEvent.AddAffectedPlayerId(this._player.Id);
			return Result.Success;
		}
	}
}
