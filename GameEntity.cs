using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002BA RID: 698
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class GameEntity : IModifiable
	{
		// Token: 0x06000D42 RID: 3394 RVA: 0x00034C49 File Offset: 0x00032E49
		public virtual void ConfigureFrom(IdentifiableStaticData data)
		{
			if (data == null)
			{
				return;
			}
			this.StaticDataReference = data.ConfigRef;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00034C5B File Offset: 0x00032E5B
		protected GameEntity(IdentifiableStaticData data)
		{
			this.ConfigureFrom(data);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00034C80 File Offset: 0x00032E80
		protected GameEntity()
		{
		}

		// Token: 0x06000D45 RID: 3397
		public abstract ModifierContext CreateContext();

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x00034C9E File Offset: 0x00032E9E
		[JsonIgnore]
		public List<EntityTag> TagList
		{
			get
			{
				return this.Tags;
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00034CA6 File Offset: 0x00032EA6
		public IEnumerable<T> EnumerateTags<T>() where T : EntityTag
		{
			foreach (EntityTag entityTag in this.Tags)
			{
				T t = entityTag as T;
				if (t != null)
				{
					yield return t;
				}
			}
			List<EntityTag>.Enumerator enumerator = default(List<EntityTag>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00034CB8 File Offset: 0x00032EB8
		public bool TryGetTag<T>(out T tag) where T : EntityTag
		{
			tag = default(T);
			foreach (EntityTag entityTag in this.Tags)
			{
				T t = entityTag as T;
				if (t != null)
				{
					tag = t;
					break;
				}
			}
			return tag != null;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00034D34 File Offset: 0x00032F34
		public bool TryGetTag<T>(SimulationRandom random, out T tag)
		{
			return IEnumerableExtensions.ToList<T>(this.Tags.OfType<T>()).TryGetRandom(random, out tag);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00034D50 File Offset: 0x00032F50
		public bool HasTag(EntityTag tag)
		{
			using (List<EntityTag>.Enumerator enumerator = this.Tags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetType() == tag.GetType())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00034DB4 File Offset: 0x00032FB4
		public bool HasTag<T>() where T : EntityTag
		{
			T t;
			return this.TryGetTag<T>(out t);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00034DC9 File Offset: 0x00032FC9
		public T AddTag<T>(ModifierContext source, T tag) where T : EntityTag
		{
			tag = tag.DeepClone(CloneFunction.FastClone);
			tag.Source = source;
			this.Tags.Add(tag);
			return tag;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00034DF4 File Offset: 0x00032FF4
		public void AddTags<T>(ModifierContext source, IEnumerable<T> tags) where T : EntityTag
		{
			foreach (T tag in tags)
			{
				this.AddTag<T>(source, tag);
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00034E40 File Offset: 0x00033040
		public bool RemoveTag<T>(T tag) where T : EntityTag
		{
			return this.Tags.Remove(tag);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00034E54 File Offset: 0x00033054
		public bool RemoveTag<T>() where T : EntityTag
		{
			T tag;
			return this.TryGetTag<T>(out tag) && this.RemoveTag<T>(tag);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00034E74 File Offset: 0x00033074
		public virtual void ClearModifiers()
		{
			this.Tags.Clear();
			this.ClearStatModifiers();
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00034E87 File Offset: 0x00033087
		protected void DeepCloneParts(GameEntity clone)
		{
			clone.Tags = this.Tags.DeepClone<EntityTag>();
			clone.StaticDataReference = this.StaticDataReference.DeepClone();
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00034EAB File Offset: 0x000330AB
		public override string ToString()
		{
			ConfigRef staticDataReference = this.StaticDataReference;
			return ((staticDataReference != null) ? staticDataReference.Id : null) ?? "Unknown";
		}

		// Token: 0x040005D9 RID: 1497
		[JsonProperty]
		[PublicKnowledge]
		public ConfigRef StaticDataReference = new ConfigRef();

		// Token: 0x040005DA RID: 1498
		[JsonProperty]
		private List<EntityTag> Tags = new List<EntityTag>();
	}
}
