using ApplicationLayer.Business_Logic.Builders;
using ApplicationLayer.Business_Logic.Sorting;
using ApplicationLayer.Models.Request;
using DataAccess.WorkUnits;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;

namespace Tests.Business.Logicgic.Builders
{
	[TestClass]
public class GameListResponseBuilderTests
{
    private MockRepository mockRepository;

    private Mock<ISortingManager> mockSortingManager;
    private Mock<IConfiguration> mockConfiguration;
    private Mock<IUnitOFWork> mockUnitOFWork;

    [TestInitialize]
    public void TestInitialize()
    {
        this.mockRepository = new MockRepository(MockBehavior.Strict);
        this.mockSortingManager = this.mockRepository.Create<ISortingManager>();
        this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
        this.mockUnitOFWork = this.mockRepository.Create<IUnitOFWork>();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        this.mockRepository.VerifyAll();
    }

    private GameListResponseBuilder CreateGameListResponseBuilder()
    {
        return new GameListResponseBuilder(
            this.mockSortingManager.Object,
            this.mockConfiguration.Object,
            this.mockUnitOFWork.Object);
    }

    [TestMethod]
    public void GetResponse_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var unitUnderTest = CreateGameListResponseBuilder();
        GameListRequest gameListRequestModel = new GameListRequest();
        ClaimsPrincipal User = new ClaimsPrincipal();

        // Act
        var result = unitUnderTest.GetResponse(
            gameListRequestModel,
            User);

        // Assert
        Assert.Fail();
    }
}
}
