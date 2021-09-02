using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Michsky.UI.Zone
{
    public class MenuManagerAndy : MonoBehaviour
    {
        [SerializeField] ContentSizeFitter[] arr;
        public void loadGame()
        {

            Initiate.Fade("GameMultiPlayer", Color.black, 1.5f);
        }

        private void Start()
        {
            


        }


        private void Update()
        {
            
        }
    }
}
