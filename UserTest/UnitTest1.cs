using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using UserMgmt.Services;
using UserMgmt.Helpers;
using UserMgmt.Authorization;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace UserTest
{
    [TestClass]
    public class UnitTest1
    {
        //private readonly IService _service;

        //private DependencyResolverHelper _serviceProvider;

        //IService _myInterface;

        readonly IService _services =
        Program.CreateHostBuilder(new string[] { }).Build().Services; // one liner

        [TestInitialize]
        public void Setup()
        {
            _myInterface = new Service();
            _systemUnderTest = new MyController(_myInterface);
        }
        public UnitTest1()
        {
           var webHost = WebHost.CreateDefaultBuilder()
                    .user<Startup>()
                    .Build();
                _serviceProvider = new DependencyResolverHelper(webHost);
        }
        [TestMethod]
        public void TestMethod1()
        {
            string id = "fakeId";
            string secret = "fakeSecret";

            //Act
            var output = _service.GetToken(id, secret);

            //Assert
            Assert.IsNotNull(output.Token);
        }
    }


    public class DependencyResolverHelper
    {
        private readonly IWebHost _webHost;

        /// <inheritdoc />
        public DependencyResolverHelper(IWebHost webHost) => _webHost = webHost;

        public T GetService<T>()
        {
            var serviceScope = _webHost.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            try
            {
                var scopedService = services.GetRequiredService<T>();
                return scopedService;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}