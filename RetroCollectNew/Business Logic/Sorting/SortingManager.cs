using DataAccess.WorkUnits;
using ModelData;
using ModelData.Request;
using Common.Enumerations;
using ApplicationLayer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using ApplicationLayer.Extensions.EnumExtensions;
using ApplicationLayer.Business_Logic.Sorting.OrderByLogic;
using HttpAccess;
using System.Threading.Tasks;

namespace ApplicationLayer.Business_Logic.Sorting
{
    public class SortingManager : ISortingManager
    {

        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IHttpManager _httpManager;

        public SortingManager(IHttpManager httpManager)
        {
            _httpManager = httpManager;
        }

        /// <summary>
        /// Return a sorted list fromt the DB based on client options
        /// </summary>
        /// <param name="gameListRequestModel"></param>
        /// <returns></returns>
        public IEnumerable<GameListModel> GetFilteredResults(GameListRequest gameListRequestModel)
        {
            try
            {
                return _httpManager.GetSortedResults(gameListRequestModel).Result;

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error retriving results from http manager");

            }
             
            return null;
        }


    //    //TODO: Possibly Move into seperate classes
    //    private Expression<Func<GameListModel, bool>> GetSearchExpression(string searchText)
    //    {
    //        Expression<Func<GameListModel, bool>> searchExpression = null;

    //        try
    //        {
    //            searchExpression = (x => x.Cover.Contains(searchText) || x.Developer.Contains(searchText));
    //        }
    //        catch (Exception e)
    //        {
    //            _logger.Error(e,"Could not create search expression");
    //        }
    //        return string.IsNullOrEmpty(searchText) ? null : searchExpression;
    //    }


    //    private Expression<Func<GameListModel, bool>> GetFormatExpression(string format)
    //    {
    //        Expression<Func<GameListModel, bool>> formatExpression = null;
    //        try
    //        {
    //            formatExpression = (x => x.Platform == format);
    //        }
    //        catch (Exception e)
    //        {
    //            _logger.Error(e, "Error creating format Expression");
    //        }
    //        return string.IsNullOrEmpty(format) ? null : formatExpression;
    //    }


    //    private IOrderable SelectOrderInstance(string sortOption)
    //    {   
    //        if (sortOption == GameListColumnNames.Developer.GetDescription())
    //        {
    //            return new OrderByDeveloper();
    //        }

    //        if (sortOption == GameListColumnNames.FirstReleaseDate.GetDescription())
    //        {
    //            return new OrderByGenre();
    //        }

    //        if (sortOption == GameListColumnNames.Publisher.GetDescription())
    //        {
    //            return new OrderByPublisher();
    //        }

    //        //Set Cover as default sort option
    //        return new OrderByName();
    //    }
    }  

}
