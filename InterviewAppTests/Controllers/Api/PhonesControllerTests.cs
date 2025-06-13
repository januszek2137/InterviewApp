using InterviewApp.Controllers.Api;
using InterviewApp.Data;
using InterviewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[TestClass]
public class PhonesControllerTests {
    private PhonesController _controller;
    private ApplicationDbContext _context;

    [TestInitialize]
    public void Init() {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);

        var phones = new List<PhoneModel>
        {
            new PhoneModel { Id = Guid.NewGuid(), Name = "OnePlus", Number = 123456789 },
            new PhoneModel { Id = Guid.NewGuid(), Name = "Samsung", Number = 987654321 }
        };
        _context.PhoneModel.AddRange(phones);
        _context.SaveChanges();

        _controller = new PhonesController(_context);
    }

    [TestCleanup]
    public void Cleanup() {
        _context.Dispose();
    }

    [TestMethod]
    public void GetPhones_ReturnsAll() {
        var result = _controller.GetPhones() as OkObjectResult;
        Assert.IsNotNull(result);

        var list = (result.Value as IEnumerable<PhoneModel>)?.ToList();
        Assert.AreEqual(2, list.Count);
    }

    [TestMethod]
    public void GetPhone_ExistingId_ReturnsOk() {
        var existing = _context.PhoneModel.First();
        var result = _controller.GetPhone(existing.Id) as OkObjectResult;

        Assert.IsNotNull(result);
        var phone = result.Value as PhoneModel;
        Assert.AreEqual(existing.Id, phone.Id);
    }

    [TestMethod]
    public void GetPhone_NonExistingId_ReturnsNotFound() {
        var result = _controller.GetPhone(Guid.NewGuid());
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public void CreatePhone_Valid_ReturnsCreated() {
        var newPhone = new PhoneModel { Id = Guid.NewGuid(), Name = "Nokia", Number = 112233445 };
        var result = _controller.CreatePhone(newPhone) as CreatedAtActionResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(nameof(PhonesController.GetPhone), result.ActionName);
        Assert.AreEqual(3, _context.PhoneModel.Count());
    }

    [TestMethod]
    public void UpdatePhone_Valid_NoContentAndDataChanged() {
        var existing = _context.PhoneModel.First();
        var updated = new PhoneModel { Id = existing.Id, Name = "Xiaomi", Number = 111222333 };

        var result = _controller.UpdatePhone(existing.Id, updated);
        Assert.IsInstanceOfType(result, typeof(NoContentResult));

        var reloaded = _context.PhoneModel.Find(existing.Id);
        Assert.AreEqual("Xiaomi", reloaded.Name);
        Assert.AreEqual(111222333, reloaded.Number);
    }

    [TestMethod]
    public void UpdatePhone_NotFound_ReturnsNotFound() {
        var fake = new PhoneModel { Id = Guid.NewGuid(), Name = "A", Number = 123456789 };
        var result = _controller.UpdatePhone(fake.Id, fake);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public void DeletePhone_Existing_ReturnsNoContent() {
        var existing = _context.PhoneModel.First();
        var result = _controller.DeletePhone(existing.Id);
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        Assert.AreEqual(1, _context.PhoneModel.Count());
    }

    [TestMethod]
    public void DeletePhone_NotFound_ReturnsNotFound() {
        var result = _controller.DeletePhone(Guid.NewGuid());
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public void CreatePhone_NameTooLong_ReturnsBadRequest() {
        var phone = new PhoneModel {
            Id = Guid.NewGuid(),
            Name = new string('X', 16),
            Number = 123456789
        };
        _controller.ModelState.AddModelError("Name", "Max length is 15");

        var result = _controller.CreatePhone(phone);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void CreatePhone_NumberNotNineDigits_ReturnsBadRequest() {
        var phone = new PhoneModel {
            Id = Guid.NewGuid(),
            Name = "ValidName",
            Number = 12345
        };
        _controller.ModelState.AddModelError("Number", "Must be exactly 9 digits");

        var result = _controller.CreatePhone(phone);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void UpdatePhone_NameTooLong_ReturnsBadRequest() {
        var existing = _context.PhoneModel.First();
        var updated = new PhoneModel {
            Id = existing.Id,
            Name = new string('Y', 20),
            Number = existing.Number
        };
        _controller.ModelState.AddModelError("Name", "Max length is 15");

        var result = _controller.UpdatePhone(existing.Id, updated);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void UpdatePhone_NumberNotNineDigits_ReturnsBadRequest() {
        var existing = _context.PhoneModel.First();
        var updated = new PhoneModel {
            Id = existing.Id,
            Name = existing.Name,
            Number = 999
        };
        _controller.ModelState.AddModelError("Number", "Must be exactly 9 digits");

        var result = _controller.UpdatePhone(existing.Id, updated);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

}
