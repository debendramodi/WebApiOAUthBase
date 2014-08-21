using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebApiOAuthBase.Infrastructure.Models
{
    public class BaseActionReturnModel
    {
        public bool Succeeded { get; set; }
        public string ReturnMessage { get; set; }
        public Exception Exception { get; set; }
        public object ReturnedObject { get; set; }
        // If a methode succeed but for exemple no change have been committed to database then HasCommunication = true (ex: Authorization already assigned
        public bool HasCommunication { get; set; }

        public BaseActionReturnModel(bool succeeded, string returnMessage,
                                        Exception exception, object returnedObject, bool hasCommunication)
        {
            Succeeded = succeeded;
            ReturnMessage = returnMessage;
            Exception = exception;
            ReturnedObject = returnedObject;
            HasCommunication = hasCommunication;
        }
        public BaseActionReturnModel()
        {

        }

        public static BaseActionReturnModel CreateException(Exception exc, bool needToLogInDatabase,
                                                                [CallerMemberName]string callingMethodeName = "",
                                                                  [CallerLineNumber] int sourceLineNumber = 0)
        {
            BaseActionReturnModel r = new BaseActionReturnModel();
            // create and log the exception
            r.Succeeded = false;
            r.Exception = exc;
            r.ReturnMessage = "Error in " + callingMethodeName + " at line " + sourceLineNumber + " (" + DateTime.Now.ToString() + ")";
            //TODO send to log
            if (needToLogInDatabase)
            {
                // log to database
            }
            return r;
        }

        public static BaseActionReturnModel CreateSuccededResult(string Message, bool NeedToLogInDatabase,
                                                                    object Object, bool hasCommunication,
                                                                     [CallerMemberName]string callingMethodeName = "",
                                                                          [CallerLineNumber] int sourceLineNumber = 0)
        {
            BaseActionReturnModel r = new BaseActionReturnModel();
            // create and log the exception
            r.Succeeded = true;
            r.Exception = null;
            r.ReturnMessage = Message;
            r.HasCommunication = hasCommunication;
            // Warning Object could be null
            r.ReturnedObject = Object;
            //TODO send to log
            if (NeedToLogInDatabase)
            {
                // log to database           
            }
            return r;
        }


    }
}
