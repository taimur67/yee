using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000506 RID: 1286
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BloodFeudState : DiplomaticState
	{
		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x00058619 File Offset: 0x00056819
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.BloodFeud;
			}
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0005861C File Offset: 0x0005681C
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return true;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0005861F File Offset: 0x0005681F
		public override CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRule.DuringMovement;
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00058622 File Offset: 0x00056822
		public override bool AllowCombat(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return true;
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x00058625 File Offset: 0x00056825
		public override bool AllowStrongholdCapture(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return true;
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00058628 File Offset: 0x00056828
		[JsonConstructor]
		public BloodFeudState()
		{
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00058630 File Offset: 0x00056830
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new BloodFeudState();
		}
	}
}
