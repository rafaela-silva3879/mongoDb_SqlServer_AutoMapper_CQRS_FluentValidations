using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Services;
using DeveloperStore.Application.Validation;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ISaleAppService _saleAppService;
        public UserController(IUserAppService userAppService,
                                 IConfiguration configuration,
                                 IEmailService emailService,
                                 ISaleAppService saleAppService )
        {
            _userAppService = userAppService;
            _configuration = configuration;
            _emailService = emailService;
            _saleAppService = saleAppService;
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccount(UserCreateViewModel model)
        {
            var error = new ErrorViewModel();

            try
            {
                var validationResult = new CreateUserModelValidation().Validate((model.Name, model.Email, model.Password));

                if (!validationResult.IsValid)
                {
                    throw new Exception(String.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage)));
                }


                var command = new UserCreateCommand
                {                    
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
                };

                await _userAppService.CreateAsync(command);

                ModelState.Clear();
                return Json("User registered successfully.");

            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpGet]
        public IActionResult PasswordForgotten()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordForgotten(PasswordForgottenViewModel model)
        {
            var error = new ErrorViewModel();
            try
            {
                var validationResult = new PasswordForgottenModelValidation().Validate((model.Email, model.ConfirmEmail));

                if (!validationResult.IsValid)
                {
                    throw new Exception(String.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                var command = new UserGetUserByEmailCommand
                {
                    Email = model.Email,
                };

                var query = await _userAppService.GetUserByEmailAsync(command);

                if (query == null)
                {
                    throw new Exception("User not found.");
                }

                query.PasswordResetToken = Guid.NewGuid().ToString();
                query.PasswordResetExpiresIn = DateTime.UtcNow.AddHours(1);

                var PreUpdatecommand = new UserPreUpdatePasswordCommand
                {
                    Id = query.Id,
                    PasswordResetToken = query.PasswordResetToken,
                    PasswordResetExpiresIn = query.PasswordResetExpiresIn.Value,
                };

                await _userAppService.UpdatePrePasswordAsync(PreUpdatecommand);

                string link = $"https://localhost:7211/User/ResetPassword?token={query.PasswordResetToken}";

                string emailBody = $@"
                    <p>Hello {query.Name},</p>
                    <p>You have requested a password reset from our store.</p>
                    <p>To register your new password, please click the link below:</p>
                    <p><a href='{link}'>Click here to reset your password</a></p>
                    <p>If you did not request a password reset, please ignore this email.</p>
                    <p>Sincerely,<br />Developer Store Team</p>";

                _emailService.Send(query.Email, "Password Redefinition", emailBody);

                return Json("Redefinition e-mail sent.");
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(String token)
        {
            var error = new ErrorViewModel();
            try
            {

                if (string.IsNullOrEmpty(token))
                    throw new Exception("Invalid token.");

                var command = new UserGetUserByTokenCommand
                {
                    Token = token,
                };

                var user = await _userAppService.GetUserByTokenAsync(command);

                if (user == null || user.PasswordResetExpiresIn < DateTime.UtcNow)
                    throw new Exception("Expired or invalid token.");

                ViewData["Token"] = user.PasswordResetToken;
                return View();
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var error = new ErrorViewModel();
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", String.Empty);

                if (String.IsNullOrEmpty(token))
                    throw new Exception("Invalid Token.");

                var command = new UserGetUserByTokenCommand
                {
                    Token = token,
                };

                var response = await _userAppService.GetUserByTokenAsync(command);

                if (response == null || response.PasswordResetExpiresIn < DateTime.UtcNow)
                    throw new Exception("Expired or Invalid Token.");

                var validationResult = new ResetPasswordModelValidation().Validate((model.NewPassword, model.ConfirmPassword));

                if (!validationResult.IsValid)
                {
                    throw new Exception(String.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                var comando = new UserUpdatePasswordCommand
                {
                    Id = response.Id,
                    NewPassword = model.NewPassword
                };

                await _userAppService.UpdatePasswordAsync(comando);

                return Json("Password reset successfully.");
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var error = new ErrorViewModel();

            try
            {
                var validationResult = new LoginValidation().Validate((model.Email, model.Password));

                if (!validationResult.IsValid)
                {
                    throw new Exception(String.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                var command = new UserValidateCredentialsCommand
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var query = await _userAppService.ValidateCredentialsAsync(command);

                if (query == null)
                {
                    throw new Exception("Invalid user or password.");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, query.Name),
                    new Claim(ClaimTypes.Email, query.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenString = tokenHandler.WriteToken(token);

                if (String.IsNullOrEmpty(tokenString))
                    throw new Exception("Acesso negado.");


                var sessionData = new WelcomeViewModel
                {
                    Name = query.Name,
                    UserProfile = query.UserProfile.ToString(),
                    Email = query.Email,
                    Id = query.Id,
                    LoginToken = tokenString
                };

                HttpContext.Session.SetString("Welcome", JsonConvert.SerializeObject(sessionData));

                // Return token to front end
                var response = new
                {
                    Message = "Welcome, " + sessionData.Name + "!",
                    Token = tokenString
                };

                return Json(response);

            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpGet]
        public ActionResult Users()
        {
            var erro = new ErrorViewModel();
            try
            {
                var welcomeJson = HttpContext.Session.GetString("Welcome");

                if (string.IsNullOrEmpty(welcomeJson))
                {
                    return RedirectToAction("Login", "User"); // Redireciona caso o modelo não esteja presente
                }

                var model = JsonConvert.DeserializeObject<WelcomeViewModel>(welcomeJson);

                return View(model);
            }
            catch (Exception ex)
            {
                erro.errorStr = ex.Message;
                return Json(erro);
            }
        }


        [HttpGet("/Product/GetAllUsersAsync")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] int _page = 1, [FromQuery] int _size = 3, [FromQuery] string _order = "name asc", [FromQuery] string _userId = "")
        {
            var error = new ErrorViewModel();
            try
            {
                var welcomeJson = HttpContext.Session.GetString("Welcome");

                if (string.IsNullOrEmpty(welcomeJson))
                {
                    return RedirectToAction("Login", "User"); // Redireciona caso o modelo não esteja presente
                }

                var list = await _userAppService.GetAllAsync();
                list = list.Where(u => u.IsDeleted == false).ToList();

                // Filtering by userId
                if (!string.IsNullOrEmpty(_userId))
                {
                    Guid userIdGuid;
                    bool converted = Guid.TryParse(_userId, out userIdGuid);

                    if (converted)
                        list = list.Where(u => u.Id == userIdGuid.ToString()).ToList();
                    else
                        throw new Exception("Id has an invalid format.");
                }

                // Ordering
                var orderParams = _order.Split(',');
                foreach (var orderParam in orderParams)
                {
                    var parts = orderParam.Split(' ');
                    var field = parts[0].Trim();
                    var direction = parts.Length > 1 ? parts[1].Trim() : "asc";

                    if (field == "name" && direction == "desc")
                        list = list.OrderByDescending(p => p.Name).ToList();
                    else if (field == "name" && direction == "asc")
                        list = list.OrderBy(p => p.Name).ToList();
                    else if (field == "userProfile" && direction == "desc")
                        list = list.OrderByDescending(p => p.UserProfile).ToList();
                    else if (field == "userProfile" && direction == "asc")
                        list = list.OrderBy(p => p.UserProfile).ToList();
                    else if (field == "email" && direction == "desc")
                        list = list.OrderByDescending(p => p.Email).ToList();
                    else if (field == "email" && direction == "asc")
                        list = list.OrderBy(p => p.Email).ToList();
                }

                // Pagination
                var totalRecords = list.Count;
                var totalPages = (int)Math.Ceiling((decimal)totalRecords / _size);

                var paginatedList = list.Skip((_page - 1) * _size).Take(_size).ToList();

                var modelList = paginatedList.Select(item => new GetAllUsersViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    UserProfile = item.UserProfile,
                }).ToList();

                var pagination = new
                {
                    CurrentPage = _page,
                    TotalPages = totalPages,
                    HasNextPage = _page < totalPages,
                    HasPreviousPage = _page > 1
                };

                return Ok(new { users = modelList, Pagination = pagination });
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] UserDeleteModel model)
        {
            var error = new ErrorViewModel();
            try
            {
                var user = await _userAppService.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }
                //verify if there are sales associated
                var sales = await _saleAppService.GetAllAsync();
                bool hasSales = sales.Exists(s => s.UserId == model.UserId);

                if (hasSales)
                {
                    // Just update to deleted
                    var updateCommand = new UserUpdateCommand
                    {
                        Id=user.Id,
                        Email= user.Email,
                        IsDeleted=true,
                        Name=user.Name,
                        Password=user.Password,
                        UserProfile = user.UserProfile
                    };

                    await _userAppService.UpdateAsync(updateCommand);
                }
                else
                {            
                    var command = new UserDeleteCommand { Id = model.UserId };
                    await _userAppService.DeleteAsync(command);
                }

                return Json(new { success = true, redirectUrl = Url.Action("Users", "User") });
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }
        
        public async Task<ActionResult> Edit(string id)
        {
            var erro = new ErrorViewModel();
            try
            {
                var welcomeJson = HttpContext.Session.GetString("Welcome");

                if (string.IsNullOrEmpty(welcomeJson))
                {
                    return RedirectToAction("Login");
                }

                var command = await _userAppService.GetByIdAsync(id);

                var model = new EditUserViewModel
                {
                    Id = command.Id,
                    Email = command.Email,
                    UserProfile= command.UserProfile.ToString(),
                    Name = command.Name    
                };
                return View(model);
            }
            catch (Exception ex)
            {
                erro.errorStr = ex.Message;
                return Json(erro);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit([FromBody]EditUserViewModel model)
        {
            var erro = new ErrorViewModel();
            try
            {
                var welcomeJson = HttpContext.Session.GetString("Welcome");

                if (string.IsNullOrEmpty(welcomeJson))
                {
                    return RedirectToAction("Login");
                }

                // Validation
                var validationResult = new UserEditModelValidation().Validate((model.Name, model.Email, model.UserProfile));
                if (!validationResult.IsValid)
                {
                    throw new Exception(string.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage)));
                }
                var user =await _userAppService.GetByIdAsync(model.Id);

                var command = new UserUpdateCommand
                {
                    Id = model.Id,
                    Name = model.Name,
                    Email = model.Email,
                    UserProfile=Convert.ToInt32(model.UserProfile),
                    IsDeleted =user.IsDeleted,
                    Password=user.Password,
                };

                await _userAppService.UpdateAsync(command);

                return Json("User updated with success!");
            }
            catch (Exception ex)
            {
                erro.errorStr = ex.Message;
                return Json(erro);
            }
        }


    }
}
