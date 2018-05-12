namespace Dotnet.Script.Server.Scaffolding.UnitTesting
{
    /// <summary>
    /// Represents creating a new unit test script file.
    /// </summary>
    public class CreateUnitTestCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUnitTestCommand"/> class.
        /// </summary>
        /// <param name="workingFolder">The current working folder for which to create the unit test file.</param>
        public CreateUnitTestCommand(string workingFolder)
        {
            WorkingFolder = workingFolder;
        }

        /// <summary>
        /// Gets the current working folder for which to create the unit test file.
        /// </summary>
        public string WorkingFolder { get; }

        public string PathToCreatedUnitTest { get; set; }
    }
}