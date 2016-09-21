using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fingo.Auth.AuthServer.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController(IEventBus eventBus , IEventWatcher eventWatcher)
        {
            eventBus.SubscribeAll(m => eventWatcher.StoreEvent("0" , m.GetType().Name , m.ToString()));
        }
    }
}