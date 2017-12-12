using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using TestActor.Interfaces;

namespace Web1.Controllers
{
    [Route("api/[controller]")]
    public class ValueController : Controller
    {
        // GET api/value
        [HttpGet]
        public Task<Result> Get()
        {
            var testActor = ActorProxy.Create<ITestActor>(new ActorId("test"), new Uri("fabric:/TestSfActors/TestActorService"));
            return testActor.GetValueAsync();
        }

        // POST api/value
        [HttpPost]
        public Task Post()
        {
            var testActor = ActorProxy.Create<ITestActor>(new ActorId("test"), new Uri("fabric:/TestSfActors/TestActorService"));
            return testActor.SetValueAsync(300);
        }

        // PUT api/value
        [HttpPut]
        public Task Put()
        {
            var testActor = ActorProxy.Create<ITestActor>(new ActorId("test"), new Uri("fabric:/TestSfActors/TestActorService"));
            return testActor.UpdateValueAndRegisterReminderAsync(300);
        }
    }
}