using MessageBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MessageBoard.Controllers
{
    public class TopicsController : ApiController
    {
        private IMessageBoardRepository m_repository = null;

        public TopicsController(IMessageBoardRepository repository)
        {
            m_repository = repository;
        }

        public IEnumerable<Topic> Get(bool includeReplies = false)
        {
            IQueryable<Topic> results = null;

            var query = from t in m_repository.Topics()
                        where t.Id < 5
                        orderby t.Title
                        select t;

            if (includeReplies)
            {
                results = m_repository.GetTopicsIncludingReplies();
            }
            else
            {
                results = m_repository.GetTopics();
            }

            return results
                .OrderByDescending(t => t.Created)
                .Take(50)
                .ToList();
        }

        public HttpResponseMessage Post([FromBody]Topic newTopic)
        {
            if (newTopic.Created == default(DateTime))
            {
                newTopic.Created = DateTime.UtcNow;
            }

            if (m_repository.AddTopic(newTopic) &&
                m_repository.Save())
            {
                return Request.CreateResponse(HttpStatusCode.Created, newTopic);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
