using Photon.Pun;
using System;
using UnityEngine;

public static  class Extensions
{    public static object GetValue(this ExitGames.Client.Photon.Hashtable customProps, string key)
    {
        if(customProps.TryGetValue(key, out object value))
        {
            return value;
        }
        return null;
    }

    public static T ToConcrete<T>(this Enum enumValue)
    {
        var res = (T)Enum.Parse(typeof(T), enumValue.ToString(), true);
        return res;
    }

    public static int GetPunOwnerActorNr(this GameObject go)
    {
        var photonView = go.GetComponent<PhotonView>();
        if(photonView != null)
        {
            return photonView.OwnerActorNr;
        }
        return -1;
    }

    public static bool IsType<T>(this Enum value)
    {
        try
        {
            value.ToConcrete<T>();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}