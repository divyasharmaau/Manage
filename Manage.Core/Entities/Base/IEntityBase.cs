using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.Core.Entities.Base
{
   public interface IEntityBase<TID>
   {
        TID ID { get; }
   }
}
