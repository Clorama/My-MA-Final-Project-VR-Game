/* InstantVR Oculus Rift extension editor
 * author: Pascal Serrarens
 * email: support@passervr.com
 * version: 3.1.0
 * date: September 26, 2015
 * 
 * - Check for VirtualRealitySupported added
 */

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(IVR_Rift))] 
public class IVR_Rift_Editor : IVR_Extension_Editor {

	private InstantVR ivr;
	private IVR_Rift ivrRift;

	private IVR_RiftHead riftHead;

#if !UNITY_ANDROID
#if !(UNITY_4_5 || UNITY_4_6 || UNITY_5_0)
    public override void OnInspectorGUI() {
        if (PlayerSettings.virtualRealitySupported == false)
            EditorGUILayout.HelpBox("VirtualRealitySupported needs to be enabled in Player Settings for Rift support", MessageType.Warning, true);

        base.OnInspectorGUI();
    }
#endif
#endif

	void OnDestroy() {
		if (ivrRift == null && ivr != null) {
			riftHead = ivr.headTarget.GetComponent<IVR_RiftHead>();
			if (riftHead != null)
				DestroyImmediate(riftHead, true);
		}
	}
	
	void OnEnable() {

		ivrRift = (IVR_Rift) target;
		ivr = ivrRift.GetComponent<InstantVR>();

		if (ivr != null) {
			riftHead = ivr.headTarget.GetComponent<IVR_RiftHead>();
			if (riftHead == null) {
				riftHead = ivr.headTarget.gameObject.AddComponent<IVR_RiftHead>();
				riftHead.extension = ivrRift;
			}

			IVR_Extension[] extensions = ivr.GetComponents<IVR_Extension>();
			if (ivrRift.priority == -1)
				ivrRift.priority = extensions.Length - 1;
			for (int i = 0; i < extensions.Length; i++) {
				if (ivrRift == extensions[i]) {
					while (i < ivrRift.priority) {
						MoveUp(riftHead);
						ivrRift.priority--;
						//Debug.Log ("Rift Move up to : " + i + " now: " + ivrRift.priority);
					}
					while (i > ivrRift.priority) {
						MoveDown(riftHead);
						ivrRift.priority++;
						//Debug.Log ("Rift Move down to : " + i + " now: " + ivrRift.priority);
					}
				}
			}
		}
	}
}
