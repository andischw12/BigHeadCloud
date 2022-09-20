using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StudioMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject NotAvaliable;
    [SerializeField] TextMeshProUGUI RankNeededText;
    [SerializeField] TextMeshProUGUI StudioNameText;
    [SerializeField] Image RankBackGround;
    [SerializeField] TextMeshProUGUI BonusText;
    public int EnviormentNum { get; set; }
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
      
         
    }



    public void AddListner() 
    {
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick() 
    {
        PhotonLobby.lobby.OnRandomBattleButtonClicked();
        PhotonRoom.room.SelecetSubject(-1);
        PhotonRoom.room.SelecetEnviorment(EnviormentNum);

    }

    public void SetStudioButton(Sprite sprite, string name, int rank, float bonusMultiplayer) 
    {
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        RankNeededText.text = rank.ToString();
        StudioNameText.text = name;
        if (bonusMultiplayer > 100)
            BonusText.text = bonusMultiplayer+ "% сереб" ;
        else
            BonusText.transform.parent.gameObject.SetActive(false);
    }


    public void MakeButtonAvaliable() 
    {
        NotAvaliable.gameObject.SetActive(false);
        GetComponent<Button>().interactable = true;
        RankBackGround.color = Color.white;
        RankNeededText.color = Color.white;
    }

    public void MakeButtonNotAvaliable()
    {
        NotAvaliable.gameObject.SetActive(true);
        GetComponent<Button>().interactable = false;
        RankBackGround.color = Color.gray;
        RankNeededText.color = Color.gray;
    }
}
