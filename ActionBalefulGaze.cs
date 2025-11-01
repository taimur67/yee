using System;

namespace LoG
{
	// Token: 0x020000EB RID: 235
	public class ActionBalefulGaze : ActionCastRitual<TrueKnowledgeRitualOrder>
	{
		// Token: 0x06000390 RID: 912 RVA: 0x00010B56 File Offset: 0x0000ED56
		protected override string GetRitualId()
		{
			return "lilith_baleful_gaze";
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00010B5D File Offset: 0x0000ED5D
		public static bool CanBeUsedByArchfiend(PlayerState wouldBeCaster)
		{
			return wouldBeCaster.ArchfiendId == "Lilith";
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00010B6F File Offset: 0x0000ED6F
		protected override Identifier GetTargetItemId()
		{
			return this._targetGamePieceId;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00010B77 File Offset: 0x0000ED77
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Baleful_Gaze;
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00010B7B File Offset: 0x0000ED7B
		protected override PowerType GetPowerType()
		{
			return PowerType.Prophecy;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00010B7E File Offset: 0x0000ED7E
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00010B81 File Offset: 0x0000ED81
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00010B84 File Offset: 0x0000ED84
		public ActionBalefulGaze(Identifier targetID)
		{
			this._targetGamePieceId = targetID;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00010B94 File Offset: 0x0000ED94
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			base.AddConstraint(new WPTrueKnowledgeGamePiece(Identifier.Invalid)
			{
				InvertLogic = true
			});
			base.AddEffect(new WPTrueKnowledgeGamePiece(this._targetGamePieceId));
			base.AddScalarCostModifier(-1f, PFCostModifier.Archfiend_Bonus);
		}

		// Token: 0x04000217 RID: 535
		public const string RitualId = "lilith_baleful_gaze";

		// Token: 0x04000218 RID: 536
		private const string ArchfiendId = "Lilith";

		// Token: 0x04000219 RID: 537
		private readonly Identifier _targetGamePieceId;
	}
}
