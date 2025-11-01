using System;

namespace LoG
{
	// Token: 0x0200019B RID: 411
	public class WPVanitysAnointed : WorldProperty
	{
		// Token: 0x06000784 RID: 1924 RVA: 0x000233F1 File Offset: 0x000215F1
		public static bool Check(TurnState turn, PlayerState caster, Identifier target = Identifier.Invalid)
		{
			if (target != Identifier.Invalid)
			{
				return turn.IsRitualActiveAndTargeting(caster, "andromalius_vanitys_anointed", target);
			}
			return turn.IsRitualActive(caster, "andromalius_vanitys_anointed");
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00023411 File Offset: 0x00021611
		public WPVanitysAnointed(Identifier targetPraetorID = Identifier.Invalid)
		{
			this.TargetPraetorID = targetPraetorID;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00023420 File Offset: 0x00021620
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			return WPVanitysAnointed.Check(viewContext.CurrentTurn, playerState, this.TargetPraetorID);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00023434 File Offset: 0x00021634
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPVanitysAnointed wpvanitysAnointed = precondition as WPVanitysAnointed;
			if (wpvanitysAnointed == null)
			{
				return WPProvidesEffect.No;
			}
			if (wpvanitysAnointed.TargetPraetorID != Identifier.Invalid && this.TargetPraetorID != wpvanitysAnointed.TargetPraetorID)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400036F RID: 879
		public Identifier TargetPraetorID;
	}
}
