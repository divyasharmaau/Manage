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
                .Where(x => x.Leave.LeaveType == "Annual Leave" && x.Leave.LeaveStatus =="Approved").ToList();
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


        public async Task<double> TotalSickLeaveTaken(string id)
        {
            double sum = 0;

            var user = await _manageContext.Users
                .Include(e => e.EmployeeLeaves)
                .ThenInclude(l => l.Leave)
               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();

            var leaveCount = user.EmployeeLeaves.Where(x => x.Leave.LeaveType == "Sick Leave" && x.Leave.LeaveStatus == "Approved").ToList();

            foreach (var item in leaveCount)
            {
                DateTime end = item.Leave.TillDate;
                DateTime start = item.Leave.FromDate;
                double numberOfHoursofLeave = 0;

                if (item.Employee.Status == "Permanent" || item.Employee.Status == "Fixed-Term")
                {
                    if (item.Leave.Duration == "First Half Day" || item.Leave.Duration == "Second Half Day")
                    {
                        numberOfHoursofLeave = (end.Subtract(start).Days + 0.5) * 7.6;
                    }
                    else
                    {
                        numberOfHoursofLeave = ((end - start).Days + 1) * 7.6;
                    }


                }
                else if (item.Employee.Status == "Part-Time" || item.Employee.Status == "Contract")
                {
                    if (item.Leave.Duration == "First Half Day" || item.Leave.Duration == "Second Half Day")
                    {
                        numberOfHoursofLeave = (end.Subtract(start).Days + 0.5) * item.Employee.NumberOfHoursWorkedPerDay;
                    }
                    else
                    {
                        numberOfHoursofLeave = ((end - start).Days + 1) * item.Employee.NumberOfHoursWorkedPerDay;
                    }

                }

                sum += numberOfHoursofLeave;
            }

            return Math.Round(sum, 2);
        }

        //All employees except casuals are entitled to paid sick and carer's leave.
        //all are entitkes to 10 days of sick leave , but according to their day length
        //a emp working for 4 hours a day is entitled to 10 sick leaves per year , 
        //but the length of the day would be 4 hours. 
        public async Task<double> TotalSickLeaveAccured(string id)
        {
            var user = await _manageContext.Users
                .Include(e => e.EmployeeLeaves)
                .ThenInclude(l => l.Leave)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            double totalSickLeaveAccured = 0;
            DateTime currentDate = DateTime.Now;
            var numberOfDaysWorked = user.DaysWorkedInWeek;
            var numberOfHoursOfEachDay = user.NumberOfHoursWorkedPerDay;

            if (user.Status == "Full-Time" || user.Status == "Contract")
            {

                totalSickLeaveAccured = (currentDate - user.JoiningDate).Days / 7 * 1.461;
            }
            else if (user.Status == "Part-Time" || user.Status == "Casual")
            {
                //totalSickLeaveAccured = (weeks)*(number of hours worked in a week)*(accural rate(2weeks/52))
                totalSickLeaveAccured = ((currentDate - user.JoiningDate).Days / 7) * (numberOfDaysWorked * numberOfHoursOfEachDay) / 38 * 1.461;
            }

            return Math.Round(totalSickLeaveAccured, 2);
        }

        public async Task<ApplicationUser> GetEmployeeWithLeaveList(string id)
        {
            var employee = await _manageContext.Users
                .Include(e => e.EmployeeLeaves)
                .ThenInclude(l => l.Leave)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return employee;
        }
    }
}

