using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000392 RID: 914
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PraetorModifier : GameEntityModifier<Praetor, PraetorModifierStaticData>
	{
		// Token: 0x0600118B RID: 4491 RVA: 0x0004396B File Offset: 0x00041B6B
		[JsonConstructor]
		protected PraetorModifier()
		{
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00043973 File Offset: 0x00041B73
		public PraetorModifier(PraetorModifierStaticData data) : base(data)
		{
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0004397C File Offset: 0x00041B7C
		public override void InstallInto(Praetor praetor, TurnState turn, bool baseAdjust = false)
		{
			praetor.AddTags<EntityTag>(base.Source, this.Data.Tags);
		}
	}
}
