using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CharacterToSprite
{
    public string name;
    public Sprite sprite;
}

[Serializable]
public struct MapRoom
{
    public string name;
    public int xCoordinate;
    public int yCoordinate;
}

[Serializable]
public struct Region
{
    public string name;
    public List<MapRoom> rooms;
}

public class MapMenu : MenuScreen
{
    public RoomData hoveredRoomData;
    public RoomCharacterDisplay characterFacePrefab;
    public GameObject charactersContainer;
    public List<RoomCharacterDisplay> characterFaces;
    public List<CharacterToSprite> charactersToSprites;
    private int currentRoomIndex;
    private int currentRegionIndex;
    List<MapRoom> rooms;
    public List<Region> regions;
    public AudioClip moveSelectionSound;
    public RectTransform roomListTransform;
    public List<ListItem> roomListUI;
    public Image locationPin;
    public ListItem listItem;

    public void SetRooms(List<MapRoom> rooms)
    {
        this.rooms = rooms;
        UpdateRoomList();
    }

    void UpdateRoomList()
    {
        foreach (ListItem roomItem in roomListUI)
        {
            Destroy(roomItem.gameObject);
        }
        
        roomListUI.Clear();
        
        foreach (MapRoom room in rooms)
        {
            ListItem instantiated = Instantiate(listItem);
            instantiated.SetText(room.name);
            instantiated.transform.SetParent(roomListTransform, false);
            roomListUI.Add(instantiated);
        }
    }
    
    public void OnRoomHovered(string roomName)
    {
        RoomData data = ProgressManager.instance.
            currentGameEvent.roomDatas.Find(x => x.room.name.Equals(roomName));
        hoveredRoomData = data;
        UpdateCharacters();
    }
    void GenerateCharacterFace(Sprite face)
    {
        RoomCharacterDisplay characterFace = Instantiate(characterFacePrefab, charactersContainer.transform);
        characterFace.SetCharacterSprite(face);
        characterFaces.Add(characterFace);
    }
    void UpdateCharacters()
    {
        foreach (RoomCharacterDisplay character in characterFaces)
        {
            Destroy(character.gameObject);
        }
        
        characterFaces.Clear();

        foreach (WorldCharacter character in hoveredRoomData.characters.worldCharacters)
        {
            CharacterToSprite characterToSprite = charactersToSprites.Find(x => x.name == character.name);
            GenerateCharacterFace(characterToSprite.sprite);
        }
    }

    void Awake()
    {
        SetRooms(regions[currentRegionIndex].rooms);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentRoomIndex = (currentRoomIndex + 1) % rooms.Count;
            UpdateUI();
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            currentRoomIndex = (currentRoomIndex - 1 + rooms.Count) %
                               rooms.Count;
            UpdateUI();
            SoundManager.instance.PlaySoundEffect(moveSelectionSound);
        } else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRegionIndex = (currentRegionIndex + 1) % regions.Count;
            currentRoomIndex = 0;
            UpdateRegion();
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            currentRegionIndex = (currentRegionIndex - 1 + regions.Count) %
                                 regions.Count;
            currentRoomIndex = 0;
            UpdateRegion();
        }
    }

    void UpdateRegion()
    {
        SetRooms(regions[currentRegionIndex].rooms);
        UpdateUI();
    }

    void UpdateUI()
    {
        string currentRoom = rooms.Count > 0 ? rooms[currentRoomIndex].name : null;
        if (currentRoom != null)
        {
            foreach (ListItem item in roomListUI)
            {
                item.SetHovered(false);
            }

            if (roomListUI.Count > 0)
            {
                roomListUI[currentRoomIndex].SetHovered(true);
                OnRoomHovered(currentRoom);
            }

            roomListTransform.anchoredPosition = new Vector2(0, Mathf.Max((currentRoomIndex - 5) * 91, 0));
        }
    }
}