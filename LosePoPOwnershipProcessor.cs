using System;
using System.Linq;

namespace LoG
{
	// Token: 0x0200072E RID: 1838
	public class LosePoPOwnershipProcessor : EdictEffectModuleProcessor<LosePoPOwnershipInstance, LosePoPOwnershipEffectStaticData>
	{
		// Token: 0x060022CA RID: 8906 RVA: 0x00078DD1 File Offset: 0x00076FD1
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_PostEdicts, new TurnModuleProcessor.ProcessEvent(this.NeutralisePoPs));
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00078DF0 File Offset: 0x00076FF0
		public void NeutralisePoPs()
		{
			int count = base._currentTurn.FindPlayerState(base.Instance.TargetPlayerId, null).RankValue + 1;
			foreach (GamePiece neutral in IEnumerableExtensions.ToList<GamePiece>(from x in base._currentTurn.GetActiveGamePiecesForPlayer(base.Instance.TargetPlayerId)
			where x.SubCategory == GamePieceCategory.PoP
			where !x.HasTag<EntityTag_CannotBeNeutral>()
			select x).SelectRandom(base._random, count))
			{
				base._currentTurn.SetNeutral(neutral);
			}
			base.RemoveSelf();
		}
	}
}
