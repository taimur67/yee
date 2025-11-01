using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000728 RID: 1832
	public class LaurelOfTriumphProcessor : EdictEffectModuleProcessor<LaurelOfTriumphInstance, LaurelOfTriumphEffectStaticData>
	{
		// Token: 0x060022BE RID: 8894 RVA: 0x00078C2D File Offset: 0x00076E2D
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnStarted));
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x00078C48 File Offset: 0x00076E48
		public void OnTurnStarted()
		{
			GamePieceModifier gamePieceModifier = new GamePieceModifier(new GamePieceModifierStaticData().SetValue(GamePieceStat.Prestige, (float)this._staticData.PrestigeBoost, ModifierTarget.ValueOffset));
			gamePieceModifier.Source = new EdictContext(base.Instance.EdictId, base.Instance.EffectId);
			GamePiece stronghold = base._currentTurn.GetStronghold(base.Instance.TargetPlayerId);
			if (stronghold != null)
			{
				gamePieceModifier.InstallInto(stronghold, base._currentTurn, false);
			}
			base.RemoveSelf();
		}
	}
}
