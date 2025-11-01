using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000649 RID: 1609
	public static class SeekManuscriptUtils
	{
		// Token: 0x06001DB0 RID: 7600 RVA: 0x00066580 File Offset: 0x00064780
		public static SeekManuscriptUtils.ManuscriptParameters GetSeekManuscriptParameters(this PlayerState player, TurnProcessContext context)
		{
			return new SeekManuscriptUtils.ManuscriptParameters
			{
				IsAvailable = player.IsDrawTributeAvailable,
				Draw = player.ManuscriptSeekNumDrawn,
				Pick = player.ManuscriptSeekNumKept,
				TypeQueue = player.ManuscriptQueue,
				PlayerTags = player.EnumerateTags<EntityTag>()
			};
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x000665E8 File Offset: 0x000647E8
		public static Result CreateAndAddSeekManuscriptDecisionToPlayer(TurnProcessContext context, PlayerState player, SeekManuscriptUtils.ManuscriptParameters offeringParams)
		{
			SelectTributeDecisionRequest decisionRequest = SeekManuscriptUtils.CreateSeekManuscriptSelection(context, player, offeringParams);
			context.CurrentTurn.AddDecisionToAskPlayer(player.Id, decisionRequest);
			return Result.Success;
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x00066618 File Offset: 0x00064818
		public static SelectTributeDecisionRequest CreateSeekManuscriptSelection(TurnProcessContext context, PlayerState player, SeekManuscriptUtils.ManuscriptParameters offeringParams)
		{
			DemandPayload demandPayload = new DemandPayload();
			bool forceLuckyRoll = IEnumerableExtensions.Any<EntityTag_CheatLuckyManuscriptDraw>(offeringParams.PlayerTags.OfType<EntityTag_CheatLuckyManuscriptDraw>());
			for (int i = 0; i < offeringParams.Draw; i++)
			{
				ManuscriptCategory manuscriptType = offeringParams.TypeQueue.Draw(context.Random);
				ManuscriptStaticData manuscriptStaticData = null;
				int num = 10;
				do
				{
					manuscriptStaticData = context.GetWeightedManuscriptForPlayer(player, manuscriptType, forceLuckyRoll);
					num--;
				}
				while (demandPayload.Manuscripts.Any((Manuscript m) => m.StaticDataId == manuscriptStaticData.Id) && num > 0);
				Manuscript item = context.CurrentTurn.SpawnManuscript(manuscriptStaticData, context.CurrentTurn.ForceMajeurePlayer);
				demandPayload.Manuscripts.Add(item);
			}
			return new SelectTributeDecisionRequest(context.CurrentTurn, demandPayload, offeringParams.Pick, false);
		}

		// Token: 0x02000A63 RID: 2659
		public struct ManuscriptParameters
		{
			// Token: 0x040019AA RID: 6570
			public bool IsAvailable;

			// Token: 0x040019AB RID: 6571
			public int Draw;

			// Token: 0x040019AC RID: 6572
			public int Pick;

			// Token: 0x040019AD RID: 6573
			public IEnumerable<EntityTag> PlayerTags;

			// Token: 0x040019AE RID: 6574
			public ManuscriptTypeQueue TypeQueue;
		}
	}
}
