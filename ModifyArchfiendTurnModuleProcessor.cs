using System;

namespace LoG
{
	// Token: 0x02000443 RID: 1091
	public class ModifyArchfiendTurnModuleProcessor : LifetimeTurnModuleProcessor<ModifyArchfiendTurnModuleInstance, ModifyArchfiendTurnModuleStaticData>
	{
		// Token: 0x060014E6 RID: 5350 RVA: 0x0004F785 File Offset: 0x0004D985
		public override void Initialize()
		{
			base.Initialize();
			base.SubscribeTo(TurnProcessStage.TurnModule_Events, new TurnModuleProcessor.ProcessEvent(this.Process));
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0004F7A4 File Offset: 0x0004D9A4
		protected override void Process()
		{
			base.Process();
			if (base._currentTurn.TurnValue == base.Instance.CreatedTurn)
			{
				this.PushModifier();
				return;
			}
			if (base._currentTurn.TurnValue >= base.Instance.CreatedTurn + base.Instance.Lifetime)
			{
				base._currentTurn.AddGameEvent<GrandEventEnded>(new GrandEventEnded(base.Instance.EventEffectId, base.Instance.AffectedPlayerIds));
				return;
			}
			base._currentTurn.AddGameEvent<GrandEventContinued>(new GrandEventContinued(base.Instance.EventEffectId, base.Instance.AffectedPlayerIds));
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0004F849 File Offset: 0x0004DA49
		public override void OnRemoved()
		{
			base.OnRemoved();
			this.PopModifier();
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0004F858 File Offset: 0x0004DA58
		private void PushModifier()
		{
			PlayerTargetGroup modifiableTargetGroup = new PlayerTargetGroup(base.Instance.Modifier, base.Instance.AffectedPlayerIds);
			base.Instance.GlobalModifierId = base._currentTurn.PushGlobalModifier(modifiableTargetGroup);
			foreach (int playerId in base.Instance.AffectedPlayerIds)
			{
				this.TurnProcessContext.RecalculateAllModifiersFor(base._currentTurn.FindPlayerState(playerId, null));
			}
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0004F8D0 File Offset: 0x0004DAD0
		private void PopModifier()
		{
			base._currentTurn.PopGlobalModifier(base.Instance.GlobalModifierId);
			foreach (int playerId in base.Instance.AffectedPlayerIds)
			{
				this.TurnProcessContext.RecalculateAllModifiersFor(base._currentTurn.FindPlayerState(playerId, null));
			}
		}
	}
}
