using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System;

public class FaceB : MonoBehaviour
{
    public Text FriendsText,usernameText,scoreText;
    private string name, id;
    private Dbmanager dbmanager = new Dbmanager();
    // Start is called before the first frame update
    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        //if (FB.IsInitialized)
        //{
        //    // Signal an app activation App Event
        //    FB.ActivateApp();
        //    // Continue with Facebook SDK
        //    // ...
        //}
        //else
        //{
        //    FriendsText.text = "Failed to Initialize the Facebook SDK";
        //}
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    #region Login / Logout
    public void FacebookLogin()
    {
        var perms = new List<string>() { "public_profile", "email", "publish_actions" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            string query = "me?fields=name,id";
            FB.API(query, HttpMethod.GET, result1 =>
            {
                IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result1.RawResult) as IDictionary;
                name = dict["name"].ToString();
                id = dict["id"].ToString();
                usernameText.text = name;
                
                FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("id").EqualTo(id).GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        // Handle the error...
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        if (snapshot.ChildrenCount == 0)
                        {
                            dbmanager.writeNewUser(id, name, "0");
                        }else
                        {
                            foreach (DataSnapshot user in snapshot.Children)
                            {
                                IDictionary dictUser = (IDictionary)user.Value;
                                string score = dictUser["score"].ToString();
                                scoreText.text = "Score: " + score;
                            }                           
                        }
                    }
                    
                });
                
            });
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public void FacebookLogout()
    {
    //    FB.LogOut();
    }
    #endregion

    public void FacebookShare()
    {
        FB.ShareLink(new System.Uri("https://resocoder.com"), "Check it out!",
            "Good programming tutorials lol!",
          new System.Uri("https://resocoder.com/wp-content/uploads/2017/01/logoRound512.png"));
    }

    #region Inviting
    public void FacebookGameRequest()
    {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Reso Coder Tutorial");
    }

    public void FacebookInvite()
    {
        FB.Mobile.AppInvite(new System.Uri("https://play.google.com/store/apps/details?id=com.tencent.ig"));
    }
    #endregion

    public void GetFriendsPlayingThisGame()
    {
       // Debug.Log(Facebook.Unity.AccessToken.CurrentAccessToken.UserId);
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            FriendsText.text = string.Empty;
            foreach (var dict in friendsList)
            {
                name = ((Dictionary<string, object>)dict)["name"].ToString();
                id = ((Dictionary<string, object>)dict)["id"].ToString();
                FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("id").EqualTo(id).GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        // Handle the error...
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        foreach (DataSnapshot user in snapshot.Children)
                        {
                            IDictionary dictUser = (IDictionary)user.Value;
                            Debug.Log(dictUser["id"] + " - " + dictUser["username"] + " - "+ dictUser["score"]);
                            string score = dictUser["score"].ToString();
                            FriendsText.text = dictUser["username"] + " score: " + score;
                        }
                    }
                });

               
            }
        });
    }
}
