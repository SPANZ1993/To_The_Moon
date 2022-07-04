using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Linq;

#if UNITY_IOS
using Unity.Notifications.iOS;
#elif UNITY_ANDROID
using Unity.Notifications.Android;
#endif


[Flags]
public enum NotificationCategories{
        Retention = 0, // A notification that was scheduled upon the closing of the application... Probably want to disable these upon the app being opened
        Event = 1 // A notification that was scheduled on a timer for a special event... Likely don't want to delete these
    }


public struct PlatformAgnosticNotification{
    
    public int id;
    public NotificationCategories category;
    public DateTime platformTimeToFire;
    public string message;

    public PlatformAgnosticNotification(int ID, NotificationCategories Category, DateTime PlatformTimeToFire, string Message){
        id = ID; 
        category = Category;
        platformTimeToFire = PlatformTimeToFire;
        message = Message;
    }

}


public class Notifications_Manager : MonoBehaviour
{



    int[] retentionIdRange = {0, 50};
    int[] eventIdRange = {51, 60};


    // This will hold the most recent notification that was sent using each ID in event id range... We will not try to send a notification if a notification with the same id and text appears here
    // This should work because we will never manually delete an event notification
    // This will be initialized by game manager upon startup
    public Dictionary<int, string> eventNotificationsInfo {get; private set;}
    [SerializeField]
    private int secondsUntilResetNotifications; // How long from the start of the game should we clear the old notifications and queue up new ones?
    private bool clearedOldNotifications = false;

    public static Notifications_Manager instance;

    // Possible notifications we might want to add upon application close. These will probably be event types
    // On application close we will see if we should add them, and if we should, then we will convert them to the proper notification type (ios/android)
    // And Schedule them
    public List<PlatformAgnosticNotification> notificationsToScheduleOnAppClose;


    #if UNITY_IOS
    //iOSNotificationCenter NotificationCenter;
    #elif UNITY_ANDROID
    //AndroidNotificationCenter NotificationCenter;
    AndroidNotificationChannel channel;
    bool notificationChannelRegistered = false;
    #endif


