using System;
using Core.StaticData;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000539 RID: 1337
	public class TurnModuleProcessor<TModuleInstance, TModuleStaticData> : TurnModuleProcessor<TModuleInstance> where TModuleInstance : TurnModuleInstance where TModuleStaticData : IdentifiableStaticData
	{
		// Token: 0x060019F1 RID: 6641 RVA: 0x0005AAEE File Offset: 0x00058CEE
		public override void Initialize()
		{
			base.Initialize();
			if (!base._database.TryFetch<TModuleStaticData>(base.Instance.StaticDataId, out this._staticData))
			{
				this._staticData = base._database.FetchSingle<TModuleStaticData>();
			}
		}

		// Token: 0x04000BD3 RID: 3027
		protected TModuleStaticData _staticData;
	}
}
