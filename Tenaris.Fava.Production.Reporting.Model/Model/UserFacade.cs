﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class UserFacade
    {
        public static bool LoginHibernate(string userName, string password)
        {
            try
            {
                return new CommonUserRepository().LoginUser(userName, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
