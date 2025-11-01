using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000365 RID: 869
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceModifier : GameEntityModifier<GamePiece, GamePieceModifierStaticData>
	{
		// Token: 0x0600108F RID: 4239 RVA: 0x0004138F File Offset: 0x0003F58F
		[JsonConstructor]
		protected GamePieceModifier()
		{
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00041397 File Offset: 0x0003F597
		public GamePieceModifier(GamePieceModifierStaticData data) : base(data)
		{
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x000413A0 File Offset: 0x0003F5A0
		public override void ApplyTo(TurnContext context, GamePiece item)
		{
			this.ApplyTo(context, item, false);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x000413AC File Offset: 0x0003F5AC
		public void ApplyTo(TurnContext context, GamePiece item, bool isDecreaseAttributeStratagem)
		{
			base.ApplyTo(context, item);
			if (this.Data == null)
			{
				return;
			}
			foreach (StatModificationBinding<GamePieceStat> statModificationBinding in this.Data.AllBindings)
			{
				ModifiableValue modifiable2;
				if (statModificationBinding.StatKey == GamePieceStat.CurHealth)
				{
					item.TemporaryHP += (int)statModificationBinding.Value;
				}
				else if (statModificationBinding.Value < 0f && statModificationBinding.ModifierTarget == ModifierTarget.ValueOffset && isDecreaseAttributeStratagem)
				{
					ModifiableValue modifiable;
					if (item.TryGetDecrease(statModificationBinding.StatKey, out modifiable))
					{
						base.ApplyStatModifier(modifiable, -(int)statModificationBinding.Value, statModificationBinding.ModifierTarget);
					}
				}
				else if (item.TryGet(statModificationBinding.StatKey, out modifiable2))
				{
					base.ApplyStatModifier(modifiable2, (int)statModificationBinding.Value, statModificationBinding.ModifierTarget);
				}
			}
			if (this.Data.Teleport)
			{
				base.ApplyBooleanModifier(item.CanTeleport, true);
			}
			if (this.Data.NoTeleport)
			{
				base.ApplyBooleanOverrideModifier(item.CanTeleport, false, null);
			}
			if (this.Data.NoBattle)
			{
				base.ApplyBooleanOverrideModifier(item.CanInitiateCombat, false, null);
			}
			if (this.Data.CanMoveThroughEnemyTerritory)
			{
				base.ApplyBooleanOverrideModifier(item.CanMoveThroughEnemyTerritory, true, null);
			}
			if (this.Data.NoCantonCapture)
			{
				base.ApplyBooleanOverrideModifier(item.CanCaptureCantons, false, null);
			}
			if (this.Data.Uncapturable)
			{
				base.ApplyBooleanModifier(item.CanBeCaptured, false);
			}
			if (this.Data.CannotBeAffectedByRituals)
			{
				this.ApplyRitualResistanceModifier(item);
			}
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00041554 File Offset: 0x0003F754
		private void ApplyRitualResistanceModifier(GamePiece item)
		{
			ModifierContext provider = base.Source;
			GamePieceStandingOnTerrainFilter gamePieceStandingOnTerrainFilter = IEnumerableExtensions.FirstOrDefault<GamePieceStandingOnTerrainFilter>(this.Data.Conditions.OfType<GamePieceStandingOnTerrainFilter>());
			if (gamePieceStandingOnTerrainFilter != null)
			{
				provider = new TerrainContext
				{
					TerrainType = IEnumerableExtensions.First<TerrainType>(gamePieceStandingOnTerrainFilter.TerrainTypes)
				};
			}
			base.ApplyBooleanOverrideModifier(item.CanBeAffectedByRituals, false, provider);
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x000415A8 File Offset: 0x0003F7A8
		public override void InstallInto(GamePiece item, TurnState turn, bool baseAdjust = false)
		{
			foreach (StatModificationBinding<GamePieceStat> statModificationBinding in this.Data.AllBindings)
			{
				ModifiableValue modifiable;
				if (item.TryGet(statModificationBinding.StatKey, out modifiable))
				{
					base.InstallStatModifier(modifiable, (int)statModificationBinding.Value, statModificationBinding.ModifierTarget, baseAdjust);
				}
			}
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0004161C File Offset: 0x0003F81C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in typeof(GamePieceModifierStaticData).GetFields())
			{
				if (!(fieldInfo.FieldType != typeof(int)))
				{
					int num = (int)fieldInfo.GetValue(this.Data);
					if (num != 0)
					{
						stringBuilder.Append(string.Format("{0}: {1} ", fieldInfo.Name, num));
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
