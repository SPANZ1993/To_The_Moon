using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerialization_Manager
{
    public bool checkForSavedData();
    public SaveGameObject loadSavedData();
    public void saveGameData();
    public void saveGameDataSerially();
    public void deleteSave();
}
