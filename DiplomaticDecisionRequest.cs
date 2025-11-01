using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004F7 RID: 1271
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public abstract class DiplomaticDecisionRequest<T> : DecisionRequest<T>, IDiplomaticDecisionRequest where T : DiplomaticDecisionResponse, new()
	{
		// Token: 0x0600181A RID: 6170 RVA: 0x00056B35 File Offset: 0x00054D35
		[JsonConstructor]
		protected DiplomaticDecisionRequest()
		{
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00056B3D File Offset: 0x00054D3D
		protected DiplomaticDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600181C RID: 6172 RVA: 0x00056B46 File Offset: 0x00054D46
		// (set) Token: 0x0600181D RID: 6173 RVA: 0x00056B4E File Offset: 0x00054D4E
		[JsonIgnore]
		[BindableValue("source_name", BindingOption.IntPlayerId)]
		public int RequestingPlayerId
		{
			get
			{
				return this._requestingPlayerId;
			}
			set
			{
				this._requestingPlayerId = value;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x0600181E RID: 6174 RVA: 0x00056B57 File Offset: 0x00054D57
		// (set) Token: 0x0600181F RID: 6175 RVA: 0x00056B5F File Offset: 0x00054D5F
		[JsonIgnore]
		[BindableValue("affected_name", BindingOption.IntPlayerId)]
		public int AffectedPlayerId
		{
			get
			{
				return this._affectedPlayerId;
			}
			set
			{
				this._affectedPlayerId = value;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001820 RID: 6176 RVA: 0x00056B68 File Offset: 0x00054D68
		// (set) Token: 0x06001821 RID: 6177 RVA: 0x00056B70 File Offset: 0x00054D70
		[JsonIgnore]
		[BindableValue("prestige", BindingOption.None)]
		public int PrestigeWager
		{
			get
			{
				return this._prestigeWager;
			}
			set
			{
				this._prestigeWager = value;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001822 RID: 6178
		[BindableValue(null, BindingOption.None)]
		public abstract OrderTypes OrderType { get; }

		// Token: 0x06001823 RID: 6179 RVA: 0x00056B7C File Offset: 0x00054D7C
		public virtual bool RelatesToPlayers(PlayerPair pair)
		{
			PlayerPair playerPair = new PlayerPair(this.AffectedPlayerId, this.RequestingPlayerId);
			return playerPair.Equals(pair);
		}

		// Token: 0x04000B72 RID: 2930
		[JsonProperty]
		private int _affectedPlayerId;

		// Token: 0x04000B73 RID: 2931
		[JsonProperty]
		private int _requestingPlayerId;

		// Token: 0x04000B74 RID: 2932
		[JsonProperty]
		private int _prestigeWager;

		// Token: 0x04000B75 RID: 2933
		[JsonProperty]
		private string _requestingPlayerArchfiendId;
	}
}
