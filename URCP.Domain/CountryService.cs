using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Lookups;
using URCP.Core.SearchEntities;
using URCP.Domain.Interface;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;
using URCP.Resources;

namespace URCP.Domain
{
    public class CountryService : ICountryService
    {
        private readonly IGenericQueryRepository _queryRepository;

        public CountryService(IGenericQueryRepository queryRepository)
        {
            this._queryRepository = queryRepository;
        }

        public Country Single(int id)
        {
            var constraints = new QueryConstraints<Country>()
           .Where(c => c.Id == id);

            var result = _queryRepository.Single(constraints);

            if (result == null)
                throw new EntityNotFoundException("Country", "No country found");

            return result;
        }

        public IQueryResult<Country> FindBy(CountrySearchCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException("Criteria", String.Format(DomainMessages.Entity_Cannot_Be_Null, "CountrySearchCriteria"));

            var constraints = new QueryConstraints<Country>()
               .Page(criteria.PageNumber, criteria.PageSize)
               .Where(c => true);

            if (criteria.Id.HasValue)
                constraints.AndAlso(c => c.Id == criteria.Id);

            if (criteria.Ids != null && criteria.Ids.Count > 0)
                constraints.AndAlso(c => criteria.Ids.Contains(c.Id));

            if (!String.IsNullOrWhiteSpace(criteria.ArabicName))
                constraints.AndAlso(c => c.Name_Ar.Contains(criteria.ArabicName));

            if (!String.IsNullOrWhiteSpace(criteria.EnglishName))
                constraints.AndAlso(c => c.Name_En.Contains(criteria.EnglishName));

            if (!String.IsNullOrWhiteSpace(criteria.ArabicNationality))
                constraints.AndAlso(c => c.Nationality_Ar.Contains(criteria.ArabicNationality));

            if (!String.IsNullOrWhiteSpace(criteria.EnglishNationality))
                constraints.AndAlso(c => c.Nationality_En.Contains(criteria.EnglishNationality));

            if (criteria.ExcludeIds != null && criteria.ExcludeIds.Count > 0)
                constraints.AndAlso(c => !criteria.ExcludeIds.Contains(c.Id));

            if (String.IsNullOrEmpty(criteria.Sort))
                constraints.SortBy(c => c.Name_En);
            else if (criteria.SortDirection == WebGridSortOrder.Ascending)
                constraints.SortBy(criteria.Sort);
            else
                constraints.SortByDescending(criteria.Sort);

            return _queryRepository.Find(constraints);
        }

    }
}
