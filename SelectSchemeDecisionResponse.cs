using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004C6 RID: 1222
	[Serializable]
	public class SelectSchemeDecisionResponse : DecisionResponse
	{
		// Token: 0x060016D8 RID: 5848 RVA: 0x00053B01 File Offset: 0x00051D01
		[JsonConstructor]
		public SelectSchemeDecisionResponse()
		{
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00053B26 File Offset: 0x00051D26
		public SelectSchemeDecisionResponse(params SchemeId[] ids) : this(ids.AsEnumerable<SchemeId>())
		{
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00053B34 File Offset: 0x00051D34
		public SelectSchemeDecisionResponse(IEnumerable<SchemeId> ids)
		{
			this.Selected.AddRange(ids);
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00053B65 File Offset: 0x00051D65
		public bool Deselect(SchemeObjective scheme)
		{
			return this.Deselect(scheme.Id);
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00053B73 File Offset: 0x00051D73
		public bool Deselect(SchemeId id)
		{
			return this.Selected.Remove(id);
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00053B81 File Offset: 0x00051D81
		public void Select(SchemeObjective scheme)
		{
			this.Select(scheme.Id);
			this.AcceptSchemes = true;
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00053B96 File Offset: 0x00051D96
		public void DiscardSchemes()
		{
			this.Selected.Clear();
			this.AcceptSchemes = false;
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00053BAA File Offset: 0x00051DAA
		public void SetSchemeVisibility(SchemeId id, bool isPrivate)
		{
			this.SchemesVisibility[id] = isPrivate;
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00053BB9 File Offset: 0x00051DB9
		public void SetSchemeVisibility(SchemeObjective scheme, bool isPrivate)
		{
			this.SetSchemeVisibility(scheme.Id, isPrivate);
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00053BC8 File Offset: 0x00051DC8
		public void Select(SchemeId id)
		{
			if (!this.IsSelected(id))
			{
				this.Selected.Add(id);
			}
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00053BDF File Offset: 0x00051DDF
		public bool IsSelected(SchemeObjective scheme)
		{
			return this.IsSelected(scheme.Id);
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00053BED File Offset: 0x00051DED
		public bool IsSelected(SchemeId id)
		{
			return this.Selected.Contains(id);
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00053BFC File Offset: 0x00051DFC
		public bool IsPrivate(SchemeId id)
		{
			bool flag;
			return this.SchemesVisibility.TryGetValue(id, out flag) && flag;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00053C1C File Offset: 0x00051E1C
		public override void DeepClone(out DecisionResponse clone)
		{
			SelectSchemeDecisionResponse selectSchemeDecisionResponse = new SelectSchemeDecisionResponse
			{
				Selected = this.Selected.DeepClone(),
				SchemesVisibility = this.SchemesVisibility.DeepClone(),
				AcceptSchemes = this.AcceptSchemes
			};
			base.DeepCloneDecisionResponseParts(selectSchemeDecisionResponse);
			clone = selectSchemeDecisionResponse;
		}

		// Token: 0x04000B3E RID: 2878
		[JsonProperty]
		public List<SchemeId> Selected = new List<SchemeId>();

		// Token: 0x04000B3F RID: 2879
		[JsonProperty]
		public Dictionary<SchemeId, bool> SchemesVisibility = new Dictionary<SchemeId, bool>();

		// Token: 0x04000B40 RID: 2880
		[JsonProperty]
		[DefaultValue(true)]
		public bool AcceptSchemes = true;
	}
}
