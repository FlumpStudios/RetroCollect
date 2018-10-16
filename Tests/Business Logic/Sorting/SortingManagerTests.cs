using ApplicationLayer.Business_Logic.Sorting;
using ApplicationLayer.Models.Request;
using DataAccess.WorkUnits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelData;
using Moq;
using System.Collections.Generic;

namespace Tests.Business.Logic.Sorting
{
[TestClass]
public class SortingManagerTests
{
    private MockRepository mockRepository;

    private Mock<IUnitOFWork> mockUnitOFWork;

    [TestInitialize]
    public void TestInitialize()
    {
        mockRepository = new MockRepository(MockBehavior.Default);

        mockUnitOFWork = mockRepository.Create<IUnitOFWork>();

            mockUnitOFWork.Setup(x => x.GameRepo.Get(null, null, null)).Returns(new List<GameListModel>());
    }

    [TestCleanup]
    public void TestCleanup()
    {
        mockRepository.VerifyAll();
    }

    private SortingManager CreateManager()
    {
        return new SortingManager(
            this.mockUnitOFWork.Object);
    }

    [TestMethod]
    public void GetFilteredResults_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var unitUnderTest = CreateManager();
        GameListRequest gameListRequestModel = new GameListRequest() { Format = "Jaguar",Page = 1,SearchText=null,ShowClientList = false,SortingOptions=null,Switchsort= false};

        // Act
        var result = unitUnderTest.GetFilteredResults(
            gameListRequestModel);

        // Assert
        Assert.Fail();
    }
}
}
