using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace ScoreTracking.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SampleController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new[] { "value1", "value2" };
    }

    [HttpGet("{id:int}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    [HttpPut("{id:int}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
    }
}