using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Umbraco.Docs.Preview.App.Models;
using Umbraco.Docs.Preview.App.Options;

namespace Umbraco.Docs.Preview.App.MiscellaneousOurStuff
{
    public class DocumentationUpdater
    {
        private readonly IOptions<UmbracoDocsOptions> _umbracoDocsSettings;

        public DocumentationUpdater(IOptions<UmbracoDocsOptions> umbracoDocsSettings)
        {
            if (umbracoDocsSettings != null) _umbracoDocsSettings = umbracoDocsSettings;
        }

        public UmbracoDocsTreeNode BuildSitemap()
        {
            var folder = new DirectoryInfo(_umbracoDocsSettings.Value.UmbracoDocsRootFolder);
            return GetFolderStructure(folder, folder.FullName, 0);
        }

        private UmbracoDocsTreeNode GetFolderStructure(DirectoryInfo dir, string rootPath, int level)
        {
            var list = new List<UmbracoDocsTreeNode>();

            var siteMapItem = new UmbracoDocsTreeNode
            {
                Name = dir.Name.Replace("-", " "),
                Path = dir.FullName.Substring(rootPath.Length).Replace('\\', '/'),
                Level = level,
                Sort = GetSort(dir.Name, level) ?? 100,
                Directories = list,
                PhysicalPath = dir.ToString(),
            };

            var ignored = new HashSet<string>
            {
                "images", 
                ".git", 
                ".github", 
                "Old-Courier-versions"
            };

            foreach (var child in dir.GetDirectories().Where(x => !ignored.Contains(x.Name)))
                list.Add(GetFolderStructure(child, rootPath, level + 1));

            siteMapItem.Directories = list.OrderBy(x => x.Sort).ToList();

            return siteMapItem;
        }

