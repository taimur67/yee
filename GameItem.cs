using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002BD RID: 701
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class GameItem : GameEntity, IIdentifiable, IEquatable<IIdentifiable>, IDeepClone<GameItem>
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000D56 RID: 3414
		public abstract GameItemCategory Category { get; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00034EED File Offset: 0x000330ED
		[JsonIgnore]
		public virtual bool IsActive
		{
			get
			{
				return this.Status == GameItemStatus.InPlay;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00034EF8 File Offset: 0x000330F8
		[JsonIgnore]
		public virtual bool CanBePlacedInVault
		{
			get
			{
				return this.Category.CanBePlacedInVault();
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00034F05 File Offset: 0x00033105
		[JsonIgnore]
		public virtual bool RemoveIfBanished
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00034F08 File Offset: 0x00033108
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00034F10 File Offset: 0x00033110
		[JsonIgnore]
		public Identifier Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00034F19 File Offset: 0x00033119
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x00034F26 File Offset: 0x00033126
		[JsonIgnore]
		public string StaticDataId
		{
			get
			{
				return this.StaticDataReference.Id;
			}
			set
			{
				this.StaticDataReference = new ConfigRef("", value);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00034F39 File Offset: 0x00033139
		public virtual int ItemCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00034F3C File Offset: 0x0003313C
		[JsonIgnore]
		public string ClientDataGuid
		{
			get
			{
				return this.StaticDataId;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00034F44 File Offset: 0x00033144
		[JsonIgnore]
		public Cost UpkeepOutstanding
		{
			get
			{
				return this.UpkeepCost - this._upkeepPaid;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x00034F57 File Offset: 0x00033157
		[JsonIgnore]
		public Cost UpkeepPaid
		{
			get
			{
				return this._upkeepPaid;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00034F5F File Offset: 0x0003315F
		[JsonIgnore]
		public IReadOnlyList<Ability> Abilities
		{
			get
			{
				return this._abilities;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x00034F67 File Offset: 0x00033167
		[JsonIgnore]
		public string NameKey
		{
			get
			{
				return this.StaticDataId;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00034F6F File Offset: 0x0003316F
		[JsonIgnore]
		public virtual bool RecordAsDeadEntityOnBanish
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00034F74 File Offset: 0x00033174
		public override void ConfigureFrom(IdentifiableStaticData data)
		{
			base.ConfigureFrom(data);
			GameItemStaticData gameItemStaticData = data as GameItemStaticData;
			if (gameItemStaticData != null)
			{
				this.CommandCost = gameItemStaticData.CommandCost;
				CostStaticData costStaticData = gameItemStaticData.Cost;
				this.Cost = costStaticData;
				costStaticData = gameItemStaticData.Upkeep;
				this.UpkeepCost = costStaticData;
				this.PublicKnowledge = gameItemStaticData.IsPublicKnowledge;
				this.AttachableTo = gameItemStaticData.SlotType;
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00034FDE File Offset: 0x000331DE
		public static implicit operator Identifier(GameItem item)
		{
			if (item == null)
			{
				return Identifier.Invalid;
			}
			return item.Id;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00034FEB File Offset: 0x000331EB
		public void PayUpkeep(Payment payment)
		{
			this._upkeepPaid.Accumulate(new ResourceAccumulation[]
			{
				payment.Total
			});
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00035008 File Offset: 0x00033208
		public void CheatPayUpkeep()
		{
			this._upkeepPaid = this.UpkeepOutstanding;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00035016 File Offset: 0x00033216
		public void UseUpkeep(bool useAllUpkeep = false)
		{
			if (useAllUpkeep)
			{
				this._upkeepPaid = Cost.None;
				return;
			}
			this._upkeepPaid.Deduct(this.UpkeepCost);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00035038 File Offset: 0x00033238
		public virtual void SetLevel(int level)
		{
			this.Level = level;
			if (this.Level > this.HighestLevel)
			{
				this.HighestLevel = this.Level;
			}
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0003505B File Offset: 0x0003325B
		public void ClearAbilities()
		{
			this._abilities.Clear();
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00035068 File Offset: 0x00033268
		public void AddAbility(Ability ability)
		{
			this._abilities.Add(ability);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00035076 File Offset: 0x00033276
		public void RemoveAbility(Ability ability)
		{
			this._abilities.Remove(ability);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00035085 File Offset: 0x00033285
		public override string ToString()
		{
			return this.NameKey;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0003508D File Offset: 0x0003328D
		public override ModifierContext CreateContext()
		{
			return new GameItemContext
			{
				SourceId = this.StaticDataId
			};
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000350A0 File Offset: 0x000332A0
		public virtual ItemBanishedEvent OnBanished(TurnProcessContext context, PlayerState itemOwner, int instigatorId = -2147483648)
		{
			SimLogger logger = SimLogger.Logger;
			if (logger != null)
			{
				logger.WarnIf(this.Status == GameItemStatus.Banished, string.Format("Trying to banish an already banished game item {0} with id {1}", this, this.Id));
			}
			this.Status = GameItemStatus.Banished;
			return new ItemBanishedEvent(this._id, instigatorId, (itemOwner != null) ? itemOwner.Id : int.MinValue, this.Category);
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00035105 File Offset: 0x00033305
		public override int GetHashCode()
		{
			return (int)this._id;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0003510D File Offset: 0x0003330D
		public bool Equals(IIdentifiable other)
		{
			return other != null && (this == other || this.Id == other.Id);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00035128 File Offset: 0x00033328
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			IIdentifiable identifiable = obj as IIdentifiable;
			return identifiable != null && this.Equals(identifiable);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00035154 File Offset: 0x00033354
		protected void DeepCloneGameItemParts(GameItem clone)
		{
			clone.Id = this._id;
			clone.Level = this.Level;
			clone.HighestLevel = this.HighestLevel;
			clone.Status = this.Status;
			clone.PublicKnowledge = this.PublicKnowledge;
			clone.AttachableTo = this.AttachableTo;
			clone.UpkeepCost = this.UpkeepCost.DeepClone<Cost>();
			clone.Cost = this.Cost.DeepClone<Cost>();
			clone.CommandCost = this.CommandCost;
			clone._upkeepPaid = this._upkeepPaid.DeepClone<Cost>();
			clone.NextUpkeepTurn = this.NextUpkeepTurn;
			clone._abilities = this._abilities.DeepClone<Ability>();
			base.DeepCloneParts(clone);
		}

		// Token: 0x06000D75 RID: 3445
		public abstract void DeepClone(out GameItem clone);

		// Token: 0x040005E2 RID: 1506
		[JsonProperty]
		private Identifier _id;

		// Token: 0x040005E3 RID: 1507
		[JsonProperty]
		[DefaultValue(1)]
		public int Level = 1;

		// Token: 0x040005E4 RID: 1508
		[JsonProperty]
		[DefaultValue(1)]
		public int HighestLevel = 1;

		// Token: 0x040005E5 RID: 1509
		[JsonProperty]
		[DefaultValue(GameItemStatus.InPlay)]
		public GameItemStatus Status = GameItemStatus.InPlay;

		// Token: 0x040005E6 RID: 1510
		[JsonProperty]
		[PublicKnowledge]
		public bool PublicKnowledge;

		// Token: 0x040005E7 RID: 1511
		[JsonProperty]
		public SlotType AttachableTo;

		// Token: 0x040005E8 RID: 1512
		[JsonProperty]
		public Cost UpkeepCost = Cost.None;

		// Token: 0x040005E9 RID: 1513
		[JsonProperty]
		public Cost Cost = Cost.None;

		// Token: 0x040005EA RID: 1514
		[JsonProperty]
		public int CommandCost;

		// Token: 0x040005EB RID: 1515
		[JsonProperty]
		private Cost _upkeepPaid = Cost.None;

		// Token: 0x040005EC RID: 1516
		[JsonProperty]
		public int NextUpkeepTurn;

		// Token: 0x040005ED RID: 1517
		[JsonProperty]
		private List<Ability> _abilities = new List<Ability>();

		// Token: 0x040005EE RID: 1518
		private IDeepClone<GameItem> _deepCloneImplementation;
	}
}
