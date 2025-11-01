using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000574 RID: 1396
	[Serializable]
	public class ObjectiveCondition_DestroyLegions : ObjectiveCondition_EventFilter<LegionKilledEvent>
	{
		// Token: 0x06001AB1 RID: 6833 RVA: 0x0005D270 File Offset: 0x0005B470
		protected override bool Filter(TurnContext context, LegionKilledEvent @event, PlayerState owner, PlayerState target)
		{
			GamePiece gamePiece;
			if (!context.CurrentTurn.TryFetchGameItem<GamePiece>(@event.GamePieceId, out gamePiece))
			{
				return false;
			}
			if (!this.TargetGamePiece.IsEmpty() && !this.TargetGamePiece.Equals(gamePiece.StaticDataReference))
			{
				return false;
			}
			List<GamePieceCategory> allowedCategories = this.AllowedCategories;
			return (allowedCategories == null || allowedCategories.Count <= 0 || this.AllowedCategories.Contains(gamePiece.SubCategory)) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x04000C13 RID: 3091
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> TargetGamePiece;

		// Token: 0x04000C14 RID: 3092
		[JsonProperty]
		public List<GamePieceCategory> AllowedCategories = new List<GamePieceCategory>();
	}
}
