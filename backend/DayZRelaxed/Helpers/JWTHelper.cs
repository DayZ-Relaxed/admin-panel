using JWT.Algorithms;
using JWT.Builder;



namespace DayZRelaxed.Helpers
{
   
    public class JWTHelper
    {
        string secret = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["JWTSecret"];

        public string Token { get; set; }

        public bool ValidateJWT()
        {
            try
            {
                var payload = JwtBuilder.Create()
                    .WithSecret(secret)
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(this.Token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IDictionary<string, object> DecodeJWT()
        {
            var payload = JwtBuilder.Create()
                    .WithSecret(secret)
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(this.Token);
            return payload;
        }
    }
}
