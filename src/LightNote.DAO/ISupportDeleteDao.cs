//============================================================
//
//    Copyright (C) 2011 翟士丹@曲阜师范大学VolcanoSoft火山软件 版权所有
//    文件名　：ISupportDelete.cs
//    功能描述：支持删除功能的Dao
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
    /// 支持删除功能的Dao接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface ISupportDeleteDao<TEntity>
    {
        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns>是否删除成功</returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// 根据主键集合删除实体
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        int DeleteAll<TId>(TId[] pkValues);
    }
}
