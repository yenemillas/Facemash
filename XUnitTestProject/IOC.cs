
using Facemash.API.Business;
using System;
using System.Collections.Generic;


namespace XUnitTestProject
{
    public static class IOC
    {

        private static Dictionary<Type, Func<object>> dico = new Dictionary<Type, Func<object>>
        {
                { typeof(ICatManagement), () => new CatManagement()}
        };

        public static TInterface Get<TInterface>() where TInterface : class
        {
            if (!dico.ContainsKey(typeof(TInterface)))
            {
                throw new ArgumentException("type inconnu");
            }

            return dico[typeof(TInterface)]() as TInterface;
        }
    }
}
