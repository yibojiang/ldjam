using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using TMPro;

public class PlayerController : Controller
{
    public int playerId;

    protected CharacterActions characterActions;

    // Start is called before the first frame update
    void Start()
    {
        characterActions = new CharacterActions();

        if (playerId == 0)
        {
            characterActions.Up.AddDefaultBinding( Key.W );
            characterActions.Up.AddDefaultBinding( InputControlType.DPadUp );

            characterActions.Down.AddDefaultBinding( Key.S );
            characterActions.Down.AddDefaultBinding( InputControlType.DPadDown );
            
            characterActions.Sprint.AddDefaultBinding( Key.D );
            characterActions.Sprint.AddDefaultBinding( InputControlType.DPadRight );

            characterActions.Jump.AddDefaultBinding( Key.F );
            characterActions.Jump.AddDefaultBinding( InputControlType.Action2 );
            
            characterActions.Attack.AddDefaultBinding( Key.G );
            characterActions.Attack.AddDefaultBinding( InputControlType.Action3 );
        }
        else if (playerId == 1)
        {
            characterActions.Up.AddDefaultBinding( Key.UpArrow );
            characterActions.Up.AddDefaultBinding( InputControlType.DPadUp );

            characterActions.Down.AddDefaultBinding( Key.DownArrow );
            characterActions.Down.AddDefaultBinding( InputControlType.DPadDown );
            
            characterActions.Sprint.AddDefaultBinding( Key.RightArrow );
            characterActions.Sprint.AddDefaultBinding( InputControlType.Action1 );

            characterActions.Jump.AddDefaultBinding( Key.Period );
            characterActions.Jump.AddDefaultBinding( InputControlType.Action2 );
            
            characterActions.Attack.AddDefaultBinding( Key.Slash );
            characterActions.Attack.AddDefaultBinding( InputControlType.Action3 );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (status == PlayerStatus.Unborn)
        {
            // Hit any key to spawn the player character
            if (characterActions.Up.WasPressed || characterActions.Down.WasPressed ||
                characterActions.Sprint.WasPressed || characterActions.Jump.WasPressed ||
                characterActions.Attack.WasPressed)
            {
                // Set the player lane base on the player id
                SpawnPlayer(playerId);
                status = PlayerStatus.Alive;
            }
        }
        else if (status == PlayerStatus.Alive)
        {
            if (character.alive == false)
            {
                status = PlayerStatus.Dead;
                return;
            }
        
            if (characterActions.Jump.WasPressed)
            {
                character.Jump();
            }

            if (characterActions.Sprint.WasPressed)
            {
                character.Sprint();
            }
        
            if (characterActions.Up.WasPressed)
            {
                character.ChangeLaneUp();
            }
        
            if (characterActions.Down.WasPressed)
            {
                character.ChangeLaneDown();
            }

            if (characterActions.Attack.WasPressed)
            {
                character.Attack();
            }
        }
    }
    
    protected override void SpawnPlayer(int inLaneId)
    {
        var laneId = inLaneId;
        Vector3 lanePosition = LaneManager.instance.Lanes[laneId].collider.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;
        GameObject newPlayer = Instantiate(characterPrefab, new Vector3(cameraPosition.x + 1f, lanePosition.y + 0.5f, lanePosition.z), Quaternion.identity);
        character = newPlayer.GetComponent<Character>();
        character.currentLane = laneId;
        character.GetComponent<SpriteRenderer>().color = playerColor;
        
        GameObject playerText = Instantiate(characterTextPrefab, Vector3.zero, Quaternion.identity);
        playerText.transform.SetParent(newPlayer.transform);
        playerText.transform.localPosition = new Vector3(0, 2, 0);
        playerText.transform.localScale = new Vector3(1, 1, 1);
        var textComponponent = playerText.GetComponent<TextMeshPro>();
        textComponponent.text = $"{inLaneId + 1}P";
        textComponponent.color = playerColor;
    }
}
