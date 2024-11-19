
using System.Reflection;
using Service.Security;
using Xunit;

namespace ProjectTests
{
    public class ProjectTests
    {
        public ProjectTests()
        {
            Console.WriteLine("Tests Starting...");
        }

        public async Task RunAllTests()
        {
            // Create an instance of your IJWTManager (or mock it)
            var jwtManager = new JWTManager(); // Or use a mock if necessary

            // Find all test classes in the Tests namespace
            var testClasses = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == "Tests.Tests" && t.GetMethods()
                    .Any(m => m.GetCustomAttributes(typeof(FactAttribute), false).Any()))
                .ToList();

            bool allTestsPassed = true;

            foreach (var testClass in testClasses)
            {
                // Create an instance of the test class and pass the jwtManager (or other dependencies)
                var constructor = testClass.GetConstructor(new[] { typeof(IJWTManager) });
                if (constructor == null)
                {
                    Console.WriteLine($"No suitable constructor found for {testClass.Name}. Skipping...");
                    continue; // Skip this class if no suitable constructor is found
                }

                var testInstance = Activator.CreateInstance(testClass, jwtManager);

                // Get all test methods marked with [Fact]
                var testMethods = testClass.GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(FactAttribute), false).Any())
                    .ToList();

                foreach (var testMethod in testMethods)
                {
                    try
                    {
                        Console.WriteLine($"Running {testClass.Name}.{testMethod.Name}...");

                        // Run the test method asynchronously
                        var task = (Task)testMethod.Invoke(testInstance, null);
                        await task;

                        Console.WriteLine($"Test {testClass.Name}.{testMethod.Name} passed.");
                    }
                    catch (Exception ex)
                    {
                        // If an exception is thrown (test fails), print the error
                        Console.WriteLine($"Test {testClass.Name}.{testMethod.Name} failed: {ex.Message}");
                        allTestsPassed = false;
                    }
                }
            }

            // If any test failed, throw an exception to stop the program
            if (!allTestsPassed)
            {
                throw new Exception("One or more tests failed. Stopping execution.");
            }
        }
    }
}