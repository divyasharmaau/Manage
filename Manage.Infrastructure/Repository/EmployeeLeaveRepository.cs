using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Infrastructure.Data;
using Manage.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Infrastructure.Repository
{
    public class EmployeeLeaveRepository : Repository<EmployeeLeave>, IEmployeeLeaveRepository
    {
        private readonly ManageContext _manageContext;
        public EmployeeLeaveRepository(ManageContext manageContext) : base(manageContext)
        {
            _manageContext = manageContext;
        }
        public async Task AddNewLeaveEmployeeLeave(EmployeeLeave employeeLeave)
        {
            await AddAsync(employeeLeave);
            await _manageContext.SaveChangesAsync();
        }

        public async Task<EmployeeLeave> GetLeaveById(int leaveId)
        {
            var leave = await _manageContext.EmployeeLeaves
                .Include(e => e.Employee)
                .Include(l => l.Leave)
                .Where(x => x.LeaveId == leaveId)
                .FirstOrDefaultAsync();
            return leave;
        }

        public async Task<double> TotalAnnualLeaveTaken(string id)
        {
            var employee = await _manageContext.Users
                .Include(e => e.EmployeeLeaves)
                .ThenInclude(l => l.Leave)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            //&& x.Leave.LeaveStatus == "Approved"
            var leaveCount = employee.EmployeeLeaves
                .Where(x => x.Leave.LeaveType == "Annual Leave" ).ToList();
            double sum = 0;
            double numberOfHoursOfLeave = 0;

            foreach (var item in leaveCount)
            {
                DateTime end = item.Leave.TillDate;
                DateTime start = item.Leave.FromDate;


                if (item.Leave.Duration == "First Half Day" || item.Leave.Duration == "Second Half Day")
                {
                    numberOfHoursOfLeave = (end.Subtract(start).Days + 0.5) * 7.6;
                }
                else
                {
                    numberOfHoursOfLeave = ((end - start).Days + 1) * 7.6;
                }

                sum += numberOfHoursOfLeave;
            }

            return Math.Round(sum, 2);
        }

        public async Task<double> TotalAnnualLeaveAccured(string id)
        {

            DateTime currentDate = DateTime.Now;
            double annualLeaveAccured = 0;

            var user = await _manageContext.Users
               .Include(e => e.EmployeeLeaves)
               .ThenInclude(l => l.Leave)
               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();

            var numberOfDaysWorked = user.DaysWorkedInWeek;
            var numberOfHoursOfEachDay = user.NumberOfHoursWorkedPerDay;


            if (user.Status == "Full-Time" || user.Status == "Contract")

            {

                //number of days = 5 , perDay 0.5846 , 5*0.5846 = 2.923
                annualLeaveAccured = (currentDate - user.JoiningDate).Days / 7 * 5 * 0.5846;
            }
            else if (user.Status == "Part-Time" || user.Status=="Casual")
            {
                //annualLeaveAccured =(weeks worked) * (number of hours worked each week) * (accural rate(4 weeks 38*4 = 152/52)) accuralRate = 2.923
                annualLeaveAccured = (currentDate - user.JoiningDate).Days / 7 * (numberOfDaysWorked * numberOfHoursOfEachDay) / 38 * 2.923;
            }


            return Math.Round(annualLeaveAccured, 2);

        }
    }
}

