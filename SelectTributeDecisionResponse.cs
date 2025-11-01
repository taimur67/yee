using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020004C9 RID: 1225
	[Serializable]
	public class SelectTributeDecisionResponse : DecisionResponse
	{
		// Token: 0x060016EE RID: 5870 RVA: 0x00053ED0 File Offset: 0x000520D0
		[JsonConstructor]
		public SelectTributeDecisionResponse()
		{
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00053EE4 File Offset: 0x000520E4
		public bool IsSelected(int selectedId)
		{
			return this.SelectedIds.Any((int t) => t == selectedId);
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00053F15 File Offset: 0x00052115
		public void Select(int selectedId)
		{
			this.SelectedIds.Add(selectedId);
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00053F23 File Offset: 0x00052123
		public void Select(IEnumerable<int> ids)
		{
			this.SelectedIds.AddRange(ids);
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00053F31 File Offset: 0x00052131
		public void Deselect(int selectedId)
		{
			this.SelectedIds.Remove(selectedId);
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00053F40 File Offset: 0x00052140
		public bool IsSelected(Manuscript manuscript)
		{
			return this.IsSelected((int)manuscript.Id);
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00053F4E File Offset: 0x0005214E
		public bool IsSelected(ResourceNFT resource)
		{
			return this.IsSelected(resource.Id);
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00053F5C File Offset: 0x0005215C
		public void Select(ResourceNFT resource)
		{
			this.Select(resource.Id);
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00053F6A File Offset: 0x0005216A
		public void Select(Manuscript manuscript)
		{
			this.Select((int)manuscript.Id);
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00053F78 File Offset: 0x00052178
		public void Deselect(ResourceNFT nft)
		{
			this.Deselect(nft.Id);
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00053F86 File Offset: 0x00052186
		public void Deselect(Manuscript manuscript)
		{
			this.Deselect((int)manuscript.Id);
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00053F94 File Offset: 0x00052194
		public void Select(params ResourceNFT[] resources)
		{
			foreach (ResourceNFT resource in resources)
			{
				this.Select(resource);
			}
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00053FBC File Offset: 0x000521BC
		public override string GetDebugString()
		{
			string text = base.GetDebugString() + ": ";
			foreach (int num in this.SelectedIds)
			{
				text = text + num.ToString() + ", ";
			}
			return text;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00054030 File Offset: 0x00052230
		public override void DeepClone(out DecisionResponse clone)
		{
			SelectTributeDecisionResponse selectTributeDecisionResponse = new SelectTributeDecisionResponse
			{
				SelectedIds = this.SelectedIds.DeepClone()
			};
			base.DeepCloneDecisionResponseParts(selectTributeDecisionResponse);
			clone = selectTributeDecisionResponse;
		}

		// Token: 0x04000B43 RID: 2883
		[JsonProperty]
		public List<int> SelectedIds = new List<int>();
	}
}
