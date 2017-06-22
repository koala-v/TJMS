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
    [Route("/tms/tjms2", "Get")]  //DriverCode=
    [Route("/tms/tjms2/update", "Post")] //
    [Route("/tms/tjms2/confirm", "Get")] //update?Key=,Remark=,TableName=
    [Route("/tms/tjms2/PickupTimeUpdate", "Post")] 
    

    public class Tobk : IReturn<CommonResponse>
    {
        public string TrxNo { get; set; }
        public string Key { get; set; }
        public string LineItemNo { get; set; }
        public string Remark { get; set; }
        public string OnBehalfName { get; set; }
        public string TableName { get; set; }
        public string UpdateAllString { get; set; }
        public string DriverCode { get; set; }
        public string BookingNo { get; set; }
        public string JobNo { get; set; }
        public string DCDescription { get; set; }
        public string ScheduleDateFlag { get; set; }
      
    }
    public class Tobk_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<tjms2> Get_tjms2_List(Tobk request)
        {

            List<tjms2> Result = null;
            try
            {                
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    string strSql = "";    
                    strSql = "select TrxNo,LineItemNo,* from tjms2 where trxno="+request .TrxNo + "";
                    Result = db.Select<tjms2>(strSql);
                }
            }
            catch { throw; }
            return Result;

        }
        public int confirm_tjms2(Tobk request)
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    string strJobNo = request.JobNo;
                    if (strJobNo != "" && strJobNo != null )
                    {
                        int intMaxLineItemNo = 1;
                        List<tjms3> list1 = db.Select<tjms3>("Select Max(LineItemNo) LineItemNo from tjms3 Where JobNo = " + Modfunction.SQLSafeValue(strJobNo));
                        if (list1 != null)
                        {
                            if (list1[0].LineItemNo > 0)
                                intMaxLineItemNo = list1[0].LineItemNo + 1;
                        }
                        if (request.DCDescription == "Collection")
                        {
                            request.DCDescription = "COLLECTED";
                        }
                        else
                        {
                            request.DCDescription = "DELIVERED";
                        }
                        db.Insert(new tjms3
                        {
                            JobNo = Modfunction.SQLSafe(strJobNo),
                            DateTime = DateTime.Now,
                            UpdateDatetime = DateTime.Now,
                            LineItemNo = intMaxLineItemNo,
                            RefNo = Modfunction.SQLSafe(request.LineItemNo).ToString(),
                            AutoFlag = "N",
                            StatusCode = "POD",
                            UpdateBy = Modfunction.SQLSafe(request.DriverCode),
                            Remark = Modfunction.SQLSafeValue(request.Remark),
                            Description = Modfunction.SQLSafe(request.DCDescription)
                        });
                     
                    }


                    string str;
                    if( request.LineItemNo != "0")
                     {
                        str = " Note = " + Modfunction.SQLSafeValue(request.Remark) + ",DeliveryDate=GETDATE(),CompleteFlag='Y'";
                        db.Update(request.TableName,
                               str,
                               " BookingNo='" + request.Key + "' and LineItemNo = '" + request.LineItemNo + "' ");
                    } else {
                        str = " Note = " + Modfunction.SQLSafeValue(request.Remark) + ",DeliveryEndDateTime=GETDATE(),StatusCode = 'POD',CompletedFlag='Y'";
                        db.Update("tjms2",
                               str,
                               " BookingNo='" + request.Key + "'");
                    }

                }

            }
            catch { throw; }
            return Result;
        }

        public int updatePickupTime(Tobk request) {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UpdateAllString != null && request.UpdateAllString != "")
                    {
                        JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                        if (ja != null)
                        {
                            for (int i = 0; i < ja.Count(); i++)
                            {
                              
                                string strKey = ja[i]["Key"].ToString();
                                string strTobk2LineItemNo = ja[i]["LineItemNo"].ToString();              
                                    string strJobNo = "";
                                    if (ja[i]["JobNo"] != null || ja[i]["JobNo"].ToString() != "")
                                        strJobNo = ja[i]["JobNo"].ToString();
                                    if (strJobNo != "")
                                    {
                                        int intMaxLineItemNo = 1;
                                        List<tjms3> list1 = db.Select<tjms3>("Select Max(LineItemNo) LineItemNo from tjms3 Where JobNo = " + Modfunction.SQLSafeValue(strJobNo));
                                        if (list1 != null)
                                        {
                                            if (list1[0].LineItemNo > 0)
                                                intMaxLineItemNo = list1[0].LineItemNo + 1;
                                        }
                                        db.Insert(new tjms3
                                        {
                                            JobNo = strJobNo,
                                            DateTime = Convert.ToDateTime(ja[i]["DateTime"]),
                                            UpdateDatetime = DateTime.Now,
                                            RefNo = strTobk2LineItemNo,
                                            LineItemNo = intMaxLineItemNo,                                         
                                            StatusCode = "USE",
                                            UpdateBy = ja[0]["DriverCode"] == null ? "" : Modfunction.SQLSafe(ja[0]["DriverCode"].ToString()),                                          
                                            Description = "PICKUP"
                                        });



                               
                             
                                }
                            

                            }
                            Result = 1;
                        }
                    }
                }
            }
            catch { throw; }
            return Result;
        }
        public int UpdateAll_tjms2(Tobk request)
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UpdateAllString != null && request.UpdateAllString != "")
                    {
                        JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                        if (ja != null)
                        {
                            for (int i = 0; i < ja.Count(); i++)
                            {
                                if (ja[i]["TableName"] == null || ja[i]["TableName"].ToString() == "")
                                { continue; }
                                string strKey = ja[i]["Key"].ToString();
                                string strTobk2LineItemNo= ja[i]["LineItemNo"].ToString();
                                string strTableName = ja[i]["TableName"].ToString();
                                string strRemark = "";
                                string strStatusCode = "";
                                string strDCDescription = ja[i]["DCFlag"].ToString();                      
                                if (ja[i]["Remark"] != null || ja[i]["Remark"].ToString() != "")
                                    strRemark = ja[i]["Remark"].ToString();
                                if (ja[i]["StatusCode"] != null || ja[i]["StatusCode"].ToString() != "")
                                    strStatusCode = ja[i]["StatusCode"].ToString();
                                if (strStatusCode.ToLower() == "cancel")
                                {
                                    string strJobNo = "";
                                    if (ja[i]["JobNo"] != null || ja[i]["JobNo"].ToString() != "")
                                        strJobNo = ja[i]["JobNo"].ToString();
                                    if (strJobNo != "")
                                    {
                                        int intMaxLineItemNo = 1;
                                        List<tjms3> list1 = db.Select<tjms3>("Select Max(LineItemNo) LineItemNo from tjms3 Where JobNo = " + Modfunction.SQLSafeValue(strJobNo));
                                        if (list1 != null)
                                        {
                                            if (list1[0].LineItemNo > 0)
                                                intMaxLineItemNo = list1[0].LineItemNo + 1;
                                        }
                                        db.Insert(new tjms3
                                        {
                                            JobNo = strJobNo,
                                            DateTime = DateTime.Now,
                                            UpdateDatetime = DateTime.Now,
                                            RefNo= strTobk2LineItemNo,
                                            LineItemNo = intMaxLineItemNo,
                                            AutoFlag = "N",
                                            StatusCode = "CANCEL",
                                            UpdateBy = ja[0]["DriverCode"] == null ? "" : Modfunction.SQLSafe(ja[0]["DriverCode"].ToString()),
                                            Remark = Modfunction.SQLSafe(strRemark),
                                            Description = ja[0]["CancelDescription"] == null ? "" : Modfunction.SQLSafe(ja[0]["CancelDescription"].ToString())
                                        });



                                        string str;
                                        if (strTobk2LineItemNo != "0")
                                        {
                                           
                                            str = " Note = " + Modfunction.SQLSafeValue(strRemark) + ",DeliveryDate=GETDATE(),ReturnFlag='Y'";
                                            db.Update(strTableName,
                                                   str,
                                                   " BookingNo='" + strKey + "' and LineItemNo='" + strTobk2LineItemNo + "'");
                                        }
                                        else
                                        {
                                            str = " Note = " + Modfunction.SQLSafeValue(strRemark) + ",DeliveryEndDateTime=GETDATE()";
                                            db.Update("tjms2",
                                                   str,
                                                   " BookingNo='" + strKey + "'");
                                        }

                                    }
                                }
                                else
                                {

                                    string strJobNo = "";
                                    if (ja[i]["JobNo"] != null || ja[i]["JobNo"].ToString() != "")
                                        strJobNo = ja[i]["JobNo"].ToString();
                                    if (strJobNo != "")
                                    {
                                        int intMaxLineItemNo = 1;
                                        List<tjms3> list1 = db.Select<tjms3>("Select Max(LineItemNo) LineItemNo from tjms3 Where JobNo = " + Modfunction.SQLSafeValue(strJobNo));
                                        if (list1 != null)
                                        {
                                            if (list1[0].LineItemNo > 0)
                                                intMaxLineItemNo = list1[0].LineItemNo + 1;
                                        }
                                        if (strDCDescription == "Collection")
                                        {
                                            strDCDescription = "COLLECTED";
                                        }
                                        else
                                        {
                                            strDCDescription = "DELIVERED";
                                        }
                                        db.Insert(new tjms3
                                        {
                                            JobNo = strJobNo,
                                            DateTime = DateTime.Now,
                                            UpdateDatetime = DateTime.Now,
                                            RefNo = strTobk2LineItemNo,
                                            LineItemNo = intMaxLineItemNo,
                                            AutoFlag = "N",
                                            StatusCode = "POD",
                                            UpdateBy = ja[0]["DriverCode"] == null ? "" : Modfunction.SQLSafe(ja[0]["DriverCode"].ToString()),
                                            Remark = Modfunction.SQLSafe(strRemark),
                                            Description = strDCDescription
                                        });
                                      
                                    }


                                    string str;
                                    if (strTobk2LineItemNo  != "0")
                                    {
                                        str = " Note = " + Modfunction.SQLSafeValue(strRemark) + ",DeliveryDate=GETDATE(),CompleteFlag='Y'";
                                        db.Update(strTableName,
                                               str,
                                               " BookingNo='" + strKey + "' and LineItemNo='" + strTobk2LineItemNo + "'");
                                    }
                                    else
                                    {
                                        str = " Note = " + Modfunction.SQLSafeValue(strRemark) + ",DeliveryEndDateTime=GETDATE(),StatusCode = 'POD',CompletedFlag='Y'";
                                        db.Update("tjms2",
                                               str,
                                               " BookingNo='" + strKey + "'");
                                    }

                                }

                            }
                            Result = 1;
                        }
                    }
                }
            }
            catch { throw; }
            return Result;
        }

    }
}
