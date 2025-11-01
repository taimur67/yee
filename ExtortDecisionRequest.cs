using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004D1 RID: 1233
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ExtortDecisionRequest : DiplomaticDecisionRequest<ExtortDecisionResponse>, IDemandRequestAccessor
	{
		// Token: 0x0600171B RID: 5915 RVA: 0x00054685 File Offset: 0x00052885
		[JsonConstructor]
		public ExtortDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x0005468E File Offset: 0x0005288E
		// (set) Token: 0x0600171D RID: 5917 RVA: 0x00054696 File Offset: 0x00052896
		[JsonProperty]
		public DemandOptions DemandOption { get; set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600171E RID: 5918 RVA: 0x0005469F File Offset: 0x0005289F
		// (set) Token: 0x0600171F RID: 5919 RVA: 0x000546A7 File Offset: 0x000528A7
		[JsonProperty]
		public int NumCards { get; set; }

		// Token: 0x06001720 RID: 5920 RVA: 0x000546B0 File Offset: 0x000528B0
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.ExtortionSentRecipient;
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001721 RID: 5921 RVA: 0x000546B4 File Offset: 0x000528B4
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Extort;
			}
		}
	}
}
