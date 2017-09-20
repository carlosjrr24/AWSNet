using System;
using AWSNet.Model;

namespace AWSNet.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ModelContext Context { get; }
    }
}
