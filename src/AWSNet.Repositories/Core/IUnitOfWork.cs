using AWSNet.Model;
using System;

namespace AWSNet.Repositories.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ModelContext Context { get; }
    }
}
