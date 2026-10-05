using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Jellyfin.Plugin.LocalIntros.Tests;

public class LocalIntrosControllerTests
{
    [Fact]
    public void LoadIntros_RejectsMissingPathWithBadRequest()
    {
        var missingPath = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());
        var controller = new LocalIntrosController(null!, NullLoggerFactory.Instance);

        var response = Assert.IsType<BadRequestObjectResult>(controller.LoadIntrosFromPath(missingPath));
        var details = Assert.IsType<ProblemDetails>(response.Value);

        Assert.Equal(400, details.Status);
        Assert.Contains(missingPath, details.Detail);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void LoadIntros_RejectsUnsetPathWithBadRequest(string path)
    {
        var controller = new LocalIntrosController(null!, NullLoggerFactory.Instance);

        var response = Assert.IsType<BadRequestObjectResult>(controller.LoadIntrosFromPath(path));
        var details = Assert.IsType<ProblemDetails>(response.Value);

        Assert.Equal(400, details.Status);
    }
}
