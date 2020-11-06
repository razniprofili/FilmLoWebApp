using Domain;
using FilmLoApp.API.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Helpers
{
    public static class SecurityHelper
    {
        public static string SecretKey { get; set; } // definisan u appsettings.json kao neki random string

        public static T Decode<T>(string value)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, urlEncoder);

            return decoder.DecodeToObject<T>(value);

        }

        public static UserJwtModel CreateLoginToken(User user)
        {
            var userJwtModel = new UserJwtModel
            {
                Id = user.Id,
                ExpirationTime = DateTime.UtcNow.AddMinutes(30), // token vazi 30 min
                Name = user.Name,
                Surname = user.Surname,
                Mail = user.Email
            };
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(new HMACSHA256Algorithm(), serializer, urlEncoder);

            userJwtModel.Token = encoder.Encode(userJwtModel, SecretKey);
            return userJwtModel;

        }
    }
}
