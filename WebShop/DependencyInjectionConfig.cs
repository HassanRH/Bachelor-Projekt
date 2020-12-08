using WebShop.Services.Interfaces;
using WebShop.Services.Implementation;
using Umbraco.Core.Composing;
using Umbraco.Core;
using WebShop.Entity;
using WebShop.Models;
using IceCat.Assets.Interfaces;
using IceCat.Assets.DataAcessLayer;
using IceCat.Assets.Classes.Configs;
using WebShop.Icecat.Assets.Interfaces;
using IceCat.Facade;
using IceCat.Entity;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Console;
using WebShop;
using Umbraco.Web;

namespace DependencyInjection
{
    public class DependencyInjectionConfig : IUserComposer
    {
        public void Compose(Composition composition)
        {
            //General dependencies
            composition.Register<IDatabase, ProductDatabase>();

            //Umbraco dependencies
            composition.Register<IOrderService<Order,CheckOutForm>, OrderService>();
            composition.Register<IProductService<product, BinaryClass>, ProductService>();
            composition.Register<ISolrService<SolrResponse>, SolrService>();

            //Icecat dependencies
            composition.Register<IDataAccessLayer, DataAccessLayer>();
            composition.Register<ICredentials, icecatCredentials>();
            composition.Register<Iicecat, icecat>();
            composition.Register<IFacade, IcecatFacade>();

            //Hangire
            //composition.Register<IHangfire, Hangfire>();
            composition.Register<IHangfire, HangfireService>();


        }
    }
}