//============================================================
//
//    Copyright (C) 2011 翟士丹@曲阜师范大学VolcanoSoft火山软件 版权所有
//    文件名　：BaseDao.cs
//    功能描述：实现了IBaseDao接口的Dao类
//    创建标识：JasonDan 2011/09/14
//    文件版本：1.0.0.0
//
//============================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightNote.DAO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using NHibernate;
using NHibernate.Criterion;

namespace LightNote.DAO
{
    /// <summary>
    /// 实现了IBaseDao接口的Dao类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <author>JasonDan</author>
    public class BaseDao<TEntity, TId> : IBaseDao<TEntity, TId> where TEntity : class
    {
        public TEntity GetById(TId id)
        {
            try
            {
                return ActiveRecordBase<TEntity>.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 查询符合搜索条件的实体数量
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        public int QueryCount(params ICriterion[] criterion)
        {
            return ActiveRecordMediator<TEntity>.Count(criterion);
        }

        /// <summary>
        /// 自定义查询
        /// </summary>
        /// <param name="activeRecordQuery"></param>
        /// <returns></returns>
        public object ExecuteQuery(IActiveRecordQuery activeRecordQuery)
        {
            return ActiveRecordMediator.ExecuteQuery(activeRecordQuery);
        }

        /// <summary>
        /// 执行Hql查询
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        public IList<TResult> ExecuteHqlQuery<TResult>(string hql, int count = 0)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(typeof(TResult));

            var query = session.CreateQuery(hql);
            if (count != 0)
            {
                query.SetMaxResults(count);
            }
            return query.List<TResult>();
        }

        /// <summary>
        /// 执行Hql查询，并将结果映射到实体
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        public IList<TResult> ExecuteHqlQueryModel<TResult>(string hql, int count = 0)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(typeof(TResult));

            var query = session.CreateSQLQuery(hql).AddEntity(typeof(TResult));
            if (count != 0)
            {
                query.SetMaxResults(count);
            }
            return query.List<TResult>();
        }

        /// <summary>
        /// 执行HQL更新语句
        /// </summary>
        /// <param name="hql"></param>
        public int ExecuteHqlUpdate(string hql)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(typeof(int));

            var query = session.CreateQuery(hql);
            return query.ExecuteUpdate();
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="pageIndex">第几页的数据，索引从1开始</param>
        /// <param name="pageSize">每页的数据条目</param>
        /// <param name="totalCount">返回数据的总条数</param>
        /// <param name="criterion">信息筛选条件，也可以加入排序条件</param>
        /// <returns>分页后的结果</returns>
        public IList<TEntity> GetPagedList(int pageIndex, int pageSize, out int totalCount, params object[] criterion)
        {
            totalCount = ActiveRecordMediator<TEntity>.Count( 
                criterion.Where(t => t is ICriterion).Select(t => t as ICriterion).ToArray());
            
            return ActiveRecordBase<TEntity>.SlicedFindAll((pageIndex - 1) * pageSize, pageSize, 
                criterion.Where(t => t is Order).Select(t => t as Order).ToArray(), 
                criterion.Where(t => t is ICriterion).Select(t => t as ICriterion).ToArray());
        }

        /// <summary>
        /// 获取分页后的数据，一般用于单张表的只获取几个字段的数据分页
        /// </summary>
        /// <typeparam name="TSEntity">指定获取分页数据的实体</typeparam>
        /// <param name="pageIndex">第几页的数据，索引从1开始</param>
        /// <param name="pageSize">每页的数据条目</param>
        /// <param name="totalCount">返回数据的总条数</param>
        /// <param name="criterion">信息筛选条件，也可以加入排序条件</param>
        /// <returns>分页后的结果</returns>
        public IList<TSEntity> GetPagedList<TSEntity>(int pageIndex, int pageSize, out int totalCount, params object[] criterion) where TSEntity : class
        {
            totalCount = ActiveRecordMediator<TSEntity>.Count(
                criterion.Where(t => t is ICriterion).Select(t => t as ICriterion).ToArray());

            return ActiveRecordBase<TSEntity>.SlicedFindAll((pageIndex - 1) * pageSize, pageSize,
                criterion.Where(t => t is Order).Select(t => t as Order).ToArray(),
                criterion.Where(t => t is ICriterion).Select(t => t as ICriterion).ToArray());
        }

