using MessageBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MessageBoard.Controllers
{
    public class RepliesController : ApiController
    {
        private IMessageBoardRepository m_repository = null;

        public RepliesController(IMessageBoardRepository repository)
        {
            m_repository = repository;
        }

        public IEnumerable<Reply> Get(int topicId)
        {
            return m_repository.GetRepliesByTopic(topicId);
        }

        public HttpResponseMessage Post(int topicId, [FromBody]Reply newReply)
        {
            if (newReply.Created == default(DateTime))
            {
                newReply.Created = DateTime.UtcNow;
            }

            newReply.TopicId = topicId;

            if (m_repository.AddReply(newReply) &&
                m_repository.Save())
            {
                return Request.CreateResponse(HttpStatusCode.Created, newReply);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
