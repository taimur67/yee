using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000249 RID: 585
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class UpkeepFailed : GameEvent
	{
		// Token: 0x06000B6E RID: 2926 RVA: 0x0002FD49 File Offset: 0x0002DF49
		[JsonConstructor]
		private UpkeepFailed()
		{
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002FD51 File Offset: 0x0002DF51
		public UpkeepFailed(int playerId, Identifier itemId, GameItemCategory itemCategory, int upkeepValueSum) : base(playerId)
		{
			this.Item = itemId;
			this.Category = itemCategory;
			this.UpkeepValueSum = upkeepValueSum;
			base.AddAffectedPlayerId(playerId);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002FD77 File Offset: 0x0002DF77
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Upkeep payment failed for {0}", this.Item);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0002FD90 File Offset: 0x0002DF90
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			TurnLogEntryType result;
			if (this.Category == GameItemCategory.ActiveRitual)
			{
				result = ((this.UpkeepValueSum <= 0) ? TurnLogEntryType.UpkeepFailedFreeRitual : TurnLogEntryType.UpkeepFailedRitual);
			}
			else
			{
				result = TurnLogEntryType.UpkeepFailed;
			}
			return result;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002FDC4 File Offset: 0x0002DFC4
		public override void DeepClone(out GameEvent clone)
		{
			UpkeepFailed upkeepFailed = new UpkeepFailed
			{
				Item = this.Item,
				Category = this.Category,
				UpkeepValueSum = this.UpkeepValueSum
			};
			base.DeepCloneGameEventParts<UpkeepFailed>(upkeepFailed);
			clone = upkeepFailed;
		}

		// Token: 0x04000515 RID: 1301
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Item;

		// Token: 0x04000516 RID: 1302
		[JsonProperty]
		public GameItemCategory Category;

		// Token: 0x04000517 RID: 1303
		[JsonProperty]
		public int UpkeepValueSum;
	}
}
