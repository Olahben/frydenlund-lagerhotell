using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.FrontendModels;

public record ExchangeCodeForTokensRequest(string Code);
public record ExchangeCodeForTokensResponse(string AccessToken, string UserId);
