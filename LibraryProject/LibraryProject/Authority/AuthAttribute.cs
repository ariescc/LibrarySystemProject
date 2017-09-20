using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryProject.Controllers
{
    public class AuthAttribute : ActionFilterAttribute
    {
        public string Code { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //如果不存在身份信息
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if(authCookie == null)
            {
                ContentResult Content = new ContentResult();
                Content.Content = string.Format("<script type='text/javascript'>alert('请先登录！');window.location.href='{0}';</script>", FormsAuthentication.LoginUrl);
                filterContext.Result = Content;
            }
            else
            {
                /* var Instance = CheckLogin.Instance;
                var user = Instance.GetUser();
                var Roles = user.Role;
                string[] Role = Roles.Split(','); */
                string[] Role = CheckLogin.Instance.GetUser().Role.Split(',');//获取所有角色
                if (!Role.Contains(Code))//验证权限
                {
                    //验证不通过
                    ContentResult Content = new ContentResult();
                    Content.Content = "<script type='text/javascript'>alert('权限验证不通过！');history.go(-1);</script>";
                    filterContext.Result = Content;
                }
            }
        }
    }
}