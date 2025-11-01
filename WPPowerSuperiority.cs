using System;

namespace LoG
{
	// Token: 0x02000189 RID: 393
	public class WPPowerSuperiority : WorldProperty
	{
		// Token: 0x06000755 RID: 1877 RVA: 0x00022D95 File Offset: 0x00020F95
		public WPPowerSuperiority(PowerType powerType, int targetPlayer, Identifier targetItem = Identifier.Invalid)
		{
			this.PowerType = powerType;
			this.TargetPlayer = targetPlayer;
			this.TargetItem = targetItem;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00022DB4 File Offset: 0x00020FB4
		private int GetTheirResistance(TurnState trueTurn)
		{
			if (this.TargetPlayer == -1)
			{
				return 0;
			}
			PlayerState playerState = this.OwningPlanner.PlayerState;
			PlayerState playerState2 = this.OwningPlanner.TrueContext.CurrentTurn.FindPlayerState(this.TargetPlayer, null);
			if (playerState2 == null)
			{
				return 0;
			}
			GameItem gameItem = (this.TargetItem == Identifier.Invalid) ? null : trueTurn.FetchGameItem(this.TargetItem);
			return (gameItem == null) ? trueTurn.CalculateRitualResistance(playerState, playerState2, this.PowerType) : trueTurn.CalculateRitualResistance(playerState, playerState2, gameItem, this.PowerType);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00022E3C File Offset: 0x0002103C
		internal override bool IsFulfilledInternal(TurnContext viewContext, PlayerState playerState, GOAPPlanner planner)
		{
			int num = planner.TrueTurn.CalculateRitualStrength(this.PowerType, playerState, null);
			int theirResistance = this.GetTheirResistance(planner.TrueTurn);
			return num >= theirResistance;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00022E74 File Offset: 0x00021074
		public override WPProvidesEffect ProvidesEffect(WorldProperty precondition)
		{
			WPPowerSuperiority wppowerSuperiority = precondition as WPPowerSuperiority;
			if (wppowerSuperiority == null)
			{
				return WPProvidesEffect.No;
			}
			if (wppowerSuperiority.TargetPlayer != this.TargetPlayer || wppowerSuperiority.PowerType != this.PowerType)
			{
				return WPProvidesEffect.No;
			}
			bool flag = this.TargetItem != Identifier.Invalid;
			if (wppowerSuperiority.TargetItem != Identifier.Invalid && flag && this.TargetItem != wppowerSuperiority.TargetItem)
			{
				return WPProvidesEffect.No;
			}
			return WPProvidesEffect.Yes;
		}

		// Token: 0x0400035A RID: 858
		public PowerType PowerType;

		// Token: 0x0400035B RID: 859
		public int TargetPlayer;

		// Token: 0x0400035C RID: 860
		public Identifier TargetItem;
	}
}
