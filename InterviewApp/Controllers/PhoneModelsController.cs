using InterviewApp.Models;
using InterviewApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewApp.Controllers {
    public class PhoneModelsController : Controller {
        private readonly PhonesService _phonesService;
        public PhoneModelsController(PhonesService phonesService) {
            _phonesService = phonesService;
        }

        // GET: PhoneModels
        public async Task<IActionResult> Index() {
            return View(await _phonesService.GetPhonesAsync());
        }

        // GET: PhoneModels/Details/5
        public async Task<IActionResult> Details(Guid? id) {
            if(id == null) {
                return NotFound();
            }

            var phoneModel = await _phonesService.GetPhoneAsync(id.Value);
            if(phoneModel == null) {
                return NotFound();
            }

            return View(phoneModel);
        }

        // GET: PhoneModels/Create
        public IActionResult Create() {
            return View();
        }

        // POST: PhoneModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Number")] PhoneModel phoneModel) {
            if(ModelState.IsValid) {
                phoneModel.Id = Guid.NewGuid();
                await _phonesService.CreatePhoneAsync(phoneModel);

                return RedirectToAction(nameof(Index));
            }
            return View(phoneModel);
        }

        // GET: PhoneModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id) {
            if(id == null) {
                return NotFound();
            }

            var phoneModel = await _phonesService.GetPhoneAsync(id.Value);
            if(phoneModel == null) {
                return NotFound();
            }
            return View(phoneModel);
        }

        // POST: PhoneModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Number")] PhoneModel phoneModel) {
            if(id != phoneModel.Id) {
                return NotFound();
            }

            if(ModelState.IsValid) {
                var updateResult = await _phonesService.UpdatePhoneAsync(id, phoneModel);
                if(!updateResult) {
                    if(await _phonesService.GetPhoneAsync(phoneModel.Id) == null) {
                        return NotFound();
                    }
                    return BadRequest("Failed to update the phone model.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(phoneModel);
        }

        // GET: PhoneModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id) {
            if(id == null) {
                return NotFound();
            }

            var phoneModel = await _phonesService.GetPhoneAsync(id.Value);
            if(phoneModel == null) {
                return NotFound();
            }

            return View(phoneModel);
        }

        // POST: PhoneModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id) {
            var phoneModel = await _phonesService.GetPhoneAsync(id);
            if(phoneModel != null) {
                await _phonesService.DeletePhoneAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
