using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;

namespace ServiceBusTestWebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index(FormCollection form, string Send)
        {
            if (Send == "Send Message")
            {
                var sendMsg = form["txtSend"].ToString();
                var connectionString = "Endpoint=sb://skpqueuetest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VhayAssHzW/WDMZuzTnGBXpTyD0qbm+Fibnwjsoar/0=";
                var queueName = "qtest1";

                var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
                var message = new BrokeredMessage(sendMsg);
                client.Send(message);
                ViewBag.Msg = "Data Sent Suuceessfully.";
            }
            else
            {
                //getQueueValue1();
                getQueueValue2();
            }
            return View();
        }
        [HttpGet]
        public ActionResult Index2()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Index2")]
        public ActionResult Index2(FormCollection form, string Send)
        {
            if (Send == "Send Message")
            {
                var sendMsg = form["txtSend"].ToString();
                var connectionString = form["txtEndpoint"].ToString(); //"Endpoint=sb://skpqueuetest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VhayAssHzW/WDMZuzTnGBXpTyD0qbm+Fibnwjsoar/0=";
                var queueName = form["txtQueueName"].ToString(); //"qtest1";

                var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
                var message = new BrokeredMessage(sendMsg);
                client.Send(message);
                ViewBag.Msg = "Data Sent Suuceessfully.";
            }
            else
            {
                //getQueueValue1();
                getQueueValue2();
            }
            return View();
        }
        [NonAction]
        public void getQueueValue1()
        {
            var connectionString = "Endpoint=sb://skpqueuetest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VhayAssHzW/WDMZuzTnGBXpTyD0qbm+Fibnwjsoar/0=";
            var queueName = "qtest1";

            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
            var msg3 = "";
            client.OnMessage(message =>
            {
                msg3 = message.GetBody<String>();
                //Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
            });
            ViewBag.Qmsg = msg3;
        }

        [NonAction]
        public void getQueueValue2()
        {
            var connectionString = "Endpoint=sb://skpqueuetest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VhayAssHzW/WDMZuzTnGBXpTyD0qbm+Fibnwjsoar/0=";
            var queueName = "qtest1";

            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
            var msg3="";
           
            BrokeredMessage message = null;
            NamespaceManager namespaceManager = NamespaceManager.Create();
            while (true)
            {
                try
                {
                    //receive messages from Queue 
                    message = client.Receive(TimeSpan.FromSeconds(5));
                    if (message != null)
                    {
                        //Console.WriteLine(string.Format("Message received: Id = {0}, Body = {1}", message.MessageId, message.GetBody<string>()));
                        // Further custom message processing could go here... 
                        var t = message.MessageId;
                        msg3 = message.GetBody<string>();
                        message.Complete();
                        break;
                    }
                    else
                    {
                       
                        ViewBag.Msg = "No active message is avilable in service bus queue.";
                        //no more messages in the queue 
                        break;
                    }
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                    else
                    {
                        //HandleTransientErrors(e);
                    }
                }
            }
            client.Close();
            ViewBag.Qmsg2 = msg3;
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}