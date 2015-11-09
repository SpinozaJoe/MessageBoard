using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBoard.Controllers;
using MessageBoard.Tests.Fakes;
using MessageBoard.Data;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using Newtonsoft.Json;

namespace MessageBoard.Tests.Controllers
{
    [TestClass]
    public class TopicsControllerTests
    {
        private TopicsController m_controller;

        [TestInitialize]
        public void Init()
        {
            m_controller = new TopicsController(new FakeMessageBoardRepository());
        }

        [TestMethod]
        public void TopicsController_Get()
        {
            IEnumerable<Topic> topics = m_controller.Get();

            Assert.IsNotNull(topics);
            Assert.IsTrue(topics.Count() == 3);
            Assert.IsNotNull(topics.First());
            Assert.IsNotNull(topics.First().Title);
        }

        [TestMethod]
        [TestCategory("Thing")]
        public void TopicsController_Post()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/Playground/MessageBoard/api/v1/topics");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary {{ "controller", "topics" }});

            m_controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            m_controller.Request = request;
            m_controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            Topic newTopic = new Topic()
            {
                Title = "A new topic",
                Body = "A body for the topic"
            };

            HttpResponseMessage response = m_controller.Post(newTopic);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var json = response.Content.ReadAsStringAsync().Result;
            var topic = JsonConvert.DeserializeObject<Topic>(json);

            Assert.IsNotNull(topic);
            Assert.IsTrue(topic.Id > 0);
            Assert.IsTrue(topic.Created > DateTime.MinValue);
        }
    }

}
