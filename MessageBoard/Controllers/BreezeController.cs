using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Breeze.WebApi2;
using MessageBoard.Data;
using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;

namespace MessageBoard.Controllers
{
    [BreezeController]
    public class BreezeController : ApiController
    {
        private readonly IMessageBoardRepository m_repository;

        public BreezeController(IMessageBoardRepository repository)
        {
            m_repository = repository;
        }

        [HttpGet]
        public string Metadata()
        {
            return m_repository.Metadata;
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return m_repository.SaveChanges(saveBundle);
        }

        [HttpGet]
        public IQueryable<Topic> Topics()
        {
            return m_repository.Topics();
        }

        [HttpGet]
        public IQueryable<Reply> Replies()
        {
            return m_repository.Replies();
        }

    }
}