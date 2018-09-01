using ApplicationLayer.Business_Logic.Sorting;
using ApplicationLayer.Models.Request;
using DataAccess.WorkUnits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
        this.mockRepository = new MockRepository(MockBehavior.Strict);

        this.mockUnitOFWork = this.mockRepository.Create<IUnitOFWork>();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        this.mockRepository.VerifyAll();
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
            GameListRequest gameListRequestModel = new GameListRequest();

        // Act
        var result = unitUnderTest.GetFilteredResults(
            gameListRequestModel);

        // Assert
        Assert.Fail();
    }
}
}
