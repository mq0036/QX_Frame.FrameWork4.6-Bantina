using QX_Frame.Helper_DG.Bantina;
using System;

namespace QX_Frame.App.Base
{
    public interface IEntity<DataBaseEntity, TEntity> where DataBaseEntity : Bantina
    {
        Boolean Add<TEntity2>(TEntity2 entity) where TEntity2 : class;
        Boolean Update<TEntity2>(TEntity2 entity) where TEntity2 : class;
        Boolean Delete<TEntity2>(TEntity2 entity) where TEntity2:class;
    }
}
