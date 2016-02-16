using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Mvc.Infrastructure;

namespace Web
{
    public class AssemblyProvider : IAssemblyProvider
    {

        private IAssemblyProvider _defaultAssemblyProvider;

        public AssemblyProvider(DefaultAssemblyProvider defaultAssemblyProvider)
        {
            _defaultAssemblyProvider = defaultAssemblyProvider;
        }
        public IEnumerable<Assembly> CandidateAssemblies
        {
            get
            {
                var assemblies =
                    _defaultAssemblyProvider.CandidateAssemblies.Union(Startup.AdditionalAssemblies).ToArray();

                return assemblies;

            }
        }
    }
}
