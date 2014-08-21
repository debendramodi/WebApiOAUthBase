using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApiOAuthBase.Infrastructure.Models;
using WebApiOAuthBase.Identity.DataModels;

namespace WebApiOAuthBase.Identity.DataAccess
{
    public class SecurityConfigurationManager : IDisposable
    {

        #region Constructors & Disposing
        private SecurityDataContext ctx;
        public SecurityConfigurationManager()
        {
            ctx = new SecurityDataContext();
        }

        public SecurityConfigurationManager(SecurityDataContext ctx)
        {
            this.ctx = ctx;
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
        #endregion

        #region SaveChange Asych Wrapper
        internal async Task<BaseActionReturnModel> SaveChangesAsync()
        {

            // create the delegate with the methode to run
            Func<BaseActionReturnModel> funcToRun = new Func<BaseActionReturnModel>(() => SaveChange());
            // run the delegate Async and return the Task result
            BaseActionReturnModel res = await Task.Factory.StartNew<BaseActionReturnModel>(funcToRun);
            return res;
        }

        private BaseActionReturnModel SaveChange()
        {
            try
            {
                int i = ctx.SaveChanges();
                return BaseActionReturnModel.CreateSuccededResult("OK " + i + " changes in database", false, null, false);
            }
            catch (Exception exc)
            {
                return BaseActionReturnModel.CreateException(exc, false);
            }
        }


        #endregion

        internal BaseActionReturnModel AddApplicationAuthorization(ApplicationAuthorization AdminAuth)
        {
            try
            {
                // do thing
                ctx.ApplicationAuthorizations.Add(AdminAuth);
                return BaseActionReturnModel.CreateSuccededResult("OK", false, null,false);
            }
            catch (Exception exc)
            {
                return BaseActionReturnModel.CreateException(exc, false);
            }
        }

        internal BaseActionReturnModel AddClientApplication(ClientApplication App)
        {
            try
            {
                ctx.ClientApplications.Add(App);
                // do thing
                return BaseActionReturnModel.CreateSuccededResult("OK", false, null,false);
            }
            catch (Exception exc)
            {
                return BaseActionReturnModel.CreateException(exc, false);
            }
        }
    }
   
}