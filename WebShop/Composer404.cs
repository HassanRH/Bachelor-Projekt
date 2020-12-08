using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace WebShop
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class Composer404 : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.SetContentLastChanceFinder<My404ContentFinder>();
        }
     }
}
