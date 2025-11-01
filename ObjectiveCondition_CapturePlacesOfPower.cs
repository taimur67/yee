using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000558 RID: 1368
	[Serializable]
	public class ObjectiveCondition_CapturePlacesOfPower : ObjectiveCondition_EventFilter<GameItemOwnershipChanged>
	{
		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x0005BFD0 File Offset: 0x0005A1D0
		public override string LocalizationKey
		{
			get
			{
				if (base.IsTargeted)
				{
					return "CapturePlacesOfPower.Targeted";
				}
				if (this.MustBePlayerControlled)
				{
					return "CapturePlacesOfPower.Enemy";
				}
				return "CapturePlacesOfPower";
			}
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0005BFF4 File Offset: 0x0005A1F4
		protected override bool Filter(TurnContext context, GameItemOwnershipChanged @event, PlayerState owner, PlayerState target)
		{
			GamePiece gamePiece;
			return (!this.MustBePlayerControlled || @event.OriginalOwner != -1) && context.CurrentTurn.TryFetchGameItem<GamePiece>(@event.Item, out gamePiece) && gamePiece.SubCategory.IsPlaceOfPower(this.ExcludePandaemonium, this.ExcludeStrongholds) && (this.TargetItem.IsEmpty() || gamePiece.StaticDataReference.Equals(this.TargetItem)) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0005C073 File Offset: 0x0005A273
		public override int GetHashCode()
		{
			return base.GetHashCode() + (this.MustBePlayerControlled ? 29 : 0);
		}

		// Token: 0x04000BF2 RID: 3058
		[JsonProperty]
		public bool MustBePlayerControlled;

		// Token: 0x04000BF3 RID: 3059
		[JsonProperty]
		public bool ExcludePandaemonium;

		// Token: 0x04000BF4 RID: 3060
		[JsonProperty]
		public bool ExcludeStrongholds;

		// Token: 0x04000BF5 RID: 3061
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> TargetItem;
	}
}
