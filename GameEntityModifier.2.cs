using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000373 RID: 883
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class GameEntityModifier<TEntity, TData> : Modifier<TEntity, TData> where TEntity : GameEntity, IModifiable where TData : ModifierStaticData
	{
		// Token: 0x060010C6 RID: 4294 RVA: 0x00041B5A File Offset: 0x0003FD5A
		protected GameEntityModifier()
		{
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00041B62 File Offset: 0x0003FD62
		protected GameEntityModifier(TData data) : base(data)
		{
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00041B6B File Offset: 0x0003FD6B
		public override bool CanApplyTo(TurnContext context, TEntity entity)
		{
			return base.CanApplyTo(context, entity) && context.CanApplyModifierTo(this.Data, entity);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00041B90 File Offset: 0x0003FD90
		public override void ApplyTo(TurnContext context, TEntity item)
		{
			if (this.Data == null)
			{
				return;
			}
			item.AddTags<EntityTag>(base.Source, this.Data.Tags);
		}
	}
}
