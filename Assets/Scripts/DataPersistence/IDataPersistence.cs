using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(PlayerData playerData);
    void SaveData(PlayerData playerData);
}
