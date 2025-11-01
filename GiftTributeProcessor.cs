using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200071F RID: 1823
	public class GiftTributeProcessor : EdictEffectModuleProcessor<GiftTributeInstance, GiftTributeEffectStaticData>
	{
		// Token: 0x060022AC RID: 8876 RVA: 0x0007889D File Offset: 0x00076A9D
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000788B8 File Offset: 0x00076AB8
		public void OnTurnEnd()
		{
			using (IEnumerator<PlayerState> enumerator = base._currentTurn.EnumeratePlayerStates(false, false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerState player = enumerator.Current;
					if (!this._staticData.ReceivingRanks.Select(new Func<ConfigRef<ArchfiendRankStaticData>, ArchfiendRankStaticData>(base._database.Fetch<ArchfiendRankStaticData>)).All((ArchfiendRankStaticData r) => r.RankValue != player.RankValue))
					{
						TributeEconomyStaticData tributeEconomyStaticData = base._database.FetchSingle<TributeEconomyStaticData>();
						CardGenerationData generationData = base._database.Fetch(tributeEconomyStaticData.DemandTributeGenerationData);
						DemandTributeUtils.TributeParameters demandTributeParameters = player.GetDemandTributeParameters(this.TurnProcessContext);
						demandTributeParameters.Draw = this._staticData.GiftAmount;
						demandTributeParameters.Pick = this._staticData.GiftAmount;
						IEnumerable<ResourceNFT> nfts = DemandTributeUtils.GenerateCandidateTribute(this.TurnProcessContext, generationData, demandTributeParameters);
						player.GiveResources(nfts);
					}
				}
			}
			base.RemoveSelf();
		}
	}
}
