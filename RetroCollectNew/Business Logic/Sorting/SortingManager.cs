using DataAccess.WorkUnits;
using ModelData;
using ApplicationLayer.Models.Request;
using ApplicationLayer.Enumerations;
using ApplicationLayer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using ApplicationLayer.Extensions.EnumExtensions;
using ApplicationLayer.Business_Logic.Sorting.OrderByLogic;

namespace ApplicationLayer.Business_Logic.Sorting
{
    public class SortingManager : ISortingManager
    {
        private readonly IUnitOFWork _unitOFWork;

        public SortingManager(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;            
        }

        /// <summary>
        /// Return a sorted list fromt the DB based on client options
        /// </summary>
        /// <param name="gameListRequestModel"></param>
        /// <returns></returns>
        public IEnumerable<GameListModel> GetFilteredResults(GameListRequest gameListRequestModel )
        {
            IOrderable orderInstance = SelectOrderInstance(gameListRequestModel.SortingOptions);          

            return _unitOFWork.GameRepo.Get(
                 orderInstance.GetOrder(gameListRequestModel.Switchsort),
                 GetSearchExpression(gameListRequestModel.SearchText), 
                 GetFormatExpression(gameListRequestModel.Format));
        }


        //TODO: Possibly Move into seperate classes
        private Expression<Func<GameListModel, bool>> GetSearchExpression(string searchText)
        {
            Expression<Func<GameListModel, bool>> searchExpression = (x => x.Name.Contains(searchText) || x.Developer.Contains(searchText));
          
            return string.IsNullOrEmpty(searchText) ? null : searchExpression;
        }


        private Expression<Func<GameListModel, bool>> GetFormatExpression(string format)
        {
            Expression<Func<GameListModel, bool>> formatExpression = (x => x.Format == format);
            
            return string.IsNullOrEmpty(format) ? null : formatExpression;
        }


        private IOrderable SelectOrderInstance(string sortOption)
        {   
            if (sortOption == GameListColumnNames.Developer.GetDescription())
            {
                return new OrderByDeveloper();
            }

            if (sortOption == GameListColumnNames.Genre.GetDescription())
            {
                return new OrderByGenre();
            }

            if (sortOption == GameListColumnNames.Publisher.GetDescription())
            {
                return new OrderByPublisher();
            }

            //Set Name as default sort option
            return new OrderByName();
        }
    }  

}
