using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200050F RID: 1295
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ExcommunicatedState : DiplomaticState
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600190C RID: 6412 RVA: 0x00058F6F File Offset: 0x0005716F
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.Excommunicated;
			}
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00058F72 File Offset: 0x00057172
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return true;
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00058F75 File Offset: 0x00057175
		public override CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRule.DuringMovement;
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00058F78 File Offset: 0x00057178
		public override bool AllowCombat(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return true;
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00058F7B File Offset: 0x0005717B
		public override bool AllowStrongholdCapture(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return true;
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00058F7E File Offset: 0x0005717E
		[JsonConstructor]
		public ExcommunicatedState()
		{
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00058F86 File Offset: 0x00057186
		public override void OnStateEntered(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState, bool interrupt)
		{
			base.HandleDiplomacyCancellation(context, pair, prevState);
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00058F91 File Offset: 0x00057191
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new ExcommunicatedState();
		}
	}
}
