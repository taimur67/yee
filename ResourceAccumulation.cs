using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using Game.Simulation.Utils;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000481 RID: 1153
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ResourceAccumulation : IDontSerializeIfDefault, IComparable<ResourceAccumulation>, IDeepClone<ResourceAccumulation>
	{
		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x00050E65 File Offset: 0x0004F065
		[JsonIgnore]
		public static ResourceAccumulation Empty
		{
			get
			{
				return new ResourceAccumulation();
			}
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00050E6C File Offset: 0x0004F06C
		static ResourceAccumulation()
		{
			if (ResourceAccumulation._resourceKeys == null)
			{
				ResourceAccumulation._resourceKeys = IEnumerableExtensions.ToArray<ResourceTypes>(EnumUtility.GetValues<ResourceTypes>());
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x00050E84 File Offset: 0x0004F084
		[JsonIgnore]
		public static IEnumerable<ResourceTypes> ResourceKeys
		{
			get
			{
				return ResourceAccumulation._resourceKeys;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x00050E8B File Offset: 0x0004F08B
		[JsonIgnore]
		public virtual bool IsZero
		{
			get
			{
				return this.Values.All((int t) => t == 0);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x00050EB7 File Offset: 0x0004F0B7
		[JsonIgnore]
		public bool IsSingleType
		{
			get
			{
				return ResourceAccumulation.ResourceKeys.Count((ResourceTypes t) => this[t] > 0) == 1;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06001579 RID: 5497 RVA: 0x00050ED2 File Offset: 0x0004F0D2
		[JsonIgnore]
		public IEnumerable<int> Values
		{
			get
			{
				yield return this._souls;
				yield return this._ichor;
				yield return this._hellfire;
				yield return this._darkness;
				yield return this._prestige;
				yield break;
			}
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00050EE2 File Offset: 0x0004F0E2
		public IEnumerable<ValueTuple<ResourceTypes, int>> EnumerateResourceValues()
		{
			foreach (ResourceTypes resourceTypes in ResourceAccumulation.ResourceKeys)
			{
				yield return new ValueTuple<ResourceTypes, int>(resourceTypes, this.GetValue(resourceTypes));
			}
			IEnumerator<ResourceTypes> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x00050EF2 File Offset: 0x0004F0F2
		[JsonIgnore]
		public int ValueSum
		{
			get
			{
				return (int)IEnumerableExtensions.Accumulate<int>(this.Values, (int t) => (float)t);
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00050F20 File Offset: 0x0004F120
		private int GetValue(ResourceTypes index)
		{
			int result;
			switch (index)
			{
			case ResourceTypes.Souls:
				result = this._souls;
				break;
			case ResourceTypes.Ichor:
				result = this._ichor;
				break;
			case ResourceTypes.Hellfire:
				result = this._hellfire;
				break;
			case ResourceTypes.Darkness:
				result = this._darkness;
				break;
			case ResourceTypes.Prestige:
				result = this._prestige;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00050F7C File Offset: 0x0004F17C
		private bool SetValue(ResourceTypes index, int value)
		{
			switch (index)
			{
			case ResourceTypes.Souls:
				this._souls = value;
				break;
			case ResourceTypes.Ichor:
				this._ichor = value;
				break;
			case ResourceTypes.Hellfire:
				this._hellfire = value;
				break;
			case ResourceTypes.Darkness:
				this._darkness = value;
				break;
			case ResourceTypes.Prestige:
				this._prestige = value;
				break;
			default:
				return false;
			}
			return true;
		}

		// Token: 0x17000314 RID: 788
		public int this[ResourceTypes type]
		{
			get
			{
				return this.GetValue(type);
			}
			set
			{
				this.SetValue(type, value);
			}
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00050FE9 File Offset: 0x0004F1E9
		public ResourceAccumulation(int[] vals)
		{
			if (vals != null)
			{
				this.SetFrom(vals);
			}
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00050FFB File Offset: 0x0004F1FB
		[JsonConstructor]
		public ResourceAccumulation() : this(0)
		{
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00051004 File Offset: 0x0004F204
		public ResourceAccumulation(int initialValue)
		{
			this.SetAll(initialValue);
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00051013 File Offset: 0x0004F213
		public static ResourceAccumulation CreateWithoutPrestige(int resourceCount)
		{
			return new ResourceAccumulation(resourceCount)
			{
				_prestige = 0
			};
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00051024 File Offset: 0x0004F224
		private void SetFrom(int[] values)
		{
			int num = 0;
			while (num < values.Length && this.SetValue((ResourceTypes)num, values[num]))
			{
				num++;
			}
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0005104C File Offset: 0x0004F24C
		private void SetAll(int value)
		{
			foreach (ResourceTypes index in ResourceAccumulation.ResourceKeys)
			{
				this.SetValue(index, value);
			}
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0005109C File Offset: 0x0004F29C
		public ResourceAccumulation(params ResourceAccumulation[] others)
		{
			this.Accumulate(others);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x000510AC File Offset: 0x0004F2AC
		public ResourceAccumulation Accumulate(params ResourceAccumulation[] others)
		{
			foreach (ResourceAccumulation other in others)
			{
				this.Add(other);
			}
			return this;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x000510D8 File Offset: 0x0004F2D8
		public ResourceAccumulation Add(ResourceAccumulation other)
		{
			if (other != null)
			{
				foreach (ResourceTypes resourceTypes in EnumUtility.GetValues<ResourceTypes>())
				{
					ResourceTypes type = resourceTypes;
					this[type] += other[resourceTypes];
				}
			}
			return this;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0005111C File Offset: 0x0004F31C
		public ResourceAccumulation Add(ResourceTypes resource, int value)
		{
			this[resource] += value;
			return this;
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0005113C File Offset: 0x0004F33C
		public static ResourceAccumulation operator +(ResourceAccumulation a, ResourceAccumulation b)
		{
			ResourceAccumulation resourceAccumulation = new ResourceAccumulation();
			foreach (ResourceTypes type in EnumUtility.GetValues<ResourceTypes>())
			{
				resourceAccumulation[type] = a[type] + b[type];
			}
			return resourceAccumulation;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0005117E File Offset: 0x0004F37E
		public static ResourceAccumulation Deduct(ResourceAccumulation lhs, ResourceAccumulation rhs)
		{
			ResourceAccumulation resourceAccumulation = new ResourceAccumulation(new ResourceAccumulation[]
			{
				lhs
			});
			resourceAccumulation.Deduct(rhs);
			return resourceAccumulation;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x00051198 File Offset: 0x0004F398
		public void Deduct(ResourceAccumulation other)
		{
			foreach (ResourceTypes type in ResourceAccumulation.ResourceKeys)
			{
				this[type] = Math.Max(this[type] - other[type], 0);
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x000511FC File Offset: 0x0004F3FC
		public IEnumerable<KeyValuePair<ResourceTypes, int>> EnumerateValues()
		{
			foreach (ResourceTypes resourceTypes in ResourceAccumulation.ResourceKeys)
			{
				yield return new KeyValuePair<ResourceTypes, int>(resourceTypes, this[resourceTypes]);
			}
			IEnumerator<ResourceTypes> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0005120C File Offset: 0x0004F40C
		public BigInteger CalculateValueHash()
		{
			return new BigInteger(IEnumerableExtensions.ToArray<byte>(this.Values.SelectMany((int t) => BitConverter.GetBytes(t))));
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x00051244 File Offset: 0x0004F444
		public void Limit(int value)
		{
			this._souls = Math.Min(value, this._souls);
			this._ichor = Math.Min(value, this._ichor);
			this._hellfire = Math.Min(value, this._hellfire);
			this._darkness = Math.Min(value, this._darkness);
			this._prestige = Math.Min(value, this._prestige);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x000512AC File Offset: 0x0004F4AC
		public bool Equal(ResourceAccumulation other)
		{
			return ResourceAccumulation.ResourceKeys.All((ResourceTypes t) => this[t] == other[t]);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000512E4 File Offset: 0x0004F4E4
		public bool GreaterThanOrEqualTo(ResourceAccumulation other)
		{
			return ResourceAccumulation.ResourceKeys.All((ResourceTypes t) => this[t] >= other[t]);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0005131C File Offset: 0x0004F51C
		public bool GreaterThanOrEqualTo(int val, bool includePrestige = true)
		{
			if (includePrestige)
			{
				return ResourceAccumulation.ResourceKeys.All((ResourceTypes t) => this[t] >= val);
			}
			return (from x in ResourceAccumulation.ResourceKeys
			where x != ResourceTypes.Prestige
			select x).All((ResourceTypes x) => this[x] >= val);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00051394 File Offset: 0x0004F594
		public bool LessThanOrEqualTo(ResourceAccumulation other, bool includePrestige = true)
		{
			return ResourceAccumulation.ResourceKeys.All((ResourceTypes t) => (includePrestige || t != ResourceTypes.Prestige) && this[t] <= other[t]);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000513D4 File Offset: 0x0004F5D4
		public bool AnyGreaterThan(ResourceAccumulation other, bool includePrestige = true)
		{
			return ResourceAccumulation.ResourceKeys.Any((ResourceTypes t) => (includePrestige || t != ResourceTypes.Prestige) && this[t] > other[t]);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00051414 File Offset: 0x0004F614
		public bool AnyGreaterThan(int val, bool includePrestige = true)
		{
			return ResourceAccumulation.ResourceKeys.Any((ResourceTypes t) => (includePrestige || t != ResourceTypes.Prestige) && this[t] > val);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00051454 File Offset: 0x0004F654
		public bool AnyGreaterThanOrEqualTo(int val, bool includePrestige = true)
		{
			return ResourceAccumulation.ResourceKeys.Any((ResourceTypes t) => (includePrestige || t != ResourceTypes.Prestige) && this[t] >= val);
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00051494 File Offset: 0x0004F694
		public bool AnyLessThan(int val, bool includePrestige = true)
		{
			return ResourceAccumulation.ResourceKeys.Any((ResourceTypes t) => (includePrestige || t != ResourceTypes.Prestige) && this[t] < val);
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x000514D4 File Offset: 0x0004F6D4
		public int ValueSumNotIncludingPrestige()
		{
			int num = 0;
			foreach (ValueTuple<ResourceTypes, int> valueTuple in this.EnumerateResourceValues())
			{
				ResourceTypes item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (item != ResourceTypes.Prestige)
				{
					num += item2;
				}
			}
			return num;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00051534 File Offset: 0x0004F734
		public IEnumerable<ResourceTypes> GetAllTypesCantAfford(Cost cost)
		{
			return from t in ResourceAccumulation.ResourceKeys
			where this[t] < cost[t]
			select t;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0005156C File Offset: 0x0004F76C
		public string ToString(bool includePrestige)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<KeyValuePair<ResourceTypes, int>> enumerable = this.EnumerateValues();
			if (!includePrestige)
			{
				enumerable = from x in enumerable
				where x.Key != ResourceTypes.Prestige
				select x;
			}
			foreach (KeyValuePair<ResourceTypes, int> keyValuePair in enumerable)
			{
				stringBuilder.Append(keyValuePair.Key);
				stringBuilder.Append(":");
				stringBuilder.Append(keyValuePair.Value);
				stringBuilder.Append(", ");
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			return stringBuilder.ToString();
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00051634 File Offset: 0x0004F834
		public int CompareTo(ResourceAccumulation other)
		{
			return this.CompareTo(other, ResourceTypes.Souls);
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0005163E File Offset: 0x0004F83E
		public override string ToString()
		{
			return this.ToString(true);
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00051647 File Offset: 0x0004F847
		public bool IsDefault(DefaultValueAttribute defaultValueAttribute)
		{
			return this.IsZero;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x00051650 File Offset: 0x0004F850
		public ResourceTypes MostAbundantResource()
		{
			ResourceTypes result = ResourceTypes.Prestige;
			int num = 0;
			foreach (KeyValuePair<ResourceTypes, int> keyValuePair in this.EnumerateValues())
			{
				if (keyValuePair.Value > num)
				{
					result = keyValuePair.Key;
					num = keyValuePair.Value;
				}
			}
			return result;
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x000516B8 File Offset: 0x0004F8B8
		public int NumNonZeroResources()
		{
			int num = 0;
			foreach (KeyValuePair<ResourceTypes, int> keyValuePair in this.EnumerateValues())
			{
				if (keyValuePair.Value != 0)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x00051710 File Offset: 0x0004F910
		protected void DeepCloneResourceAccumulationParts(ResourceAccumulation clone)
		{
			clone._souls = this._souls;
			clone._ichor = this._ichor;
			clone._hellfire = this._hellfire;
			clone._darkness = this._darkness;
			clone._prestige = this._prestige;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0005174E File Offset: 0x0004F94E
		public virtual void DeepClone(out ResourceAccumulation clone)
		{
			clone = new ResourceAccumulation();
			this.DeepCloneResourceAccumulationParts(clone);
		}

		// Token: 0x04000AE2 RID: 2786
		private static readonly ResourceTypes[] _resourceKeys;

		// Token: 0x04000AE3 RID: 2787
		[JsonProperty]
		public int _souls;

		// Token: 0x04000AE4 RID: 2788
		[JsonProperty]
		public int _ichor;

		// Token: 0x04000AE5 RID: 2789
		[JsonProperty]
		public int _hellfire;

		// Token: 0x04000AE6 RID: 2790
		[JsonProperty]
		public int _darkness;

		// Token: 0x04000AE7 RID: 2791
		[JsonProperty]
		public int _prestige;
	}
}
