using MessageBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.Tests.Fakes
{
    public class FakeMessageBoardRepository : IMessageBoardRepository
    {
        private Topic getFakeTopic(int id = 1)
        {
            return new Topic()
            {
                Id = id,
                Title = string.Format("Fake title for topic {0}", id),
                Body = string.Format("Fake body for topic {0}", id),
                Created = DateTime.UtcNow,
                Flagged = false
            };
        }

        public IQueryable<Topic> GetTopics()
        {
            return new Topic[]
            {
                getFakeTopic(1),
                getFakeTopic(2),
                getFakeTopic(3)
            }.AsQueryable();
        }

        public IQueryable<Topic> GetTopicsIncludingReplies()
        {
            return GetTopics();
        }

        public IQueryable<Reply> GetRepliesByTopic(int topicId)
        {
            return new Reply[]
            {
                new Reply() {
                    Body = "Fake body for reply",
                    Created = DateTime.UtcNow
                }
            }.AsQueryable();
        }

        public bool Save()
        {
            return true;
        }

        public bool AddTopic(Topic newTopic)
        {
            newTopic.Id = 4;

            return true;
        }

        public bool AddReply(Reply newReply)
        {
            return true;
        }

        public string Metadata
        {
            get { throw new NotImplementedException(); }
        }

        public Breeze.ContextProvider.SaveResult SaveChanges(Newtonsoft.Json.Linq.JObject saveBundle)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Topic> Topics()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Reply> Replies()
        {
            throw new NotImplementedException();
        }
    }
}
