//============================================================
//
//    Copyright (C) 2011 翟士丹@曲阜师范大学VolcanoSoft火山软件 版权所有
//    文件名　：IBaseDao.cs
//    功能描述：带有Retrieval方法的最基本的Dao接口定义
//    创建标识：StanZhai 2011/09/14
//    文件版本：1.0.0.0
//
//============================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace LightNote.DAO
{
    /// <summary>
    /// 带有Retrieval方法的最基本的Dao接口定义
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TId">实体Id类型</typeparam>
    public interface IBaseDao<TEntity, TId>
    {
        /// <summary>
        /// 根据Id，获取实体
        /// </summary>
        /// <param name="id">指定的Id</param>
        /// <returns>指定Id的实体</returns>
        TEntity GetById(TId id);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// 判断指定条件的实体是否存在
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        bool Exists(params ICriterion[] criterion);

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="searchCount">要搜索的实体数量，为0表示不限数量</param>
        /// <param name="criterion">搜索条件，也可加入排序规则</param>
        /// <returns>符合条件的实体</returns>
        IList<TEntity> SearchEntity(int searchCount, params Object[] criterion);

        /// <summary>
        /// 查询符合搜索条件的实体数量
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        int QueryCount(params ICriterion[] criterion);

        /// <summary>
        /// 自定义查询
        /// </summary>
        /// <param name="activeRecordQuery"></param>
        /// <returns></returns>
        object ExecuteQuery(IActiveRecordQuery activeRecordQuery);

        /// <summary>
        /// 执行Hql查询
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="count">获取的记录条数，默认为0表示获取所有符合条件的记录</param>
        /// <returns></returns>
        IList<TResult> ExecuteHqlQuery<TResult>(string hql, int count = 0);

        /// <summary>
        /// 执行Hql查询，并将结果映射到实体
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="count">获取的记录条数，默认为0表示获取所有符合条件的记录</param>
        /// <returns></returns>
        IList<TResult> ExecuteHqlQueryModel<TResult>(string hql, int count = 0);

        /// <summary>
        /// 执行HQL更新语句
        /// </summary>
        /// <param name="hql"></param>
        int ExecuteHqlUpdate(string hql);

        /// <summary>
        /// 执行返回单一值Hql查询
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="hql">hql语句</param>
        /// <returns></returns>
        TResult ExecuteUniqueResult<TResult>(string hql);

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="pageIndex">第几页的数据，索引从1开始</param>
        /// <param name="pageSize">每页的数据条目</param>
        /// <param name="totalCount">返回数据的总条数</param>
        /// <param name="criterion">信息筛选条件，也可以加入排序条件</param>
        /// <returns>分页后的结果</returns>
        IList<TEntity> GetPagedList(int pageIndex, int pageSize, out int totalCount, params Object[] criterion);

        /// <summary>
        /// 获取分页后的数据，一般用于单张表的只获取几个字段的数据分页
        /// </summary>
        /// <typeparam name="TSEntity">指定获取分页数据的实体</typeparam>
        /// <param name="pageIndex">第几页的数据，索引从1开始</param>
        /// <param name="pageSize">每页的数据条目</param>
        /// <param name="totalCount">返回数据的总条数</param>
        /// <param name="criterion">信息筛选条件，也可以加入排序条件</param>
        /// <returns>分页后的结果</returns>
        IList<TSEntity> GetPagedList<TSEntity>(int pageIndex, int pageSize, out int totalCount,
                                               params Object[] criterion) where TSEntity : class;

        /// <summary>
        /// 从HQL执行查询并分页并返回Model List，一般用于多表关联查询，HQL带有查询，排序
        /// </summary>
        /// <typeparam name="TEntity">返回的实体类型</typeparam>
        /// <param name="selectClause">select子句：a.ArticleId,a.Title</param>
        /// <param name="fromClause">from子句:Article a, ArticleColumn ac</param>
        /// <param name="whereClause">where子句:and a.Title like '%StanZhai%', and a.IsValid = true</param>
        /// <param name="orderClause">order子句:a.ArticleId desc, a.ClickCount asc</param>
        /// <param name="page">第几页的数据，索引从1开始</param>
        /// <param name="pageSize">每页的数据条目</param>
        /// <param name="totalRows">返回数据的总条数</param>
        /// <returns></returns>
        IList<object[]> GetPagedListFromHql(string selectClause, string fromClause, string whereClause, string orderClause, int page, int pageSize, out int totalRows);

        /// <summary>
        /// 从HQL执行查询并分页并返回Model List，一般用于多表关联查询，HQL带有查询，排序
        /// </summary>
        /// <typeparam name="THqlEntity">返回的实体类型</typeparam>
        /// <param name="hql">要执行查询的Hql语句</param>
        /// <param name="page">第几页的数据，索引从1开始</param>
        /// <param name="pageSize">每页的数据条目</param>
        /// <param name="totalRows">返回数据的总条数</param>
        /// <returns></returns>
        IList<TEntity> GetPagedListModelFromHql(string hql, int page, int pageSize, out int totalRows);

        /// <summary>
        /// 批量切换布尔字段值
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="field">字段名</param>
        /// <param name="pk">主键名</param>
        /// <param name="ids">主键字符串，“,”间隔</param>
        /// <returns></returns>
        int BatToggleBoolField(string table, string field, string pk, string ids);

        /// <summary>
        /// 调用存储过程，并返回实体列表
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parms">存储过程参数</param>
        /// <returns>返回的实体列表</returns>
        IList<TEntity> GetEntityBySP<TEntity>(string spName, IDictionary<string, object> parms);
    } 
}
