using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.TMS;

namespace WebApi.ServiceInterface.TMS
{
    public class TableService
    {
        public void TS_tjms2(Auth auth, Tobk request, Tobk_Logic Tobk_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {

                if (uri.IndexOf("/tms/tjms2/confirm") > 0)
                {
                    ecr.data.results = Tobk_Logic.confirm_tjms2(request);
                }
                else if (uri.IndexOf("/tms/tjms2/update") > 0)
                {
                    ecr.data.results = Tobk_Logic.UpdateAll_tjms2(request);
                }
                else if (uri.IndexOf("/tms/tjms2/PickupTimeUpdate") > 0)
                {
                    ecr.data.results = Tobk_Logic.updatePickupTime(request);
                }
                else if (uri.IndexOf("/tms/tjms2") > 0)
                {
                    ecr.data.results = Tobk_Logic.Get_tjms2_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }

        public void TS_Rcbp(Auth auth, Rcbp request, Rcbp_Logic rcbp_logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/tms/rcbp1") > 0)
                {
                    ecr.data.results = rcbp_logic.Get_rcbp1_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }

        public void TS_tjms(Auth auth, tjms request, tjms_logic tjms_logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/tms/tjms1/confirm") > 0)
                {
                    ecr.data.results = tjms_logic.ConfirmAll_tjms1(request);
                }
               else if (uri.IndexOf("/tms/tjms1") > 0)
                {
                    ecr.data.results = tjms_logic.Get_tjms1_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }


        public void DownLoadImg(Auth auth, DownLoadImg request, DownLoadImg_Logic logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/tms/tjms2/attach") > 0)
                {
                    ecr.data.results = logic.Get_tjms1_Attach_List(request);
                }
                if (uri.IndexOf("/tms/tjms1/doc") > 0)
                {
                    //ecr.data.results = logic.Get_tjms1_Doc_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }

    }
}
