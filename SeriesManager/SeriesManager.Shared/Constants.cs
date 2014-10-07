namespace SeriesManager
{
    /// <summary>
    /// Constant key store like api key.
    /// Class needs to be partial so that git does not pushes the api key to the public repo
    /// </summary>
    partial class Constants
    {
        // Do not write the api key directly inside the quotes or otherwise your api key will be pushed too
        // while commiting to the repository (github). Do as described below!!!
        internal static readonly string ApiKey = "";
    }
}


// 1. Create a file called Constants.Secret.cs inside the root of SeriesManager.Shared
// 2. Paste the following code inside the file:

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

// 3. Git will automatically ignore this file because .gitignore contains a rule 
//    for all files with following names: *.[Ss]ecret.cs
// 4. Have fun ;-)