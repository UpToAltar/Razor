using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using Razor.Model;

#nullable disable

namespace Razor.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                });
            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Title", "Content", "Author", "CreatedAt", "UpdatedAt" },
                values: new object[] { "Title 1", "Content 1", "Author 1", new DateTime(2023,8,20), new DateTime(2023,8,20)}
            );
            
                
            //Seed Data
            Randomizer.Seed = new Random(8675309);
            var faker = new Faker<Article>()
                .RuleFor(a => a.Title, f => f.Lorem.Sentence(5, 5))
                .RuleFor(a => a.Content, f => f.Lorem.Paragraph())
                .RuleFor(a => a.Author, f => f.Name.FullName())
                .RuleFor(a => a.CreatedAt, f => f.Date.Past())
                .RuleFor(a => a.UpdatedAt, f => f.Date.Past());
            var articles = faker.Generate(100);
            for(int i = 0; i < articles.Count; i++)
            {
                migrationBuilder.InsertData(
                    table: "Article",
                    columns: new[] { "Title", "Content", "Author", "CreatedAt", "UpdatedAt" },
                    values: new object[] { articles[i].Title, articles[i].Content, articles[i].Author, articles[i].CreatedAt, articles[i].UpdatedAt });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article");
        }
    }
}
