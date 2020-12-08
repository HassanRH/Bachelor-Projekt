using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web.Routing;

namespace WebShop
{
        public class My404ContentFinder : IContentLastChanceFinder
        {
            private readonly IDomainService _domainService;
            public My404ContentFinder(IDomainService domainService)
            {
                _domainService = domainService;
            }
            public bool TryFindContent(PublishedRequest contentRequest)
            {
                //find the root node with a matching domain to the incoming request
                var url = contentRequest.Uri.ToString();
                var allDomains = _domainService.GetAll(true);
                var domain = allDomains?.Where(f => f.DomainName == contentRequest.Uri.Authority || f.DomainName == "https://" + contentRequest.Uri.Authority).FirstOrDefault();
                var siteId = domain != null ? domain.RootContentId : (allDomains.Any() ? allDomains.FirstOrDefault().RootContentId : null);
                var siteRoot = contentRequest.UmbracoContext.Content.GetById(false, siteId ?? -1);
                if (siteRoot == null) { siteRoot = contentRequest.UmbracoContext.Content.GetAtRoot().FirstOrDefault(); }
                if (siteRoot == null)
                {
                    return false;
                }
                //assuming the 404 page is in the root of the language site with alias fourOhFourPageAlias
                IPublishedContent notFoundNode = siteRoot.Children.FirstOrDefault(f => f.ContentType.Alias == "page404");

                
                if (notFoundNode != null)
                {
                    contentRequest.PublishedContent = notFoundNode;
                }
                // return true or false depending on whether our custom 404 page was found
                return contentRequest.PublishedContent != null;
            }
        }
}