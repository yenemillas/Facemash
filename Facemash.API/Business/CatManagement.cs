using System;
using System.Collections.Generic;
using System.Linq;
using Facemash.API.DTO;
using Microsoft.Extensions.Options;

namespace Facemash.API.Business
{
    public class CatManagement : ICatManagement
    {
        private readonly IOptions<List<Cat>> _options;

        static Random rnd = new Random();

        public CatManagement(IOptions<List<Cat>> options)
        {
            _options = options;
        }

        public CatManagement()
        {

        }

        public IEnumerable<Cat> GetResult()
        {
            return _options.Value.OrderByDescending(x => x.Ranking);
        }


        /// <summary>
        /// Evité un match déjà joué
        /// Favoriser les chats avec peu de rencontre
        /// </summary>
        /// <returns></returns>
        public Match GetMatch()
        {

            int minHistory = _options.Value.OrderBy(x => x.History.Count()).First().History.Count();
            List<Cat> selectedCat = _options.Value.Where(x => x.History.Count == minHistory).ToList();

            int r1 = rnd.Next(selectedCat.Count);
            Cat cat1 = selectedCat[r1];

            //On retire le chat précedement choisit
            //La prise en coompte de l'historique peut etre interessant pour exclure des match déjà fait mais pour l'exercice je ne vais pas limiter le nombre de match 
            int minHistory2 = _options.Value.Where(x => x.Id != cat1.Id).OrderBy(x => x.History.Count()).First().History.Count();
            List<Cat> selectedCat2 = _options.Value.Where(x => x.History.Count == minHistory2 && x.Id != cat1.Id).ToList();

            int r2 = rnd.Next(selectedCat2.Count);
            Cat cat2 = selectedCat2[r2];

            Match match = new Match();
            match.Cat_1 = cat1;
            match.Cat_2 = cat2;

            return match;
        }

        /// <summary>
        ///La victoire rapport de base 10 points
        ///pour chaque tranche de 10 point de cote un bonus/malus de 1 sera appliquer
        ///pour eviter que deux chats avec une difference de cote trop importante rapporte/enleve trop de point
        ///une vicoire rapport 1 point minimum et au maximum 20 points
        /// </summary>
        /// <param name="match"></param>
        public void PostMatch(Match match)
        {
            int point = 10;
            int step = 10;

            var cat1 = _options.Value.First(x => x.Id == match.Cat_1.Id);
            var cat2 = _options.Value.First(x => x.Id == match.Cat_2.Id);

            var winner = match.Result ? cat1 : cat2;
            var losing = match.Result ? cat2 : cat1;

            if (winner.Ranking > losing.Ranking)
            {
                int balancepoint = (int)(point - Math.Truncate((double)(winner.Ranking - losing.Ranking) / step));
                if (balancepoint == 0) balancepoint = 1;
                winner.Ranking += balancepoint;
                losing.Ranking -= balancepoint;

            }
            else if (winner.Ranking < losing.Ranking)
            {
                int balancepoint = (int)(point + Math.Truncate((double)(losing.Ranking - winner.Ranking) / step));
                if (balancepoint >= 20) balancepoint = 20;
                winner.Ranking += balancepoint;
                losing.Ranking -= balancepoint;
            }
            else
            {
                winner.Ranking += point;
                losing.Ranking -= point;
            }

            winner.History.Add(losing.Id);
            losing.History.Add(winner.Id);

        }
    }
}
