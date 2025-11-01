using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001FB RID: 507
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RepatriateLegionEvent : GameEvent
	{
		// Token: 0x060009D6 RID: 2518 RVA: 0x0002D52E File Offset: 0x0002B72E
		[JsonConstructor]
		private RepatriateLegionEvent()
		{
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0002D536 File Offset: 0x0002B736
		public RepatriateLegionEvent(GamePiece legion)
		{
			base.AddAffectedPlayerId(legion.ControllingPlayerId);
			this.LegionId = legion.Id;
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0002D558 File Offset: 0x0002B758
		public override void DeepClone(out GameEvent clone)
		{
			RepatriateLegionEvent repatriateLegionEvent = new RepatriateLegionEvent
			{
				LegionId = this.LegionId
			};
			base.DeepCloneGameEventParts<RepatriateLegionEvent>(repatriateLegionEvent);
			clone = repatriateLegionEvent;
		}

		// Token: 0x040004B5 RID: 1205
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public Identifier LegionId;
	}
}
