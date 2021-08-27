using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Data.Entities
{
    public interface IEntity
    {
        int Id { get; set; }

        //bool WasDeleted { get; set; }
    }
}
