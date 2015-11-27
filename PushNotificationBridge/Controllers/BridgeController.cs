using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PushNotificationBridge.Controllers
{
    public class BridgeController : ApiController
    {
        private static Dictionary<string, string> Channels { get; }= new Dictionary<string, string>(); 

        // GET api/values
        public IEnumerable<string> Get()
        {
            return from c in Channels
                select c.Value;
        }

        // GET api/values/5
        public string Get(string id)
        {
            if (Channels.ContainsKey(id))
            {
                return Channels[id];
            }
            return null;
        }

        // PUT api/values/5
        public async Task Post(string id, HttpRequestMessage value)
        {
            Channels[id] = await value.Content.ReadAsStringAsync();
        }

        // DELETE api/values/5
        public void Delete(string id)
        {
            if (Channels.ContainsKey(id))
            {
                Channels.Remove(id);
            }
        }
    }
}