using System;
using Core.StaticData;

namespace LoG
{
	// Token: 0x020006BB RID: 1723
	public class ActionPhase_SelectPraetorCombatMove : ActionPhase_SingleTarget<ConfigRef<PraetorCombatMoveStaticData>>
	{
		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x0006C64C File Offset: 0x0006A84C
		public Identifier Praetor
		{
			get
			{
				TargetContext targetContext = this._targetContext;
				if (targetContext == null)
				{
					return Identifier.Invalid;
				}
				return targetContext.ItemId;
			}
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0006C65F File Offset: 0x0006A85F
		public ActionPhase_SelectPraetorCombatMove(Identifier praetor, Action<ConfigRef<PraetorCombatMoveStaticData>> setTarget) : this(new TargetContext(praetor), setTarget)
		{
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0006C66E File Offset: 0x0006A86E
		public ActionPhase_SelectPraetorCombatMove(Identifier praetor, Action<ConfigRef<PraetorCombatMoveStaticData>> setTarget, ActionPhase_SingleTarget<ConfigRef<PraetorCombatMoveStaticData>>.IsValidFunc validateTarget) : this(new TargetContext(praetor), setTarget, validateTarget)
		{
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0006C67E File Offset: 0x0006A87E
		public ActionPhase_SelectPraetorCombatMove(TargetContext target, Action<ConfigRef<PraetorCombatMoveStaticData>> setTarget) : this(target, setTarget, ActionPhase_SingleTarget<ConfigRef<PraetorCombatMoveStaticData>>.ValidFunc)
		{
			this._targetContext = target;
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0006C694 File Offset: 0x0006A894
		public ActionPhase_SelectPraetorCombatMove(TargetContext target, Action<ConfigRef<PraetorCombatMoveStaticData>> setTarget, ActionPhase_SingleTarget<ConfigRef<PraetorCombatMoveStaticData>>.IsValidFunc validateTarget) : base(setTarget, validateTarget)
		{
			this._targetContext = target;
		}

		// Token: 0x04000D12 RID: 3346
		private TargetContext _targetContext;
	}
}
