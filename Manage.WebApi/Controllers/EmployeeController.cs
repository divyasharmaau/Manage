using AutoMapper;
using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.WebApi.Dto;
using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeePageService _employeePageService;
        private readonly IDepartmentPageService _departmentPageService;
        private readonly IEmployeePersonalDetailsPageService _employeePersonalDetailsPageService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IUploadImageHelper _uploadImageHelper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public EmployeeController(IEmployeePageService employeePageService, IDepartmentPageService departmentPageService
            , IEmployeePersonalDetailsPageService employeePersonalDetailsPageService
            , IMapper mapper, IWebHostEnvironment webHostEnvironment
            , ILogger<EmployeeController> logger, IUploadImageHelper uploadImageHelper
            , IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager)
        {
            _employeePageService = employeePageService;
            _departmentPageService = departmentPageService;
            _employeePersonalDetailsPageService = employeePersonalDetailsPageService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _uploadImageHelper = uploadImageHelper;
            _employeeRepository = employeeRepository;
            _userManager = userManager;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employeeList = await _employeePageService.GetEmployeeList();
          
            if(employeeList == null)
            {
                return NotFound();
            }

            foreach (var employee in employeeList)
            {
                UpdateEmployeePersonalDetailsPhoto(employee.EmployeePersonalDetails);
            }

            return Ok(employeeList);          
        }


   
        [HttpGet("{id}" , Name ="GetEmployeeOfficialDetailsById")]
        public async Task<ActionResult<ApplicationUserViewModel>> GetEmployeeOfficialDetailsById(string id)
        {
            var employee = await _employeePageService.GetEmployeeById(id);

            UpdateEmployeePersonalDetailsPhoto(employee.EmployeePersonalDetails);

            if (employee != null)
            {
                return employee;
            }
            return NotFound();
        }


 
        [HttpGet("GetEmployeePersonalDetails/{id}", Name = "GetEmployeePersonalDetailsById") ]
        public async Task<ActionResult<EmployeePersonalDetailsViewModel>> GetEmployeePersonalDetailsById(string id)
        {
            var employeePersonalDetails = await _employeePersonalDetailsPageService.GetEmployeePersonalDetailsById(id);
            employeePersonalDetails.FullName = employeePersonalDetails.ApplicationUser.FirstName
                + " " + employeePersonalDetails.ApplicationUser.LastName;

            UpdateEmployeePersonalDetailsPhoto(employeePersonalDetails);            

            if (employeePersonalDetails != null)
            {
                return employeePersonalDetails;
            }
            return NotFound();

        }


        [HttpPost]
        public async Task<ActionResult>CreateEmployee(CreateEmployeeViewModel model)
        {
            
            if (model == null)
            {
                return NotFound();
            }
            ApplicationUserViewModel user = new ApplicationUserViewModel();
            user.Id = Guid.NewGuid().ToString();
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
            user.DaysWorkedInWeek = model.DaysWorkedInWeek;
            user.NumberOfHoursWorkedPerDay = model.NumberOfHoursWorkedPerDay;
            user.DepartmentId = model.DepartmentId;
            user.Manager = model.Manager;
           
            var result = await _employeePageService.CreateEmployee(user, model.Password);
            return CreatedAtRoute(nameof(GetEmployeeOfficialDetailsById), new { id = user.Id }, null);

        }

        [HttpPost("CreateEmployeePersonalDetails")]
        public async Task<ActionResult<EmployeePersonalDetailsViewModel>> CreateEmployeePersonalDetails(
          [FromForm]CreateEmployeePersonalDetailsViewModel model )
        {
         
            if(model.Photo.Length > 0)
            {
                var folderName = Path.Combine("Uploads", "img");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fullPath = Path.Combine(pathToSave, model.Photo.FileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {

                    await model.Photo.CopyToAsync(fileStream);
                }

            }
      
            var empOfficialDetails = await _employeePageService.GetEmployeeById(model.Id);
            model.FullName = empOfficialDetails.FullName;
 
            var mappedEmployeePersonalDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
            var photoPath = model.Photo.FileName;
            model.ApiPhotoPath = photoPath;

            //add using the mapped variable which is an instance of EmployeePersonalDetailsViewModel
            var newEmployeePersonalDetails = await _employeePersonalDetailsPageService.AddAsync(mappedEmployeePersonalDetails);
            //return NoContent();
            return CreatedAtRoute(nameof(GetEmployeePersonalDetailsById), new { id = newEmployeePersonalDetails.Id }, newEmployeePersonalDetails);

        
        }

        [HttpPut("{id}")]
        public  async Task<ActionResult> Update(string id , EditEmployeeOfficialDetailsViewModel model)
        {

            ApplicationUser user = await _userManager.FindByIdAsync(id);
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.JoiningDate = model.JoiningDate;
            user.JobTitle = model.JobTitle;
            user.Status = model.Status;
            user.DaysWorkedInWeek = model.DaysWorkedInWeek;
            user.NumberOfHoursWorkedPerDay = model.NumberOfHoursWorkedPerDay;
            user.DepartmentId = model.DepartmentId;
            user.Manager = model.Manager;
            user.Id = id;

           await _userManager.UpdateAsync(user);
         
            return NoContent();
        }


        [HttpPut("UpdateOfficialDetailsByAdmin/{id}")]
        public async Task<ActionResult> UpdateOfficialDetailsByAdmin(string id, EditEmployeeOfficialDetailsViewModel model)
        {
            model.Id = id;
            var modelFromRepo = await _employeePageService.GetEmployeeById(id);
            if (modelFromRepo == null)
            {
                return NotFound();
            }
            //source(existing) , map to modelFromrepo) 
            var mapped = _mapper.Map(model, modelFromRepo);
            await _employeePageService.Update(modelFromRepo);
            return NoContent();
        }


        [HttpPut("UpdateEmployeePersonalDetails/{id}")]
        public async Task<ActionResult> UpdateEmployeePersonalDetails(string id, 
            [FromForm] EditEmployeePersonalDetailsViewModel model)
        {

            string fileName = null;
            model.Id = id;
            var modelFromRepo = await _employeePersonalDetailsPageService.GetEmployeePersonalDetailsById(id);

            if (modelFromRepo == null)
            {
                return NotFound();
            }

             if (model.Photo != null)
            {
                var folderName = Path.Combine("Uploads", "img");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                fileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                var fullPath = Path.Combine(pathToSave, fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {

                    await model.Photo.CopyToAsync(fileStream);
                }

                modelFromRepo.ApiPhotoPath = fileName;

            }
            else
            {
                modelFromRepo.ApiPhotoPath = model.ExistingPhotoPath;
            }
                     
            // Map (source, destination)
            _mapper.Map(model, modelFromRepo);
            await _employeePersonalDetailsPageService.UpdateAsync(modelFromRepo);
            return NoContent();

        }

 
        private void UpdateEmployeePersonalDetailsPhoto(EmployeePersonalDetailsViewModel model)
        {
            if (model == null ||
                   string.IsNullOrEmpty(model.ApiPhotoPath))
            {
                return;
            }

            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/img",
             model.ApiPhotoPath);
            if (!System.IO.File.Exists(photoPath))
            {
                model.ApiPhotoPath = null;
                return;
            }

            var photoBytes = System.IO.File.ReadAllBytes(photoPath);

            var fileExtension = model.ApiPhotoPath.Split('.')[1];

            model.ApiPhotoPath =
                $"data:image/{fileExtension};base64,{Convert.ToBase64String(photoBytes)}";

        }

    }
}
