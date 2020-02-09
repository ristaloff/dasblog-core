﻿using System.Threading.Tasks;
using DasBlog.Core.Common;
using DasBlog.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DasBlog.Web.TagHelpers.Layout
{
	public class SitePageControlTagHelper : TagHelper
	{
		public string NewerPostsText { get; set; } = "<< Newer Posts";

		public string OlderPostsText { get; set; } = "Older Posts >>";

		private int PostCount { get; set; }
		private int PageNumber { get; set; }
		private const string PAGEANCHOR = "<a href='{0}'>{1}</a>";

		[ViewContext]
		public ViewContext ViewContext { get; set; }

		private IDasBlogSettings dasBlogSettings;

		public SitePageControlTagHelper(IDasBlogSettings dasBlogSettings)
		{
			this.dasBlogSettings = dasBlogSettings;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			PostCount = (int)ViewContext.ViewData[Constants.PostCount];
			PageNumber = (int)ViewContext.ViewData[Constants.PageNumber];

			var pagecontrol = string.Empty;

			output.TagName = "span";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes.SetAttribute("class", "dbc-span-page-control");

			var content = await output.GetChildContentAsync();
			var seperator = " | ";
			
			if(!string.IsNullOrEmpty(content.GetContent()))
			{
				seperator = content.GetContent();
			}

			var separatorRequired = PostCount > 0 && PageNumber > 0;

			if (PageNumber > 0)
			{
				pagecontrol = string.Format(PAGEANCHOR, dasBlogSettings.RelativeToRoot(string.Format("page/{0}", PageNumber - 1)), NewerPostsText);
			}
			
			if (separatorRequired)
			{
				pagecontrol += seperator;
			}

			if (PostCount > 0)
			{
				pagecontrol += string.Format(PAGEANCHOR, dasBlogSettings.RelativeToRoot(string.Format("page/{0}", PageNumber + 1)), OlderPostsText);
			}

			output.Content.SetHtmlContent(pagecontrol);
		}
	}
}
