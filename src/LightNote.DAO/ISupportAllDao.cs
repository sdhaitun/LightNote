using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightNote.DAO
{
    public interface ISupportAllDao<TEntity, TId> : IBaseDao<TEntity, TId>, ISupportDeleteDao<TEntity>, ISupportSaveDao<TEntity, TId> where TEntity : class
    {

    }
}

