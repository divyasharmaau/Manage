using AutoMapper;
using Manage.Application.Models;
using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IAdministrationPageService _administrationPageService;
        private readonly IMapper _mapper;

        public AdministrationController(IAdministrationPageService administrationPageService, IMapper mapper)
        {
            _administrationPageService = administrationPageService;
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
                //specify a unique role name to create new roles
                //IdentityRole<int> identityRole = new IdentityRole<int>
                //{
                //    Name = model.RoleName
                //};
                //IdentityResult result = await _roleManager.CreateAsync(identityRole);

                ////saves the role in the underlying AspNetRoles table
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    //adding errors to teh model state error
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
    }
}
