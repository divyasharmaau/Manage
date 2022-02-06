using AutoMapper;
using Manage.Application.Models;
using Manage.Core.Entities;
using Manage.Web.Interface;
using Manage.Web.Models;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace Manage.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly IAdministrationPageService _administrationPageService;
        private readonly IEmployeePageService _employeePageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdministrationController(IAdministrationPageService administrationPageService,IEmployeePageService employeePageService 
            ,UserManager<ApplicationUser> userManager
            ,IMapper mapper )
        {
            _administrationPageService = administrationPageService;
            _employeePageService = employeePageService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                var mapped = _mapper.Map<ApplicationRoleViewModel>(model);
                var result = await _administrationPageService.CreateRoleAsync(mapped);
               
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return View(model);
        }

        public async Task<IActionResult> RolesList()
        {
            var roleList =  await _administrationPageService.GetRolesList();
            return View(roleList);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            var role =  await _administrationPageService.GetRoleById(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {id} could not be found";
                return View("NotFound");
            }
            var users = await _administrationPageService.GetUsersInRole(role.Name);
            
            EditRoleViewModel model = new EditRoleViewModel();
            model.Name = role.Name;
            model.Id = role.Id;
           
            foreach (var item in users)
            {
                model.Users.Add(item.UserName); 
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _administrationPageService.GetRoleById(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {model.Id} could not be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.Name;
                var result = await _administrationPageService.Update(role);
                if(!result.Succeeded)
                {
                    return RedirectToAction("RolesList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            ViewBag.roleId = id;

            var role = await _administrationPageService.GetRoleById(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            // var users = await _administrationPageService.GetUsersInRole(role.Name);
            var allUsers = await _employeePageService.GetEmployeeList();
            var model = new List<UserRoleViewModel>();
           
            foreach (var emp in allUsers)
            {
                UserRoleViewModel userRoleViewModel = new UserRoleViewModel
                {
                    UserId = emp.Id,
                    UserName = emp.UserName
                };
                
                if(await _administrationPageService.UserInRole(emp , role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model , string id)
        {
            var role = await _administrationPageService.GetRoleById(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var employee = await _employeePageService.GetEmployeeById(model[i].UserId);
                var user = await _administrationPageService.UserInRole(employee, role.Name);

                IdentityResult result = null;

                if(model[i].IsSelected && !user)
                {
                    result = await _administrationPageService.AddToRoleAsync(employee, role.Name);
                }
                else if(!model[i].IsSelected && user)
                {
                    result = await _administrationPageService.RemoveFromRoleAsync(employee, role.Name);
                }
                else
                {
                    continue;
                }

                if(result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = id });
                }
            }
            return RedirectToAction("EditRole", new { Id = id });
        }

        [HttpPost]
        [Authorize(Policy="DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _administrationPageService.GetRoleById(id);
            if(role==null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await _administrationPageService.DeleteRole(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("RolesList");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    return View("RolesList");
                }
                catch(DbUpdateException ex)
                {
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role." +
                        $" If you want to delete this role, please remove the users from the role and then try to delete";
                    return View("Error");
                }
              
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> UsersList(string sortOrder,string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortByName = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var userList =  await _administrationPageService.GetUsers();

            switch (sortOrder)
            {
                case "name_desc":
                    userList = userList.OrderByDescending(e => e.FullName);
                    break;
                default:
                    userList = userList.OrderBy(e => e.FullName);
                    break;

            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(userList.ToPagedList(pageNumber, pageSize));           
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRolesAndClaims(string id)
        {
            var user = await _administrationPageService.GetUserById(id);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            var userRoles = await _administrationPageService.GetUserRoles(id);

            //claims
            var userForClaims = await _userManager.FindByIdAsync(id);
            var userClaims = await _userManager.GetClaimsAsync(userForClaims);

            EditUserViewModel model = new EditUserViewModel();
            model.UserName = user.UserName;
            model.UserId = user.Id;
            model.Roles = userRoles.ToList();
            model.Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await _administrationPageService.GetUserById(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id:{userId} not found";
                return View("NotFound");
            }
            else
            {
                var model = new List<UserRolesViewModel>();
                var rolesList = await _administrationPageService.GetAllRoles();

                foreach (var role in rolesList)
                {
                    UserRolesViewModel userRolesViewModel = new UserRolesViewModel()
                    {
                        RoleName = role.Name,
                        RoleId = role.Id
                    };

                    if (await _administrationPageService.UserInRole(user, role.Name))
                    {
                        userRolesViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRolesViewModel.IsSelected = false;
                    }
                    model.Add(userRolesViewModel);
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await _administrationPageService.GetUserById(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var role = await _administrationPageService.GetRoleById(model[i].RoleId);
                var userHasRole = await _administrationPageService.UserInRole(user , role.Name);

                IdentityResult result = null;
                if (model[i].IsSelected && !userHasRole)
                {
                   result =   await _administrationPageService.AddToRoleAsync(user, role.Name);
                }
                else if(!model[i].IsSelected && userHasRole)
                {
                    result = await _administrationPageService.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditUserRolesAndClaims", new { Id = userId });
                }
            }
            return RedirectToAction("EditUserRolesAndClaims", new { Id = userId });

        }

        [HttpGet]
        public async Task<IActionResult>ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId); 
            if (user == null)
            {
                ViewBag.ErrorMessage($"The user with Id: {userId} not found");
                return View("NotFound");
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);
            var model = new UserClaimsViewModel
            {
                UserId = userId,

            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim()
                {
                    ClaimType = claim.Type
                };
                
                //check if the user has the claim 
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user,
               model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));
         
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            //return RedirectToAction("GetUsers","Administration");
            return RedirectToAction("EditUserRolesAndClaims", new { Id = model.UserId});

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
