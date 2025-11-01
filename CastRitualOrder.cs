using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005C5 RID: 1477
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class CastRitualOrder : TargetedActionableOrder
	{
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x0005FB63 File Offset: 0x0005DD63
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.Ritual;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001B9B RID: 7067 RVA: 0x0005FB67 File Offset: 0x0005DD67
		[JsonIgnore]
		public override bool UseBloodLordDiplomacy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x0005FB6A File Offset: 0x0005DD6A
		public override Result TargetSelf(ConfigRef ritual, int selfId, bool selfTargetingRequired = false)
		{
			return new Result.CastRitualOnSelfProblem(ritual, selfId, selfTargetingRequired);
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x0005FB74 File Offset: 0x0005DD74
		public override Result TargetItemStolen(ConfigRef ritual, int originalOwner, GameItem item)
		{
			return new Result.CastRitualOnStolenItemProblem(ritual, originalOwner, item);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x0005FB7E File Offset: 0x0005DD7E
		public override Result TargetItemBanished(ConfigRef ritual, GameItem item)
		{
			return new Result.CastRitualOnBanishedItemProblem(ritual, item);
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0005FB87 File Offset: 0x0005DD87
		public override Result TargetItemInvalid(ConfigRef ritual, GameItem item)
		{
			return new Result.CastRitualOnGameItemProblem(ritual, item);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0005FB90 File Offset: 0x0005DD90
		public override Result TargetItemHasWrongType(ConfigRef ritual, GameItem item)
		{
			return new Result.CastRitualOnWrongTypeOfItemProblem(ritual, item);
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0005FB99 File Offset: 0x0005DD99
		public override Result TargetHexWrongTerrain(ConfigRef ritual, HexCoord hexCoord, TerrainStaticData terrainType)
		{
			return new Result.CastRitualOnWrongTerrainProblem(ritual, hexCoord, terrainType);
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0005FBA3 File Offset: 0x0005DDA3
		public override Result TargetHexOccupied(ConfigRef ritual, HexCoord hexCoord, GamePiece occupant)
		{
			return new Result.CastRitualOnOccupiedHexProblem(ritual, hexCoord, occupant);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0005FBB2 File Offset: 0x0005DDB2
		public override Result TargetHexNotNextToFixture(ConfigRef ritual, HexCoord hexCoord)
		{
			return new Result.CastRitualOnHexNotNextToFixtureProblem(ritual, hexCoord);
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0005FBBB File Offset: 0x0005DDBB
		public override Result TargetHexInvalid(ConfigRef ritual, HexCoord hexCoord)
		{
			return new Result.CastRitualOnInvalidHexProblem(ritual, hexCoord);
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0005FBC4 File Offset: 0x0005DDC4
		public override Result TargetArchfiendInvalid(ConfigRef ritual, int playerID)
		{
			return new Result.CastRitualOnPlayerProblem(ritual, playerID);
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0005FBCD File Offset: 0x0005DDCD
		public override Result TargetArchfiendEliminated(ConfigRef ritual, int playerID)
		{
			return new Result.CastRitualOnEliminatedPlayerProblem(ritual, playerID);
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0005FBD6 File Offset: 0x0005DDD6
		public override Result TargetArchfiendHasNoItemToTarget(ConfigRef ritual, int playerID, GameItemCategory desiredCategory)
		{
			return new Result.PlayerHasNoItemRitualCanTargetProblem(ritual, playerID, desiredCategory);
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0005FBE0 File Offset: 0x0005DDE0
		public void SetRitualMaskingSettings(RitualMaskingSettings settings)
		{
			this.RitualMaskingSettings = settings.DeepClone<RitualMaskingSettings>();
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0005FBEE File Offset: 0x0005DDEE
		public override Problem PaymentFailedProblem(ConfigRef ritual, Problem originalProblem)
		{
			if (!(originalProblem is PaymentProcessor.CannotAfford))
			{
				return originalProblem;
			}
			return new Result.CannotAffordRitualProblem(ritual);
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0005FC00 File Offset: 0x0005DE00
		public override Result TargetArchfiendInvalidDiplomacy(ConfigRef ritual, int targetPlayerID, DiplomaticStateValue stateType)
		{
			if ((this.RitualMaskingSettings.MaskingMode != RitualMaskingMode.NoMasking || stateType == DiplomaticStateValue.Excommunicated) && targetPlayerID != -1 && targetPlayerID != -2147483648)
			{
				return Result.Success;
			}
			return new Result.InvalidDiplomacyForRitualProblem(ritual, targetPlayerID, stateType);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0005FC34 File Offset: 0x0005DE34
		public override Result TargetItemHasInvalidOwner(ConfigRef ritual, Problem invalidOwnerProblem, GameItem gameItem)
		{
			Result.CastRitualOnPlayerProblem castRitualOnPlayerProblem = invalidOwnerProblem as Result.CastRitualOnPlayerProblem;
			Problem result;
			if (castRitualOnPlayerProblem != null)
			{
				result = new Result.CastRitualOnItemOwnershipProblem(ritual, castRitualOnPlayerProblem.TargetPlayerID, gameItem);
			}
			else
			{
				result = invalidOwnerProblem;
			}
			return result;
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0005FC60 File Offset: 0x0005DE60
		public override Result TargetHexHasInvalidOwner(ConfigRef ritual, Problem invalidOwnerProblem, HexCoord hexCoord)
		{
			Result.CastRitualOnPlayerProblem castRitualOnPlayerProblem = invalidOwnerProblem as Result.CastRitualOnPlayerProblem;
			Problem result;
			if (castRitualOnPlayerProblem != null)
			{
				result = new Result.CastRitualOnHexOwnershipProblem(ritual, hexCoord, castRitualOnPlayerProblem.TargetPlayerID);
			}
			else
			{
				result = invalidOwnerProblem;
			}
			return result;
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001BAD RID: 7085 RVA: 0x0005FC8A File Offset: 0x0005DE8A
		// (set) Token: 0x06001BAE RID: 7086 RVA: 0x0005FC92 File Offset: 0x0005DE92
		public string RitualId
		{
			get
			{
				return this.AbilityId;
			}
			set
			{
				this.AbilityId = value;
			}
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x0005FC9B File Offset: 0x0005DE9B
		[JsonConstructor]
		protected CastRitualOrder()
		{
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0005FCAE File Offset: 0x0005DEAE
		public CastRitualOrder(string ritualId) : this(ritualId, RitualMaskingSettings.NoMasking)
		{
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x0005FCBC File Offset: 0x0005DEBC
		public CastRitualOrder(string ritualId, RitualMaskingSettings settings)
		{
			this.AbilityId = ritualId;
			this.RitualMaskingSettings = settings;
		}

		// Token: 0x04000C65 RID: 3173
		[JsonProperty]
		public RitualMaskingSettings RitualMaskingSettings = RitualMaskingSettings.NoMasking;
	}
}
