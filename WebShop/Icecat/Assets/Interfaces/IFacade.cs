using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Icecat.Assets.Interfaces
{
    public interface IFacade
    {
        void initDatabaseWithProducts();

        void updateProducts();

    }
}
