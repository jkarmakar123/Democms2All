using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using Democms2.Models.Pages;
using System;
using System.Text;
using Democms2.Models.ViewModels;


namespace Democms2.Jobs
{
    [ScheduledJob(
        DisplayName = "Content Staleness Checker",
         GUID ="deeb5a4e-c6bc-4a69-bd71-1fa60fb102bd"
        )] // Daily = 1 day
    public class ContentStalenessCheckerJob : ScheduledJobBase
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentRepository _contentRepository;
        private bool _stopSignaled;

        public ContentStalenessCheckerJob(
            IContentLoader contentLoader,
            IContentRepository contentRepository)
        {
            _contentLoader = contentLoader;
            _contentRepository = contentRepository;
            IsStoppable = true;
        }

        public override void Stop()
        {
            _stopSignaled = true;
        }


    public override string Execute()
{
    OnStatusChanged("Starting Content Staleness Checker...");

    var output = new StringBuilder();
    var cutoffDate = DateTime.Now.AddDays(-2);//DateTime.Now.AddDays(-180);
    var staleCount = 0;
    var scannedCount = 0;
    var errorCount = 0;

    try
    {
        // Find the KnowledgeBase page directly under StartPage
        var knowledgeBasePages = _contentRepository
            .GetChildren<KnowledgeBasePage>(ContentReference.StartPage);

        foreach (var knowledgeBase in knowledgeBasePages)
        {
            if (_stopSignaled) break;

            // Only get direct children (KnowledgeArticlePages) under KnowledgeBase
            var articles = _contentRepository
                .GetChildren<KnowledgeArticlePage>(knowledgeBase.ContentLink);

            foreach (var article in articles)
            {
                if (_stopSignaled)
                {
                    output.AppendLine("Job was stopped by user.");
                    break;
                }

                try
                {
                    scannedCount++;

                    var publishedDate = article.StartPublish ?? DateTime.MinValue;

                    if (publishedDate < cutoffDate && article.IsFeatured)
                    {
                        staleCount++;
                        var message = $"[STALE] Title: '{article.Name}' | Published: {publishedDate:yyyy-MM-dd}";
                        output.AppendLine(message);
                        OnStatusChanged(message);
                    }
                }
                catch (Exception ex)
                {
                    // Single article failure must not abort entire run
                    errorCount++;
                    output.AppendLine($"[ERROR] Failed to process '{article.Name}': {ex.Message}");
                }
            }
        }
    }
    catch (Exception ex)
    {
        output.AppendLine($"[FATAL] Unexpected error: {ex.Message}");
    }

    output.AppendLine("----------------------------------------");
    output.AppendLine($"Scan Complete.");
    output.AppendLine($"Total Articles Scanned : {scannedCount}");
    output.AppendLine($"Stale Featured Articles: {staleCount}");
    output.AppendLine($"Errors                 : {errorCount}");

    return output.ToString();
}
    
    }
}