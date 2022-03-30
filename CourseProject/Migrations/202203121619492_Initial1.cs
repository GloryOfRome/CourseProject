namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "UpVote", c => c.Int(nullable: false));
            AddColumn("dbo.Answers", "DownVote", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "UpVote", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "DownVote", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "DownVote");
            DropColumn("dbo.Questions", "UpVote");
            DropColumn("dbo.Answers", "DownVote");
            DropColumn("dbo.Answers", "UpVote");
        }
    }
}
