using AutoMapper;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Manage.Web.Controllers
{ 
    public class EmployeeController : Controller
    {
        private readonly IEmployeePageService _employeePageService;
        private readonly IDepartmentPageService _departmentPageService;
        private readonly IEmployeePersonalDetailsPageService _employeePersonalDetailsPageService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IUploadImageHelper _uploadImageHelper;

        public EmployeeController(IEmployeePageService employeePageService , IDepartmentPageService departmentPageService  
            ,IEmployeePersonalDetailsPageService employeePersonalDetailsPageService
            , IMapper mapper , IWebHostEnvironment webHostEnvironment
            ,ILogger<EmployeeController> logger ,IUploadImageHelper uploadImageHelper)
        {
            _employeePageService = employeePageService;
            _departmentPageService = departmentPageService;
            _employeePersonalDetailsPageService = employeePersonalDetailsPageService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _uploadImageHelper = uploadImageHelper;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> EmployeeList(string sortOrder, int? page, string searchByName, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortByName = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

           
            _logger.LogInformation($"Employee List Requested");


            try
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
                    list.EmployeePersonalDetails = emp.EmployeePersonalDetails;

                    employeeList.Add(list);
                }

                if (searchByName != null)
                {

                    employeeList = employeeList.Where(e => e.FullName.ToLower().Contains(searchByName.ToLower())).ToList();
                }
                else
                {
                    searchByName = currentFilter;
                }
                ViewBag.CurrentFilter = searchByName;

                switch (sortOrder)
                {
                    case "name_desc":
                        employeeList = employeeList.OrderByDescending(e => e.FirstName).ToList();
                        break;
                    default:
                        employeeList = employeeList.OrderBy(e => e.FullName).ToList();
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(employeeList.ToPagedList(pageNumber, pageSize));
               
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw; 
            }

           
      
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var userEmail = await _employeePageService.FindEmail(email);

            if (userEmail == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email '{email}' is already in use.");
            }

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

                //if (user.Status == "Full-Time")
                //{
                //    user.DaysWorkedInWeek = 5;
                //    user.NumberOfHoursWorkedPerDay = 7.6;
                //}
                //else
                //{
                //    user.DaysWorkedInWeek = model.DaysWorkedInWeek;
                //    user.NumberOfHoursWorkedPerDay = model.NumberOfHoursWorkedPerDay;
                //}
                
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
                user.DaysWorkedInWeek = model.DaysWorkedInWeek;
                user.NumberOfHoursWorkedPerDay = model.NumberOfHoursWorkedPerDay;

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

            try
            {
                var employeeDetails = await _employeePageService.GetEmployeeById(id);
                if (employeeDetails != null)
                {
                    return View(employeeDetails);
                }
                else
                {
                    //Response.StatusCode = 404;
                    _logger.LogError($"Employee not found {id}");
                    return View("EmployeeNotFound", id);
                }
              
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
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
            if(ModelState.IsValid)
            {
                PhotoUploadViewModel photoUploadViewModel = new PhotoUploadViewModel();
                photoUploadViewModel.Photo = model.Photo;
                var uploadedImage = _uploadImageHelper.UploadImage(photoUploadViewModel);
                model.PhotoPath = uploadedImage.uniqueFileName;

                //mapping
                var empDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
                var employeePersonalDetails = await _employeePersonalDetailsPageService.AddAsync(empDetails);
                return RedirectToAction("EmployeePersonalDetails", new { id = empDetails.Id });
            }
            else
            {
                return View();
            }  
        }

        public async Task<IActionResult> EmployeePersonalDetails(string id)
        {
            var emp = await _employeePageService.GetEmployeeById(id);
            if(emp.EmployeePersonalDetails == null)
            {
                return RedirectToAction("CreateEmployeePersonalDetails", new { id = emp.Id });
            }
            else
            {
                var employee = await _employeePersonalDetailsPageService.GetEmployeePersonalDetailsById(id);
                employee.FullName = emp.FullName;
                employee.PhotoPath = emp.EmployeePersonalDetails.PhotoPath;
                return View(employee);
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployeePersonalDetails(string id)
        {
            var emp = await _employeePageService.GetEmployeeById(id);
            var empDetails = await _employeePersonalDetailsPageService.GetEmployeePersonalDetailsById(id);
          
                var employeePersonalDetails = _mapper.Map<EditEmployeePersonalDetailsViewModel>(empDetails);
                employeePersonalDetails.FullName = emp.FullName;
                employeePersonalDetails.ExistingPhotoPath = empDetails.PhotoPath;
                return View(employeePersonalDetails);
           
           
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployeePersonalDetails(EditEmployeePersonalDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                //mapping
                var empDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
                //upload image
                //var uniqueFileName = "";
                if (model.ExistingPhotoPath == null || empDetails.PhotoPath != model.ExistingPhotoPath)
                {
                    PhotoUploadViewModel photoUploadViewModel = new PhotoUploadViewModel();
                    photoUploadViewModel.Photo = model.Photo;
                    var uploadedImage = _uploadImageHelper.UploadImage(photoUploadViewModel);
                    empDetails.PhotoPath = uploadedImage.uniqueFileName;
                }
                else
                {
                    empDetails.PhotoPath = model.ExistingPhotoPath;
                }
                var employeePersonalDetails = await _employeePersonalDetailsPageService.UpdateAsync(empDetails);
                return RedirectToAction("EmployeePersonalDetails" , new { id = empDetails.Id });
            }
            else
            {
                return View();
            }
        }

    }
}
