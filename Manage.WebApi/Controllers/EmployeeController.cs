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

        //GET api/employee/{id}
        [HttpGet("{id}" , Name ="GetEmployeeOfficialDetailsById")]
        public async Task<ActionResult<ApplicationUserViewModel>> GetEmployeeOfficialDetailsById(string id)
        {
            var employee = await _employeePageService.GetEmployeeById(id);
            //employee.EmployeePersonalDetails.PhotoPath = "https://localhost:44330/uploads/img/" + employee.EmployeePersonalDetails.PhotoPath;
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
            employeePersonalDetails.FullName = employeePersonalDetails.ApplicationUser.FirstName + " " + employeePersonalDetails.ApplicationUser.LastName;
            //var empOfficialDetails = await _employeePageService.GetEmployeeById(id);
            //employeePersonalDetails.FullName = empOfficialDetails.FirstName + " " + empOfficialDetails.LastName;
            //employeePersonalDetails.PhotoPath =  "https://localhost:44330/uploads/img/" + employeePersonalDetails.PhotoPath ;
            if (employeePersonalDetails != null)
            {
                return employeePersonalDetails;
            }
            return NotFound();

        }

        //POST api/employee
        //[HttpPost]
        //public CreatedAtRouteResult CreateEmployee(CreateEmployeeViewModel createEmployeeViewModel)
        //{
        //    var model = _mapper.Map<ApplicationUserViewModel>(createEmployeeViewModel);
        //    model.Id = Guid.NewGuid().ToString();
        //    var employeeTask = _employeePageService.CreateEmployee(model);
        //    employeeTask.Wait();
        //    var dto = _mapper.Map<EmployeeOfficialDetailsReadDto>(model);
        //    return  CreatedAtRoute(nameof(GetEmployeeOfficialDetailsById), new { id = dto.Id }, dto);           
        //}
        ///------------------------------------------------------------------------------------------
        //[HttpPost]
        //public async Task<ActionResult<EmployeeOfficialDetailsReadDto>> CreateEmployee(CreateEmployeeViewModel model )
        //{
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }
        //    var createFromDto = _mapper.Map<ApplicationUserViewModel>(model);
        //    createFromDto.Id = Guid.NewGuid().ToString();
        //    var employee = await _employeePageService.CreateEmployee(createFromDto);
        //    var employeeDetailsReadDto = _mapper.Map<EmployeeOfficialDetailsReadDto>(createFromDto);
        //    return CreatedAtRoute(nameof(GetEmployeeOfficialDetailsById), new { id = employeeDetailsReadDto.Id }, employeeDetailsReadDto);
        //}

        //[HttpPost]
        //public async Task<ActionResult> CreateEmployee(CreateEmployeeDto model)
        //{
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }
        //    var createFromDto = _mapper.Map<ApplicationUserDto>(model);
        //    createFromDto.Id = Guid.NewGuid().ToString();
        //    var dtoMapped = _mapper.Map<ApplicationUserViewModel>(createFromDto);
        //    var employee = await _employeePageService.CreateEmployee(dtoMapped, model.Password);
        //    return CreatedAtRoute(nameof(GetEmployeeOfficialDetailsById), new { id = createFromDto.Id },null);


        //}

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
           // model.Id = id;
            var empOfficialDetails = await _employeePageService.GetEmployeeById(model.Id);
            model.FullName = empOfficialDetails.FullName;
            // add async method requires an instance of EmployeePersonalDetailsViewModel
            //map the CreateEmployeePersonalDetailsDto with EmployeePersonalDetailsViewModel
            var mappedEmployeePersonalDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
            mappedEmployeePersonalDetails.PhotoPath = "https://localhost:44330/uploads/img/" + model.Photo.FileName;
               
            //add using the mapped variable which is an instance of EmployeePersonalDetailsViewModel
            var newEmployeePersonalDetails = await _employeePersonalDetailsPageService.AddAsync(mappedEmployeePersonalDetails);

            //var employeePersonalDetailsReadDto = _mapper.Map<EmployeePersonalDetailsDto>(newEmployeePersonalDetails);
            //return NoContent();
            return CreatedAtRoute(nameof(GetEmployeePersonalDetailsById), new { id = newEmployeePersonalDetails.Id }, newEmployeePersonalDetails);

            //to get the url
            //return CreatedAtRoute(nameof(GetEmployeePersonalDetailsById), new { id = model.Id }, model);

            // //var modelFromRepo = await _employeePageService.GetEmployeeById(id);
            // //if (modelFromRepo == null)
            // //{
            // //    return NotFound();
            // //}

            // //create and add to db
            // var createEmployeePersonalDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
            // //createEmployeePersonalDetails.Id = modelFromRepo.Id;
            // //createEmployeePersonalDetails.FullName = modelFromRepo.FullName;
            // var employee = await _employeePersonalDetailsPageService.AddAsync(createEmployeePersonalDetails);

            // var employeePersonalDetailsReadDto = _mapper.Map<EmployeePersonalDetailsReadDto>(createEmployeePersonalDetails);

            // return CreatedAtRoute(nameof(GetEmployeePersonalDetailsById), new { id = employeePersonalDetailsReadDto.Id }, employeePersonalDetailsReadDto);

            //// return employee;
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


            //var modelFromRepo = await _employeePageService.GetEmployeeById(id);
            //if(modelFromRepo == null)
            //{
            //    return NotFound();
            //}
            ////source(existing) , map to modelFromrepo) 
            // _mapper.Map(model, modelFromRepo);
            //try
            //{
            //    await _employeePageService.Update(modelFromRepo);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
         
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


            }
         
            
            // Map (source, destination)
            _mapper.Map(model, modelFromRepo);

            //modelFromRepo.PhotoPath = fileName 
            modelFromRepo.PhotoPath = "https://localhost:44330/uploads/img/" +fileName ?? modelFromRepo.PhotoPath;

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


        //public async Task<IActionResult> EmployeeOfficialDetails(string id)
        //public async Task<IActionResult> EmployeeList()
        //{

        //    _logger.LogInformation($"Employee List Requested");

        //    try
        //    {
        //        var empList = await _employeePageService.GetEmployeeList();
        //        List<EmployeeListViewModel> employeeList = new List<EmployeeListViewModel>();
        //        foreach (var emp in empList)
        //        {
        //            EmployeeListViewModel list = new EmployeeListViewModel();
        //            list.Id = emp.Id;
        //            list.FirstName = emp.FirstName;
        //            list.MiddleName = emp.MiddleName;
        //            list.LastName = emp.LastName;
        //            list.Department = emp.Department;
        //            list.JobTitle = emp.JobTitle;
        //            list.Status = emp.Status;
        //            list.Manager = emp.Manager;
        //            list.Email = emp.Email;
        //            list.EmployeePersonalDetails = emp.EmployeePersonalDetails;

        //            employeeList.Add(list);
        //        }
        //        return View(employeeList);
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        throw; 
        //    }
        //    //var model = new EditEmployeeOfficialDetailsAdminViewModel
        //    //{
        //    //    Title = "Blah Blah",
        //    //    Id = "a9067a75-b6ee-4c23-a3e2-f19648957347"
        //    //};

        //    //var mapped = _mapper.Map<ApplicationUserViewModel>(model);

        //    //await _employeePageService.Update(mapped);

        //    //return null;
        //}

        //[AcceptVerbs("Get", "Post")]
        //[AllowAnonymous]
        //public async Task<IActionResult> IsEmailInUse(string email)
        //{
        //    var userEmail = await _employeePageService.FindEmail(email);

        //    if (userEmail == null)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json($"Email '{email}' is already in use.");
        //    }

        //}


        //[HttpGet]
        //public async Task<IActionResult> CreateEmployee()
        //{
        //    var dList = await _departmentPageService.GetDepartmentList();

        //    var deptList = dList.Select(dept => new SelectListItem()
        //    {
        //        Text = dept.Name,
        //        Value = dept.Id.ToString()
        //    }).ToList();

        //    deptList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });

        //    CreateEmployeeViewModel model = new CreateEmployeeViewModel();
        //    model.departmentList = deptList;
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUserViewModel user = new ApplicationUserViewModel();
        //        user.Id = Guid.NewGuid().ToString();

        //        if (user.Status == "Full-Time")
        //        {
        //            user.DaysWorkedInWeek = 5;
        //            user.NumberOfHoursWorkedPerDay = 7.6;
        //        }
        //        else
        //        {
        //            user.DaysWorkedInWeek = model.DaysWorkedInWeek;
        //            user.NumberOfHoursWorkedPerDay = model.NumberOfHoursWorkedPerDay;
        //        }

        //        user.Title = model.Title;
        //        user.FirstName = model.FirstName;
        //        user.MiddleName = model.MiddleName;
        //        user.LastName = model.LastName;
        //        user.Email = model.Email;
        //        user.UserName = model.UserName;
        //        user.Password = model.Password;
        //        user.ConfirmPassword = model.ConfirmPassword;
        //        user.JoiningDate = model.JoiningDate;
        //        user.JobTitle = model.JobTitle;
        //        user.Status = model.Status;
        //        user.DepartmentId = model.DepartmentId;
        //        user.Manager = model.Manager;

        //        var result = await _employeePageService.CreateEmployee(user);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("EmployeeList", "Employee");
        //        }
        //        foreach (var errors in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, errors.Description);
        //        }
        //    }
        //    return View(model);
        //}

        //public async Task<IActionResult>EmployeeOfficialDetails(string id)
        //{

        //    try
        //    {
        //        var employeeDetails = await _employeePageService.GetEmployeeById(id);
        //        if (employeeDetails != null)
        //        {
        //            return View(employeeDetails);
        //        }
        //        else
        //        {
        //            //Response.StatusCode = 404;
        //            _logger.LogError($"Employee not found {id}");
        //            return View("EmployeeNotFound", id);
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        throw;
        //    }


        //}

        //[HttpGet]
        //public async Task<IActionResult>EditEmployeeOfficialDetailsAdmin(string id)
        //{
        //    var empDetails = await _employeePageService.GetEmployeeById(id);
        //    var employeeDetails = _mapper.Map<EditEmployeeOfficialDetailsAdminViewModel>(empDetails);
        //    var dList = await _departmentPageService.GetDepartmentList();

        //    var deptList = dList.Select(dept => new SelectListItem()
        //    {
        //        Text = dept.Name,
        //        Value = dept.Id.ToString()
        //    }).ToList();

        //    deptList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });
        //    employeeDetails.departmentList = deptList;

        //    return View(employeeDetails);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditEmployeeOfficialDetailsAdmin(EditEmployeeOfficialDetailsAdminViewModel model)
        //{ 
        //    if(ModelState.IsValid)
        //    {
        //        var mapped = _mapper.Map<ApplicationUserViewModel>(model);
        //        await  _employeePageService.Update(mapped);
        //        return RedirectToAction("EmployeeOfficialDetails", new { id = mapped.Id });
        //    }
        //    return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> EditEmployeeOfficialDetails(string id)
        //{
        //    var empDetails =  await _employeePageService.GetEmployeeById(id);
        //    var employeeDetails = _mapper.Map<EditEmployeeOfficialDetailsViewModel>(empDetails);
        //    return View(employeeDetails);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditEmployeeOfficialDetails(EditEmployeeOfficialDetailsViewModel model)
        //{
        //    if(ModelState.IsValid)
        //    {
        //       var mapped =  _mapper.Map<ApplicationUserViewModel>(model);
        //        await _employeePageService.Update(mapped);
        //        return RedirectToAction("EmployeeOfficialDetails", new { id = mapped.Id });
        //    }
        //    return View();

        //}


        //[HttpGet]
        //public async Task<IActionResult> CreateEmployeePersonalDetails(string id)
        //{
        //    var user = await _employeePageService.GetEmployeeById(id);
        //    CreateEmployeePersonalDetailsViewModel model = new CreateEmployeePersonalDetailsViewModel();
        //    model.FullName = user.FullName;
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateEmployeePersonalDetails(CreateEmployeePersonalDetailsViewModel model)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        PhotoUploadViewModel photoUploadViewModel = new PhotoUploadViewModel();
        //        photoUploadViewModel.Photo = model.Photo;
        //        var uploadedImage = _uploadImageHelper.UploadImage(photoUploadViewModel);
        //        model.PhotoPath = uploadedImage.uniqueFileName;

        //        //mapping
        //        var empDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
        //        var employeePersonalDetails = await _employeePersonalDetailsPageService.AddAsync(empDetails);
        //        return RedirectToAction("EmployeePersonalDetails", new { id = empDetails.Id });
        //    }
        //    else
        //    {
        //        return View();
        //    }  
        //}

        //public async Task<IActionResult> EmployeePersonalDetails(string id)
        //{
        //    var emp = await _employeePageService.GetEmployeeById(id);
        //    if(emp.EmployeePersonalDetails == null)
        //    {
        //        return RedirectToAction("CreateEmployeePersonalDetails", new { id = emp.Id });
        //    }
        //    else
        //    {
        //        var employee = await _employeePersonalDetailsPageService.GetEmployeePersonalDetailsById(id);
        //        employee.FullName = emp.FullName;
        //        employee.PhotoPath = emp.EmployeePersonalDetails.PhotoPath;
        //        return View(employee);
        //    }

        //}

        //[HttpGet]
        //public async Task<IActionResult> EditEmployeePersonalDetails(string id)
        //{
        //    var emp = await _employeePageService.GetEmployeeById(id);
        //    var empDetails = await _employeePersonalDetailsPageService.GetEmployeePersonalDetailsById(id);

        //        var employeePersonalDetails = _mapper.Map<EditEmployeePersonalDetailsViewModel>(empDetails);
        //        employeePersonalDetails.FullName = emp.FullName;
        //        employeePersonalDetails.ExistingPhotoPath = empDetails.PhotoPath;
        //        return View(employeePersonalDetails);


        //}

        //[HttpPost]
        //public async Task<IActionResult> EditEmployeePersonalDetails(EditEmployeePersonalDetailsViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //mapping
        //        var empDetails = _mapper.Map<EmployeePersonalDetailsViewModel>(model);
        //        //upload image
        //        //var uniqueFileName = "";
        //        if (model.ExistingPhotoPath == null || empDetails.PhotoPath != model.ExistingPhotoPath)
        //        {
        //            PhotoUploadViewModel photoUploadViewModel = new PhotoUploadViewModel();
        //            photoUploadViewModel.Photo = model.Photo;
        //            var uploadedImage = _uploadImageHelper.UploadImage(photoUploadViewModel);
        //            empDetails.PhotoPath = uploadedImage.uniqueFileName;
        //        }
        //        else
        //        {
        //            empDetails.PhotoPath = model.ExistingPhotoPath;
        //        }
        //        var employeePersonalDetails = await _employeePersonalDetailsPageService.UpdateAsync(empDetails);
        //        return RedirectToAction("EmployeePersonalDetails" , new { id = empDetails.Id });
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

    }
}
