using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementApp.Pages.Admin;
using Xunit;

namespace StudentManagementApp.Tests.Pages.Admin;

public class QuizQuestionsModelTests
{
    [Fact]
    public void QuestionInputModel_allows_fill_in_blank_without_multiple_choice_options()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddOptions();
        services.AddRazorPages();

        var serviceProvider = services.BuildServiceProvider();
        var objectValidator = serviceProvider.GetRequiredService<IObjectModelValidator>();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor(),
            new ModelStateDictionary());

        var model = new QuizQuestionsModel.QuestionInputModel
        {
            QuestionText = "Tôi là ____.",
            QuestionType = "FillInBlank",
            Point = 10,
            FillInAnswer = "chó",
            OptionA = null,
            OptionB = null,
            OptionC = null,
            OptionD = null
        };

        objectValidator.Validate(actionContext, validationState: null, prefix: string.Empty, model: model);

        Assert.True(actionContext.ModelState.IsValid);
    }
}
