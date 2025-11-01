using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001FC RID: 508
	[BindableGameEvent]
	[Serializable]
	public class ConclaveFavouriteChangedEvent : GameEvent
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0002D582 File Offset: 0x0002B782
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0002D585 File Offset: 0x0002B785
		[JsonConstructor]
		private ConclaveFavouriteChangedEvent()
		{
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0002D58D File Offset: 0x0002B78D
		public ConclaveFavouriteChangedEvent(int previousFavouriteId, int newFavouriteId)
		{
			this.PreviousFavouriteId = previousFavouriteId;
			this.NewFavouriteId = newFavouriteId;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0002D5A3 File Offset: 0x0002B7A3
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.ConclaveFavouriteChanged;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0002D5AC File Offset: 0x0002B7AC
		public override void DeepClone(out GameEvent clone)
		{
			ConclaveFavouriteChangedEvent conclaveFavouriteChangedEvent = new ConclaveFavouriteChangedEvent
			{
				PreviousFavouriteId = this.PreviousFavouriteId,
				NewFavouriteId = this.NewFavouriteId
			};
			base.DeepCloneGameEventParts<ConclaveFavouriteChangedEvent>(conclaveFavouriteChangedEvent);
			clone = conclaveFavouriteChangedEvent;
		}

		// Token: 0x040004B6 RID: 1206
		[BindableValue("previous", BindingOption.IntPlayerId)]
		[JsonProperty]
		public int PreviousFavouriteId;

		// Token: 0x040004B7 RID: 1207
		[BindableValue("new", BindingOption.IntPlayerId)]
		[JsonProperty]
		public int NewFavouriteId;
	}
}
