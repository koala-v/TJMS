using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;
namespace WebApi.ServiceModel.TMS
{
    [Route("/tms/rcbp1", "Get")]  // rcbp1?BookingNo=
    public class Rcbp : IReturn<CommonResponse>
    {
    
        public string BusinessPartyName { get; set; }
     
    }
    public class Rcbp_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<Rcbp1> Get_rcbp1_List(Rcbp request)
        {
            List<Rcbp1> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
     
                    if (!string.IsNullOrEmpty(request.BusinessPartyName))
                    {
                        if (!string.IsNullOrEmpty(request.BusinessPartyName))
                        {
                            string strSQL = "Select Top 10 *,(Select Top 1 CountryName From Rccy1 Where CountryCode=Rcbp1.CountryCode) AS CountryName From Rcbp1 Where IsNUll(StatusCode,'')<>'DEL' And BusinessPartyName LIKE '" + request.BusinessPartyName + "%' Order By BusinessPartyCode Asc";
                            Result = db.Select<Rcbp1>(strSQL);
                        }

                    }

                }

            }
            catch { throw; }
            return Result;

        }

    }
}
