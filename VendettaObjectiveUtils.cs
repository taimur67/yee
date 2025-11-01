using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x02000530 RID: 1328
	public static class VendettaObjectiveUtils
	{
		// Token: 0x060019CE RID: 6606 RVA: 0x0005A678 File Offset: 0x00058878
		public static IEnumerable<VendettaObjective> GetObjectives(this VendettaObjectiveGroup group, GameDatabase db, TurnState turn, PlayerState player, PlayerState target)
		{
			return (from x in @group.ObjectiveGenerators
			select db.Fetch(x).GenerateVendetta(turn, player, target)).ExcludeNull<VendettaObjective>();
		}
	}
}
