using System.Threading.Tasks;

namespace FakebookNotifications.Domain
{
    interface INotifciationRepo
    {
        Task GetUser();
    }
}