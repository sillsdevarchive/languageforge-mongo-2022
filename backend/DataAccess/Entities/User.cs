﻿namespace LanguageForge.Api.Entities;

public class User : EntityDocument
{
    public required string Name { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
    public required bool Active { get; init; }
}
