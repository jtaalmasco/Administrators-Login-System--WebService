using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace OperatorWebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string OperatorLogin(string userName, string passWord)
        {
            DBConnect dbObject = new DBConnect();
            string result;

            result = dbObject.operatorLogin(userName, passWord);
            return result;
        }

        [WebMethod]
        public List<string> GetDomains()
        {
            DBConnect dbObject = new DBConnect();
            List<string> result = new List<string>();

            result = dbObject.GetDomainID();
            return result;
        }

        [WebMethod]
        public List<string> GetClientsPerGroup(int GroupID)
        {
            DBConnect dbObject = new DBConnect();
            List<string> result = new List<string>();

            result = dbObject.getClientsPerGroupFunc(GroupID);
            return result;
        }

        [WebMethod]
        public void AddNewDomain(string operatorName, int ID, string Domain)
        {
            DBConnect dbObject = new DBConnect();
            dbObject.AddDomain(operatorName, ID, Domain);
        }

        [WebMethod]
        public void EditDomain(string operatorName, int ID, string OrigDomain, string newDomain)
        {
            DBConnect dbObject = new DBConnect();
            dbObject.EditDomainF(operatorName, ID, OrigDomain, newDomain);
        }

        [WebMethod]
        public void DeleteDomain(string operatorName, int ID, string Domain)
        {
            DBConnect dbObject = new DBConnect();
            dbObject.DeleteDomainF(operatorName, ID, Domain);
        }

        [WebMethod]
        public List<Prop> GetOperatorLogs()
        {
            DBConnect dbObject = new DBConnect();
            var result = new List<Prop>();

            result = dbObject.GetOperatorsLogFS();
            return result;
        }
    }
}