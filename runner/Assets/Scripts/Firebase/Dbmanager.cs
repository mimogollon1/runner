using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class Dbmanager : MonoBehaviour
{
    public DatabaseReference reference;
    // Start is called before the first frame update
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://runner-f71e9.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    public void writeNewUser(string id, string name,string score)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://runner-f71e9.firebaseio.com/");

        //// Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        User user = new User(name, id, score);
        string json = JsonUtility.ToJson(user);

        reference.Child("users").Child(id).SetRawJsonValueAsync(json);
    }
}

public class User
{
    public string id, username, score;

    public User()
    {
    }

    public User(string username, string id,string score)
    {
        this.username = username;
        this.id = id;
        this.score = score;
    }
}