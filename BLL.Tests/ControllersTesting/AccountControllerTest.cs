using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Controllers;
using WebApi.Models;
using DAL.Interfaces;
using Moq;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore;


namespace BLL.Tests.ControllersTesting
{
    public class AccountControllerTest
    {
        [Fact]
        public void IndexReturnAViewResultWithAListOFUsers()
        {
            var mock = new Mock<IUnitOfWork>();
        }
    }
}