    void Awake()
    {
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            #if UNITY_IOS
            //Debug.Log("NOTIFICATION CENTER IOS");
            //NotificationCenter = new iOSNotificationCenter();
            //NotificationCenter.clearNotifications();
            #elif UNITY_ANDROID
            //Debug.Log("NOTIFICATION CENTER ANDROID");
            //NotificationCenter = new AndroidNotificationCenter();
            channel = new AndroidNotificationChannel()
                {
                Id = "Default",
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
                };
            try{
                AndroidNotificationCenter.RegisterNotificationChannel(channel);
                notificationChannelRegistered = true;
            }
            catch(Exception e){
                Debug.Log("Couldn't register Android notification channel " + e);
            }
            #endif
            //NotificationCenter.clearNotifications();
            //Debug.Log("SETTING EMPTY NOTIFICATIONS INFO");
            eventNotificationsInfo = new Dictionary<int, string>();
            foreach(int id in Enumerable.Range(eventIdRange[0], eventIdRange[1])){
                eventNotificationsInfo[id] = "";
            }
            notificationsToScheduleOnAppClose = new List<PlatformAgnosticNotification>();
            
            #if UNITY_IOS
            StartCoroutine(ClearOldNotificationsAfterDelay());
            #elif UNITY_ANDROID
            if(notificationChannelRegistered){
                StartCoroutine(ClearOldNotificationsAfterDelay());
            }
            #endif
        }
        else{
            Destroy(this.gameObject);
        }
    }



    void OnApplicationQuit(){
        if(clearedOldNotifications){
            List<PlatformAgnosticNotification> retentionNotifications = chooseRetentionNotifications();
            foreach(PlatformAgnosticNotification retentionNotification in retentionNotifications){
                //Debug.Log("TRYING TO ADD NOTIFICATION WITH MESSAGE " + retentionNotification.message);
                #if UNITY_IOS
                    scheduleNotification(retentionNotification);
                #elif UNITY_ANDROID
                    if(notificationChannelRegistered){
                        scheduleNotification(retentionNotification);
                    }
                #endif
            }
        }
    }



    // Should probably only return 1 or 0 of these, but we'll make it a list just in case
    List<PlatformAgnosticNotification> chooseRetentionNotifications(){
        List<PlatformAgnosticNotification> retentionNotifications = new List<PlatformAgnosticNotification>();


        int tmpId = -1;
        // If there is some open ID in our range of retention ids
        if(Enumerable.Range(retentionIdRange[0], retentionIdRange[1]).Where(i => !notificationsToScheduleOnAppClose.Select(n => n.id).Contains(i)).ToList().Count > 0){
            retentionNotifications.Add(
                new PlatformAgnosticNotification(
                    ID: Enumerable.Range(retentionIdRange[0], retentionIdRange[1]).Where(i => !notificationsToScheduleOnAppClose.Select(n => n.id).Contains(i)).ToList()[0], // The first open Retention Id in our defined range
                    Category: NotificationCategories.Retention,
                    PlatformTimeToFire: Game_Manager.instance.localSessionCurrentTime.AddSeconds(60), // THIS WILL CHANGE DEPENDING ON WHAT WE ARE ALERTING ON... i.e. mine cooldown done, ship cooldown done, etc.
                    Message: "I LIKE BEANS N' SUCH... SCHEDULED AT " + DateTime.Now.ToString()
                )
            );       
        }
        
        return retentionNotifications;
    }



    
    #if UNITY_IOS
    void scheduleNotification(PlatformAgnosticNotification agnosticNotification){
        // Title
        // Body
        try{            
            iOSNotification notification = new iOSNotification(){
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = agnosticNotification.id.ToString(),
                Title = "Blockchain Blastoff",
                Body = agnosticNotification.message,
                Subtitle = "Blockchain Blastoff",
                ShowInForeground = false,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = agnosticNotification.category.ToString(),
                ThreadIdentifier = "",
                Trigger = new iOSNotificationTimeIntervalTrigger(){  
                    Repeats = false,
                    TimeInterval = agnosticNotification.platformTimeToFire - Game_Manager.instance.localSessionCurrentTime
                }
            };

            iOSNotificationCenter.ScheduleNotification(notification);
            //Debug.Log("JUST SCHEDULED A NOTIFICATION ON IOS");
            
        }
        catch(Exception e){
            Debug.Log("Error trying to schedule iOS Notification " + e);
        }
    }
    #elif UNITY_ANDROID
    void scheduleNotification(PlatformAgnosticNotification agnosticNotification){
        // Title
        // Text
        // AndroidNotification("Blockchain Blastoff", agnosticNotification.message, DateTime fireTime)
        // SendNotificationWithExplicitID(AndroidNotification notification, string channel, int id)
        if(notificationChannelRegistered){
            try{
                AndroidNotification notification = new AndroidNotification();
                notification.Title = "Blockchain Blastoff";
                notification.Text = agnosticNotification.message;
                notification.FireTime = agnosticNotification.platformTimeToFire;

                AndroidNotificationCenter.SendNotification(notification, "Default");
            }
            catch(Exception e){
                Debug.Log("Error trying to schedule Android Notification " + e);
            }
        }
        else{
            Debug.Log("Tried to schedule notification before registering channel");
        }
    }
    #endif





    #if UNITY_IOS
    IEnumerator ClearOldNotificationsAfterDelay(){
        yield return new WaitForSeconds(secondsUntilResetNotifications);
        clearNotifications();
    }


    //ios
    void clearNotifications(){
        foreach(int id in Enumerable.Range(retentionIdRange[0], retentionIdRange[1])){
            try{
                iOSNotificationCenter.RemoveScheduledNotification(id.ToString());
                //Debug.Log("Removed Scheduled Notification: " + id.ToString());
            }
            catch(Exception e){
                Debug.Log("No Scheduled Notification with id: " + id.ToString());
            }

            try{
                iOSNotificationCenter.RemoveDeliveredNotification(id.ToString());
                //Debug.Log("Removed Delivered Notification: " + id.ToString());
            }
            catch(Exception e){
                Debug.Log("No Delivered Notification with id: " + id.ToString());
            }
            clearedOldNotifications = true;
        }
    }
    #elif UNITY_ANDROID

    IEnumerator ClearOldNotificationsAfterDelay(){
        yield return new WaitForSeconds(secondsUntilResetNotifications);
        clearNotifications();
    }


    //android
    void clearNotifications(string pattern=""){
        foreach(int id in Enumerable.Range(retentionIdRange[0], retentionIdRange[1])){
            try{
                AndroidNotificationCenter.CancelScheduledNotification(id);
                //Debug.Log("Removed Scheduled Notification: " + id.ToString());
            }
            catch(Exception e){
                Debug.Log("No Scheduled Notification with id: " + id.ToString());
            }

            try{
                AndroidNotificationCenter.CancelDisplayedNotification(id);
                //Debug.Log("Removed Delivered Notification: " + id.ToString());
            }
            catch(Exception e){
                Debug.Log("No Delivered Notification with id: " + id.ToString());
            }
        }
        clearedOldNotifications = true;
    }
    #endif
}
