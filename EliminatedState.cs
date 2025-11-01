using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200050E RID: 1294
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EliminatedState : DiplomaticState
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x00058F31 File Offset: 0x00057131
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.Eliminated;
			}
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x00058F35 File Offset: 0x00057135
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return false;
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x00058F38 File Offset: 0x00057138
		public override CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRule.CannotCapture;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00058F3B File Offset: 0x0005713B
		public override bool AllowCombat(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return false;
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x00058F3E File Offset: 0x0005713E
		public override bool AllowStrongholdCapture(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return false;
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001903 RID: 6403 RVA: 0x00058F41 File Offset: 0x00057141
		[JsonIgnore]
		public override bool AllowAnyDiplomacy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x00058F44 File Offset: 0x00057144
		[JsonIgnore]
		public override bool AllowFriendlyDiplomacy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x00058F47 File Offset: 0x00057147
		[JsonIgnore]
		public override bool AllowHostileDiplomacy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001906 RID: 6406 RVA: 0x00058F4A File Offset: 0x0005714A
		[JsonIgnore]
		public override bool AllowSupport
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x00058F4D File Offset: 0x0005714D
		public override bool AllowNearbyHealingProvidedBy(int providingPlayerId)
		{
			return false;
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001908 RID: 6408 RVA: 0x00058F50 File Offset: 0x00057150
		[JsonIgnore]
		public override bool AllowVassalRelationshipRequest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x00058F53 File Offset: 0x00057153
		[JsonConstructor]
		public EliminatedState()
		{
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x00058F5B File Offset: 0x0005715B
		public override void OnStateEntered(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState, bool interrupt)
		{
			base.HandleDiplomacyCancellation(context, pair, prevState);
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00058F66 File Offset: 0x00057166
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new EliminatedState();
		}
	}
}
