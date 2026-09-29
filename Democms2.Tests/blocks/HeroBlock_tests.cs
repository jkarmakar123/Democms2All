using System.Reflection;
using Democms2.Models.Blocks;
using EPiServer.DataAnnotations;
using Xunit;

namespace Democms2.Tests.blocks;

public class HeroBlock_tests
{
    [Fact]
    public void HeroBlock_can_store_heading()
    {
        var hero = new HeroBlock
        {
            Heading = "Welcome to our website"
        };

        Assert.Equal("Welcome to our website", hero.Heading);
    }

    [Fact]
    public void HeroBlock_can_store_subtext()
    {
        var hero = new HeroBlock
        {
            SubText = "This is the hero description."
        };

        Assert.Equal("This is the hero description.", hero.SubText);
    }

    [Fact]
    public void HeroBlock_can_store_cta_text()
    {
        var hero = new HeroBlock
        {
            CTAButtonText = "Learn More"
        };

        Assert.Equal("Learn More", hero.CTAButtonText);
    }

    [Fact]
    public void HeroBlock_can_store_all_text_properties()
    {
        var hero = new HeroBlock
        {
            Heading = "Main heading",
            SubText = "Supporting text",
            CTAButtonText = "Read More"
        };

        Assert.Equal("Main heading", hero.Heading);
        Assert.Equal("Supporting text", hero.SubText);
        Assert.Equal("Read More", hero.CTAButtonText);
    }

    [Fact]
    public void HeroBlock_has_correct_content_type_name()
    {
        var contentType = typeof(HeroBlock)
            .GetCustomAttribute<ContentTypeAttribute>();

        Assert.NotNull(contentType);
        Assert.Equal("Hero Block", contentType!.DisplayName);
    }
}