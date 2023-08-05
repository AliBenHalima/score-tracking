using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Providers
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
