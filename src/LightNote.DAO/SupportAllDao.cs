//============================================================
//
//    Copyright (C) 2011 翟士丹@曲阜师范大学VolcanoSoft火山软件 版权所有
//    文件名　：SupportAllDao.cs
//    功能描述：支持所有操作的Dao，实现了IBaseDao，ISupportDeleteDao和ISupportSaveDao
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
    /// 支持所有操作的Dao，实现了IBaseDao，ISupportDeleteDao和ISupportSaveDao
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <author>JasonDan</author>
    public class SupportAllDao<TEntity, TId> : BaseDao<TEntity, TId>, ISupportAllDao<TEntity, TId> where TEntity : class
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

        public bool Save(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }
            (entity as ActiveRecordBase<TEntity>).SaveAndFlush();
            return true;
        }

        public bool Update(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }
            (entity as ActiveRecordBase<TEntity>).UpdateAndFlush();
            return true;
        }
    }
}
