using LogicBusiness.UseCase;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class AIModelFactory : IAIModelFactory
    {
        private readonly IConfiguration _config;

        public AIModelFactory(IConfiguration config)
        {
            _config = config;
        }

        public GenerativeModel Create()
        {
            var apiKey = _config["GoogleAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Missing GoogleAI:ApiKey in configuration.");

            var googleAI = new GoogleAI(apiKey);
            return googleAI.GenerativeModel(model: Model.Gemini25Flash);
        }
    }
}
