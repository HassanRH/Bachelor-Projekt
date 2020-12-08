//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v8.6.4
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder.Embedded;

namespace Umbraco.Web.PublishedModels
{
	/// <summary>About</summary>
	[PublishedModel("about")]
	public partial class About : PublishedContentModel, IMetaData, IShowInNavigationBar
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		public new const string ModelTypeAlias = "about";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<About, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public About(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// aboutBannerImage
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("aboutBannerImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent AboutBannerImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("aboutBannerImage");

		///<summary>
		/// aboutBannerTitle
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("aboutBannerTitle")]
		public string AboutBannerTitle => this.Value<string>("aboutBannerTitle");

		///<summary>
		/// aboutSection1Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("aboutSection1Description")]
		public string AboutSection1Description => this.Value<string>("aboutSection1Description");

		///<summary>
		/// aboutSection1Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("aboutSection1Title")]
		public string AboutSection1Title => this.Value<string>("aboutSection1Title");

		///<summary>
		/// aboutSection2Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("aboutSection2Description")]
		public string AboutSection2Description => this.Value<string>("aboutSection2Description");

		///<summary>
		/// aboutSection2Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("aboutSection2Title")]
		public string AboutSection2Title => this.Value<string>("aboutSection2Title");

		///<summary>
		/// description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("metaDescription")]
		public string MetaDescription => global::Umbraco.Web.PublishedModels.MetaData.GetMetaDescription(this);

		///<summary>
		/// title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("metaTitle")]
		public string MetaTitle => global::Umbraco.Web.PublishedModels.MetaData.GetMetaTitle(this);

		///<summary>
		/// showInNav
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.6.4")]
		[ImplementPropertyType("showInNavbar")]
		public bool ShowInNavbar => global::Umbraco.Web.PublishedModels.ShowInNavigationBar.GetShowInNavbar(this);
	}
}