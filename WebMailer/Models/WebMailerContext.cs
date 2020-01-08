using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebMailer.Models
{
    public class WebMailerContext : DbContext, IWebMailerContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<UserInformation> UsersInformation { get; set; }
        public DbSet<UserLocation> UsersLocations { get; set; }

        IQueryable<Location> IWebMailerContext.Locations
        {
            get { return Locations; }
        }

        IQueryable<Campaign> IWebMailerContext.Campaigns
        {
            get { return Campaigns; }
        }

        IQueryable<UserInformation> IWebMailerContext.UsersInformation
        {
            get { return UsersInformation; }
        }

        IQueryable<UserLocation> IWebMailerContext.UsersLocations
        {
            get { return UsersLocations; }
        }

        int IWebMailerContext.SaveChanges()
        {
            return SaveChanges();
        }

        Location IWebMailerContext.FindLocation(int id)
        {
            return Set<Location>().Find(id);
        }

        Campaign IWebMailerContext.FindCampaign(int id)
        {
            return Set<Campaign>().Find(id);
        }

        IEnumerable<GetCampaigns> IWebMailerContext.FindCampaignsByUserAndLocations(int UserId)
        {
            IEnumerable<GetCampaigns> campaigns = (from Campaign campaign in Campaigns
                                               join Location locations in Locations
                                               on campaign.LocationID equals locations.LocationID
                                               join UserLocation userLoc in UsersLocations
                                               on locations.LocationID equals userLoc.LocationId
                                               where userLoc.UserId == UserId
                                               select new GetCampaigns
                                               {
                                                   CampaignID = campaign.CampaignID,
                                                   CampaignName = campaign.CampaignName,
                                                   LocationName = locations.LocationName
                                               }).ToList();
            return campaigns;
        }

        IEnumerable<Campaigns> IWebMailerContext.FindCampaignsByLocation(int locationId)
        {
            IEnumerable<Campaigns> campaigns = (from Campaign campaign in Campaigns
                                               join Location location in Locations
                                               on campaign.LocationID equals location.LocationID
                                               where campaign.LocationID == locationId
                                               select new Campaigns
                                               {
                                                   CampaignName = campaign.CampaignName,
                                                   CampaignID = campaign.CampaignID,
                                               }).ToList();
            return campaigns;
        }

        UserInformation IWebMailerContext.FindUserInformation(int id)
        {
            return Set<UserInformation>().Find(id);
        }

        UserInformation IWebMailerContext.FindUserInformation(string name)
        {
            return Set<UserInformation>().FirstOrDefault(x => x.UserName == name);
        }

        UserLocation IWebMailerContext.FindUserLocation(int idUsr, int loc)
        {
            return Set<UserLocation>().Find(idUsr, loc);
        }

        T IWebMailerContext.Add<T>(T entity)
        {
            return Set<T>().Add(entity);
        }

        T IWebMailerContext.Remove<T>(T entity)
        {
            return Set<T>().Remove(entity);
        }

        void IWebMailerContext.RemoveRange(IEnumerable<UserLocation> entities)
        {
            UsersLocations.RemoveRange(entities);
        }

        void IWebMailerContext.RemoveRange(IEnumerable<Campaign> entities)
        {
            Campaigns.RemoveRange(entities);
        }
    }
}