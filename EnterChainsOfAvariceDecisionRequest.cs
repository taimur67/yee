using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004CE RID: 1230
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EnterChainsOfAvariceDecisionRequest : DiplomaticDecisionRequest<EnterChainsOfAvariceDecisionResponse>
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x0005451F File Offset: 0x0005271F
		[BindableValue(null, BindingOption.None)]
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.ChainsOfAvarice;
			}
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x00054523 File Offset: 0x00052723
		[JsonConstructor]
		protected EnterChainsOfAvariceDecisionRequest()
		{
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0005452B File Offset: 0x0005272B
		public EnterChainsOfAvariceDecisionRequest(DecisionId decisionId) : base(decisionId)
		{
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x00054534 File Offset: 0x00052734
		public override TurnLogEntryType GetTurnLogEntryType()
		{
			return TurnLogEntryType.ChainsOfAvariceSentRecipient;
		}

		// Token: 0x04000B48 RID: 2888
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public int Duration;
	}
}
