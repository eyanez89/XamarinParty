using EfEnumToLookup.LookupGenerator;
using HangMan.Data.EFContext;
using System.Data.Entity.Migrations;

namespace HangMan.Data.Migrations
{
    public class Configuration : DbMigrationsConfiguration<HangManContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(HangManContext context)
        {
            var enumToLookup = new EnumToLookup()
            {
                TableNamePrefix = null
            };
            enumToLookup.Apply(context);
        }
    }
}