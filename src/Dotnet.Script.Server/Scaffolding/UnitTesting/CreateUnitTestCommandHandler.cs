using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dotnet.Script.Server.CQRS;

namespace Dotnet.Script.Server.Scaffolding.UnitTesting
{
    /// <summary>
    /// Handles the <see cref="CreateUnitTestCommand"/> and creates a new test script file in the <see cref="CreateUnitTestCommand.WorkingFolder"/>.
    /// </summary>
    public class CreateUnitTestCommandHandler : ICommandHandler<CreateUnitTestCommand>
    {
        /// <inheritdoc />
        public async Task HandleAsync(CreateUnitTestCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var template = TemplateLoader.ReadTemplate("UnitTesting.ScriptUnit.csx.template");
            var fileName = GetNextAvailableFilename(command.WorkingFolder);
            using (var streamWriter = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                await streamWriter.WriteAsync(template);
                await streamWriter.FlushAsync();
            }
        }

        private string GetNextAvailableFilename(string workingFolder)
        {
            string filename = Path.Combine(workingFolder, "UnitTests.csx");
            int count = 0;
            while (File.Exists(filename))
            {
                count++;
                filename = Path.Combine(workingFolder, $"UnitTests{count}.csx");
            }
            return filename;
        }
    }
}