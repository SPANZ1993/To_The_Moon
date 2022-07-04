using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

// UNITY IS REALLY COOL AND DOESN'T WORK IF YOU SET THE ONCLICK FUNCTION OF A BUTTON TO A SINGLETON... SO WE'RE JUST GONNA GET A REFERENCE TO THE UI HANDLER AND
// CALL THE FUNCTIONS WE NEED FROM HERE...
public class Main_Area_Button_Handlers : MonoBehaviour
{

    public void saveGame(){
        Game_Manager.instance.saveData(false, false, true);
    }

    public void onButtonTmp(){
        UI_Controller.instance.onButtonTmp();
    }

    public void onButtonTmp2(){
        UI_Controller.instance.onButtonTmp2();
    }

    public void onButtonTmp3(){
        UI_Controller.instance.onButtonTmp3();
    }

    public void onButtonTmp4(){
        UI_Controller.instance.onButtonTmp4();
    }

    public void onButtonTmp5(){
        UI_Controller.instance.onButtonTmp5();
    }

    public void onButtonTmp6(){
        UI_Controller.instance.onButtonTmp6();
    }

    public void onButtonTmp7(){
        Game_Manager.instance.onApplicationFocus();
    }


    public void closeMenus(){
        UI_Controller.instance.closeMenus();
    }
    

    public void researchersIdCardButtonHandler(GameObject IdCard){
        UI_Controller.instance.researchersIdCardButtonHandler(IdCard);
    }

    public void selectExperiments(){
        UI_Controller.instance.selectExperiments();
    }

    public void selectResearch(){
        UI_Controller.instance.selectResearch();
    }

    public void finishResearchButtonHandler(GameObject ResearchPanel){
        UI_Controller.instance.finishResearchButtonHandler(ResearchPanel);
    }

    public void researchPanelButtonHandler(GameObject ResearchPanel){
        UI_Controller.instance.researchPanelButtonHandler(ResearchPanel);
    }


    public void experimentPanelButtonHandler(GameObject ExperimentPanel){
        UI_Controller.instance.experimentPanelButtonHandler(ExperimentPanel);
    }


    public void researchersConfirmationBoxNoButtonHandler(){
        UI_Controller.instance.researchersConfirmationBoxNoButtonHandler();
    }

    public void researchersConfirmationBoxYesButtonHandler(){
        UI_Controller.instance.researchersConfirmationBoxYesButtonHandler();
    }

    public void onNotEnoughCoinsButtonPressed(){
        UI_Controller.instance.onNotEnoughCoinsButtonPressed();
    }


    public void onSpeechBannerButtonPressed(){
        UI_Controller.instance.onSpeechBannerButtonPressed();
        // if(!UI_Controller.instance.speechIsDisplayed){
        //     UI_Controller.instance.displayExampleSpeech();
        // }
    }


  

    public void selectMineUpgrade(){
        UI_Controller.instance.selectMineUpgrade();
    }

    public void selectCartUpgrade(){
        UI_Controller.instance.selectCartUpgrade();
    }




    public void onHashingUpgradeButtonPressed(){
        UI_Controller.instance.onHashingUpgradeButtonPressed();
    }

    public void onBlockChainNetworkUpgradeButtonPressed(){
        UI_Controller.instance.onBlockChainNetworkUpgradeButtonPressed();
    }

    public void onGraphicsCardUpgradeButtonPressed(){
        UI_Controller.instance.onGraphicsCardUpgradeButtonPressed();
    }

    public void onColdStorageUpgradeButtonPressed(){
        UI_Controller.instance.onColdStorageUpgradeButtonPressed();
    }




    public void autopilotConfirmationBoxYesButtonHandler(){
        UI_Controller.instance.autopilotConfirmationBoxYesButtonHandler();
    }
    
    public void autopilotConfirmationBoxNoButtonHandler(){
        UI_Controller.instance.autopilotConfirmationBoxNoButtonHandler();
    }


    public void nameSubmitButtonPressed(){
        //Debug.Log("YO");
        UI_Controller.instance.onNameSubmitButtonPressed();
    }

    public void nameSubmitConfirmationButtonPressedYes(){
        //Debug.Log("Name Submit Yes");
        UI_Controller.instance.onNameSubmitConfirmationButtonPressed(true);
    }

    public void nameSubmitConfirmationButtonPressedNo(){
        //Debug.Log("Name Submit No");
        UI_Controller.instance.onNameSubmitConfirmationButtonPressed(false);
    }

    public void coinNameSubmitButtonPressed(){
        //Debug.Log("YO");
        UI_Controller.instance.onCoinNameSubmitButtonPressed();
    }

    public void coinNameSubmitConfirmationButtonPressedYes(){
        //Debug.Log("Coin Name Submit Yes");
        UI_Controller.instance.onCoinNameSubmitConfirmationButtonPressed(true);
    }

    public void coinNameSubmitConfirmationButtonPressedNo(){
        //Debug.Log("Coin Name Submit No");
        UI_Controller.instance.onCoinNameSubmitConfirmationButtonPressed(false);
    }


    public void selectOptions(){
        UI_Controller.instance.selectOptions();
    }

    public void selectRecords(){
        UI_Controller.instance.selectRecords();
    }

    public void selectShop(){
        UI_Controller.instance.selectShop();
    }

    public void selectExchange(){
        UI_Controller.instance.selectExchange();
    }

    public void onExchangeBuyButtonPressed(GameObject panel){
        Crypto_Scriptable_Object crypto = (Crypto_Scriptable_Object)panel.GetComponent<ObjectHolder>().Obj;
        //Debug.Log("BUYING: " + crypto.CoinName);
        UI_Controller.instance.onBuySellCryptoButtonPressed(true, crypto);
    }

    public void onExchangeSellButtonPressed(GameObject panel){
        Crypto_Scriptable_Object crypto = (Crypto_Scriptable_Object)panel.GetComponent<ObjectHolder>().Obj;
        //Debug.Log("SELLING: " + crypto.CoinName);
        UI_Controller.instance.onBuySellCryptoButtonPressed(false, crypto);
    }

    public void onExchangeBuySellConfirmButtonPressed(){
        //Debug.Log("CONFIRM");
        UI_Controller.instance.onExchangeBuySellConfirmButtonPressed();
    }  

    public void onExchangeBuySellCancelButtonPressed(){
        //Debug.Log("CANCEL");
        UI_Controller.instance.onExchangeBuySellCancelButtonPressed();
    }

    public void onExchangeBuySellConfirmationBoxInputFieldNumSelect(){
        //Debug.Log("SELECT");
        UI_Controller.instance.onExchangeBuySellConfirmationBoxInputFieldNumSelect();
    }

    public void onExchangeBuySellConfirmationBoxInputFieldNumEndEdit(){
        //Debug.Log("END EDIT");
        UI_Controller.instance.onExchangeBuySellConfirmationBoxInputFieldNumEndEdit();
    }

    public void onExchangeBuySellConfirmationBoxInputFieldNumDeselect(){
        //Debug.Log("DESELECT");
        UI_Controller.instance.onExchangeBuySellConfirmationBoxInputFieldNumEndEdit();
    }

    public void onExchangeBuySellConfirmationBoxInputFieldNumValueChanged(){
        //Debug.Log("VALUE CHANGED");
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
