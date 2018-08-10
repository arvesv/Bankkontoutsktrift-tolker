using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class TrumfVisa : IParser
    {
        private readonly string[] _content;

        public TrumfVisa(string[] content)
        {
            _content = content;
        }

        public IEnumerable<Trasaction> GeTrasactions()
        {
            return new List<Trasaction>();
        }

        public bool IsParseable
        {

            get
            {
                return _content.FirstOrDefault(l => l.Contains("Trumf Visa")) != null;
            }
        }
    }
}