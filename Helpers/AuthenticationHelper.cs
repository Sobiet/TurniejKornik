﻿using System.Security.Cryptography;
using KornikTournament.Models;

namespace KornikTournament.Helpers;

public static class AuthenticationHelper
{
    private static RandomNumberGenerator rng = RandomNumberGenerator.Create();
    
    private static byte[] GenerateSalt(int size)
    {
        var salt = new byte[size];
        rng.GetBytes(salt);
        return salt;
    }

    public static string GenerateHash(string password, string salt)
    {
        var salt1 = Convert.FromBase64String(salt);

        using var hashGenerator = new Rfc2898DeriveBytes(password, salt1);
        hashGenerator.IterationCount = 10101;
        var bytes = hashGenerator.GetBytes(24);
        return Convert.ToBase64String(bytes);
    }

    public static void ProvideSaltAndHash(this Participant participant)
    {
        var salt = GenerateSalt(24);
        participant.Salt = Convert.ToBase64String(salt);
        participant.PasswordHash = GenerateHash(participant.PasswordHash, participant.Salt);
    }
}