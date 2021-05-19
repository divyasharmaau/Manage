using Manage.Core.Entities;
using Manage.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Repository
{
    public interface IEmployeePersonalDetailsRepository : IRepository<EmployeePersonalDetails>
    {
        Task<EmployeePersonalDetails> GetEmployeeById(int id);
    }
}
