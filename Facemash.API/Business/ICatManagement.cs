using Facemash.API.DTO;
using System.Collections.Generic;

namespace Facemash.API.Business
{
    public interface ICatManagement
    {
        IEnumerable<Cat> GetResult();

        Match GetMatch();

        void PostMatch(Match match);

    }
}