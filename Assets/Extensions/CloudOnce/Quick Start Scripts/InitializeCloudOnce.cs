// <copyright file="InitializeCloudOnce.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
using CloudOnce;
//namespace CloudOnce.QuickStart
//{
using UnityEngine;

/// <summary>
/// Attach this anywhere in the scene you want the players to log in to the native services.
/// </summary>
[AddComponentMenu("CloudOnce/Initialize CloudOnce", 0)]
public class InitializeCloudOnce : MonoBehaviour
{
    [SerializeField]
    private bool cloudSaveEnabled = true;

    [SerializeField]
    private bool autoSignIn = true;

    [SerializeField]
    private bool autoCloudLoad = true;


    static bool ListenersReady = false;

    public static string GooglePlayUserID = "";

    private void Start()
    {

#if UNITY_STANDALONE
      
        Destroy(gameObject);
        return;
#endif


        if (!ListenersReady)
        {
            Cloud.OnSignedInChanged += signed =>
            {

                if (signed)
                {
                    Debug.Log("CLOUD ONCE SIGNED IN - Player Id : " + Cloud.PlayerID);
                    if (string.IsNullOrEmpty(GooglePlayUserID))
                        GooglePlayUserID = Cloud.PlayerID;

                }
            };

            Cloud.OnInitializeComplete += () =>
            {

                Debug.Log("CLOUD ONCE INIT COMPLETE - Player Id : " + Cloud.PlayerID);
                if (string.IsNullOrEmpty(GooglePlayUserID))
                    GooglePlayUserID = Cloud.PlayerID;
            };

            Cloud.OnSignInFailed += () =>
            {

                Debug.Log("CLOUD ONCE SIGN FAIL");

            };

            ListenersReady = true;

        }

        Cloud.Initialize(cloudSaveEnabled, autoSignIn, autoCloudLoad);

    }
}
//}
