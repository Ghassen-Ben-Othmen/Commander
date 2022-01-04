using Data.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Utils.Utility;

namespace Services.Test
{
    public class BaseTest
    {
        protected readonly DbContextOptions<CommanderContext> _contextOptions;

        public BaseTest(DbContextOptions<CommanderContext> contextOptions)
        {
            _contextOptions = contextOptions;
            Seed();
        }

        private void Seed()
        {
            using var contex = new CommanderContext(_contextOptions);
            contex.Database.EnsureDeleted();
            contex.Database.EnsureCreated();

            var attachments = new List<Attachment>
            {
                new Attachment
                {
                    Name = "Test attachment PDF",
                    Type = AttachmentType.PDF
                },
                new Attachment
                {
                    Name = "Test attachment TXT",
                    Type = AttachmentType.TXT
                }
            };

            var argumentsDotNetCmd = new List<Argument>
            {
                new Argument { Value = "-c Release", Description = "--configuration <config> Runs with specified config" }
            };

            var argumentsDockerCmd = new List<Argument>
            {
                new Argument 
                { 
                    Value = "-p 8081:8080", 
                    Description = "Bind port to container hostPort:containerPort" 
                },
                new Argument 
                { 
                    Value = "-d", 
                    Description = "Run in detached mode" 
                }
            };

            var dotnetCommands = new List<Command>
            {
                new Command
                {
                    Title="dotnet core run command", 
                    Description="build and run dotnet project",  
                    Cmd = "dotnet run" , 
                    Arguments = argumentsDotNetCmd ,
                    Attachments = attachments
                },
            };

            var dockerCommands = new List<Command>
            {
                new Command
                {
                    Title="Docker run command", 
                    Description="Creates Docker container",  
                    Cmd = "docker run" , 
                    Arguments = argumentsDockerCmd 
                },
                new Command
                {
                    Title="Docker build command",
                    Description="Creates Docker image",
                    Cmd = "docker build . -t tag"
                },
                new Command
                {
                    Title="Docker ps command",
                    Description="list active containers",
                    Cmd = "docker ps -a"
                }
            };

            var platforms = new List<Platform>
            {
                new Platform { Title = "dotnet core", Commands = dotnetCommands },
                new Platform { Title = "Docker", Commands = dockerCommands },
                new Platform { Title = "ABC" }
            };

            contex.Platforms.AddRange(platforms);
            contex.SaveChanges();
        }
    }
}
