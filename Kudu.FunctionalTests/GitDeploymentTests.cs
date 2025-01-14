﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Kudu.Client;
using Kudu.Contracts.Settings;
using Kudu.Core;
using Kudu.Core.Deployment;
using Kudu.FunctionalTests.Infrastructure;
using Kudu.TestHarness;
using Kudu.TestHarness.Xunit;
using Xunit;

namespace Kudu.FunctionalTests
{
    [KuduXunitTestClass]
    public class AspNetAppOrchardTests : GitDeploymentTests
    {
        // ASP.NET apps

        [Fact]
        public void PushAndDeployAspNetAppOrchard()
        {
            PushAndDeployApps("Orchard", "master", "Welcome to Orchard", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class AspNetAppProjectWithNoSolutionTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployAspNetAppProjectWithNoSolution()
        {
            PushAndDeployApps("ProjectWithNoSolution", "master", "Project without solution", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class AspNetAppHiddenFoldersAndFilesTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployAspNetAppHiddenFoldersAndFiles()
        {
            PushAndDeployApps("HiddenFoldersAndFiles", "master", "Hello World", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class WebApiAppTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployWebApiApp()
        {
            PushAndDeployApps("Dev11_Net45_Mvc4_WebAPI", "master", "HelloWorld", HttpStatusCode.OK, "", resourcePath: "api/values", httpMethod: "POST", jsonPayload: "\"HelloWorld\"");
        }
    }

    [KuduXunitTestClass]
    public class AspNetAppWebSiteInSolutionTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployAspNetAppWebSiteInSolution()
        {
            PushAndDeployApps("WebSiteInSolution", "master", "SomeDummyLibrary.Class1", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class AspNetAppWebSiteInSolutionWithDeploymentFileTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployAspNetAppWebSiteInSolutionWithDeploymentFile()
        {
            PushAndDeployApps("WebSiteInSolution", "UseDeploymentFile", "SomeDummyLibrary.Class1", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class AspNetAppKuduGlobTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployAspNetAppKuduGlob()
        {
            PushAndDeployApps("kuduglob", "master", "ASP.NET MVC", HttpStatusCode.OK, "酷度");
        }
    }

    [KuduXunitTestClass]
    public class AspNetAppUnicodeNameTests : GitDeploymentTests
    {
        // Still needs more work in the deployment script to work
        // [Fact]
        public void PushAndDeployAspNetAppUnicodeName()
        {
            PushAndDeployApps("-benr-", "master", "Hello!", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class AspNetAppAppWithPostBuildEventTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployAspNetAppAppWithPostBuildEvent()
        {
            PushAndDeployApps("AppWithPostBuildEvent", "master", "Hello Kudu", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class FSharpWebApplicationTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployFSharpWebApplication()
        {
            PushAndDeployApps("fsharp-owin-sample", "master", "Owin Sample with F#", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class NodeAppExpressTests : GitDeploymentTests
    {
        [Fact]
        // Ensure node is installed. --> PrivateOnly
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployNodeAppExpress()
        {
            PushAndDeployApps("Express-Template", "master", "Modify this template to jump-start your Node.JS Express Web Pages application", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class NodeJsVS2017Tests : GitDeploymentTests
    {
        [Fact]
        // Ensure node is installed. --> PrivateOnly
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployNodeJsVS2017()
        {
            PushAndDeployApps("NodeJSVS17project", "master", "Hello World", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class Html5WithAppJsTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployHtml5WithAppJs()
        {
            PushAndDeployApps("Html5Test", "master", "html5", HttpStatusCode.OK, String.Empty);
        }
    }

    [KuduXunitTestClass]
    public class EFMVC45AppSqlCompactMAPEIATests : GitDeploymentTests
    {
        //Entity Framework 4.5 MVC Project with SQL Compact DB (.sdf file)
        //and Metadata Artifact Processing set to 'Embed in Assembly'
        [Fact]
        public void PushAndDeployEFMVC45AppSqlCompactMAPEIA()
        {
            PushAndDeployApps("MvcApplicationEFSqlCompact", "master", "Reggae", HttpStatusCode.OK, "");
        }
    }

    [KuduXunitTestClass]
    public class CustomDeploymentScriptShouldHaveDeploymentSettingTests : GitDeploymentTests
    {
        // Other apps

        [Fact]
        public async Task CustomDeploymentScriptShouldHaveDeploymentSetting()
        {
            // use a fresh guid so its impossible to accidently see the right output just by chance.
            var guidtext = Guid.NewGuid().ToString();
            var unicodeText = "酷度酷度";
            var normalVar = "TESTED_VAR";
            var normalVarText = "Settings Were Set Properly" + guidtext;
            var kuduSetVar = "KUDU_SYNC_CMD";
            var kuduSetVarText = "Fake Kudu Sync " + guidtext;
            var expectedLogFeedback = "Using custom deployment setting for {0} custom value is '{1}'.".FormatCurrentCulture(kuduSetVar, kuduSetVarText);

            string randomTestName = "CustomDeploymentScriptShouldHaveDeploymentSetting";
            await ApplicationManager.RunAsync(randomTestName, async appManager =>
            {
                appManager.SettingsManager.SetValue(normalVar, normalVarText).Wait();
                appManager.SettingsManager.SetValue(kuduSetVar, kuduSetVarText).Wait();

                // Act
                using (TestRepository testRepository = Git.Clone("CustomDeploymentSettingsTest"))
                {
                    appManager.GitDeploy(testRepository.PhysicalPath, "master");
                }
                var results = appManager.DeploymentManager.GetResultsAsync().Result.ToList();

                // Assert
                Assert.Equal(1, results.Count);
                Assert.Equal(DeployStatus.Success, results[0].Status);

                // Also validate custom script output supports unicode
                string[] expectedStrings = {
                    unicodeText,
                    normalVar + "=" + normalVarText,
                    kuduSetVar + "=" + kuduSetVarText,
                    expectedLogFeedback };
                KuduAssert.VerifyLogOutput(appManager, results[0].Id, expectedStrings);

                var ex = await Assert.ThrowsAsync<HttpUnsuccessfulRequestException>(() => appManager.DeploymentManager.GetDeploymentScriptAsync());
                Assert.Equal(HttpStatusCode.NotFound, ex.ResponseMessage.StatusCode);
                Assert.Contains("Operation only supported if not using a custom deployment script", ex.ResponseMessage.ExceptionMessage);
            });
        }
    }

    [KuduXunitTestClass]
    public class HelloKuduWithCorruptedGitTestsTests : GitDeploymentTests
    {
        [Fact]
        public async Task PushHelloKuduWithCorruptedGitTests()
        {
            const string randomTestName = "PushHelloKuduWithCorruptedGitTests";
            await ApplicationManager.RunAsync(randomTestName, async appManager =>
            {
                // Act
                using (TestRepository testRepository = Git.Clone("HelloKudu"))
                {
                    appManager.GitDeploy(testRepository.PhysicalPath);
                    var results = await appManager.DeploymentManager.GetResultsAsync();

                    // Assert
                    Assert.Equal(1, results.Count());
                    Assert.Equal(DeployStatus.Success, results.ElementAt(0).Status);

                    var content = await appManager.VfsManager.ReadAllTextAsync("site/repository/.git/HEAD");
                    Assert.Equal("ref: refs/heads/master", content.Trim());

                    // Corrupt the .git/HEAD file
                    appManager.VfsManager.WriteAllBytes("site/repository/.git/HEAD", new byte[23]);
                    content = await appManager.VfsManager.ReadAllTextAsync("site/repository/.git/HEAD");
                    Assert.Equal('\0', content[0]);

                    testRepository.WriteFile("somefile.txt", String.Empty);
                    Git.Commit(testRepository.PhysicalPath, "some commit");

                    var result = appManager.GitDeploy(testRepository.PhysicalPath);

                    content = await appManager.VfsManager.ReadAllTextAsync("site/repository/.git/HEAD");
                    Assert.Equal("ref: refs/heads/master", content.Trim());

                    results = await appManager.DeploymentManager.GetResultsAsync();

                    // Assert
                    Assert.Equal(2, results.Count());
                    Assert.Equal(DeployStatus.Success, results.ElementAt(0).Status);
                    Assert.Equal(DeployStatus.Success, results.ElementAt(1).Status);
                }
            });
        }
    }

    [KuduXunitTestClass]
    public class CustomGeneratorArgsTests : GitDeploymentTests
    {
        [Fact]
        public async Task CustomGeneratorArgs()
        {
            await ApplicationManager.RunAsync("UpdatedTargetPathShouldChangeDeploymentDestination", async appManager =>
            {
                // Even though it's a WAP, use custom script generator arguments to treat it as a web site,
                // deploying only its content folder
                await appManager.SettingsManager.SetValue(SettingsKeys.ScriptGeneratorArgs, "--basic -p MvcApplication14/content");

                using (TestRepository testRepository = Git.Clone("Mvc3AppWithTestProject"))
                {
                    appManager.GitDeploy(testRepository.PhysicalPath, "master");
                }
                var results = appManager.DeploymentManager.GetResultsAsync().Result.ToList();

                Assert.Equal(1, results.Count);
                Assert.Equal(DeployStatus.Success, results[0].Status);
                KuduAssert.VerifyUrl(appManager.SiteUrl + "themes/base/jquery.ui.accordion.css", ".ui-accordion-header");
            });
        }
    }

    [KuduXunitTestClass]
    public class UpdatedTargetPathShouldChangeDeploymentDestinationTests : GitDeploymentTests
    {
        [Fact]
        public void UpdatedTargetPathShouldChangeDeploymentDestination()
        {
            ApplicationManager.Run("UpdatedTargetPathShouldChangeDeploymentDestination", appManager =>
            {
                using (TestRepository testRepository = Git.Clone("TargetPathTest"))
                {
                    appManager.GitDeploy(testRepository.PhysicalPath, "master");
                }
                var results = appManager.DeploymentManager.GetResultsAsync().Result.ToList();

                Assert.Equal(1, results.Count);
                Assert.Equal(DeployStatus.Success, results[0].Status);
                KuduAssert.VerifyUrl(appManager.SiteUrl + "myTarget/index.html", "Target Path Test");
            });
        }
    }

    [KuduXunitTestClass]
    public class MVCAppWithLatestNugetTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployMVCAppWithLatestNuget()
        {
            PushAndDeployApps("MVCAppWithLatestNuget", "master", "MVCAppWithLatestNuget", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class MVCAppWithNuGetAutoRestoreTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployMVCAppWithNuGetAutoRestore()
        {
            PushAndDeployApps("MvcApplicationWithNuGetAutoRestore", "master", "MvcApplicationWithNuGetAutoRestore", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class MvcAppWithAutoRestoreFailsIfRestoreFailsTests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployMvcAppWithAutoRestoreFailsIfRestoreFails()
        {
            PushAndDeployApps("MvcApplicationWithNuGetAutoRestore", "bad-config", null, HttpStatusCode.OK, "Unable to find version '1.2.34' of package 'Package.That.Should.NotExist'.", DeployStatus.Failed);
        }
    }

    [KuduXunitTestClass]
    public class MvcAppWithTypeScriptTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployMvcAppWithTypeScript()
        {
            PushAndDeployApps("MvcAppWithTypeScript", "master", "Hello, TypeScript Footer!", HttpStatusCode.OK, "Deployment successful", resourcePath: "/Scripts/ts/FooterUpdater.js");
        }
    }

    [KuduXunitTestClass]
    public class PreviewWebApi5Tests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployPreviewWebApi5()
        {
            PushAndDeployApps("PreviewWebApi5", "master", "ASP.NET Preview WebAPI 5", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class PreviewSpa5Tests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployPreviewSpa5()
        {
            PushAndDeployApps("PreviewSpa5", "master", "Preview SPA 5", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class PreviewMvc5Tests : GitDeploymentTests
    {
        [Fact]
        public void PushAndDeployPreviewMvc5()
        {
            PushAndDeployApps("PreviewMvc5", "master", "ASP.NET Preview MVC5 App", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class ChakraMsieTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployChakraMsieTest()
        {
            PushAndDeployApps("ChakraMsieTest", "master", "170 - 2 = 168", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class AspNetCore21VS17WithLibTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployAspNetCore21VS17WithLib()
        {
            PushAndDeployApps("AspNetCore2.1.0VS17WithLib", "master", "DotNetCore210", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class AspNetCore20VS17WithLibTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployAspNetCore20VS17WithLib()
        {
            PushAndDeployApps("AspNetCore2.0.0VS17WithLib", "master", "DotNetCore200", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class AspNetCore10VS17WithLibTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployAspNetCore10VS17WithLib()
        {
            PushAndDeployApps("AspNetCore1.0.0VS17WithLib", "master", "DotNetCore100", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class AspNetCore11VS17WithLibTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployAspNetCore11VS17WithLib()
        {
            PushAndDeployApps("AspNetCore1.1.0VS17WithLib", "master", "DotNetCore110", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class AspNetCore2CliWithLibTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployAspNetCore2CliWithLib()
        {
            PushAndDeployApps("AspNetCore2.0CliWithLib", "master", "lib success", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class AspNetCore2CliTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployAspNetCore2Cli()
        {
            PushAndDeployApps("AspNetCore2.0.0Cli", "master", "Hello World!", HttpStatusCode.OK, "Deployment successful");
        }
    }

    [KuduXunitTestClass]
    public class AspNetCoreRC4WebApiVsSlnTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployAspNetCoreRC4WebApiVsSln()
        {
            PushAndDeployApps("AspNetCoreRC4WebApiVsSln", "master", "[\"classlibrary\",\"netstandard\"]", HttpStatusCode.OK, "Deployment successful", resourcePath: "/api/values");
        }
    }

    [KuduXunitTestClass]
    public class AspNetCore21WebApiCliTests : GitDeploymentTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public void PushAndDeployAspNetCore21WebApiCli()
        {
            PushAndDeployApps("AspNetCore2.1.0WebApiCli", "master", "[\"value1\",\"value2\"]", HttpStatusCode.OK, "Deployment successful", resourcePath: "/api/values");
        }
    }

    [KuduXunitTestClass]
    public class DumpAllAppTests
    {
        [Fact]
        [KuduXunitTest(PrivateOnly = true)]
        public async Task DumpAllAppTest()
        {
            // Arrange
            string appName = "DumpAllApp";
            using (var repo = Git.Clone(appName))
            {
                await ApplicationManager.RunAsync(appName, async appManager =>
                {
                    // Act
                    appManager.GitDeploy(repo.PhysicalPath);

                    // Assert
                    var results = (await appManager.DeploymentManager.GetResultsAsync()).ToList();
                    Assert.Equal(1, results.Count);
                    Assert.Equal(DeployStatus.Success, results[0].Status);

                    // Verify Content
                    var requestId = $"{Guid.NewGuid()}";
                    var expected = $"<li><strong>Request-Id</strong><span> = {requestId}</span></li>";
                    await KuduAssert.VerifyUrlAsync(appManager.SiteUrl, content: expected, headers: new[] { new NameValueHeaderValue("Request-Id", requestId) });
                });
            }
        }
    }

    public abstract class GitDeploymentTests
    {
        //Common code
        internal static void PushAndDeployApps(string repoCloneUrl, string defaultBranchName, string verificationText,
                                              HttpStatusCode expectedResponseCode, string verificationLogText,
                                              DeployStatus expectedStatus = DeployStatus.Success, string resourcePath = "",
                                              string httpMethod = "GET", string jsonPayload = "", bool deleteSCM = false)
        {
            using (new LatencyLogger("PushAndDeployApps - " + repoCloneUrl))
            {
                Uri uri;
                if (!Uri.TryCreate(repoCloneUrl, UriKind.Absolute, out uri))
                {
                    uri = null;
                }

                string randomTestName = uri != null ? Path.GetFileNameWithoutExtension(repoCloneUrl) : repoCloneUrl;
                ApplicationManager.Run(randomTestName, appManager =>
                {
                    // Act
                    using (TestRepository testRepository = Git.Clone(randomTestName, uri != null ? repoCloneUrl : null))
                    {
                        using (new LatencyLogger("GitDeploy"))
                        {
                            appManager.GitDeploy(testRepository.PhysicalPath, defaultBranchName);
                        }
                    }
                    var results = appManager.DeploymentManager.GetResultsAsync().Result.ToList();

                    // Assert
                    Assert.Equal(1, results.Count);
                    Assert.Equal(expectedStatus, results[0].Status);
                    var url = new Uri(new Uri(appManager.SiteUrl), resourcePath);
                    if (!String.IsNullOrEmpty(verificationText))
                    {
                        KuduAssert.VerifyUrl(url.ToString(), verificationText, expectedResponseCode, httpMethod, jsonPayload);
                    }
                    if (!String.IsNullOrEmpty(verificationLogText))
                    {
                        KuduAssert.VerifyLogOutput(appManager, results[0].Id, verificationLogText.Trim());
                    }
                    if (deleteSCM)
                    {
                        using (new LatencyLogger("SCMAndWebDelete"))
                        {
                            appManager.RepositoryManager.Delete(deleteWebRoot: false, ignoreErrors: false).Wait();
                        }
                    }
                });
            }
        }
    }
}
