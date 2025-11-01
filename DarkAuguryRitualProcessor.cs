using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000667 RID: 1639
	public class DarkAuguryRitualProcessor : TargetedRitualActionProcessor<DarkAuguryRitualOrder, DarkAuguryRitualData, RitualCastEvent>
	{
		// Token: 0x06001E41 RID: 7745 RVA: 0x00068420 File Offset: 0x00066620
		protected override Result ProcessInternal(ActionProcessContext context)
		{
			RitualCastEvent ritualCastEvent;
			Problem problem = base.CheckPlayerRitualResistance(base.request.TargetPlayerId, out ritualCastEvent) as Problem;
			if (problem != null)
			{
				return problem;
			}
			PlayerState targetPlayer = base._currentTurn.FindPlayerState(base.request.TargetPlayerId, null);
			InformationRevealedEvent informationRevealedEvent = new InformationRevealedEvent(this._player.Id, base.request.TargetPlayerId);
			ritualCastEvent.AddChildEvent<InformationRevealedEvent>(informationRevealedEvent);
			if (base.data.PowerRevelations != PowerRevelations.None)
			{
				this.TurnProcessContext.RevealPowersToPlayer(this._player, targetPlayer, base.data.PowerRevelations);
				informationRevealedEvent.Revealed.Powers = true;
			}
			if (base.data.RevealVault)
			{
				this.TurnProcessContext.RevealVaultToPlayer(this._player, targetPlayer);
				informationRevealedEvent.Revealed.Vault = true;
			}
			if (base.data.RevealTribute)
			{
				this.TurnProcessContext.RevealTributeToPlayer(this._player, targetPlayer);
			}
			if (base.data.RevealEvents)
			{
				this.TurnProcessContext.RevealEventsToPlayer(this._player, targetPlayer);
				informationRevealedEvent.Revealed.Events = true;
			}
			if (base.data.RevealRituals)
			{
				this.TurnProcessContext.RevealRitualsToPlayer(this._player, targetPlayer);
				informationRevealedEvent.Revealed.Rituals = true;
			}
			if (base.data.RevealSchemes)
			{
				this.TurnProcessContext.RevealSchemesToPlayer(this._player, targetPlayer);
				informationRevealedEvent.Revealed.Schemes = true;
			}
			if (base.data.RevealRelics)
			{
				this.TurnProcessContext.RevealRelicsToPlayer(this._player, targetPlayer);
				informationRevealedEvent.Revealed.Relics = true;
			}
			return Result.Success;
		}
	}
}
