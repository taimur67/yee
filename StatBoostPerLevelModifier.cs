using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000395 RID: 917
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class StatBoostPerLevelModifier : Modifier<GamePiece, StatBoostPerLevelModifierStaticData>
	{
		// Token: 0x06001195 RID: 4501 RVA: 0x00043AAA File Offset: 0x00041CAA
		[JsonConstructor]
		protected StatBoostPerLevelModifier()
		{
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00043AB2 File Offset: 0x00041CB2
		public StatBoostPerLevelModifier(StatBoostPerLevelModifierStaticData data = null) : base(data)
		{
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00043ABC File Offset: 0x00041CBC
		public override void ApplyTo(TurnContext context, GamePiece gamePiece)
		{
			if (!this.Data.AllowedGamePieceCategories.Contains(gamePiece.SubCategory))
			{
				return;
			}
			base.ApplyStatModifier(gamePiece.Get(this.Data.TargetStat), gamePiece.Level, this.Data.TargetType);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00043B0C File Offset: 0x00041D0C
		public override void InstallInto(GamePiece gamePiece, TurnState turn, bool baseAdjust = false)
		{
			if (!this.Data.AllowedGamePieceCategories.Contains(gamePiece.SubCategory))
			{
				return;
			}
			base.InstallStatModifier(gamePiece.Get(this.Data.TargetStat), gamePiece.Level, this.Data.TargetType, false);
		}
	}
}
