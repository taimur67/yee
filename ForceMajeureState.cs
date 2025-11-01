using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000510 RID: 1296
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ForceMajeureState : DiplomaticState
	{
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001914 RID: 6420 RVA: 0x00058F9A File Offset: 0x0005719A
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.ForceMajeure;
			}
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x00058F9D File Offset: 0x0005719D
		public override CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRule.DuringMovement;
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00058FA0 File Offset: 0x000571A0
		[JsonConstructor]
		public ForceMajeureState()
		{
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x00058FA8 File Offset: 0x000571A8
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new ForceMajeureState();
		}
	}
}
