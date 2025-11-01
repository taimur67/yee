using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200031E RID: 798
	public class MessageTrigger_OnTargetCapturedHexTargeted : MessageTriggerCondition
	{
		// Token: 0x06000F7C RID: 3964 RVA: 0x0003D56C File Offset: 0x0003B76C
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when they capture <Hex {1}>", this.RecipientArchfiendId, this.HexCoord);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0003D58C File Offset: 0x0003B78C
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.RecipientArchfiendId);
			foreach (CantonClaimedEvent cantonClaimedEvent in context.CurrentTurn.GetGameEvents().OfType<CantonClaimedEvent>())
			{
				if (!(cantonClaimedEvent.Hex != this.HexCoord) && cantonClaimedEvent.NewOwner == playerState.Id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400073A RID: 1850
		public HexCoord HexCoord;
	}
}
