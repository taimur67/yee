using System;

namespace LoG
{
	// Token: 0x0200055D RID: 1373
	[Serializable]
	public class ObjectiveCondition_CastSacrificeRituals : ObjectiveCondition_CastRituals
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x0005C36A File Offset: 0x0005A56A
		public override string LocalizationKey
		{
			get
			{
				return "CastSacrificeRituals";
			}
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0005C371 File Offset: 0x0005A571
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			return base.Filter(context, @event, owner, target) && @event.AffectedPlayerIds.Count <= 1 && @event.AffectedPlayerID == owner.Id && @event.Contains<LegionKilledEvent>();
		}
	}
}
