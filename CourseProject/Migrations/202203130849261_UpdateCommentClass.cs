namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCommentClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CommentDetails", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "CommentDetails");
        }
    }
}