        private int? GetSort(string name, int level)
        {
            switch (level)
            {
                case 1:
                    switch (name.ToLowerInvariant())
                    {
                        case "getting-started":
                            return 0;
                        case "fundamentals":
                            return 1;
                        case "implementation":
                            return 2;
                        case "extending":
                            return 3;
                        case "reference":
                            return 4;
                        case "tutorials":
                            return 5;
                        case "add-ons":
                            return 6;
                        case "umbraco-uno":
                            return 7;
                        case "umbraco-cloud":
                            return 8;
                        case "umbraco-heartcore":
                            return 9;
                        case "contribute":
                            return 10;
                    }
                    break;

                case 2:
                    switch (name.ToLowerInvariant())
                    {
                        //Getting-Started
                        case "managing-an-umbraco-project":
                            return 0;
                        case "editing-websites-with-umbraco":
                            return 1;
                        case "creating-websites-with-umbraco":
                            return 2;
                        case "developing-websites-with-umbraco":
                            return 3;
                        case "hosting-an-umbraco-infrastructure":
                            return 4;
                        case "where-can-i-get-help":
                            return 5;

                        //Fundamentals
                        case "setup":
                            return 0;
                        case "backoffice":
                            return 1;
                        case "data":
                            return 2;
                        case "design":
                            return 3;
                        case "code":
                            return 4;

                        //Implementation
                        case "default-routing":
                            return 0;
                        case "custom-routing":
                            return 1;
                        case "controllers":
                            return 2;
                        case "data-persistence":
                            return 3;
                        case "rest-api":
                            return 4;

                        //Extending
                        case "dashboards":
                            return 0;
                        case "section-trees":
                            return 1;
                        case "property-editors":
                            return 2;
                        case "macro-parameter-editors":
                            return 3;
                        case "healthcheck":
                            return 4;
                        case "language-files":
                            return 5;

                        //Reference
                        case "config":
                            return 0;
                        case "templating":
                            return 1;
                        case "querying":
                            return 2;
                        case "routing":
                            return 3;
                        case "searching":
                            return 4;
                        case "events":
                            return 5;
                        case "management":
                            return 6;
                        case "plugins":
                            return 7;
                        case "cache":
                            return 8;
                        case "packaging":
                            return 9;
                        case "security":
                            return 10;
                        case "common-pitfalls":
                            return 11;

                        //Tutorials
                        case "creating-basic-site":
                            return 0;
                        case "creating-a-custom-dashboard":
                            return 1;
                        case "creating-a-property-editor":
                            return 2;
                        case "multilanguage-setup":
                            return 3;
                        case "starter-kit":
                            return 4;
                        case "editors-manual":
                            return 5;

                        //Add ons
                        case "umbracoforms":
                            return 0;
                        case "umbraco-deploy":
                            return 1;
                        case "umbracocourier":
                            return 2;

                        //Umbraco Cloud
                        case "getting-started":
                            return 0;
                        case "set-up":
                            return 1;
                        case "deployment":
                            return 2;
                        case "databases":
                            return 3;
                        case "upgrades":
                            return 4;
                        case "troubleshooting":
                            return 5;
                        case "frequently-asked-questions":
                            return 6;

                        //Umbraco Heartcore
                        case "getting-started-cloud":
                            return 0;
                        case "api-documenation":
                            return 1;
                        case "client-libraries":
                            return 2;
                        case "versions-and-updates":
                            return 3;
                    }
                    break;

                case 3:
                    switch (name.ToLowerInvariant())
                    {
                        //Fundamentals - Setup
                        case "requirements":
                            return 0;
                        case "install":
                            return 1;
                        case "upgrading":
                            return 2;
                        case "server-setup":
                            return 3;

                        //Fundamentals - Backoffice
                        case "sections":
                            return 0;
                        case "property-editors":
                            return 1;
                        case "login":
                            return 2;

                        //fundamentals - Data
                        case "defining-content":
                            return 0;
                        case "creating-media":
                            return 1;
                        case "members":
                            return 2;
                        case "data-types":
                            return 3;
                        case "scheduled-publishing":
                            return 4;

                        //Fundamentals - Design
                        case "templates":
                            return 0;
                        case "rendering-content":
                            return 1;
                        case "rendering-media":
                            return 2;
                        case "stylesheets-javascript":
                            return 3;

                        //Fundamentals - Code
                        case "umbraco-services":
                            return 0;
                        case "subscribing-to-events":
                            return 1;
                        case "creating-forms":
                            return 2;

                        //Implementation - Default Routing
                        case "inbound-pipeline":
                            return 0;
                        case "controller-selection":
                            return 1;
                        case "execute-request":
                            return 2;

                        //Reference - Config
                        case "webconfig":
                            return 0;
                        case "404handlers":
                            return 1;
                        case "applications":
                            return 2;
                        case "embeddedmedia":
                            return 3;
                        case "examineindex":
                            return 4;
                        case "examinesettings":
                            return 5;
                        case "filesystemproviders":
                            return 6;
                        case "baserestextensions":
                            return 7;
                        case "tinymceconfig":
                            return 8;
                        case "trees":
                            return 9;
                        case "umbracosettings":
                            return 10;
                        case "dashboard":
                            return 11;
                        case "healthchecks":
                            return 12;

                        //Reference - Templating
                        case "mvc":
                            return 0;
                        case "masterpages":
                            return 1;
                        case "macros":
                            return 2;
                        case "modelsbuilder":
                            return 3;

                        //Reference - Querying
                        case "ipublishedcontent":
                            return 0;
                        case "dynamicpublishedcontent":
                            return 1;
                        case "umbracohelper":
                            return 2;
                        case "membershiphelper":
                            return 3;

                        //Reference - Routing
                        case "authorized":
                            return 0;
                        case "request-pipeline":
                            return 1;
                        case "webapi":
                            return 2;
                        case "iisrewriterules":
                            return 3;
                        case "url-tracking":
                            return 4;

                        //Tutorials - Basic site from scratch
                        case "getting-started":
                            return 0;
                        case "document-types":
                            return 1;
                        case "creating-your-first-template-and-content-node":
                            return 2;
                        case "css-and-images":
                            return 3;
                        case "displaying-the-document-type-properties":
                            return 4;
                        case "creating-master-template-part-1":
                            return 5;
                        case "creating-master-template-part-2":
                            return 6;
                        case "setting-the-navigation-menu":
                            return 7;
                        case "articles-parent-and-article-items":
                            return 8;
                        case "adding-language-variants":
                            return 9;
                        case "conclusions-where-next":
                            return 10;

                        //Tutorials - Editor Manual
                        case "introduction":
                            return 0;
                        case "getting-started-with-umbraco":
                            return 1;
                        case "working-with-content":
                            return 2;
                        case "version-management":
                            return 3;
                        case "media-management":
                            return 4;
                        case "tips-and-tricks":
                            return 5;

                        //Add Ons - UmbracoForms
                        case "installation":
                            return 0;
                        case "editor":
                            return 1;
                        case "developer":
                            return 2;

                        //Add ons - Umbraco Deploy
                        case "get-started-with-deploy":
                            return 0;
                        case "installing-deploy":
                            return 1;
                        case "deployment-workflow":
                            return 2;
                        case "deploy-settings":
                            return 3;
                        case "upgrades":
                            return 4;
                        case "troubleshooting":
                            return 5;

                        //Add ons - UmbracoCourier
                        case "architechture":
                            return 1;

                        //Umbraco Cloud - Getting Started
                        case "project-overview":
                            return 0;
                        case "environments":
                            return 1;
                        case "the-umbraco-cloud-portal":
                            return 2;
                        case "baselines":
                            return 3;
                        case "migrate-existing-site":
                            return 4;

                        //Umbraco Cloud - Set Up
                        case "working-locally":
                            return 0;
                        case "visual-studio":
                            return 1;
                        case "working-with-visual-studio":
                            return 2;
                        case "working-with-uaas-cli":
                            return 3;
                        case "project-settings":
                            return 4;
                        case "team-members":
                            return 5;
                        case "media":
                            return 6;
                        case "smtp-settings":
                            return 7;
                        case "manage-hostnames":
                            return 8;
                        case "config-transforms":
                            return 9;
                        case "power-tools":
                            return 10;

                        //Umbraco Cloud - Deployment
                        case "local-to-cloud":
                            return 0;
                        case "cloud-to-cloud":
                            return 1;
                        case "content-transfer":
                            return 2;
                        case "restoring-content":
                            return 3;
                        case "deployment-webhook":
                            return 4;

                        //Umbraco Cloud - Troubleshooting
                        case "deployments":
                            return 0;
                        case "log-files":
                            return 1;
                        case "faq":
                            return 2;
                        case "courier":
                            return 3;

                    }
                    break;
            }
            return null;
        }
    }
}
