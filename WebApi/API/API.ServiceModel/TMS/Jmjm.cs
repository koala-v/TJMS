using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace WebApi.ServiceModel.TMS
{
    [Route("/tms/tjms1", "Get")]  //DeliveryAgentCode=
    [Route("/tms/tjms1/confirm", "Post")] //

    public class tjms : IReturn<CommonResponse>
    {
        public string CustomerCode { get; set; }
        public string JobNo { get; set;  }
        public string confirmAllString { get; set; }
    
    }
    public class tjms_logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<tjms1> Get_tjms1_List(tjms request)
        {

            List<tjms1> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    if (!string.IsNullOrEmpty(request.CustomerCode))
                    {

                        Result = db.Select<tjms1>(
                            "select Top 10 * from tjms1 where isNull(CustomerCode,'')='" + request .CustomerCode +"'"
                              );

                   
                        
                 
                    }
                    else if (!string.IsNullOrEmpty(request.JobNo ))
                    {
                      Result = db.Select<tjms1>(
                          "select Top 10 * from tjms1 where isNull(JobNo,'') LIKE  '" + request.JobNo + "%'"
                            );
                    }

                }
            }
            catch { throw; }
            return Result;
        }
        public int ConfirmAll_tjms1(tjms request)
        {
            int Result = -1;
            //try
            //{
            //    using (var db = DbConnectionFactory.OpenDbConnection())
            //    {
            //        if (request.confirmAllString != null && request.confirmAllString != "")
            //        {
            //            JArray ja = (JArray)JsonConvert.DeserializeObject(request.confirmAllString);
            //            if (ja != null)
            //            {
            //                for (int i = 0; i < ja.Count(); i++)
            //                {
            //                    string strJobNo = "";
            //                    string strActualArrivalDate = "";
            //                    string strDeliveryDate = "";
            //                    if (ja[i]["JobNo"] != null || ja[i]["JobNo"].ToString() != "")
            //                    strJobNo = ja[i]["JobNo"].ToString();
            //                    strActualArrivalDate = ja[i]["ActualArrivalDate"].ToString();
            //                    strDeliveryDate = ja[i]["DeliveryDate"].ToString();
            //                    if (strJobNo != "")
            //                    {
            //                        if (strActualArrivalDate != "") {
            //                        db.Update("tjms3",
            //                          " DateTime = '" + Modfunction.SQLSafe(strActualArrivalDate) + "'",
            //                          " JobNo='" + strJobNo + "' and Description = 'ACTUAL ARRIVAL DATE'");
            //                        }
            //                        if (strDeliveryDate != "") { 
            //                        db.Update("tjms1",
            //                          " DeliveryDateTime = '" + Modfunction.SQLSafe(strDeliveryDate) + "'",
            //                          " JobNo='" + strJobNo + "'");
            //                        }
            //                    }
            //                }
            //                Result = 1;
            //            }
            //        }
            //    }
            //}
            //catch { throw; }
            return Result;
        }


    }
}
