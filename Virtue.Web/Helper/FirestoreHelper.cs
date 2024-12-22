using Google.Cloud.Firestore;

namespace Virtue.Web.Helper
{
    internal static class FirestoreHelper
    {
        static string fireconfig = @"
        {
          ""type"": ""service_account"",
          ""project_id"": ""virtue-e759c"",
          ""private_key_id"": ""3c2ba32788bec67a23e80dc09236e883c5810d22"",
          ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC7tnKta3ogFhQD\n/qwR6WKU+b5d9eJ3efxFqbYTkIX2Gs29hz+5SXuIdXoTsUCSUCiQfTrXhsm1XrcQ\nRjjgCm1dilNvTyp6ckPEj4c9UVjCf5QPI4gMQ+/LD6XziaYq6nCRho2MrcaIOC+V\n355VTZt3/szoVwr8krP+0Tr/8zxlebKPoawZ8pnzMfJJN1ga2kVi1wTyY+NE2rB7\n/mcOV4FvQhpOcPO4AlEHIDEoXvpKzBbSt5wZRfTfI6ilGxxFAKC+eaffQApdb5z5\niaVGpUmK4QPuM5wI28eC68/191T97FCTeKxSQjizDrP368NLRu6rSH6/FhBw26zv\nUUsH5XSpAgMBAAECggEAAPfJZ/5RJgc0+51bbretskE50d5YpTOabPFEN/NXS1yT\nnuDEQfO7HYoEeEOGc6VbGcqQ2hvJnlhEFYVVCvIrWhC57pyrKGetfz75JvMww+kM\nohAmo+fzX7jgmdqMUSKZzdyyfPSdgP2lcbPL88+4CMZMk0GXpRxMR04pBewtfq6V\nFB7RG1OeCmrPTmweexpHRCxAYdaXPXUdpdkbqg4v24ojTpjRRhfNpc9HMhA/a70l\n/Qdce9XO/n3bkLeqLgjIlPEtQHWQ5ztiMhRAcjEm+ksi5JKw28vz0eV6I7Zrs8je\nN2j4oF90Sg7uxJVxuUy+LbaditGa14yf498KRqgN8QKBgQD+5fN/WkbGdxhDJvzW\nsmcbJ/nBxet46jPN3tKI6nlJI+kRMJZnksNgJ9CFBJ45KCpBN2PZzwN4MBhLLwQh\nDT6x09+2usq1ILHicj1DiVN6rRISOfmumoNZYGWAh22R50/9ZpvxjOaYfyIjLLs+\nJR0zVQi1y3ZTZABKl+CIEmxohQKBgQC8hieqI+BS08kwjHe870X8/aqXZTSumQGP\nNdMzQ/L99+uPZzDapQK8VtuP2glDcX2g2S5aGt5ROcmeEtQXk3LIY2+gAdmpEujJ\nbg+72ravhRbSAZv7ACOxRFa1jolltEDQApm8IFqRkpnFbkE0o5zRxKdGLGEEpdSw\nGByp1g/m1QKBgQDFjaEXvfcGkH0MZqYlG+XPZT8sEGI8t39o+l4/4UIZDqzvNrIL\nGfM0jgcNYRPkBp4hJ3XK9KeiudvKQWk42JQTrLtBWyoKEYnskE+tDTzW4lYULDy4\nN2a/mXdxkwS3xQf4xFLudh6uaDIkAuIn+wnTo/75LvTJEsMrDOTncSf2JQKBgQCK\nEAleTZXLsrQFbOmHoQd1ZmQXKZPyRbVhpr58Lcf3fCezxpN9nBiry0RSThA4pxWk\nxUhvznt/qr1fmVRfy6jk3gVyfchkTKfUVFkLbvoZAnOa6njL0edRu8VwgulWZTKN\n40mo/1y0fVNFxh/Up/mBme4Ssw998uAW8iGTs2PD3QKBgAV1EIq3tvFU//KRyti1\n3geiDUQkTxkiB+nL98yiNBYfGIt4ouOw7VnDnIN+SwDxch9hN0eor9GapttGGAqr\nOKS2Xnc0IGvfDFWck1X2yLR2lZjCngnFGq1+U7/DsOalz+u7ZI8lv/oHw9TX3oxF\nMvOcPU+UPP2WyeV89Av94P/M\n-----END PRIVATE KEY-----\n"",
          ""client_email"": ""firebase-adminsdk-po3zn@virtue-e759c.iam.gserviceaccount.com"",
          ""client_id"": ""106314430774129572901"",
          ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
          ""token_uri"": ""https://oauth2.googleapis.com/token"",
          ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
          ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-po3zn%40virtue-e759c.iam.gserviceaccount.com"",
          ""universe_domain"": ""googleapis.com""
        }";

        private static string filepath = "";

        public static FirestoreDb? Database { get; private set; }

        public static void InitializeFirestore()
        {
            filepath = Path.Combine(Path.GetTempPath(), "firebase_credentials.json");
            if (!File.Exists(filepath))
            {
                File.WriteAllText(filepath, fireconfig);
                File.SetAttributes(filepath, FileAttributes.Hidden);
            }

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            Database = FirestoreDb.Create("virtue-e759c");
        }

        public static string GetCredentialFilePath() => filepath;
    }
}
