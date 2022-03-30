namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionAndDateTimeToQuestionClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Description", c => c.String());
            AddColumn("dbo.Questions", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "DateTime");
            DropColumn("dbo.Questions", "Description");
        }
    }
}
