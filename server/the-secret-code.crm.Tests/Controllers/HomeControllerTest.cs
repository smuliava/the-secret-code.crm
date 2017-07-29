using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using the_secret_code.crm;
using the_secret_code.crm.Controllers;

namespace the_secret_code.crm.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
