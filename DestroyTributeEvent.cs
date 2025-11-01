using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200066C RID: 1644
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class DestroyTributeEvent : GameEvent
	{
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x00068AB4 File Offset: 0x00066CB4
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x00068AB7 File Offset: 0x00066CB7
		[JsonConstructor]
		protected DestroyTributeEvent()
		{
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x00068ABF File Offset: 0x00066CBF
		public DestroyTributeEvent(int triggeringPlayerID, PlayerState affectedPlayer, int numberOfTokensInVault, int numberDestroyed, bool destroyedManuscripts = false) : base(triggeringPlayerID)
		{
			this.TargetPlayerId = affectedPlayer.Id;
			this.NumberOfTokensInVault = numberOfTokensInVault;
			this.NumberOfTokensDestroyed = numberDestroyed;
			this.DestroyedManuscripts = destroyedManuscripts;
			base.AddAffectedPlayerId(this.TargetPlayerId);
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x00068AF7 File Offset: 0x00066CF7
		private string DestroyedObjectType
		{
			get
			{
				if (!this.DestroyedManuscripts)
				{
					return "tribute tokens";
				}
				return "manuscripts";
			}
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x00068B0C File Offset: 0x00066D0C
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} of {1}'s {2} destroyed by {3}", new object[]
			{
				this.NumberOfTokensInVault,
				this.TargetPlayerId,
				this.DestroyedObjectType,
				this.TriggeringPlayerID
			});
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x00068B5C File Offset: 0x00066D5C
		public override void DeepClone(out GameEvent clone)
		{
			DestroyTributeEvent destroyTributeEvent = new DestroyTributeEvent
			{
				TargetPlayerId = this.TargetPlayerId,
				NumberOfTokensInVault = this.NumberOfTokensInVault,
				NumberOfTokensDestroyed = this.NumberOfTokensDestroyed,
				Target = this.Target,
				DestroyedManuscripts = this.DestroyedManuscripts
			};
			base.DeepCloneGameEventParts<DestroyTributeEvent>(destroyTributeEvent);
			clone = destroyTributeEvent;
		}

		// Token: 0x04000CD1 RID: 3281
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int TargetPlayerId;

		// Token: 0x04000CD2 RID: 3282
		[BindableValue("max_value", BindingOption.None)]
		[JsonProperty]
		public int NumberOfTokensInVault;

		// Token: 0x04000CD3 RID: 3283
		[BindableValue("value", BindingOption.None)]
		[JsonProperty]
		public int NumberOfTokensDestroyed;

		// Token: 0x04000CD4 RID: 3284
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public bool DestroyedManuscripts;

		// Token: 0x04000CD5 RID: 3285
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target;
	}
}
