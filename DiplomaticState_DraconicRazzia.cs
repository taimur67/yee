using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200050D RID: 1293
	public class DiplomaticState_DraconicRazzia : DiplomaticState
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060018F4 RID: 6388 RVA: 0x00058E63 File Offset: 0x00057063
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.DraconicRazia;
			}
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00058E67 File Offset: 0x00057067
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return true;
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x00058E6A File Offset: 0x0005706A
		public override bool AllowCombat(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return true;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x00058E6D File Offset: 0x0005706D
		public override CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRule.OnStop;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00058E70 File Offset: 0x00057070
		public override CantonCaptureRestrictions GetCantonCaptureRestrictions(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRestrictions.SharedPerimeter;
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00058E73 File Offset: 0x00057073
		[JsonConstructor]
		public DiplomaticState_DraconicRazzia()
		{
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x00058E8D File Offset: 0x0005708D
		public DiplomaticState_DraconicRazzia(int actorId, int duration)
		{
			this.ActorId = actorId;
			this.TurnsRemaining = duration;
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00058EB5 File Offset: 0x000570B5
		public override void OnStateEntered(TurnProcessContext context, DiplomaticPairStatus pair, DiplomaticState prevState, bool interrupt)
		{
			base.OnStateEntered(context, pair, prevState, interrupt);
			pair.ActorId = this.ActorId;
			context.CurrentTurn.AddGameEvent<DraconicRazziaCommencementEvent>(new DraconicRazziaCommencementEvent(pair.ActorId, pair.TargetID)
			{
				OrderType = OrderTypes.DraconicRazzia
			});
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x00058EF3 File Offset: 0x000570F3
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new DiplomaticState_DraconicRazzia(this.ActorId, this.TurnsRemaining);
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x00058F08 File Offset: 0x00057108
		public override void Update(TurnProcessContext context, DiplomaticPairStatus relationship)
		{
			int num = this.TurnsRemaining - 1;
			this.TurnsRemaining = num;
			if (num <= 0)
			{
				relationship.SetNeutral(context, false);
			}
		}

		// Token: 0x04000B94 RID: 2964
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int ActorId = int.MinValue;

		// Token: 0x04000B95 RID: 2965
		[JsonProperty]
		[DefaultValue(1)]
		public int TurnsRemaining = 1;
	}
}
