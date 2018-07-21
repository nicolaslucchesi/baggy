package md5ef46fe81bdebdb88dcc965f555bbee58;


public class SensorService
	extends mono.android.app.IntentService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onHandleIntent:(Landroid/content/Intent;)V:GetOnHandleIntent_Landroid_content_Intent_Handler\n" +
			"n_onStartCommand:(Landroid/content/Intent;II)I:GetOnStartCommand_Landroid_content_Intent_IIHandler\n" +
			"n_onCreate:()V:GetOnCreateHandler\n" +
			"n_onTaskRemoved:(Landroid/content/Intent;)V:GetOnTaskRemoved_Landroid_content_Intent_Handler\n" +
			"n_onBind:(Landroid/content/Intent;)Landroid/os/IBinder;:GetOnBind_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("MobileExample.Droid.Services.SensorService, MobileExample.Android", SensorService.class, __md_methods);
	}


	public SensorService (java.lang.String p0)
	{
		super (p0);
		if (getClass () == SensorService.class)
			mono.android.TypeManager.Activate ("MobileExample.Droid.Services.SensorService, MobileExample.Android", "System.String, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public SensorService ()
	{
		super ();
		if (getClass () == SensorService.class)
			mono.android.TypeManager.Activate ("MobileExample.Droid.Services.SensorService, MobileExample.Android", "", this, new java.lang.Object[] {  });
	}

	public SensorService (android.content.Context p0)
	{
		super ();
		if (getClass () == SensorService.class)
			mono.android.TypeManager.Activate ("MobileExample.Droid.Services.SensorService, MobileExample.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public void onHandleIntent (android.content.Intent p0)
	{
		n_onHandleIntent (p0);
	}

	private native void n_onHandleIntent (android.content.Intent p0);


	public int onStartCommand (android.content.Intent p0, int p1, int p2)
	{
		return n_onStartCommand (p0, p1, p2);
	}

	private native int n_onStartCommand (android.content.Intent p0, int p1, int p2);


	public void onCreate ()
	{
		n_onCreate ();
	}

	private native void n_onCreate ();


	public void onTaskRemoved (android.content.Intent p0)
	{
		n_onTaskRemoved (p0);
	}

	private native void n_onTaskRemoved (android.content.Intent p0);


	public android.os.IBinder onBind (android.content.Intent p0)
	{
		return n_onBind (p0);
	}

	private native android.os.IBinder n_onBind (android.content.Intent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
