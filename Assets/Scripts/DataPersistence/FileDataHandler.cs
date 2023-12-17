using System.Collections.Generic;
using System;
using System.IO;
using Newtonsoft.Json;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public PlayerData Load(string profileId) {
        // base case - if the profileId is null, return right away
        if (profileId == null) {
            return null;
        }
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        PlayerData loadedData = null;
        if (File.Exists(fullPath)) {
            try {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from Json back into the C# object
                loadedData = JsonConvert.DeserializeObject<PlayerData>(dataToLoad);
            } catch (Exception e) {
                // Debug.LogError("Error occured when trying to load file at path: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(PlayerData data, string profileId) {
        // base case - if the profileId is null, return right away
        if (profileId == null) {
            return;
        }
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object into Json
            string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        } catch (Exception e) {
            // Debug.LogError("Error occured when trying to save file at path: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileId) {
        // base case - if the profileId is null, return right away
        if (profileId == null) {
            return;
        }
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try {
            // ensure the data file exists at this path before deleting the directory
            if (File.Exists(fullPath)) {
                // delete the profile folder and everything within it
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            } else {
                // Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
        } catch (Exception e) {
            // Debug.LogError("Failed to delete profile data for profileId: " + profileId + " at path: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, PlayerData> LoadAllProfiles() {
        Dictionary<string, PlayerData> profileDictionary = new Dictionary<string, PlayerData>();

        // loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos) {
            string profileId = dirInfo.Name;

            // defensive programming - check if the data file exists
            // if it doesn't, then this folder isn't a profile and should be skipped
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath)) {
                // Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            // load the game data for this profile and put it in the dictionary
            PlayerData profileData = Load(profileId);
            // defensive programming - ensure the profile data isn't null,
            // because if it is then something went wrong and we should let ourselves know
            if (profileData != null) {
                profileDictionary.Add(profileId, profileData);
            } else {
                // Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId() {
        string mostRecentProfileId = null;
        Dictionary<string, PlayerData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, PlayerData> pair in profilesGameData) {
            string profileId = pair.Key;
            PlayerData playerData = pair.Value;

            // skip this entry if the gamedata is null
            if (playerData == null) {
                continue;
            }

            // if this is the first data we've come across that exists, it's the most recent so far
            if (mostRecentProfileId == null) {
                mostRecentProfileId = profileId;
            }
            // otherwise, compare to see which date is the most recent
            else {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastPlayed);
                DateTime newDateTime = DateTime.FromBinary(playerData.lastPlayed);
                // the greatest DateTime value is the most recent
                if (newDateTime > mostRecentDateTime) {
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }
}
