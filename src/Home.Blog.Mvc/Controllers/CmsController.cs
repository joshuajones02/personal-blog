namespace Home.Blog.Mvc.Controllers;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Home.Blog.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using Piranha.AspNetCore.Services;
using Piranha.Models;

[ApiExplorerSettings(IgnoreApi = true)]
public class CmsController : Controller
{
    // TempData keys used for comment form round-tripping.
    internal const string CommentFormStateKey = "CommentFormState";
    internal const string CommentFormSuccessKey = "CommentFormSuccess";

    private readonly IApi _api;
    private readonly IModelLoader _loader;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="api">The current api</param>
    public CmsController(IApi api, IModelLoader loader)
    {
        _api = api;
        _loader = loader;
    }

    /// <summary>
    /// Gets the blog archive with the given id.
    /// </summary>
    [Route("archive")]
    public async Task<IActionResult> ArchiveAsync(Guid id, int? year = null, int? month = null, int? page = null,
        Guid? category = null, Guid? tag = null, bool draft = false)
    {
        try
        {
            var model = await _loader.GetPageAsync<StandardArchive>(id, HttpContext.User, draft);
            model.Archive = await _api.Archives.GetByIdAsync<PostInfo>(id, page, category, tag, year, month);

            return View(model);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    /// <summary>
    /// Gets the page with the given id.
    /// </summary>
    [Route("page")]
    public async Task<IActionResult> PageAsync(Guid id, bool draft = false)
    {
        try
        {
            var model = await _loader.GetPageAsync<StandardPage>(id, HttpContext.User, draft);

            return View(model);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    /// <summary>
    /// Gets the post with the given id.
    /// </summary>
    [Route("post")]
    public async Task<IActionResult> PostAsync(Guid id, bool draft = false)
    {
        try
        {
            var model = await _loader.GetPostAsync<StandardPost>(id, HttpContext.User, draft);

            if (model.IsCommentsOpen)
            {
                model.Comments = await _api.Posts.GetAllCommentsAsync(model.Id, true);
            }
            return View(model);
        }
        catch
        {
            return Unauthorized();
        }
    }

    /// <summary>
    /// Saves the given comment and then redirects to the post.
    /// On validation/spam failure, persists field errors + entered values via TempData
    /// so the Post view can re-render the form with messages.
    /// </summary>
    [HttpPost]
    [Route("post/comment")]
    public async Task<IActionResult> SavePostCommentAsync(SaveCommentModel commentModel)
    {
        StandardPost model;
        try
        {
            model = await _loader.GetPostAsync<StandardPost>(commentModel.Id, HttpContext.User);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }

        // Server-side validation via DataAnnotations.
        if (!ModelState.IsValid)
        {
            StashFormState(commentModel);
            return Redirect(model.Permalink + "#comment-form");
        }

        try
        {
            var comment = new PostComment
            {
                IpAddress = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = Request?.Headers?.ContainsKey("User-Agent") == true
                    ? Request.Headers["User-Agent"].ToString()
                    : "",
                Author = commentModel.CommentAuthor,
                Email = commentModel.CommentEmail,
                Url = commentModel.CommentUrl,
                Body = commentModel.CommentBody
            };
            await _api.Posts.SaveCommentAndVerifyAsync(commentModel.Id, comment);

            TempData[CommentFormSuccessKey] = "1";
            return Redirect(model.Permalink + "#comments");
        }
        catch (ValidationException vex)
        {
            // Piranha throws ValidationException when fields fail server-side rules
            // (empty body, blocked content, etc.). Surface as a form-level error.
            ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(vex.Message)
                ? "Your comment couldn't be posted. Please review and try again."
                : vex.Message);
            StashFormState(commentModel);
            return Redirect(model.Permalink + "#comment-form");
        }
    }

    /// <summary>
    /// Persists ModelState errors and submitted values to TempData so the
    /// next GET of the post page can repopulate the form.
    /// </summary>
    private void StashFormState(SaveCommentModel submitted)
    {
        var fieldErrors = ModelState
            .Where(kvp => kvp.Value != null && kvp.Value.Errors.Count > 0 && !string.IsNullOrEmpty(kvp.Key))
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.First().ErrorMessage);

        var formErrors = ModelState
            .Where(kvp => string.IsNullOrEmpty(kvp.Key) && kvp.Value != null && kvp.Value.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage))
            .ToList();

        var state = new StashedCommentFormState
        {
            Author = submitted.CommentAuthor,
            Email = submitted.CommentEmail,
            Url = submitted.CommentUrl,
            Body = submitted.CommentBody,
            FieldErrors = fieldErrors,
            FormErrors = formErrors
        };

        TempData[CommentFormStateKey] = JsonSerializer.Serialize(state);
    }

    /// <summary>
    /// Internal DTO serialized into TempData for the comment-form round trip.
    /// </summary>
    internal sealed class StashedCommentFormState
    {
        public string Author { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public System.Collections.Generic.Dictionary<string, string> FieldErrors { get; set; } = new();
        public System.Collections.Generic.List<string> FormErrors { get; set; } = new();
    }
}
