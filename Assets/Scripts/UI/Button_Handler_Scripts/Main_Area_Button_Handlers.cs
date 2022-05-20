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



}
