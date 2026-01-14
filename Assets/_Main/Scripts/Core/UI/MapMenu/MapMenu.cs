using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DIALOGUE;
using TMPro;
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
    private int currentRoomIndex;
    private int currentRegionIndex;
    List<MapRoom> rooms;
    public List<Region> regions;
    public AudioClip moveSelectionSound;
    public RectTransform roomListTransform;
    public List<ListItem> roomListUI;
    public Image locationPin;
    public ListItem listItem;
    public UILine line;
    public RectTransform self;
    public RectTransform locationPinTransform;
    public TextMeshProUGUI regionText;
    public MenuScreenContainer container;
    public AudioClip selectSound;
    public AudioClip failSound;
    public RectTransform dialogueContainer;
    public GameObject noPeopleMessage;

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

        UpdateUI();
    }

    private void OnRoomHovered(MapRoom room)
    {
        RoomData data = ProgressManager.instance.currentGameEvent.roomDatas.Find(x => x.room.name.Equals(room.name));
        hoveredRoomData = data;
        UpdateCharacters();
        locationPin.rectTransform.anchoredPosition = new Vector2(room.xCoordinate, room.yCoordinate);
        if (room.name.Equals(WorldManager.instance.currentRoom.name))
        {
            locationPin.DOFade(0f, 0f).SetUpdate(true);
            line.gameObject.SetActive(false);
        }
        else
        {
            locationPin.DOFade(1f, 0f).SetUpdate(true);
            if (regions[currentRegionIndex].rooms
                .Any((room) => room.name.Equals(WorldManager.instance.currentRoom.name)))
            {
                line.gameObject.SetActive(true);
                line.SetPositions(self.anchoredPosition, locationPinTransform.anchoredPosition);
            }
        }
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

        if (hoveredRoomData != null && hoveredRoomData.characters != null)
        {
            foreach (WorldCharacter character in hoveredRoomData.characters.worldCharacters)
            {
                GenerateCharacterFace(character.character.faceSprite);
            }

            noPeopleMessage.SetActive(false);
        }
        else
        {
            noPeopleMessage.SetActive(true);
        }
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
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentRegionIndex = (currentRegionIndex + 1) % regions.Count;
            currentRoomIndex = 0;
            UpdateRegion();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentRegionIndex = (currentRegionIndex - 1 + regions.Count) %
                                 regions.Count;
            currentRoomIndex = 0;
            UpdateRegion();
        }
        else if (PlayerInputManager.instance.DefaultInput())
        {
            Room roomToLoad = ProgressManager.instance.currentGameEvent.roomDatas.Find((roomData) =>
                roomData.room.name.Equals(rooms[currentRoomIndex].name))?.room;
            if (roomToLoad != null && roomToLoad.name != WorldManager.instance.currentRoom.name)
            {
                SelectRoom(roomToLoad);
            }
            else
            {
                FailSelect();
            }
        }
    }

    void SelectRoom(Room roomToLoad)
    {
        SoundManager.instance.PlaySoundEffect(selectSound);
        container.ClosePauseScreen();
        WorldManager.instance.StartLoadingRoom(roomToLoad, null);
    }

    void FailSelect()
    {
        SoundManager.instance.PlaySoundEffect(failSound);
        dialogueContainer.DOKill();
        dialogueContainer.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() =>
        {
            dialogueContainer.DOScale(0f, 0.4f).SetEase(Ease.InBack).SetDelay(0.5f).SetUpdate(true);
        });
    }

    void UpdateRegion()
    {
        regionText.text = regions[currentRegionIndex].name;
        SetRooms(regions[currentRegionIndex].rooms);
        int index = regions[currentRegionIndex].rooms
            .FindIndex((curr) => curr.name.Equals(WorldManager.instance.currentRoom.name));
        if (index != -1)
        {
            self.gameObject.SetActive(true);
            currentRoomIndex = index;
        }
        else
        {
            line.gameObject.SetActive(false);
            self.gameObject.SetActive(false);
            currentRoomIndex = 0;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (rooms.Count > 0)
        {
            MapRoom currentRoom = rooms[currentRoomIndex];

            foreach (ListItem item in roomListUI)
            {
                item.SetHovered(false);
            }

            roomListUI[currentRoomIndex].SetHovered(true);
            OnRoomHovered(currentRoom);

            roomListTransform.anchoredPosition = new Vector2(0, Mathf.Max((currentRoomIndex - 5) * 91, 0));
        }
    }

    public override void Open()
    {
        base.Open();
        Room currentRoom = WorldManager.instance.currentRoom;
        currentRegionIndex = regions.FindIndex((region) =>
            region.rooms.Any((room) => room.name.Equals(currentRoom.name)));
        UpdateRegion();
        currentRoomIndex = rooms.FindIndex((room) => room.name.Equals(currentRoom.name));
        MapRoom currentMapRoom = rooms[currentRoomIndex];
        self.anchoredPosition = new Vector2(currentMapRoom.xCoordinate, currentMapRoom.yCoordinate);
    }
}