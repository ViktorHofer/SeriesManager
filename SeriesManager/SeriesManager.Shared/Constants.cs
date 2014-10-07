namespace SeriesManager
{
    /// <summary>
    /// Constant key store like api key.
    /// Class needs to be partial so that git does not pushes the api key to the public repo
    /// </summary>
    partial class Constants
    {
        internal static readonly string ApiKey = "";
    }
}

// Second example part of partial class
// File must have the following name: *.Secret.cs or *.secret.cs (git ignores all files with that suffix)

//namespace SeriesManager
//{
//    partial class Constants
//    {
//        static Constants()
//        {
//            ApiKey = "REAL API KEY WHICH WILL NOT BE PUSHED TO GIT";
//        }
//    }
//}