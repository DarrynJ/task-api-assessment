using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Tasks.Domain.Repositories;

namespace Tasks.Tests.Extensions
{
    public static class RepositoryExtensions
    {
        public static void OnAdd<TDomain>(this IRepository<TDomain> repository, Action<TDomain> addAction)
        {
            repository.When(x => x.Add(Arg.Any<TDomain>())).Do(ci => addAction(ci.Arg<TDomain>()));
        }
    }
}
