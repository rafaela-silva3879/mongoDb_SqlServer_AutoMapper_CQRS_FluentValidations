using DeveloperStore.Application.Validation;
using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Presentation.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using DeveloperStore.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using DeveloperStore.Application.Models.Queries;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NuGet.Packaging;

namespace DeveloperStore.Presentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IProductAppService _productAppService;
        private readonly IUserAppService _userAppService;
        private readonly ISaleItemAppService _saleItemAppService;
        private readonly ISaleAppService _saleAppService;
        private readonly IEmailService _emailService;

        public ProductController(IProductAppService productAppService,
            IUserAppService userAppService,
            IWebHostEnvironment hostingEnvironment,
            ISaleItemAppService saleItemAppService,
            ISaleAppService saleAppService,
            IEmailService emailService)
        {
            _productAppService = productAppService;
            _userAppService = userAppService;
            _hostingEnvironment = hostingEnvironment;
            _saleItemAppService = saleItemAppService;
            _saleAppService = saleAppService;
            _emailService = emailService;
        }

        [HttpGet]
        public ActionResult ProductRegistration()
        {
            var error = new ErrorViewModel();
            try
            {
                var welcomeJson = HttpContext.Session.GetString("Welcome");

                if (string.IsNullOrEmpty(welcomeJson))
                {
                    return RedirectToAction("Login");
                }

                var model = JsonConvert.DeserializeObject<WelcomeViewModel>(welcomeJson);

                return View(model);
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ProductRegistration(IFormCollection form)
        {
            var error = new ErrorViewModel();
            try
            {
                var userId = form["UserId"].ToString();
                var name = form["Name"].ToString();
                var unitPrice = form["UnitPrice"].ToString();
                var quantity = form["Quantity"].ToString();
                var photo = form.Files.FirstOrDefault();

                var validationResult = new ProductCreateModelValidation().Validate((name, unitPrice, quantity, photo));
                if (!validationResult.IsValid)
                {
                    throw new Exception(string.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

               if (photo == null)
                {
                    throw new Exception("A foto do produto é obrigatória.");
                }

                var welcomeJson = HttpContext.Session.GetString("Welcome");

                if (string.IsNullOrEmpty(welcomeJson))
                {
                    return RedirectToAction("Login"); // Redireciona caso o modelo não esteja presente
                }

                var welcomeModel = JsonConvert.DeserializeObject<WelcomeViewModel>(welcomeJson);

                var fileExtension = Path.GetExtension(photo.FileName).ToLower();
                if (!_allowedExtensions.Contains(fileExtension))
                {
                    throw new Exception("Somente arquivos JPG e JPEG são permitidos.");
                }

                var command = new ProductCreateCommand
                {
                    Name = name,
                    Photo = Guid.NewGuid().ToString().ToLower() + fileExtension, // Salvar com a extensão correta
                    UnitPrice = Convert.ToDecimal(unitPrice),
                    UserId = userId,
                    Quantity = quantity,
                };

                string contentRootPath = _hostingEnvironment.ContentRootPath;
                string tempPath = Path.Combine(contentRootPath, "Uploads", command.Photo);

                // Saving photo
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
                                
                await _productAppService.CreateAsync(command);

                return Json("Product " + name + " successfully registered!");
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] ProductDeleteModel model)
        {
            var error = new ErrorViewModel();
            try
            {
                var product = await _productAppService.GetByIdAsync(model.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found.");
                }

                var saleItems = await _saleItemAppService.GetAllAsync();
                bool hasSales = saleItems.Exists(s => s.ProductId == model.ProductId);

                if (hasSales)
                {
                   var updateCommand = new ProductUpdateCommand
                    {
                        Id = product.Id,
                        Name = product.Name,
                        UnitPrice = product.UnitPrice,
                        Quantity = product.Quantity,
                        UserId = product.UserId,
                        IsDeleted = true,
                        Photo = product.Photo
                    };

                    await _productAppService.UpdateAsync(updateCommand);
                }
                else
                {
                    if (!string.IsNullOrEmpty(product.Photo))
                    {
                        string contentRootPath = _hostingEnvironment.ContentRootPath;
                        string fotoPath = Path.Combine(contentRootPath, "Uploads", product.Photo);
                        if (System.IO.File.Exists(fotoPath))
                        {
                            System.IO.File.Delete(fotoPath);
                        }
                    }

                    var command = new ProductDeleteCommand { Id = model.ProductId };
                    await _productAppService.DeleteAsync(command);
                }

                return Json(new { success = true, redirectUrl = Url.Action("RegisteredProducts", "Product") });
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
                    return RedirectToAction("Login", "User");
                }

                var command = await _productAppService.GetByIdAsync(id);

                var model = new EditProductViewModel
                {
                    Id = command.Id,
                    UserId = command.UserId.ToString(),
                    Name = command.Name,
                    Photo = command.Photo,
                    UnitPrice = command.UnitPrice.ToString(),
                    Quantity = command.Quantity.ToString(),
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
        public async Task<ActionResult> Edit(EditProductViewModel model)
        {
            var erro = new ErrorViewModel();
            try
            {
                ValidatePhoto(model.PhotoJPGJPEG);
                var validationResult = new ProductEditModelValidation().Validate((model.Name, model.UnitPrice, model.Quantity, model.UserId));
                if (!validationResult.IsValid)
                {
                    throw new Exception(string.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage)));
                }
       
                var oldProduct = await _productAppService.GetByIdAsync(model.Id);
                if (oldProduct == null)
                {
                    erro.errorStr = "Product not found.";
                    return Json(erro);
                }

                string caminhoFotos = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");  // Caminho relativo para a pasta Uploads
                string caminhoFotoAntiga = Path.Combine(caminhoFotos, oldProduct.Photo.ToLower());

               Console.WriteLine($"Caminho da foto antiga: {caminhoFotoAntiga}");

               if (model.PhotoJPGJPEG != null && model.PhotoJPGJPEG.Length > 0)
                {
                    model.Photo = Guid.NewGuid().ToString().ToLower() + Path.GetExtension(model.PhotoJPGJPEG.FileName.ToLower());

                    if (!oldProduct.Photo.ToLower().Equals(model.Photo.ToLower()) && !string.IsNullOrEmpty(oldProduct.Photo))
                    {
                        if (System.IO.File.Exists(caminhoFotoAntiga))
                        {
                            try
                            {
                                Console.WriteLine($"Deletando a foto antiga: {caminhoFotoAntiga}");
                                System.IO.File.Delete(caminhoFotoAntiga);
                                Console.WriteLine("Foto antiga excluída com sucesso.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro ao excluir a foto antiga: {ex.Message}");
                                erro.errorStr = $"Erro ao excluir a foto antiga: {ex.Message}";
                                return Json(erro);
                            }
                        }
                        else
                        {
                            Console.WriteLine("A foto antiga não foi encontrada.");
                        }
                    }
                }
                else
                {
                    model.Photo = oldProduct.Photo;
                }

               var command = new ProductUpdateCommand
                {
                    UserId = model.UserId,
                    Name = model.Name,
                    UnitPrice = Convert.ToDecimal(model.UnitPrice),
                    Photo = model.Photo.ToLower(),
                    Id = model.Id,
                    Quantity = Convert.ToInt32(model.Quantity),
                };

                string photoPath = Path.Combine(caminhoFotos, command.Photo); // Caminho completo com nome da foto

                if (model.PhotoJPGJPEG != null && model.PhotoJPGJPEG.Length > 0)
                {
                    try
                    {
                       if (!Directory.Exists(caminhoFotos))
                        {
                            Directory.CreateDirectory(caminhoFotos);
                        }

                        using (var stream = new FileStream(photoPath, FileMode.Create))
                        {
                            await model.PhotoJPGJPEG.CopyToAsync(stream);  // Salva o arquivo na pasta
                        }

                        Console.WriteLine("Arquivo salvo com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao salvar o arquivo: {ex.Message}");
                        erro.errorStr = "Erro ao salvar a foto.";
                        return Json(erro); // Retorna o erro de foto
                    }
                }

                await _productAppService.UpdateAsync(command);

                return Json("Product updated with success!");
            }
            catch (Exception ex)
            {
                erro.errorStr = ex.Message;
                return Json(erro);
            }
        }


        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public bool ValidatePhoto(IFormFile photo)
        {
           
            try
            {
               if (photo == null)
                {
                    return true; 
                }

                string extension = Path.GetExtension(photo.FileName).ToLower();
                if (!_allowedExtensions.Contains(extension.ToLower()))
                {
                    throw new Exception("Only JPG and JPEG files are allowed.");

                }

               if (photo.Length > MaxFileSize)
                {
                    throw new Exception("File size must not exceed 5MB.");

                }

                return true; 
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public ActionResult RegisteredProducts()
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


        [HttpGet("/Product/GetAllProductsAsync")]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] int _page = 1, [FromQuery] int _size = 3, [FromQuery] string _order = "name asc")
        {
            var error = new ErrorViewModel();
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Token recebido: {token}");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "User");
                }

                var list = await _productAppService.GetAllAsync();
                list = list.Where(p => p.IsDeleted == false).ToList();
                //Ordering
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
                    else if (field == "price" && direction == "desc")
                        list = list.OrderByDescending(p => p.UnitPrice).ToList();
                    else if (field == "price" && direction == "asc")
                        list = list.OrderBy(p => p.UnitPrice).ToList();
                }

                // Pagination
                var totalRecords = list.Count;
                var totalPages = (int)Math.Ceiling((decimal)totalRecords / _size);

                var paginatedList = list.Skip((_page - 1) * _size).Take(_size).ToList();

                var modelList = paginatedList.Select(item => new GetAllProductsViewModel
                {
                    Name = item.Name,
                    UnitPrice = item.UnitPrice,
                    Id = item.Id,
                    Quantity = item.Quantity >= 20 ? 20 : item.Quantity,
                    PhotoPath = "/Uploads/" + item.Photo
                }).ToList();

                var pagination = new
                {
                    CurrentPage = _page,
                    TotalPages = totalPages,
                    HasNextPage = _page < totalPages,
                    HasPreviousPage = _page > 1
                };

                return Ok(new { Products = modelList, Pagination = pagination });
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> CalculateTotalAmount([FromBody] List<SubmitSelectionViewModel> selectedProducts)
        {
            var error = new ErrorViewModel();
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Token recebido: {token}");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "User");
                }

                foreach (var item in selectedProducts)
                {
                    var validationResult = new ProductSubmitSelectionViewModelValidation()
                        .Validate((item.UserId, item.ProductId, item.Quantity));

                    if (!validationResult.IsValid)
                    {
                        return BadRequest(new
                        {
                            success = false,
                            message = string.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage))
                        });
                    }
                }

                var saleItemsTotalVM = new SaleItemsTotalAmountViewModel
                {
                    SaleItemsList = new List<SaleItemsViewModel>(),
                    PurchaseTotalAmount = 0
                };

                foreach (var item in selectedProducts)
                {
                    var command = new SaleItemCalculateTotalItemAmountCommand
                    {
                        UserId = item.UserId,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId
                    };

                    var query = await _saleItemAppService.CalculateTotalItemAmmount(command);
                    var product = await _productAppService.GetByIdAsync(item.ProductId);

                    var saleItem = new SaleItemsViewModel
                    {
                        TotalItemAmount = query.TotalItemAmount,
                        Discount = query.Discount,
                        ProductName = product.Name,
                        Quantity = query.Quantity
                    };

                    saleItemsTotalVM.SaleItemsList.Add(saleItem);
                    saleItemsTotalVM.PurchaseTotalAmount += saleItem.TotalItemAmount;
                }

                return Json(new { success = true, response = saleItemsTotalVM });
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> SubmitSelection([FromBody] List<SubmitSelectionViewModel> productsList)
        {
            var error = new ErrorViewModel();
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Token recebido: {token}");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "User");
                }

                //validation
                foreach (var item in productsList)
                {
                    var validationResult = new ProductSubmitMakeSaleViewModelValidation().Validate((item.UserId, item.ProductId, item.Quantity));
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(new
                        {
                            success = false,
                            message = string.Join("<br>", validationResult.Errors.Select(e => e.ErrorMessage))
                        });
                    }
                }

                var command = new SaleItemSaleCompleteCommand();
                command.SaleItems = new List<SaleItemCommand>();
                var si = new SaleItemCommand();
                command.UserId = productsList[0].UserId;
                foreach (var item in productsList)
                {
                    si = new SaleItemCommand();
                    si.ProductId = item.ProductId;
                    si.Quantity = item.Quantity;
                    command.SaleItems.Add(si);
                }

                var query = await _saleAppService.MakeSale(command);

                await SendConfirmationEmail(query);

                return Json("An e-mail was sent to you to confirm your purchase.");

            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendConfirmationEmail(SaleQuery data)
        {
            var error = new ErrorViewModel();
            try
            {
                var user = await _userAppService.GetByIdAsync(data.UserId);

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                string link = $"https://localhost:7211/sale/CancelSale?saleId={data.Id}";

                 string saleItemsDescription = "";
                foreach (var item in data.SaleItems)
                {
                   saleItemsDescription += $@"
                <li>
                    <strong>{item.Product.Name}</strong><br />
                    Quantity: {item.Quantity}<br />
                    Unit Price: {item.Product.UnitPrice / item.Quantity:C}<br />
                    Discount: {item.Discount:C}<br />
                    Price after Discount: {item.TotalItemAmount:C}<br />
                </li>
                <br />";

                }

                string emailBody = $@"
            <p>Hello {user.Name},</p>
            <p>Here is the description of your purchase made on {data.SaleDate}:<p/>
            <p>You have 7 days to cancel your purchase by clicking on the link below:</p>
            <p><a href='{link}'>For cancellation, click here</a></p>
            <p><strong>Items Purchased:</strong></p>
            <ul>
                {saleItemsDescription}
            </ul>            
            <p><strong>Total Sale Amount: {data.TotalSaleAmount:C}</strong></p>
            <br /><br />
            <p>If you did not made a purchase, please ignore this email.</p>
            <p>Sincerely,<br />Developer Store Team</p>";

                _emailService.Send(user.Email, "Purchase confirmation", emailBody);


                return Json(new { success = true, redirectUrl = Url.Action("RegisteredProducts", "Product") });
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }

        }


    }

}
