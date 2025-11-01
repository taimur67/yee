using System;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020005FE RID: 1534
	public abstract class GrandEventActionProcessor<TOrder, TStaticData> : GrandEventActionProcessor<TOrder, TStaticData, GrandEventPlayed> where TOrder : PlayGrandEventOrder, new() where TStaticData : EventEffectStaticData
	{
	}
}
