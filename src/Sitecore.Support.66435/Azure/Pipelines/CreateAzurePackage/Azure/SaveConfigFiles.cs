using Sitecore.Azure.Pipelines.BasePipeline;
using Sitecore.Azure.Pipelines.CreateAzurePackage;
using Sitecore.Diagnostics;
using Sitecore.IO;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sitecore.Support.Azure.Pipelines.CreateAzurePackage.Azure
{
  public class SaveConfigFiles : RolePipelineProcessor
  {
    protected override void Action(RolePipelineArgsBase arguments)
    {
      CreateAzureDeploymentPipelineArgs args = arguments as CreateAzureDeploymentPipelineArgs;
      Assert.IsNotNull(args, "args");
      DirectoryInfo sourceIncludeDir = args.SourceIncludeDir;
      sourceIncludeDir.Create();
      this.MoveSectionToIncludeFile("commands", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("mediaLibrary", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("icons", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("portraits", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("languageDefinitions", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("xamlsharp", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("fieldTypes", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("events", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("processors", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("analyticsExcludeRobots", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("settings", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("pipelines", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("contentSearch", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("scheduling", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("ui", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("databases", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("search", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("aggregation", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("sitecore.experienceeditor.speak.requests", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("marketingDefinitions", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("api", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("experienceAnalytics", sourceIncludeDir, args);
      this.MoveSectionToIncludeFile("social", sourceIncludeDir, args);
      args.WebConfig.Save(FileUtil.MakePath(args.TargetWebSiteFolder.FullName, "web.config", '\\'), SaveOptions.None);
      XAttribute connectionStringsPath = this.GetConnectionStringsPath(args);
      if (connectionStringsPath != null)
      {
        args.ConnectionString.Save(FileUtil.MapPath(FileUtil.MakePath(args.TargetWebSiteFolder.FullName, connectionStringsPath.Value)));
      }
    }

    private XAttribute GetConnectionStringsPath(CreateAzureDeploymentPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      XElement element1 = args.WebConfig.XPathSelectElement("configuration/connectionStrings");
      Assert.IsNotNull(element1, "connectionStringsPath");
      return element1.Attribute("configSource");
    }

    private void MoveSectionToIncludeFile(string nodename, DirectoryInfo includeDir, CreateAzureDeploymentPipelineArgs args)
    {
      Assert.ArgumentNotNull(nodename, "nodename");
      Assert.ArgumentNotNull(includeDir, "includeDir");
      Assert.ArgumentNotNull(args, "args");
      XDocument node = XDocument.Parse("<configuration xmlns:patch=\"http://www.sitecore.net/xmlconfig/\"><sitecore></sitecore></configuration>");
      XElement content = args.WebConfig.XPathSelectElement("./configuration/sitecore/" + nodename);
      if (content != null)
      {
        content.Remove();
        node.XPathSelectElement("./configuration/sitecore").Add(content);
        node.Save(FileUtil.MakePath(includeDir.FullName, nodename + ".config", '\\'));
      }
    }

    public override string OperationName
    {
      get
      {
        return "Saving configuration files";
      }
    }
  }


}
