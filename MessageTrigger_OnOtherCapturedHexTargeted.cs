using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200032D RID: 813
	public class MessageTrigger_OnOtherCapturedHexTargeted : MessageTriggerCondition
	{
		// Token: 0x06000FAA RID: 4010 RVA: 0x0003E0C8 File Offset: 0x0003C2C8
		public override string GetDescription()
		{
			return string.Format("Message <{0}> when <{1}> captures <Hex: {2}>", this.RecipientArchfiendId, this.TargetArchfiendId, this.HexCoord);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0003E0EC File Offset: 0x0003C2EC
		public override bool Evaluate(TurnState newTurn, TurnProcessContext context, GameDatabase database)
		{
			PlayerState playerState = context.CurrentTurn.FindPlayerState(this.TargetArchfiendId);
			foreach (CantonClaimedEvent cantonClaimedEvent in context.CurrentTurn.GetGameEvents().OfType<CantonClaimedEvent>())
			{
				if (!(cantonClaimedEvent.Hex != this.HexCoord) && cantonClaimedEvent.NewOwner == playerState.Id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000750 RID: 1872
		public string TargetArchfiendId;

		// Token: 0x04000751 RID: 1873
		public HexCoord HexCoord;
	}
}
