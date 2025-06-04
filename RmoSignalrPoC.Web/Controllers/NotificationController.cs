using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using RmoSignalrPoC.Web.Services;
using System;
using System.Threading.Tasks;

namespace RmoSignalrPoC.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationClient _notificationClient;

        public NotificationController(NotificationClient notificationClient)
        {
            _notificationClient = notificationClient;
        }

        // GET: /Notification
        public async Task<IActionResult> Index()
        {
            // Ensure connection is attempted before displaying the page
            await _notificationClient.ConnectAsync();
            
            // Pass the connection state to the view
            ViewData["ConnectionState"] = _notificationClient.ConnectionState.ToString();
            return View();
        }

        // POST: /Notification/Send
        [HttpPost]
        public async Task<IActionResult> Send(string text, string category)
        {
            // Send notification using the client service
            await _notificationClient.SendNotificationAsync("Admin", text, category);

            // Return to input page
            return RedirectToAction("Index");
        }
        
        // GET: /Notification/GetConnectionStatus
        [HttpGet]
        public IActionResult GetConnectionStatus()
        {
            return Json(new { status = _notificationClient.ConnectionState.ToString()});
        }
    }
}
