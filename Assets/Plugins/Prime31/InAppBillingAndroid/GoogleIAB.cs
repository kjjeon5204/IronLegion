using UnityEngine;
using System.Collections;



// Not needed...
//#if UNITY_ANDROID
public class GoogleIAB
{
#if UNITY_ANDROID
	private static AndroidJavaObject _plugin;
#endif

	static GoogleIAB()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

	#if UNITY_ANDROID
		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.prime31.GoogleIABPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	#endif
	}


	// Toggles high detail logging on/off
	public static void enableLogging( bool shouldEnable )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		if( shouldEnable )
			Debug.LogWarning( "YOU HAVE ENABLED HIGH DETAIL LOGS. DO NOT DISTRIBUTE THE GENERATED APK PUBLICLY. IT WILL DUMP SENSITIVE INFORMATION TO THE CONSOLE!" );
			
#if UNITY_ANDROID
		_plugin.Call( "enableLogging", shouldEnable );
#endif
	}


	// Toggles automatic signature verification on/off
	public static void setAutoVerifySignatures( bool shouldVerify )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

#if UNITY_ANDROID
		_plugin.Call( "setAutoVerifySignatures", shouldVerify );
#endif
	}


	// Initializes the billing system
	public static void init( string publicKey )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

#if UNITY_ANDROID
		_plugin.Call( "init", publicKey );
#endif
	}


	// Unbinds and shuts down the billing service
	public static void unbindService()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

#if UNITY_ANDROID
		_plugin.Call( "unbindService" );
#endif
	}
	
	
	// Sends a request to get all completed purchases and product information as setup in the Play dashboard about the provided skus
	public static void queryInventory( string[] skus )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

#if UNITY_ANDROID
		_plugin.Call( "queryInventory", new object[] { skus } );
#endif
	}


	// Sends our a request to purchase the product
	public static void purchaseProduct( string sku )
	{
		purchaseProduct( sku, string.Empty );
	}
	
	public static void purchaseProduct( string sku, string developerPayload )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

#if UNITY_ANDROID
		_plugin.Call( "purchaseProduct", sku, developerPayload );
#endif
	}


	// Sends out a request to consume the product
	public static void consumeProduct( string sku )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

#if UNITY_ANDROID
		_plugin.Call( "consumeProduct", sku );
#endif
	}
	
	
	// Sends out a request to consume all of the provided products
	public static void consumeProducts( string[] skus )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

#if UNITY_ANDROID
		_plugin.Call( "consumeProducts", new object[] { skus } );
#endif
	}

}
//#endif