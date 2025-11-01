using System;

namespace LoG
{
	// Token: 0x020003F5 RID: 1013
	[Serializable]
	public class SimulationError : DebugProblem
	{
		// Token: 0x06001435 RID: 5173 RVA: 0x0004D5BB File Offset: 0x0004B7BB
		public SimulationError(string error) : base(error)
		{
		}
	}
}
