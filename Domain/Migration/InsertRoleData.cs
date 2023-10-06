using AspNetCore.Identity.MongoDbCore;
using Domain.Entities;
using domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Mongo.Migration.Migrations.Database;
using Mongo.Migration.Migrations.Document;
using MongoDB.Bson;
using MongoDB.Driver;

namespace domain.Migration;

public class InsertRoleData 
{
    private readonly RoleManager<AppRole> _roleManager;

    public InsertRoleData(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }
 
    public static void Up(RoleManager<AppRole> roleManager)
    {
        List<AppRole> newroles = new List<AppRole>();

        AppRole khiAdmin = new AppRole();
        khiAdmin.Name = "KHI";
        khiAdmin.Description = "KHI Employee";
        khiAdmin.ConcurrencyStamp = DateTimeOffset.UtcNow.ToString();
        newroles.Add(khiAdmin);
        
        AppRole integratorAdmin = new AppRole();
        integratorAdmin.Name = "Integrator";
        khiAdmin.Description = "Integrator Employee";
        integratorAdmin.ConcurrencyStamp = DateTimeOffset.UtcNow.ToString();
        newroles.Add(integratorAdmin);
        
        AppRole clientAdmin = new AppRole();
        clientAdmin.Name = "Customer";
        khiAdmin.Description = "Customer Employee";
        clientAdmin.ConcurrencyStamp = DateTimeOffset.UtcNow.ToString();
        newroles.Add(clientAdmin);
        

    }

}