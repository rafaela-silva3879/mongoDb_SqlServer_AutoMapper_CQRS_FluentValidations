using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DeveloperStore.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;

namespace DeveloperStore.Presentation.Controllers
{
    public class SaleController : Controller
    {

        private readonly ISaleAppService _saleAppService;


        public SaleController(IProductAppService productAppService,
             ISaleAppService saleAppService)
        {
            _saleAppService = saleAppService;
        }

        [HttpGet]
        public ActionResult CancelSale(string saleId)
        {
            var model = new CancelSaleViewModel();
            model.SaleId = saleId;
            model.Cancel = false;
            return View(model);
        }

        [HttpPost]
        public async Task <IActionResult> CancelSale(CancelSaleViewModel model)
        {
            var error = new ErrorViewModel();
            try
            {
                var query = await _saleAppService.GetByIdAsync(model.SaleId);
                var command = new SaleUpdateCommand();
                command.Id = query.Id;
                command.TotalSaleAmount = query.TotalSaleAmount;
                command.SaleDate = query.SaleDate;
                command.IsCancelled = true;

                if (DateTime.Now > command.SaleDate.AddDays(7))
                    throw new Exception("Your seven-day cancellation period has expired.");

                await _saleAppService.UpdateAsync(command);
                return Json("Purchase cancelled. The value of "+ command.TotalSaleAmount+" will be back to your account in three working days.");
            }
            catch (Exception ex)
            {
                error.errorStr = ex.Message;
                return Json(error);
            }
        }
    }
}
