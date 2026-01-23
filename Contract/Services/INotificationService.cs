using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Services
{
  
    public interface INotificationService
    {
            Task NotifyTaskAssignedAsync(string userId, string taskTitle);
         
            Task<List<Notification>> GetMyNotificationsAsync(string userId);
            Task MarkReadAsync(int id);
            Task NotifyMentionsAsync(Dictionary<string, string> userMessages);
    }

}
