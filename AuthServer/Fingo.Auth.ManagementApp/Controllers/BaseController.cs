using System.Security.Claims;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.ManagementApp.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Fingo.Auth.ManagementApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController(IEventWatcher eventWatcher, IEventBus eventBus)
        {
            eventBus.SubscribeAll(
                m => eventWatcher.StoreEvent(User.FindFirstValue(ClaimTypes.UserData), m.GetType().Name, m.ToString()));
        }

        protected void Alert(AlertType type , string message)
        {
            string key = string.Format("Alert{0}" , type);
            TempData[key] = message;
        }

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
        }
    }
}
