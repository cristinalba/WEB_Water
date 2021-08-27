using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Data
{
    public class AddressRepository: GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(DataContext context) : base(context)
        {

        }
    }
}
