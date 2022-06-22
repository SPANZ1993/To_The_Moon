using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

// UNITY IS REALLY COOL AND DOESN'T WORK IF YOU SET THE ONCLICK FUNCTION OF A BUTTON TO A SINGLETON... SO WE'RE JUST GONNA GET A REFERENCE TO THE UI HANDLER AND
// CALL THE FUNCTIONS WE NEED FROM HERE...
public class Main_Area_Button_Handlers : MonoBehaviour
{

    UI_Controller uiController;
    Game_Manager gameManager;

    void Start(){
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
    }


    public void saveGame(){
        gameManager.saveData(false, false, true);
    }

    public void onButtonTmp(){
        uiController.onButtonTmp();
    }

    public void onButtonTmp2(){
        uiController.onButtonTmp2();
    }

    public void onButtonTmp3(){
        uiController.onButtonTmp3();
    }

    public void onButtonTmp4(){
        uiController.onButtonTmp4();
    }

    public void onButtonTmp5(){
        uiController.onButtonTmp5();
    }

    public void onButtonTmp6(){
        uiController.onButtonTmp6();
    }

    public void onButtonTmp7(){
        gameManager.onApplicationFocus();
    }


    public void closeMenus(){
        uiController.closeMenus();
    }
    

    public void researchersIdCardButtonHandler(GameObject IdCard){
        uiController.researchersIdCardButtonHandler(IdCard);
    }

    public void selectExperiments(){
        uiController.selectExperiments();
    }

    public void selectResearch(){
        uiController.selectResearch();
    }

    public void finishResearchButtonHandler(GameObject ResearchPanel){
        uiController.finishResearchButtonHandler(ResearchPanel);
    }

    public void researchPanelButtonHandler(GameObject ResearchPanel){
        uiController.researchPanelButtonHandler(ResearchPanel);
    }


    public void experimentPanelButtonHandler(GameObject ExperimentPanel){
        uiController.experimentPanelButtonHandler(ExperimentPanel);
    }


    public void researchersConfirmationBoxNoButtonHandler(){
        uiController.researchersConfirmationBoxNoButtonHandler();
    }

    public void researchersConfirmationBoxYesButtonHandler(){
        uiController.researchersConfirmationBoxYesButtonHandler();
    }

    public void onNotEnoughCoinsButtonPressed(){
        uiController.onNotEnoughCoinsButtonPressed();
    }


    public void onSpeechBannerButtonPressed(){
        uiController.onSpeechBannerButtonPressed();
        // if(!UI_Controller.instance.speechIsDisplayed){
        //     UI_Controller.instance.displayExampleSpeech();
        // }
    }


  

    public void selectMineUpgrade(){
        uiController.selectMineUpgrade();
    }

    public void selectCartUpgrade(){
        uiController.selectCartUpgrade();
    }




    public void onHashingUpgradeButtonPressed(){
        uiController.onHashingUpgradeButtonPressed();
    }

    public void onBlockChainNetworkUpgradeButtonPressed(){
        uiController.onBlockChainNetworkUpgradeButtonPressed();
    }

    public void onGraphicsCardUpgradeButtonPressed(){
        uiController.onGraphicsCardUpgradeButtonPressed();
    }

    public void onColdStorageUpgradeButtonPressed(){
        uiController.onColdStorageUpgradeButtonPressed();
    }




    public void autopilotConfirmationBoxYesButtonHandler(){
        uiController.autopilotConfirmationBoxYesButtonHandler();
    }
    
    public void autopilotConfirmationBoxNoButtonHandler(){
        uiController.autopilotConfirmationBoxNoButtonHandler();
    }


    public void nameSubmitButtonPressed(){
        Debug.Log("YO");
        uiController.onNameSubmitButtonPressed();
    }

    public void nameSubmitConfirmationButtonPressedYes(){
        Debug.Log("Name Submit Yes");
        uiController.onNameSubmitConfirmationButtonPressed(true);
    }

    public void nameSubmitConfirmationButtonPressedNo(){
        Debug.Log("Name Submit No");
        uiController.onNameSubmitConfirmationButtonPressed(false);
    }

    public void coinNameSubmitButtonPressed(){
        Debug.Log("YO");
        uiController.onCoinNameSubmitButtonPressed();
    }

    public void coinNameSubmitConfirmationButtonPressedYes(){
        Debug.Log("Coin Name Submit Yes");
        uiController.onCoinNameSubmitConfirmationButtonPressed(true);
    }

    public void coinNameSubmitConfirmationButtonPressedNo(){
        Debug.Log("Coin Name Submit No");
        uiController.onCoinNameSubmitConfirmationButtonPressed(false);
    }


    public void selectOptions(){
        uiController.selectOptions();
    }

    public void selectRecords(){
        uiController.selectRecords();
    }

    public void selectShop(){
        uiController.selectShop();
    }

    public void selectExchange(){
        uiController.selectExchange();
    }

    public void onExchangeBuyButtonPressed(GameObject panel){
        Crypto_Scriptable_Object crypto = (Crypto_Scriptable_Object)panel.GetComponent<ObjectHolder>().Obj;
        Debug.Log("BUYING: " + crypto.CoinName);
        UI_Controller.instance.onBuySellCryptoButtonPressed(true, crypto);
    }

    public void onExchangeSellButtonPressed(GameObject panel){
        Crypto_Scriptable_Object crypto = (Crypto_Scriptable_Object)panel.GetComponent<ObjectHolder>().Obj;
        Debug.Log("SELLING: " + crypto.CoinName);
        UI_Controller.instance.onBuySellCryptoButtonPressed(false, crypto);
    }

    public void onExchangeBuySellConfirmButtonPressed(){
        Debug.Log("CONFIRM");
        UI_Controller.instance.onExchangeBuySellConfirmButtonPressed();
    }  

    public void onExchangeBuySellCancelButtonPressed(){
        Debug.Log("CANCEL");
        UI_Controller.instance.onExchangeBuySellCancelButtonPressed();
    }

    public void onExchangeBuySellConfirmationBoxInputFieldNumSelect(){
        Debug.Log("SELECT");
        UI_Controller.instance.onExchangeBuySellConfirmationBoxInputFieldNumSelect();
    }

    public void onExchangeBuySellConfirmationBoxInputFieldNumEndEdit(){
        Debug.Log("END EDIT");
        UI_Controller.instance.onExchangeBuySellConfirmationBoxInputFieldNumEndEdit();
    }

    public void onExchangeBuySellConfirmationBoxInputFieldNumDeselect(){
        Debug.Log("DESELECT");
        UI_Controller.instance.onExchangeBuySellConfirmationBoxInputFieldNumEndEdit();
    }

    public void onExchangeBuySellConfirmationBoxInputFieldNumValueChanged(){
        Debug.Log("VALUE CHANGED");
        UI_Controller.instance.onExchangeBuySellConfirmationBoxInputFieldNumValueChanged();
    }
    
    //public void 

    // Not a button handler... but... whatever
    public void onSoundFXSliderChanged(GameObject sliderObj){
        Game_Manager.instance.soundFxSoundLevel = sliderObj.GetComponent<Slider>().value;
        Audio_Manager.instance.UpdateChannelVolumes();
    }

    public void onMusicSliderChanged(GameObject sliderObj){
        Game_Manager.instance.musicSoundLevel = sliderObj.GetComponent<Slider>().value;
        Audio_Manager.instance.UpdateChannelVolumes();
    }

    public void selectTextSpeed(GameObject ToggleObj){
        Game_Manager.instance.textSpeed = ToggleObj.GetComponent<TextSpeedHolder>().textSpeed;
    }
    //
}
