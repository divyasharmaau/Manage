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
            , ILogger<EmployeeController> logger, IUploadImageHelper uploadImageHelper, IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager)
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

        //GET api/employee
        [HttpGet]
       
        public async Task<IActionResult> Get()
        {
            var employeeList = await _employeePageService.GetEmployeeList();
          
            if(employeeList == null)
            {
                return NotFound();
            }

            foreach(var employee in employeeList)
            {
                UpdateEmployeePersonalDetailsPhoto(employee.EmployeePersonalDetails);
            }

            return Ok(employeeList);          
        }

        
        //GET api/employee
        // [HttpGet(Name = "SearchByEmail")]
        [HttpGet("Search/{searchTerm}", Name = "Search")]
        public async Task<IActionResult> Search( string searchTerm)
        {
            var employeeList = await _employeePageService.GetEmployeeList();
            searchTerm = searchTerm.ToLower();
            var result = employeeList.Where(
                                             x => x.Email.ToLower().Contains(searchTerm)
                                            || x.Department.Name.ToLower().Contains(searchTerm)
                                            //|| x.Manager.ToLower().Contains(searchTerm)
                                            || x.FullName.ToLower().Contains(searchTerm)
                                            //|| x.Status.ToLower().Contains(searchTerm )
                                            ).ToList();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
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


        //GET api/employee/{id}
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
            // add async method requires an instance of EmployeePersonalDetailsViewModel
            //map the CreateEmployeePersonalDetailsDto with EmployeePersonalDetailsViewModel
            var mappedEmployeePersonalDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
            var photoPath = "https://localhost:44330/uploads/img/" + model.Photo.FileName;

            var photoBytes = System.IO.File.ReadAllBytes(photoPath);

            model.PhotoPath = $"data:image/{model.Photo.ContentType};base64,{Convert.ToBase64String(photoBytes)}";

            //add using the mapped variable which is an instance of EmployeePersonalDetailsViewModel
            var newEmployeePersonalDetails = await _employeePersonalDetailsPageService.AddAsync(mappedEmployeePersonalDetails);

            //var employeePersonalDetailsReadDto = _mapper.Map<EmployeePersonalDetailsDto>(newEmployeePersonalDetails);
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

                modelFromRepo.PhotoPath = "https://localhost:44330/uploads/img/" + fileName;

            }
            else
            {
                modelFromRepo.PhotoPath = model.ExistingPhotoPath;
            }
         
            
            // Map (source, destination)
            _mapper.Map(model, modelFromRepo);



            await _employeePersonalDetailsPageService.UpdateAsync(modelFromRepo);
            return NoContent();

        }

        //PATCH api/controller/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialEmployeeOfficialDetailsUpdate(string id , JsonPatchDocument<EditEmployeeOfficialDetailsDto> patchDoc)
        {
            var modelFromRepo = await _employeePageService.GetEmployeeById(id);
            if (modelFromRepo == null)
            {
                return NotFound();
            }

            var modelToPatch = _mapper.Map<EditEmployeeOfficialDetailsDto>(modelFromRepo);
            patchDoc.ApplyTo(modelToPatch, ModelState);
            if(!TryValidateModel(modelToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(modelToPatch, modelFromRepo);
            await _employeePageService.Update(modelFromRepo);
            return NoContent();
        }


        //PATCH api/controller/{id}
        [HttpPatch("PartialEmployeePersonalDetailUpdate/{id}")]
        public async Task<ActionResult> PartialEmployeePersonalDetailsUpdate(string id, JsonPatchDocument<EditEmployeePersonalDetailsDto> patchDoc)
        {
            var modelFromRepo = await _employeePersonalDetailsPageService.GetEmployeePersonalDetailsById(id);
            if (modelFromRepo == null)
            {
                return NotFound();
            }

            var modelToPatch = _mapper.Map<EditEmployeePersonalDetailsDto>(modelFromRepo);
            patchDoc.ApplyTo(modelToPatch, ModelState);
            if (!TryValidateModel(modelToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(modelToPatch, modelFromRepo);
            await _employeePersonalDetailsPageService.UpdateAsync(modelFromRepo);
            return NoContent();
        }
        private void UpdateEmployeePersonalDetailsPhoto(EmployeePersonalDetailsViewModel model)
        {
            if (model == null ||
                   string.IsNullOrEmpty(model.PhotoPath))
            {
                return;
            }

            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/img",
             model.PhotoPath);
            if (!System.IO.File.Exists(photoPath))
            {
                model.PhotoPath = null;
                return;
            }

            var photoBytes = System.IO.File.ReadAllBytes(photoPath);

            var fileExtension = model.PhotoPath.Split('.')[1];

            model.PhotoPath =
                $"data:image/{fileExtension};base64,{Convert.ToBase64String(photoBytes)}";

        }

    }
}
