package crc644103bb497e895a1b;


public class InputLayoutToggleViewRenderer_OnClickListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.XForms.Android.TextInputLayout.InputLayoutToggleViewRenderer+OnClickListener, Syncfusion.Core.XForms.Android", InputLayoutToggleViewRenderer_OnClickListener.class, __md_methods);
	}


	public InputLayoutToggleViewRenderer_OnClickListener ()
	{
		super ();
		if (getClass () == InputLayoutToggleViewRenderer_OnClickListener.class)
			mono.android.TypeManager.Activate ("Syncfusion.XForms.Android.TextInputLayout.InputLayoutToggleViewRenderer+OnClickListener, Syncfusion.Core.XForms.Android", "", this, new java.lang.Object[] {  });
	}

	public InputLayoutToggleViewRenderer_OnClickListener (crc644103bb497e895a1b.InputLayoutToggleViewRenderer p0)
	{
		super ();
		if (getClass () == InputLayoutToggleViewRenderer_OnClickListener.class)
			mono.android.TypeManager.Activate ("Syncfusion.XForms.Android.TextInputLayout.InputLayoutToggleViewRenderer+OnClickListener, Syncfusion.Core.XForms.Android", "Syncfusion.XForms.Android.TextInputLayout.InputLayoutToggleViewRenderer, Syncfusion.Core.XForms.Android", this, new java.lang.Object[] { p0 });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

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
