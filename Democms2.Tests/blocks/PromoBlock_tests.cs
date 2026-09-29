using System.ComponentModel.DataAnnotations;
using Democms2.Models.Blocks;
using Xunit;

namespace Democms2.Tests.blocks;

public class PromoBlock_tests
{
    private static List<ValidationResult> Validate(PromoBlock block)
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
    public void PromoBlock_requires_title()
    {
        var block = new PromoBlock();

        var errors = Validate(block);

        Assert.Contains(
            errors,
            error => error.MemberNames.Contains(nameof(PromoBlock.Title)));
    }

    [Fact]
    public void PromoBlock_is_valid_when_title_is_provided()
    {
        var block = new PromoBlock
        {
            Title = "Important announcement"
        };

        var errors = Validate(block);

        Assert.Empty(errors);
    }

    [Fact]
    public void PromoBlock_stores_title()
    {
        var block = new PromoBlock
        {
            Title = "Special offer"
        };

        Assert.Equal("Special offer", block.Title);
    }

    [Fact]
    public void PromoBlock_stores_cta_text()
    {
        var block = new PromoBlock
        {
            Title = "Announcement",
            CTAText = "Read More"
        };

        Assert.Equal("Read More", block.CTAText);
    }

    [Fact]
    public void PromoBlock_stores_background_color()
    {
        var block = new PromoBlock
        {
            Title = "Announcement",
            BackgroundColor = "blue"
        };

        Assert.Equal("blue", block.BackgroundColor);
    }
}