using System;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005A7 RID: 1447
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ObjectiveCondition_StealCantonOfType : ObjectiveCondition_SuccessfullyCastRitual<RitualCastEvent>, IDynamicObjective
	{
		// Token: 0x06001B33 RID: 6963 RVA: 0x0005EA49 File Offset: 0x0005CC49
		public ObjectiveCondition_StealCantonOfType()
		{
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0005EA51 File Offset: 0x0005CC51
		public ObjectiveCondition_StealCantonOfType(TerrainType requiredTerrainType, bool ritualCasterMustBeBeneficiary = false)
		{
			this.RequiredTerrainType = requiredTerrainType;
			this.RitualCasterMustBeBeneficiary = ritualCasterMustBeBeneficiary;
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0005EA67 File Offset: 0x0005CC67
		public ObjectiveDifficulty CalculateDifficulty(TurnContext context, PlayerState player)
		{
			if (this.RequiredTerrainType != TerrainType.Plain)
			{
				return ObjectiveDifficulty.Hard;
			}
			return ObjectiveDifficulty.Moderate;
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0005EA74 File Offset: 0x0005CC74
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			return @event.LocalChildEvents.OfType<CantonClaimedEvent>().Any((CantonClaimedEvent cantonClaimed) => (!this.RitualCasterMustBeBeneficiary || cantonClaimed.NewOwner == owner.Id) && this.IsAcceptableHex(context.CurrentTurn, cantonClaimed.Hex)) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0005EAD1 File Offset: 0x0005CCD1
		private bool IsAcceptableTerrainType(TerrainType terrainType)
		{
			return (terrainType == TerrainType.Plain && this.RequiredTerrainType == TerrainType.LandBridge) || (terrainType == TerrainType.LandBridge && this.RequiredTerrainType == TerrainType.Plain) || terrainType == this.RequiredTerrainType;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x0005EAF8 File Offset: 0x0005CCF8
		private bool IsAcceptableHex(TurnState turn, HexCoord hexCoord)
		{
			Hex hex = turn.HexBoard[hexCoord];
			return hex != null && this.IsAcceptableTerrainType(hex.Type);
		}

		// Token: 0x04000C4F RID: 3151
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public TerrainType RequiredTerrainType;

		// Token: 0x04000C50 RID: 3152
		[JsonProperty]
		public bool RitualCasterMustBeBeneficiary;
	}
}
