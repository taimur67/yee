using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200063F RID: 1599
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class OrderMoveLegion : ActionableOrder
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x00065FEF File Offset: 0x000641EF
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.MarchLegion;
			}
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x00065FF3 File Offset: 0x000641F3
		protected OrderMoveLegion()
		{
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x00066002 File Offset: 0x00064202
		protected OrderMoveLegion(Identifier gamePiece, AttackOutcomeIntent attackOutcomeIntent = AttackOutcomeIntent.Default, FlankIntent moveIntent = FlankIntent.Undefined)
		{
			this.GamePieceId = gamePiece;
			this.AttackOutcomeIntent = attackOutcomeIntent;
			this.MoveIntent = moveIntent;
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x00066026 File Offset: 0x00064226
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new MoveLegionActionConflict(this.GamePieceId);
			yield break;
		}

		// Token: 0x04000C92 RID: 3218
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier GamePieceId = Identifier.Invalid;

		// Token: 0x04000C93 RID: 3219
		[JsonProperty]
		[DefaultValue(false)]
		public AttackOutcomeIntent AttackOutcomeIntent;

		// Token: 0x04000C94 RID: 3220
		[JsonProperty]
		[DefaultValue(FlankIntent.Undefined)]
		public FlankIntent MoveIntent;
	}
}
