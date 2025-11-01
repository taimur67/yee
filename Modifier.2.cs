using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000371 RID: 881
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class Modifier<TModifiableType, TData> : Modifier<TModifiableType> where TModifiableType : IModifiable where TData : ModifierStaticData
	{
		// Token: 0x060010C1 RID: 4289 RVA: 0x00041AFF File Offset: 0x0003FCFF
		protected Modifier()
		{
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00041B07 File Offset: 0x0003FD07
		protected Modifier(TData data)
		{
			this.Data = data;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00041B18 File Offset: 0x0003FD18
		public override bool CanApplyTo(TurnContext context, TModifiableType item)
		{
			GameEntity gameEntity = item as GameEntity;
			if (gameEntity != null)
			{
				return context.CanApplyModifierTo(this.Data, gameEntity);
			}
			return base.CanApplyTo(context, item);
		}

		// Token: 0x040007C2 RID: 1986
		[JsonProperty]
		public TData Data;
	}
}
