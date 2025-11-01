using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200071B RID: 1819
	public class ExcommunicatePlayerEffectStaticData : EdictEffectStaticData
	{
		// Token: 0x060022A4 RID: 8868 RVA: 0x000787F5 File Offset: 0x000769F5
		public override bool ValidateCandidate(PlayerState candidate)
		{
			return base.ValidateCandidate(candidate) && !candidate.Excommunicated;
		}
	}
}
