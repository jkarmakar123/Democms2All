using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using Democms2.Models.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Democms2.Tests.controllers;

public class KnowledgeBasePageIntegrationTests
{
    [Fact]
    public void Index_controller_has_index_action()
    {
        var method = typeof(KnowledgeBasePageController)
            .GetMethod("Index", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(method);
    }

    [Fact]
    public void Index_accepts_content_loader_dependency()
    {
        var constructor = typeof(KnowledgeBasePageController)
            .GetConstructor(new[] { typeof(IContentLoader) });

        Assert.NotNull(constructor);
    }
}
