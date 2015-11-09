using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageBoard.Data
{
    public class MessageBoardRepository : IMessageBoardRepository
    {
        private MessageBoardContext m_context;
        private readonly EFContextProvider<MessageBoardContext> m_contextProvider = new EFContextProvider<MessageBoardContext>();

        public MessageBoardRepository(MessageBoardContext context)
        {
            m_context = context;
        }

        public IQueryable<Topic> GetTopics()
        {
            return m_context.Topics;
        }

        public IQueryable<Reply> GetRepliesByTopic(int topicId)
        {
            return m_context.Replies.Where(r => r.TopicId == topicId);
        }

        public IQueryable<Topic> GetTopicsIncludingReplies()
        {
            return m_context.Topics.Include("Replies");
        }

        public bool Save()
        {
            try
            {
                return m_context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                // TODO: log this
                return false;
            }
        }

        public bool AddTopic(Topic newTopic)
        {
            try
            {
                m_context.Topics.Add(newTopic);

                return true;
            }
            catch (Exception)
            {
                // TODO: log this
                return false;
            }
        }

        public bool AddReply(Reply newReply)
        {
            try
            {
                m_context.Replies.Add(newReply);

                return true;
            }
            catch (Exception)
            {
                // TODO: log this
                return false;
            }
        }


        //Breeze stuff below:

        public string Metadata
        {
            get { return m_contextProvider.Metadata(); }
        }

        public SaveResult SaveChanges(JObject saveBundle)
        {
            return m_contextProvider.SaveChanges(saveBundle);
        }

        public IQueryable<Topic> Topics()
        {
            return m_contextProvider.Context.Topics;
        }

        public IQueryable<Reply> Replies()
        {
            return m_contextProvider.Context.Replies;
        }
    }
}