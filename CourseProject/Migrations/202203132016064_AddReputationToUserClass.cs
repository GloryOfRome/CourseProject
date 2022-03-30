namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReputationToUserClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Reputation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Reputation");
        }
    }
}
