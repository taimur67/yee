using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006A0 RID: 1696
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class StealTributeEvent : GameEvent
	{
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x0006B958 File Offset: 0x00069B58
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0006B95B File Offset: 0x00069B5B
		[JsonConstructor]
		protected StealTributeEvent()
		{
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x0006B963 File Offset: 0x00069B63
		public StealTributeEvent(int triggeringPlayerID, PlayerState affectedPlayer, int numberOfTokensInVault, int numberOfTokensStolen, bool stoleManuscripts = false) : base(triggeringPlayerID)
		{
			this.TargetPlayerId = affectedPlayer.Id;
			this.NumberOfTokensInVault = numberOfTokensInVault;
			this.NumberOfTokensStolen = numberOfTokensStolen;
			this.StoleManuscripts = stoleManuscripts;
			base.AddAffectedPlayerId(this.TargetPlayerId);
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001F2E RID: 7982 RVA: 0x0006B99B File Offset: 0x00069B9B
		private string StolenObjectType
		{
			get
			{
				if (!this.StoleManuscripts)
				{
					return "tribute tokens";
				}
				return "manuscripts";
			}
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x0006B9B0 File Offset: 0x00069BB0
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0}/{1} {2} stolen from {3} by {4}", new object[]
			{
				this.NumberOfTokensStolen,
				this.NumberOfTokensInVault,
				this.StolenObjectType,
				this.TargetPlayerId,
				this.TriggeringPlayerID
			});
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x0006BA10 File Offset: 0x00069C10
		public override void DeepClone(out GameEvent clone)
		{
			StealTributeEvent stealTributeEvent = new StealTributeEvent
			{
				TargetPlayerId = this.TargetPlayerId,
				NumberOfTokensInVault = this.NumberOfTokensInVault,
				NumberOfTokensStolen = this.NumberOfTokensStolen,
				Target = this.Target,
				StoleManuscripts = this.StoleManuscripts
			};
			base.DeepCloneGameEventParts<StealTributeEvent>(stealTributeEvent);
			clone = stealTributeEvent;
		}

		// Token: 0x04000CE7 RID: 3303
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int TargetPlayerId;

		// Token: 0x04000CE8 RID: 3304
		[BindableValue("max_value", BindingOption.None)]
		[JsonProperty]
		public int NumberOfTokensInVault;

		// Token: 0x04000CE9 RID: 3305
		[BindableValue("value", BindingOption.None)]
		[JsonProperty]
		public int NumberOfTokensStolen;

		// Token: 0x04000CEA RID: 3306
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public bool StoleManuscripts;

		// Token: 0x04000CEB RID: 3307
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target;
	}
}
