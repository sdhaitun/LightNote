//============================================================
//
//    Copyright (C) 2011 翟士丹@曲阜师范大学VolcanoSoft火山软件 版权所有
//    文件名　：SupportDeleteDao.cs
//    功能描述：带有删除功能的Dao，实现了ISupportDeleteDao和IBaseDao
//    创建标识：JasonDan 2011/09/16
//    文件版本：1.0.0.0
//
//============================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightNote.DAO;
using Castle.ActiveRecord;

namespace LightNote.DAO
{
    /// <summary>
    /// 带有删除功能的Dao，实现了ISupportDeleteDao和IBaseDao
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <author>JasonDan</author>
    public class SupportDeleteDao<TEntity, TId> : BaseDao<TEntity, TId>, ISupportDeleteDao<TEntity> where TEntity : class
    {
        public bool Delete(TEntity entity)
        {   
            if (entity == null)
            {
                return false;
            }
            (entity as ActiveRecordBase<TEntity>).DeleteAndFlush();           
            return true;
        }

        public int DeleteAll<TId>(TId[] pkValues)
        {
            return ActiveRecordBase<TEntity>.DeleteAll(pkValues);
        }
    }
}
