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
//         var matchmakingModule = RGNCoreBuilder.I.GetModule<MatchmakingModule>();
//         var response = await matchmakingModule.StartMatch("practice");
//         string rawResponse = JsonUtility.ToJson(response);
//         Debug.Log(rawResponse);
//     }
// }
