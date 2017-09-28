﻿using AWSNet.Model;
using AWSNet.Repositories.Core;
using System;
using System.Threading.Tasks;

namespace AWSNet.Repositories.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public ModelContext Context { get; private set; }

        public UnitOfWork()
        {
            this.Context = new ModelContext();
        }

        #region IDisposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing && this.Context != null)
                {
                    this.Context.Dispose();
                    this.Context = null;
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
