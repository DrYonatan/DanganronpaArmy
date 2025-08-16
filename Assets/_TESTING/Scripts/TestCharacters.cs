// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DIALOGUE;
// using CHARACTERS;
//
// namespace TESTING
// {
//     public class TestCharacters : MonoBehaviour
//     {
//         // Start is called before the first frame update
//         void Start()
//         {
//             StartCoroutine(Test());
//         }
//
//         IEnumerator Test()
//         {
//             Character Protagonist = CharacterManager.instance.CreateCharacter("Protagonist");
//             yield return new WaitForSeconds(1f);
//             yield return Protagonist.Hide();
//             yield return new WaitForSeconds(1f);
//             yield return Protagonist.Show();
//
//             yield return Protagonist.Say("Hello");
//         }
//
//         // Update is called once per frame
//         void Update()
//         {
//
//         }
//     }
// }