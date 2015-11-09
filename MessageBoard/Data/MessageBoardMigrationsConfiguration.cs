using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace MessageBoard.Data
{
    public class MessageBoardMigrationsConfiguration
        : DbMigrationsConfiguration<MessageBoardContext>
    {
        public MessageBoardMigrationsConfiguration()
        {
            // Be wary of this in release mode
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MessageBoardContext context)
        {
            // Called every time a new app domain is created
            base.Seed(context);

            // Can create dummy data for debug purposes
#if DEBUG
            if (context.Topics.Count() == 0)
            {
                // Add some for test
                Topic topic = new Topic()
                {
                    Title = "Where am I",
                    Body = "I am a bit lost. Help.",
                    Created = DateTime.UtcNow,
                    Replies = new List<Reply>()
                    {
                        new Reply()
                        {
                            Body = "In Edinburgh",
                            Created = DateTime.UtcNow
                        },
                        new Reply()
                        {
                            Body = "In New York",
                            Created = DateTime.UtcNow
                        }
                    }
                };

                context.Topics.Add(topic);
                context.Topics.Add(new Topic()
                {
                    Title = "Apples are better than bananas",
                    Body = "So much better.",
                    Created = DateTime.UtcNow
                });

                context.SaveChanges();
            }
#endif
        }
    }
}
