using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMailer.Models
{
    interface IWebMailerContext
    {
        IQueryable<Location> Locations { get; }
        IQueryable<Campaign> Campaigns { get; }
        IQueryable<UserInformation> UsersInformation { get; }
        IQueryable<UserLocation> UsersLocations { get; }
        int SaveChanges();
        Location FindLocation(int id);
        Campaign FindCampaign(int id);
        IEnumerable<GetCampaigns> FindCampaignsByUserAndLocations(int UserId);
        IEnumerable<Campaigns> FindCampaignsByLocation(int locationId); 
        UserInformation FindUserInformation(int id);
        UserInformation FindUserInformation(string name);
        UserLocation FindUserLocation(int idUsr, int loc);
        T Add<T>(T entity) where T : class;
        T Remove<T>(T entity) where T : class;
        void RemoveRange(IEnumerable<UserLocation> entities);
        void RemoveRange(IEnumerable<Campaign> entities);

    }
}
