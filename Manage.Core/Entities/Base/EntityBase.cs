using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.Core.Entities.Base
{
    public class EntityBase<TID> : IEntityBase<TID>
    {
        public virtual TID ID { get; set; }
    }
}
