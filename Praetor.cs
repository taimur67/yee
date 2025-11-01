using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002CB RID: 715
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Praetor : GameItem
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x000369B1 File Offset: 0x00034BB1
		public override GameItemCategory Category
		{
			get
			{
				return GameItemCategory.Praetor;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x000369B4 File Offset: 0x00034BB4
		public override bool RecordAsDeadEntityOnBanish
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x000369B7 File Offset: 0x00034BB7
		[JsonIgnore]
		public IReadOnlyList<PraetorCombatMoveInstance> CombatMoves
		{
			get
			{
				return this._combatMoves;
			}
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x000369C0 File Offset: 0x00034BC0
		public bool TryGetTechniqueInstance(ConfigRef move, out PraetorCombatMoveInstance instance)
		{
			instance = null;
			int index;
			if (this.TryGetTechniqueInstanceSlot(move, out index))
			{
				instance = this._combatMoves[index];
			}
			return instance != null;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x000369F0 File Offset: 0x00034BF0
		public bool TryGetTechniqueInstanceSlot(ConfigRef move, out int slot)
		{
			for (slot = 0; slot < this._combatMoves.Count; slot++)
			{
				PraetorCombatMoveInstance praetorCombatMoveInstance = this._combatMoves[slot];
				if (praetorCombatMoveInstance != null && praetorCombatMoveInstance.CombatMoveReference.Equals(move))
				{
					return true;
				}
			}
			slot = -1;
			return false;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00036A3C File Offset: 0x00034C3C
		public bool IsSlotInRange(int slot)
		{
			return slot >= 0 && slot < this._combatMoves.Count;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00036A52 File Offset: 0x00034C52
		public bool TryGetTechniqueAtSlot(int slot, out PraetorCombatMoveInstance instance)
		{
			instance = (this.IsSlotInRange(slot) ? this._combatMoves[slot] : null);
			return instance != null;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00036A73 File Offset: 0x00034C73
		public bool ReplaceTechnique(PraetorCombatMoveInstance instance, int slot)
		{
			if (slot < 0 || slot >= this._combatMoves.Count)
			{
				return false;
			}
			this._combatMoves[slot] = instance;
			return true;
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00036A98 File Offset: 0x00034C98
		public override void ConfigureFrom(IdentifiableStaticData data)
		{
			base.ConfigureFrom(data);
			PraetorStaticData praetorStaticData = data as PraetorStaticData;
			if (praetorStaticData == null)
			{
				return;
			}
			CostStaticData cost = praetorStaticData.Cost;
			this.Cost = cost;
			this.SetLevel(praetorStaticData.Level);
			foreach (PraetorCombatMoveInstance praetorCombatMoveInstance in praetorStaticData.StartingTechniques)
			{
				if (praetorCombatMoveInstance != null)
				{
					this._combatMoves.Add(praetorCombatMoveInstance.DeepClone<PraetorCombatMoveInstance>());
				}
			}
			this.NumTechniqueSlots.SetBase((float)praetorStaticData.StartingTechniques.Count);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00036B3C File Offset: 0x00034D3C
		public bool IsPoPAdministrator(GameDatabase database, out ResourceAccumulation tributeGenerated)
		{
			tributeGenerated = new Cost();
			IEnumerable<ItemAbilityStaticData> enumerable = from id in database.Fetch<PraetorStaticData>(base.StaticDataId).ProvidedAbilities
			select database.Fetch<ItemAbilityStaticData>(id);
			List<TurnEffect_GenerateTribute> list = new List<TurnEffect_GenerateTribute>();
			foreach (ItemAbilityStaticData itemAbilityStaticData in enumerable)
			{
				foreach (AbilityEffect abilityEffect in itemAbilityStaticData.Effects)
				{
					TurnEffect_GenerateTribute turnEffect_GenerateTribute = abilityEffect as TurnEffect_GenerateTribute;
					if (turnEffect_GenerateTribute != null)
					{
						list.Add(turnEffect_GenerateTribute);
					}
				}
			}
			if (list != null && list.Count > 0)
			{
				ResourceAccumulation resourceAccumulation = new ResourceAccumulation();
				foreach (TurnEffect_GenerateTribute turnEffect_GenerateTribute2 in list)
				{
					resourceAccumulation.Add(turnEffect_GenerateTribute2.GetTotalResourcesGenerated());
				}
			}
			return list != null && IEnumerableExtensions.Any<TurnEffect_GenerateTribute>(list);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00036C70 File Offset: 0x00034E70
		public bool HasTechniqueOfType(GameDatabase database, PraetorCombatMoveStyle type)
		{
			foreach (PraetorCombatMoveInstance praetorCombatMoveInstance in this.CombatMoves)
			{
				ConfigRef<PraetorCombatMoveStaticData> combatMoveReference = praetorCombatMoveInstance.CombatMoveReference;
				PraetorCombatMoveStaticData praetorCombatMoveStaticData = database.Fetch(combatMoveReference);
				if (database.Fetch(praetorCombatMoveStaticData.TechniqueType).Id == type.Id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00036CE8 File Offset: 0x00034EE8
		public sealed override void DeepClone(out GameItem gameItem)
		{
			Praetor praetor = new Praetor();
			base.DeepCloneGameItemParts(praetor);
			praetor._combatMoves = this._combatMoves.DeepClone<PraetorCombatMoveInstance>();
			praetor.Experience = this.Experience;
			praetor.Victories = this.Victories;
			praetor.NumTechniqueSlots = this.NumTechniqueSlots.DeepClone<ModifiableValue>();
			gameItem = praetor;
		}

		// Token: 0x0400062D RID: 1581
		[JsonProperty]
		private List<PraetorCombatMoveInstance> _combatMoves = new List<PraetorCombatMoveInstance>();

		// Token: 0x0400062E RID: 1582
		[JsonProperty]
		public int Experience;

		// Token: 0x0400062F RID: 1583
		[JsonProperty]
		public int Victories;

		// Token: 0x04000630 RID: 1584
		[JsonProperty]
		[DefaultValue(3)]
		public ModifiableValue NumTechniqueSlots = new ModifiableValue(3f, 0, int.MaxValue, RoundingMode.RoundDown);
	}
}
