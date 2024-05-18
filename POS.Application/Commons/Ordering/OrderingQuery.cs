﻿using POS.Application.Commons.Bases.Request;
using System.Linq.Dynamic.Core;

namespace POS.Application.Commons.Ordering
{
    public class OrderingQuery : IOrderingQuery
    {
        public IQueryable<TDTO> Ordering<TDTO>(BasePaginationRequest request, IQueryable<TDTO> queryable, bool pagination = false) where TDTO : class
        {
            IQueryable<TDTO> queryDTO = request.Order == "desc" ? queryable.OrderBy($"{request.Sort} descending") : queryable.OrderBy($"{request.Sort} ascending");

            if (pagination) queryDTO = queryDTO.Paginate(request);

            return queryDTO;
        }
    }
}
