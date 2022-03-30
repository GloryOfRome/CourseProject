namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTagToQuestionClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Tag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Tag");
        }
    }
}
