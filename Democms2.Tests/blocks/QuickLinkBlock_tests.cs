using System.ComponentModel.DataAnnotations;
using Democms2.Models.Blocks;
using Xunit;

namespace Democms2.Tests.blocks;

public class QuickLinkBlockTests
{
    private static List<ValidationResult> Validate(QuickLinkBlock block)
    {
        var errors = new List<ValidationResult>();

        Validator.TryValidateObject(
            block,
            new ValidationContext(block),
            errors,
            validateAllProperties: true);

        return errors;
    }

    [Fact]
    public void QuickLinkBlock_requires_link_title()
    {
        var block = new QuickLinkBlock();

        var errors = Validate(block);

        Assert.Contains(
            nameof(QuickLinkBlock.LinkTitle),
            errors.SelectMany(error => error.MemberNames));
    }

    [Fact]
    public void QuickLinkBlock_is_valid_when_link_title_is_provided()
    {
        var block = new QuickLinkBlock
        {
            LinkTitle = "Knowledge Base"
        };

        var errors = Validate(block);

        Assert.Empty(errors);
    }

    [Fact]
    public void QuickLinkBlock_stores_link_title()
    {
        var block = new QuickLinkBlock
        {
            LinkTitle = "Contact Us"
        };

        Assert.Equal("Contact Us", block.LinkTitle);
    }

    [Fact]
    public void QuickLinkBlock_stores_description()
    {
        var block = new QuickLinkBlock
        {
            LinkTitle = "Resources",
            Description = "View all available resources."
        };

        Assert.Equal(
            "View all available resources.",
            block.Description);
    }

    [Fact]
    public void QuickLinkBlock_stores_icon_class()
    {
        var block = new QuickLinkBlock
        {
            LinkTitle = "Home",
            IconClass = "icon-home"
        };

        Assert.Equal("icon-home", block.IconClass);
    }
}