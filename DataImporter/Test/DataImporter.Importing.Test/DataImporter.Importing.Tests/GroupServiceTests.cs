using Autofac.Extras.Moq;
using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Exceptions;
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
        public void CreateGroup_ValidGroup_CreteGroup()
        {
            // Arrange
            var group = new Group()
            {
                Name = "Books",
                ApplicationUserId = new Guid("FA8E36E2-D9A4-4F41-CD41-08D98C6D37B8")
            };

            _importingUnitOfWork.Setup(x => x.Groups)
                .Returns(_groupRepository.Object);

            _groupRepository.Setup(x => x.GetById(0))
                .Returns(new Entities.Group());

            _groupRepository.Setup(x => x.GetCount(It.IsAny<Expression<Func<Entities.Group, bool>>>()))
                .Returns(1);

            _mapperMock.Setup(x => x.Map<Group>(new Entities.Group())).Returns(group);

            _importingUnitOfWork.Setup(x => x.Groups.Add(
                It.IsAny<Entities.Group>()
                ))
                .Verifiable();

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
            _groupService.CreateGroup(group);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _importingUnitOfWork.Verify()
            );
        }

        [Test]
        public void CreateGroup_NullGroup_InvalidParameterException()
        {
            // Arrange 
            Group group = null;

            // Act 
            try
            {
                _groupService.CreateGroup(group);
            }
            catch { }

            // Assert 
            var exception = Should.Throw<InvalidParameterException>(() => _groupService.CreateGroup(group), "Group was not provided");
        }

        [Test]
        public void CreateGroup_DuplicateName_DuplicateNameException()
        {
            // Arrange 
            var group = new Group()
            {
                Name = "Students"
            };

            _importingUnitOfWork.Setup(x => x.Groups)
                .Returns(_groupRepository.Object);

            _groupRepository.Setup(x => x.GetById(0))
                .Returns(new Entities.Group());

            _groupRepository.Setup(x => x.GetCount(It.IsAny<Expression<Func<Entities.Group, bool>>>()))
                .Returns(1);

            // Act 
            try
            {
                _groupService.CreateGroup(group);
            }
            catch { }

            // Assert 
            var exception = Should.Throw<System.Data.DuplicateNameException>(() => _groupService.CreateGroup(group), "Group name already exists");
        }

        [Test]
        public void CreateGroup_InvalidApplicationId_InvalidParameterException()
        {
            // Arrange 
            var group = new Group()
            {
                Name = "Students",
            };

            _importingUnitOfWork.Setup(x => x.Groups)
                .Returns(_groupRepository.Object);

            _groupRepository.Setup(x => x.GetById(0))
                .Returns(new Entities.Group());

            _groupRepository.Setup(x => x.GetCount(It.IsAny<Expression<Func<Entities.Group, bool>>>()))
                .Returns(0);

            var fakeGuid = "FK8E36E2-D9A4-4F41-CD41-08D98C6D37B8";

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
            try
            {
                _groupService.CreateGroup(group);
            }
            catch { }

            // Assert 
            var exception = Should.Throw<InvalidParameterException>(() => _groupService.CreateGroup(group), "Something Went Wrong");
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

        [Test]
        public void GetGroup_GroupNotExists_ReturnNullFromWorkerService()
        {
            // Arrange 
            var Id = 0;
            Group group = null;
            Entities.Group groupEntity = null;

            _importingUnitOfWork.Setup(x => x.Groups)
                .Returns(_groupRepository.Object);

            _groupRepository.Setup(x => x.GetById(Id))
                .Returns(groupEntity);

            _groupRepository.Setup(x => x.GetCount(It.IsAny<Expression<Func<Entities.Group, bool>>>()))
                .Returns(1);

            _mapperMock.Setup(x => x.Map<Group>(groupEntity)).Returns(group);


            // Act 
            var result = _groupService.GetGroup(Id, true);

            //Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(group)
                );
        }

        [Test]
        public void GetGroup_GroupNotExists_ReturnNull()
        {
            // Arrange 
            var Id = 0;
            Group group = null;
            Entities.Group groupEntity = null;

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

            //Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldBe(group)
                );
        }

        [Test]
        public void GetGroup_InvalidApplicationId_InvalidParameterException()
        {
            // Arrange

            var fakeGuid = "foawejfgoa";

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
            try
            {
                var result = _groupService.GetGroup(0);

            }
            catch { }

            // Assert
            var exception = Should.Throw<InvalidParameterException>(() => _groupService.GetGroup(0), "Something Went Wrong");
        }

        [Test]
        public void GetGroup_NotGroupOwner_InvalidParameterException()
        {
            // Arrange

            _importingUnitOfWork.Setup(x => x.Groups)
            .Returns(_groupRepository.Object);

            _groupRepository.Setup(x => x.GetById(0))
                .Returns(new Entities.Group());

            _groupRepository.Setup(x => x.GetCount(It.IsAny<Expression<Func<Entities.Group, bool>>>()))
                .Returns(0);

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
            try
            {
                var result = _groupService.GetGroup(0);

            }
            catch { }

            // Assert
            var exception = Should.Throw<InvalidParameterException>(() => _groupService.GetGroup(0), "Unauthorized Access");
        }
    }
}