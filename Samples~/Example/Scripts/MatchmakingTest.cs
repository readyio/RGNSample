// using RGN;
// using RGN.Modules;
// using UnityEngine;
//
// public class MatchmakingTest : MonoBehaviour
// {
//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Alpha1))
//         {
//             StartMatchTest();
//         }
//     }
//
//     private async void StartMatchTest()
//     {
//         var response = await MatchmakingModule.I.StartMatch("practice");
//         string rawResponse = JsonUtility.ToJson(response);
//         Debug.Log(rawResponse);
//     }
// }
