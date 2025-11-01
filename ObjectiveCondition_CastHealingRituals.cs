using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200055A RID: 1370
	[Serializable]
	public class ObjectiveCondition_CastHealingRituals : ObjectiveCondition_CastRituals
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x0005C1D7 File Offset: 0x0005A3D7
		public override string LocalizationKey
		{
			get
			{
				return "CastHealingRituals";
			}
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0005C1E0 File Offset: 0x0005A3E0
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			if (!@event.Contains<HealGamePieceEvent>())
			{
				return false;
			}
			foreach (HealGamePieceEvent healGamePieceEvent in @event.Enumerate<HealGamePieceEvent>())
			{
				if (healGamePieceEvent.HealingAmount >= this.MinimumHealing)
				{
					if (!this.MustResultInFullHealth)
					{
						return true;
					}
					GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(healGamePieceEvent.Target);
					if (gamePiece != null && gamePiece.HP == gamePiece.TotalHP)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000BF8 RID: 3064
		[JsonProperty]
		public int MinimumHealing = 1;

		// Token: 0x04000BF9 RID: 3065
		[JsonProperty]
		public bool MustResultInFullHealth;
	}
}
