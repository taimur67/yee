using System;
using Core.StaticData;

namespace LoG
{
	// Token: 0x0200052A RID: 1322
	[Serializable]
	public abstract class VendettaObjectiveGenerator : IdentifiableStaticData
	{
		// Token: 0x060019C1 RID: 6593
		public abstract VendettaObjective GenerateVendetta(TurnState turn, PlayerState player, PlayerState target);
	}
}
