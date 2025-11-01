using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x020002C8 RID: 712
	public static class ManuscriptExtensions
	{
		// Token: 0x06000DC6 RID: 3526 RVA: 0x00036825 File Offset: 0x00034A25
		public static ManuscriptCategory GetCategory(this Manuscript manuscript, GameDatabase db)
		{
			return db.Fetch<ManuscriptStaticData>(manuscript.StaticDataId).ManuscriptCategory;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00036838 File Offset: 0x00034A38
		public static int GetFragmentCount(this Manuscript manuscript, GameDatabase db)
		{
			ManuscriptStaticData manuscriptStaticData = db.Fetch<ManuscriptStaticData>(manuscript.StaticDataId);
			if (manuscriptStaticData == null)
			{
				return 1;
			}
			return manuscriptStaticData.FragmentCount;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00036854 File Offset: 0x00034A54
		public static bool IsManuscriptCompleted(this TurnContext context, int playerId, Manuscript manuscript)
		{
			int fragmentCount = manuscript.GetFragmentCount(context.Database);
			return context.GetManuscriptCurrentFragmentCount(playerId, manuscript) >= fragmentCount;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0003687C File Offset: 0x00034A7C
		public static bool IsManuscriptCompleted(this TurnContext context, int playerId, ManuscriptStaticData manuscript)
		{
			return context.GetManuscriptCurrentFragmentCount(playerId, manuscript.Id) >= manuscript.FragmentCount;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00036896 File Offset: 0x00034A96
		public static int GetManuscriptCurrentFragmentCount(this TurnProcessContext context, int playerId, Manuscript manuscript)
		{
			return context.GetManuscriptCurrentFragmentCount(playerId, manuscript);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x000368A0 File Offset: 0x00034AA0
		public static int GetManuscriptCurrentFragmentCount(this TurnContext context, int playerId, Manuscript manuscript)
		{
			return context.GetManuscriptCurrentFragmentCount(playerId, manuscript.StaticDataId);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x000368B0 File Offset: 0x00034AB0
		public static int GetManuscriptCurrentFragmentCount(this TurnContext context, int playerId, string manuscriptStaticDataId)
		{
			return context.CurrentTurn.GetGameItemsControlledBy<Manuscript>(playerId).Count((Manuscript x) => x.StaticDataId == manuscriptStaticDataId);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000368E8 File Offset: 0x00034AE8
		public static IEnumerable<Manuscript> GetAllManuscriptFragmentsControlledBy(this TurnProcessContext context, int playerId, string manuscriptStaticDataId)
		{
			return from x in context.CurrentTurn.GetGameItemsControlledBy<Manuscript>(playerId)
			where x.StaticDataId == manuscriptStaticDataId
			select x;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00036920 File Offset: 0x00034B20
		public static PraetorCombatMoveStaticData GetPraetorMoveFromManuscript(this TurnContext context, ManuscriptStaticData manuscriptStaticData)
		{
			ConfigRef<AbilityStaticData> providedAbility = manuscriptStaticData.ProvidedAbility;
			ConfigRef<PraetorCombatMoveStaticData> move = context.Database.Fetch<UpgradePraetorAbilityStaticData>(providedAbility.Id).Move;
			return context.Database.Fetch(move);
		}
	}
}
