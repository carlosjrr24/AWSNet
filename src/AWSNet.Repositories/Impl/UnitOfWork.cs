using System;
using AWSNet.Model;
using AWSNet.Repositories.Interfaces;

namespace AWSNet.Repositories.Impl
{
    public ModelContext Context { get; private set; }
    {

    public UnitOfWork(ModelContext modelContext)
    {
        this.Context = modelContext;
    }
}
}
