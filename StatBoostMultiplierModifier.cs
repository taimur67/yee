using System;
using System.Linq;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000394 RID: 916
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class StatBoostMultiplierModifier : Modifier<GamePiece, StatBoostMultiplierModiferStaticData>
	{
		// Token: 0x06001190 RID: 4496 RVA: 0x000439A6 File Offset: 0x00041BA6
		[JsonConstructor]
		protected StatBoostMultiplierModifier()
		{
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x000439AE File Offset: 0x00041BAE
		public StatBoostMultiplierModifier(StatBoostMultiplierModiferStaticData data) : base(data)
		{
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x000439B7 File Offset: 0x00041BB7
		public override void ApplyTo(TurnContext context, GamePiece item)
		{
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x000439BC File Offset: 0x00041BBC
		public override void PostApplyTo(TurnContext context, GamePiece item)
		{
			ModifiableValue stat = item.GetStat(this.Data.TargetStat);
			float num = 0f;
			foreach (StatModifier statModifier in stat.ActiveModifiers.OfType<StatModifier>())
			{
				GameItemContext gameItemContext = statModifier.Provider as GameItemContext;
				if (gameItemContext != null)
				{
					GameItem gameItem = context.CurrentTurn.FetchGameItem(gameItemContext.SourceId);
					if (gameItem != null && IEnumerableExtensions.Contains<GameItemCategory>(this.Data.AffectsCategories, gameItem.Category))
					{
						num += (float)statModifier.Value * (this.Data.Multiplier - 1f);
					}
				}
			}
			if (num == 0f)
			{
				return;
			}
			StatModifier modifier = new StatModifier((int)Math.Ceiling((double)num), base.Source, ModifierTarget.ValueOffset);
			stat.AddModifier(modifier);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00043AA8 File Offset: 0x00041CA8
		public override void InstallInto(GamePiece item, TurnState turn, bool baseAdjust = false)
		{
		}
	}
}
