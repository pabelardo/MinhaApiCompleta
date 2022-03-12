﻿using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DevIO.Business.Intefaces;

public interface IUser
{
    string Name { get; }
    Guid GetUserId();
    string GetUserEmail();
    bool IsAuthenticated();
    bool IsInRole(string roleName);
    IEnumerable<Claim> GetClaimsIdentity();
}