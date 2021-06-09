using AutoMapper;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeePageService _employeePageService;
        private readonly IDepartmentPageService _departmentPageService;
        private readonly IEmployeePersonalDetailsPageService _employeePersonalDetailsPageService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironmwnt;

        public EmployeeController(IEmployeePageService employeePageService , IDepartmentPageService departmentPageService  
            ,IEmployeePersonalDetailsPageService employeePersonalDetailsPageService
            , IMapper mapper , IWebHostEnvironment webHostEnvironmwnt)
        {
            _employeePageService = employeePageService;
            _departmentPageService = departmentPageService;
            _employeePersonalDetailsPageService = employeePersonalDetailsPageService;
            _mapper = mapper;
            _webHostEnvironmwnt = webHostEnvironmwnt;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> EmployeeList()
        {
            var empList = await _employeePageService.GetEmployeeList();

            List<EmployeeListViewModel> employeeList = new List<EmployeeListViewModel>();
            foreach (var emp in empList)
            {
                EmployeeListViewModel list = new EmployeeListViewModel();
                list.Id = emp.Id;
                list.FirstName = emp.FirstName;
                list.MiddleName = emp.MiddleName;
                list.LastName = emp.LastName;
                list.Department = emp.Department;
                list.JobTitle = emp.JobTitle;
                list.Status = emp.Status;
                list.Manager = emp.Manager;
                list.Email = emp.Email;

                employeeList.Add(list);
            }
            return View(employeeList);

            //var model = new EditEmployeeOfficialDetailsAdminViewModel
            //{
            //    Title = "Blah Blah",
            //    Id = "a9067a75-b6ee-4c23-a3e2-f19648957347"
            //};

            //var mapped = _mapper.Map<ApplicationUserViewModel>(model);

            //await _employeePageService.Update(mapped);

            //return null;
        }


        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            var dList = await _departmentPageService.GetDepartmentList();

            var deptList = dList.Select(dept => new SelectListItem()
            {
                Text = dept.Name,
                Value = dept.Id.ToString()
            }).ToList();

            deptList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            CreateEmployeeViewModel model = new CreateEmployeeViewModel();
            model.departmentList = deptList;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if(ModelState.IsValid)
            {
                ApplicationUserViewModel user = new ApplicationUserViewModel();
                user.Id = Guid.NewGuid().ToString();

                if (user.Status == "Full-Time")
                {
                    user.DaysWorkedInWeek = 5;
                    user.NumberOfHoursWorkedPerDay = 7.6;
                }
                else
                {
                    user.DaysWorkedInWeek = model.DaysWorkedInWeek;
                    user.NumberOfHoursWorkedPerDay = model.NumberOfHoursWorkedPerDay;
                }
                
                user.Title = model.Title;
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.ConfirmPassword = model.ConfirmPassword;
                user.JoiningDate = model.JoiningDate;
                user.JobTitle = model.JobTitle;
                user.Status = model.Status;
                user.DepartmentId = model.DepartmentId;
                user.Manager = model.Manager;

                var result = await _employeePageService.CreateEmployee(user);
                if (result.Succeeded)
                {
                   return RedirectToAction("EmployeeList", "Employee");
                }
                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult>EmployeeOfficialDetails(string id)
        {
           var employeeDetails =  await _employeePageService.GetEmployeeById(id);
            if(employeeDetails != null)
            {
                return View(employeeDetails);
            }
            else
            {
                return View();
            }
           
        }

        [HttpGet]
        public async Task<IActionResult>EditEmployeeOfficialDetailsAdmin(string id)
        {
            var empDetails = await _employeePageService.GetEmployeeById(id);
            var employeeDetails = _mapper.Map<EditEmployeeOfficialDetailsAdminViewModel>(empDetails);
            var dList = await _departmentPageService.GetDepartmentList();

            var deptList = dList.Select(dept => new SelectListItem()
            {
                Text = dept.Name,
                Value = dept.Id.ToString()
            }).ToList();

            deptList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            employeeDetails.departmentList = deptList;
           
            return View(employeeDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployeeOfficialDetailsAdmin(EditEmployeeOfficialDetailsAdminViewModel model)
        { 
            if(ModelState.IsValid)
            {
                var mapped = _mapper.Map<ApplicationUserViewModel>(model);
                await  _employeePageService.Update(mapped);
                return RedirectToAction("EmployeeOfficialDetails", new { id = mapped.Id });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployeeOfficialDetails(string id)
        {
            var empDetails =  await _employeePageService.GetEmployeeById(id);
            var employeeDetails = _mapper.Map<EditEmployeeOfficialDetailsViewModel>(empDetails);
            return View(employeeDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployeeOfficialDetails(EditEmployeeOfficialDetailsViewModel model)
        {
            if(ModelState.IsValid)
            {
               var mapped =  _mapper.Map<ApplicationUserViewModel>(model);
                await _employeePageService.Update(mapped);
                return RedirectToAction("EmployeeOfficialDetails", new { id = mapped.Id });
            }
            return View();

        }


        [HttpGet]
        public async Task<IActionResult> CreateEmployeePersonalDetails(string id)
        {
            var user = await _employeePageService.GetEmployeeById(id);
            CreateEmployeePersonalDetailsViewModel model = new CreateEmployeePersonalDetailsViewModel();
            model.FullName = user.FullName;
            return View(model);
        }
      
        [HttpPost]
        public async Task<IActionResult> CreateEmployeePersonalDetails(CreateEmployeePersonalDetailsViewModel model)
        {
            //upload image
            var uniqueFileName = "";
            //to get to the path of the wwwwrootfolder
           var uploadsFolder =  Path.Combine(_webHostEnvironmwnt.WebRootPath, "dist/img");
            //append GUID value  and undersacore for unique File Name
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //copy file to images folder
            model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            //mapping
            var empDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
            empDetails.PhotoPath = uniqueFileName;

            var employeePersonalDetails =  await _employeePersonalDetailsPageService.AddAsync(empDetails);
            
            return View();
        }


       

    }
}
