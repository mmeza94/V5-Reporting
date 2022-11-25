using NHibernate;
using NHMA = NHibernate.Mapping.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using Expression = NHibernate.Criterion.Expression;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess
{
    public class BaseDataAccess
    {
        public static readonly string FORJA_CONNSTR_NAME = Configurations.Instance.VersionApplication;
        //public static readonly string MECA_CONNSTR_NAME = "MecaReportProductionDB";

        protected ISession m_session;

        public BaseDataAccess()
        {
            m_session = NhibernateHttpModule.CurrentSession;
        }

        /// <summary>
        /// Save a collection.
        /// </summary>
        /// <param name="items">Lista de objetos</param>
        public virtual void Save(IList items)
        {
            ITransaction tx = null;

            try
            {
                if (items != null)
                {
                    tx = m_session.BeginTransaction();

                    foreach (object item in items)
                    {
                        m_session.SaveOrUpdate(item);
                    }

                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                tx.Rollback();

                throw ex;
            }
        }

        /// <summary>
        /// Saves an item and then saves the child items inside of a transaction.
        /// </summary>
        /// <param name="parentItem">objeto padre</param>
        /// <param name="childItems">objetos hijos</param>
        public virtual void Save(object parentItem, IList childItems)
        {
            ITransaction tx = null;

            try
            {
                if (childItems != null)
                {
                    tx = m_session.BeginTransaction();

                    m_session.SaveOrUpdate(parentItem);

                    foreach (object item in childItems)
                    {
                        m_session.SaveOrUpdate(item);
                    }

                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                tx.Rollback();

                throw ex;
            }
        }

        ///// <summary>
        ///// Saves an item and then saves the child items inside of a transaction.
        ///// </summary>
        ///// <param name="parentItem">objeto first item</param>
        ///// <param name="childItems">objetos hijos</param>
        ///// <param name="childItems">objetos second item</param>
        //public virtual void Save(object firstItem, IList childItems,object secondItem)
        //{
        //    ITransaction tx = null;

        //    try
        //    {
        //        if (childItems != null)
        //        {
        //            tx = m_session.BeginTransaction();

        //            m_session.SaveOrUpdate(firstItem);

        //            m_session.SaveOrUpdate(secondItem);

        //            foreach (object item in childItems)
        //            {
        //                m_session.SaveOrUpdate(item);
        //            }

        //            tx.Commit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        tx.Rollback();

        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Saves an item and then saves the child items inside of a transaction.
        /// </summary>
        /// <param name="parentItem">objeto first item</param>
        /// <param name="childItems">objetos hijos</param>
        /// <param name="childItems">objeto second item</param>
        /// <param name="childItems">Objeto second hijos </param>
        public virtual void Save(object firstItem, IList childItems, object secondItem, IList secondchildItems)
        {
            ITransaction tx = null;

            try
            {
                if (childItems != null)
                {
                    tx = m_session.BeginTransaction();

                    m_session.SaveOrUpdate(firstItem);

                    m_session.SaveOrUpdate(secondItem);

                    foreach (object item in childItems)
                    {
                        m_session.SaveOrUpdate(item);
                    }

                    foreach (object sItem in secondchildItems)
                    {
                        m_session.SaveOrUpdate(sItem);
                    }

                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                tx.Rollback();

                throw ex;
            }
        }

        /// <summary>
        /// Saves an item and then saves the child items inside of a transaction.
        /// </summary>
        /// <param name="parentItem">objeto padre</param>
        /// <param name="childItems">objetos hijos</param>
        public virtual void Save(object parentItem, IList childItems, IList secondChildItems)
        {
            ITransaction tx = null;

            try
            {
                if (childItems != null)
                {
                    tx = m_session.BeginTransaction();

                    m_session.SaveOrUpdate(parentItem);

                    foreach (object item in childItems)
                    {
                        m_session.SaveOrUpdate(item);
                    }
                    foreach (object item in secondChildItems)
                    {
                        m_session.SaveOrUpdate(item);
                    }

                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                tx.Rollback();

                throw ex;
            }
        }


        /// <summary>
        /// Saves an item and then saves the child items inside of a transaction.
        /// </summary>
        /// <param name="parentItem">objeto padre</param>
        /// <param name="childItems">objetos hijos</param>
        public virtual void Save(object parentItem, IList childItems, IList secondChildItems, IList thirdChildItems)
        {
            ITransaction tx = null;

            try
            {
                if (childItems != null)
                {
                    tx = m_session.BeginTransaction();

                    m_session.SaveOrUpdate(parentItem);

                    foreach (object item in childItems)
                    {
                        m_session.SaveOrUpdate(item);
                    }
                    foreach (object item in secondChildItems)
                    {
                        m_session.SaveOrUpdate(item);
                    }
                    foreach (object item in thirdChildItems)
                    {
                        m_session.SaveOrUpdate(item);
                    }

                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                tx.Rollback();

                throw ex;
            }
        }

        /// <summary>
        /// Saves the item.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Save(object item)
        {
            Save(item, true);
        }

        /// <summary>
        /// Salva(Guarda) un item.
        /// </summary>
        /// <param name="item">Objeto a guardar</param>
        protected virtual void Save(object item, bool pointlessParameter)
        {
            ITransaction tx = null;

            try
            {
                tx = m_session.BeginTransaction();

                m_session.SaveOrUpdate(item);

                tx.Commit();
            }
            catch (Exception ex)
            {
                if (tx != null) tx.Rollback();

                throw ex;
            }
        }

        /// <summary>
        /// Regresa uan Lista de Items de acuerdo al tipo especificado.
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <returns>Lista de Objetos</returns>
        public IList Get(Type type)
        {
            return GetByType(type);
        }

        /// <summary>
        /// Regresa un objeto del tipo y el identificador espeficicado.
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="id">Identificador</param>
        /// <returns>Objeto</returns>
        public object Get(Type type, object id)
        {
            object returnValue = null;

            try
            {
                returnValue = m_session.Load(type, id);

                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una Lista por  el tipo, una propiedad del VO y su valor
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="propertyName">Nombre de la Propiedad(Campo)</param>
        /// <param name="propertyValue">Valor</param>
        /// <returns>Lista de items</returns>
        public IList GetListByPropertyValue(Type type, string propertyName, object propertyValue)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                crit.Add(Expression.Eq(propertyName, propertyValue));

                return crit.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Obtiene una Lista por  el tipo, una propiedad del VO y su valor.
        /// Lo ordena de acuerdo a una propiedad ascendente o descendente
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="propertyName">Nombre de la Propiedad</param>
        /// <param name="propertyValue">Valor de la propiedad</param>
        /// <param name="orderProperty">Propiedad para ordenar</param>
        /// <param name="ascending">True para Ascendente, False para Descendente</param>
        /// <returns>Lista de Objetos</returns>
        public IList GetListByPropertyValue(Type type, string propertyName,
            object propertyValue, string orderProperty, bool ascending)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                crit.Add(Expression.Eq(propertyName, propertyValue));
                crit.AddOrder(new Order(orderProperty, ascending));
                return crit.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Obtiene una Lista por  el tipo, una propiedad del VO y su valor.
        /// Lo ordena de acuerdo a una propiedad ascendente o descendente
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="propertyName">Nombre de la Propiedad</param>
        /// <param name="propertyValue">Valor de la propiedad</param>
        /// <param name="propertyName2">Nombre de la segunda Propiedad</param>
        /// <param name="propertyValue2">Valor de la segunda propiedad</param>
        /// <returns>Una lista de items</returns>
        public IList GetListByPropertyValue(Type type, string propertyName, object propertyValue,
            string propertyName2, object propertyValue2)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                crit.Add(Expression.Eq(propertyName, propertyValue));
                crit.Add(Expression.Eq(propertyName2, propertyValue2));

                return crit.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Regresa la primera coincidencia por tipo, nombre de propiedad y valor de la propiedad
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="propertyName">Nombre de la Propiedad</param>
        /// <param name="propertyValue">valor de la propiedad</param>
        /// <returns>Objeto</returns>
        public object Get(
            Type type,
            string propertyName, object propertyValue)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                crit.Add(Expression.Eq(propertyName, propertyValue));

                IList list = crit.List();

                if (list == null || list.Count < 1)
                {
                    return null;
                }
                else
                {
                    return list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Regresa un conjunto de objetos por tipo,ordena de de acuerdo a una propiedad
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="propertyName">Nombre de la Propiedad por la cual se va a ordenar</param>
        /// <param name="ascending">Ordenamiento ascendente o no</param>
        /// <returns>Objeto</returns>
        public IList Get(
            Type type,
            string propertyName, bool ascending)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                if (ascending)
                    crit.AddOrder(Order.Asc(propertyName));
                else
                    crit.AddOrder(Order.Desc(propertyName));

                IList list = crit.List();

                if (list == null || list.Count < 1)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Regresa la primera coincidencia a través de múltiples propiedades y sus valores
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="propValues">HashTable, donde key = nombrepropiedad y value =valorpropiedad</param>
        /// <returns>Objeto</returns>
        public object Get(Type type, Hashtable propValues)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                foreach (DictionaryEntry entry in propValues)
                {
                    crit.Add(Expression.Eq(entry.Key.ToString(), entry.Value));
                }



                IList list = crit.List();

                if (list == null || list.Count < 1)
                {
                    return null;
                }
                else
                {
                    return list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Obtiene una lista de objetos por el valor de sus propiedades
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="propValues">Propiedades y valores en HashTable, donde key = nombrepropiedad y value =valorpropiedad</param>
        /// <returns>Lista de Objetos</returns>
        public IList GetList(Type type, Hashtable propValues)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                foreach (DictionaryEntry entry in propValues)
                {
                    crit.Add(Expression.Eq(entry.Key.ToString(), entry.Value));
                }



                IList list = crit.List();

                if (list == null || list.Count < 1)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IList GetList(Type type, Hashtable propValues, string orderProperty, bool ascending)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                foreach (DictionaryEntry entry in propValues)
                {
                    crit.Add(Expression.Eq(entry.Key.ToString(), entry.Value));
                }

                crit.AddOrder(ascending ? Order.Asc(orderProperty) : Order.Desc(orderProperty));

                IList list = crit.List();

                if (list == null || list.Count < 1)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// Obtiene la primera coincidencia a través del valor de dos propiedades de un objeto
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <param name="propertyName">Nombre de la primera propiedad</param>
        /// <param name="propertyValue">Valor de la primera propiedad</param>
        /// <param name="propertyName2">Nombre de la segunda propiedad</param>
        /// <param name="propertyValue2">Valor de la segunda propiedad</param>
        /// <returns>Objeto</returns>
        public object Get(
            Type type,
            string propertyName, object propertyValue,
            string propertyName2, object propertyValue2)
        {
            try
            {
                ICriteria crit = m_session.CreateCriteria(type);

                crit.Add(Expression.Eq(propertyName, propertyValue));
                crit.Add(Expression.Eq(propertyName2, propertyValue2));

                IList list = crit.List();

                if (list == null || list.Count < 1)
                {
                    return null;
                }
                else
                {
                    return list[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Regresa una lista de objetos de acuerdo al Tipo especificado
        /// </summary>
        /// <param name="type">Tipo</param>
        /// <returns>Lista de Objetos</returns>
        private IList GetByType(Type type)
        {
            IList items = null;
            ITransaction tx = null;

            try
            {
                tx = m_session.BeginTransaction();

                items = m_session.CreateCriteria(type).List();

                tx.Commit();

                return items;
            }
            catch (Exception ex)
            {
                if (tx != null) tx.Rollback();

                throw ex;
            }
        }
        /// <summary>
        /// Elimina un objeto
        /// </summary>
        /// <param name="item">Objeto a eliminar</param>
        public void Delete(object item)
        {
            ITransaction tx = null;

            try
            {
                tx = m_session.BeginTransaction();

                m_session.Delete(item);

                tx.Commit();
            }
            catch (NHibernate.ADOException adoEX)
            {
                m_session.Clear();
                if (tx != null) tx.Rollback();
                throw adoEX;
            }
            catch (Exception ex)
            {
                if (tx != null) tx.Rollback();
                throw ex;
            }
        }

        public void Delete(IList items)
        {
            ITransaction tx = null;

            try
            {
                tx = m_session.BeginTransaction();

                foreach (object item in items)
                {
                    m_session.Delete(item);
                }

                tx.Commit();
            }
            catch (Exception ex)
            {
                if (tx != null) tx.Rollback();

                throw ex;
            }
        }

        public void DeleteSave(object parentItem, IList itemsToDelete, IList itemsToSave)
        {
            ITransaction tx = null;

            try
            {
                tx = m_session.BeginTransaction();

                m_session.SaveOrUpdate(parentItem);

                if (itemsToDelete != null)
                    foreach (object item in itemsToDelete)
                    {
                        m_session.Delete(item);

                    }
                if (itemsToSave != null)
                    foreach (object item in itemsToSave)
                    {
                        m_session.SaveOrUpdate(item);
                    }


                tx.Commit();

            }
            catch (Exception ex)
            {
                if (tx != null) tx.Rollback();

                throw ex;
            }
        }

        public void DeleteSave(IList itemsToDelete, IList itemsToSave)
        {
            ITransaction tx = null;

            try
            {
                tx = m_session.BeginTransaction();
                if (itemsToDelete != null)
                    foreach (object item in itemsToDelete)
                    {
                        m_session.Delete(item);
                    }

                if (itemsToSave != null)
                    foreach (object item in itemsToSave)
                    {
                        m_session.Save(item);
                    }
                tx.Commit();




            }
            catch (Exception ex)
            {
                if (tx != null) tx.Rollback();

                throw ex;
            }
        }
    }
}
