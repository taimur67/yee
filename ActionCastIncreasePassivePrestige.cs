using System;
using System.Linq;

namespace LoG
{
	// Token: 0x020000F6 RID: 246
	public class ActionCastIncreasePassivePrestige : ActionCastRitual<IncreasePassivePrestigeRitualOrder>
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x000118FF File Offset: 0x0000FAFF
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Increase_Prestige_Production;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00011903 File Offset: 0x0000FB03
		protected override string GetRitualId()
		{
			return "procured_honour";
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001190A File Offset: 0x0000FB0A
		protected override PowerType GetPowerType()
		{
			return PowerType.Charisma;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0001190D File Offset: 0x0000FB0D
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00011910 File Offset: 0x0000FB10
		protected override int CooldownDuration
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00011914 File Offset: 0x0000FB14
		public override void Prepare()
		{
			int num = this.OwningPlanner.TrueTurn.GetActiveGamePiecesForPlayer(this.OwningPlanner.PlayerId).Count((GamePiece gp) => gp.PassivePrestige > 0 && gp.IsFixture());
			base.AddEffect(new WPPrestigeProduction(2 * num));
			if (num <= 1)
			{
				base.AddScalarCostIncrease(0.5f, PFCostModifier.Heuristic_Bonus);
			}
			base.Prepare();
		}

		// Token: 0x0400022F RID: 559
		public const string RitualId = "procured_honour";
	}
}
