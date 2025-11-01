using System;

namespace LoG
{
	// Token: 0x02000734 RID: 1844
	public class PubliciseSchemesProcessor : EdictEffectModuleProcessor<PubliciseSchemesInstance, PubliciseSchemesEffectStaticData>
	{
		// Token: 0x060022D6 RID: 8918 RVA: 0x00078FDD File Offset: 0x000771DD
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_TurnEnd, new TurnModuleProcessor.ProcessEvent(this.OnTurnEnd));
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x00078FF8 File Offset: 0x000771F8
		public void OnTurnEnd()
		{
			foreach (Identifier id in base._currentTurn.FindPlayerState(base.Instance.TargetPlayerId, null).ActiveSchemeCards)
			{
				base._currentTurn.FetchGameItem<SchemeCard>(id).Scheme.RevealPrivateScheme();
			}
			base.RemoveSelf();
		}
	}
}
