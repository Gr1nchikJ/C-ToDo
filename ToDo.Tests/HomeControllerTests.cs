using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using ToDo.Captcha;
using ToDo.Controllers;
using ToDo.Data;
using ToDo.Models;
using Xunit;

namespace ToDo.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void PopulateForm_ShouldReturn_JsonResult()
        {
            // Arrange
            var mockRepo = new Mock<ITodoRepository>();
            var mockLog = new Mock<ILogger<HomeController>>();
            var mockCaptcha = new Mock<ICaptchaValidator>();
            mockRepo.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new TodoItem());
            var controller = new HomeController(mockLog.Object, mockRepo.Object,mockCaptcha.Object);

            // Act
            var result = controller.PopulateForm(34);

            // Assert
            var viewResult = Assert.IsType<JsonResult>(result);
            
        }
    }
}
