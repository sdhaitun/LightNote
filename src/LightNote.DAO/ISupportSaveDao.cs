//============================================================
//
//    Copyright (C) 2011 翟士丹@曲阜师范大学VolcanoSoft火山软件 版权所有
//    文件名　：ISupportSaveDao.cs
//    功能描述：支持保存功能的Dao
//    创建标识：StanZhai 2011/09/30
//    文件版本：1.0.0.0
//
//============================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightNote.DAO
{
    /// <summary>
    /// 支持保存功能的Dao接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TId">Id类型</typeparam>
    public interface ISupportSaveDao<TEntity, TId>
    {
        /// <summary>
        /// 保存指定的实体
        /// </summary>
        /// <param name="entity">要保持的实体</param>
        /// <returns>保存实体是否成功</returns>
        bool Save(TEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <returns>更新实体是否成功</returns>
        bool Update(TEntity entity);
    }
}
