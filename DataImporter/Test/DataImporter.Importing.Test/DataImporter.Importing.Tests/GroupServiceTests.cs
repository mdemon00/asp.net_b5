using Autofac.Extras.Moq;
using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Repositories;
using DataImporter.Importing.Services;
using DataImporter.Importing.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq.Expressions;
using System.Security.Claims;

namespace DataImporter.Importing.Tests
{
    public class GroupServiceTests
    {
        private AutoMock _mock;
        private Mock<IImportingUnitOfWork> _importingUnitOfWork;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IMapper> _mapperMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private IGroupService _groupService;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void ClassCleanup()
        {
            _mock?.Dispose();
        }

        [SetUp]
        public void TestSetup()
        {
            _importingUnitOfWork = _mock.Mock<IImportingUnitOfWork>();
            _groupRepository = _mock.Mock<IGroupRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _httpContextAccessorMock = _mock.Mock<IHttpContextAccessor>();
            _groupService = _mock.Create<GroupService>();
        }

        [TearDown]
        public void TestCleanup()
        {
            _importingUnitOfWork.Reset();
            _groupRepository.Reset();
            _mapperMock.Reset();
        }

        [Test]
        public void GetGroup_GroupExists_ReturnGroup()
        {
            // Arrange
            var Id = 1; 

            var group = new Group { Id = 1, Name = "Contacts" };

            var groupEntity = new Entities.Group { Id = 1, Name = "Contacts" };

            _importingUnitOfWork.Setup(x => x.Groups)
            .Returns(_groupRepository.Object);

            _groupRepository.Setup(x => x.GetById(Id))
                .Returns(groupEntity);

            _mapperMock.Setup(x => x.Map<Group>(groupEntity)).Returns(group);         

            // Act
            var result = _groupService.GetGroup(Id, true);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _importingUnitOfWork.VerifyAll(),
                () => _groupRepository.VerifyAll(),
                () => result.ShouldBe(group)
            );

        }

        [Test]
        public void GetGroup_GroupExists_ReturnGroupFromWorkerService()
        {
            // Arrange
            var Id = 1;

            var group = new Group { Id = 1, Name = "Contacts" };

            var groupEntity = new Entities.Group { Id = 1, Name = "Contacts" };


            _importingUnitOfWork.Setup(x => x.Groups)
            .Returns(_groupRepository.Object);

            _groupRepository.Setup(x => x.GetById(Id))
                .Returns(groupEntity);

            _groupRepository.Setup(x => x.GetCount(It.IsAny<Expression<Func<Entities.Group, bool>>>()))
                .Returns(1);

            _mapperMock.Setup(x => x.Map<Group>(groupEntity)).Returns(group);

            var fakeGuid = "FA8E36E2-D9A4-4F41-CD41-08D98C6D37B8";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, fakeGuid),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var context = new DefaultHttpContext();
            context.User = user;

            
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);

            _httpContextAccessorMock.Setup(x => x.HttpContext.User)
                .Returns(user);

            // Act
            var result = _groupService.GetGroup(Id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _importingUnitOfWork.VerifyAll(),
                () => _groupRepository.VerifyAll(),
                () => result.ShouldBe(group)
            );
        }
    }
}