        /// <summary>
        /// 从HQL执行查询并分页并返回Model List，一般用于多表关联查询，HQL带有查询，排序
        /// </summary>
        /// <typeparam name="TEntity">返回的实体类型</typeparam>
        /// <param name="selectClause">select子句：a.ArticleId,a.Title</param>
        /// <param name="fromClause">from子句:Article a, ArticleColumn ac</param>
        /// <param name="whereClause">where子句:and a.Title like '%JasonDan%', and a.IsValid = true</param>
        /// <param name="orderClause">order子句:a.ArticleId desc, a.ClickCount asc</param>
        /// <param name="page">第几页的数据，索引从1开始</param>
        /// <param name="pageSize">每页的数据条目</param>
        /// <param name="totalRows">返回数据的总条数</param>
        /// <returns></returns>
        public IList<object[]> GetPagedListFromHql(string selectClause, string fromClause, string whereClause, string orderClause, int page, int pageSize, out int totalRows)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(typeof(int));

            selectClause = String.Format(" select {0} ", selectClause);
            fromClause = String.Format(" from {0} ", fromClause);
            if (!String.IsNullOrEmpty(whereClause))
            {
                whereClause = " where 1=1 " +
                    whereClause.Split(',').Aggregate((i, j) => String.Format(" {0} ", i) + String.Format(" {0} ", j));
            }
            if (!String.IsNullOrEmpty(orderClause))
            {
                orderClause = String.Format(" order by {0} ", orderClause);    
            }
            

            totalRows = (int)session.CreateQuery("select count(*)" + fromClause + whereClause).UniqueResult<long>();

            session = holder.CreateSession(typeof(object));
            var query = session.CreateQuery(selectClause + fromClause + whereClause + orderClause);
            query.SetMaxResults(pageSize);
            query.SetFirstResult((page - 1) * pageSize);

            return query.List<object[]>();
        }

        public IList<TEntity> GetPagedListModelFromHql(string hql, int page, int pageSize, out int totalRows)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(typeof(int));

            // 查询符合条件的数据数量
            var fromClause = hql.Substring(hql.IndexOf("from"));
            if (fromClause.Contains("order"))
            {
                fromClause = fromClause.Remove(fromClause.IndexOf("order"));
            }
            totalRows = (int)session.CreateQuery("select count(*) " + fromClause).UniqueResult<long>();

            session = holder.CreateSession(typeof(TEntity));
            var query = session.CreateSQLQuery(hql).AddEntity(typeof(TEntity));
            query.SetMaxResults(pageSize);
            query.SetFirstResult((page - 1) * pageSize);

            return query.List<TEntity>();
        }

        /// <summary>
        /// 执行返回单一值Hql查询
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="hql">hql语句</param>
        /// <returns></returns>
        public TResult ExecuteUniqueResult<TResult>(string hql)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(typeof(int));

            return session.CreateQuery(hql).UniqueResult<TResult>();
        }

        public IList<TEntity> GetAll()
        {
            return ActiveRecordBase<TEntity>.FindAll();
        }

        /// <summary>
        /// 判断指定条件的实体是否存在
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        public bool Exists(params ICriterion[] criterion)
        {
            return ActiveRecordBase<TEntity>.Exists(criterion);
        }

        public IList<TEntity> SearchEntity(int searchCount, params object[] criterion)
        {
            if (searchCount == 0)
            {
                return ActiveRecordBase<TEntity>.FindAll(
                    criterion.Where(t => t is Order).Select(t => t as Order).ToArray(),
                    criterion.Where(t => t is ICriterion).Select(t => t as ICriterion).ToArray());
            }
            return ActiveRecordBase<TEntity>.SlicedFindAll(0, searchCount,
                criterion.Where(t => t is Order).Select(t => t as Order).ToArray(),
                criterion.Where(t => t is ICriterion).Select(t => t as ICriterion).ToArray());
        }

        public int BatToggleBoolField(string table, string field, string pk, string ids)
        {
            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(typeof(int));
            var query = session.CreateQuery(string.Format(
                "update {0} set {1} = {1} -1 where {2} in ({3})", table, field, pk, ids));
            return query.ExecuteUpdate();
        }

        public IList<TEntity> GetEntityBySP<TEntity>(string spName, IDictionary<string, object> parms)
        {
            Type entityType = typeof(TEntity);

            ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = holder.CreateSession(entityType);

            StringBuilder sql = new StringBuilder();
            sql.Append("exec " + spName + " ");
            foreach (var item in parms)
            {
                sql.AppendFormat(":{0},", item.Key);
            }
            if (sql.Length != 0)
            {
                sql.Length--;
            }

            var query = session.CreateSQLQuery(sql.ToString())
                .AddEntity(entityType);
            foreach (var item in parms)
            {
                query.SetParameter(item.Key, item.Value);
            }
            return query.List<TEntity>();
        }
    }
}
