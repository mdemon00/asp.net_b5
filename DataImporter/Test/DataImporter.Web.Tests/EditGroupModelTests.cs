using Autofac.Extras.Moq;
using AutoMapper;
using DataImporter.Areas.Member.Models;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Services;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace DataImporter.Web.Tests
{
    [ExcludeFromCodeCoverage]
    public class EditGroupModelTests
    {
        private AutoMock _mock;
        private Mock<IMapper> _mapperMock;
        private Mock<IGroupService> _groupServiceMock;
        private EditGroupModel _model;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void ClassCleanUp()
        {
            _mock?.Dispose();
        }

        [SetUp]
        public void TestSetup()
        {
            _groupServiceMock = _mock.Mock<IGroupService>();
            _mapperMock = _mock.Mock<IMapper>();
            _model = _mock.Create<EditGroupModel>();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _groupServiceMock?.Reset();
            _mapperMock?.Reset();
        }

        [Test]
        public void LoadeModelData_GroupExists_LoadProperties()
        {
            // Arrange
            const int id = 5;

            var group = new Group
            {
                Id = 5,
                Name = "Students"
            };

            _groupServiceMock.Setup(x => x.GetGroup(id, false)).Returns(group);

            _mapperMock.Setup(x => x.Map(group, _model)).Verifiable();

            // Act
            _model.LoadModelData(id);

            // Assert
            //Assert.AreEqual(id, _model.Id);
            _groupServiceMock.VerifyAll();
            _mapperMock.VerifyAll();
        }
    }
}