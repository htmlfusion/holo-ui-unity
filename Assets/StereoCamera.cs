using UnityEngine;
using System.Collections;

public class StereoCamera : MonoBehaviour {

	public string eye = "left";
	public string screenName = "Screen";

	public float left = -0.2F;
	public float right = 0.2F;
	public float top = 0.2F;
	public float bottom = -0.2F;

	public float screenWidth;
	public float screenHeight;

	private GameObject screen;

	// Use this for initialization
	void Start () {
		screen = GameObject.Find(screenName);
		Renderer mf = screen.GetComponent<Renderer>();
		screenWidth = mf.bounds.size.x;
		screenHeight = mf.bounds.size.y;
	}
	
	// Update is called once per frame
	void Update () {
		Camera cam = GetComponent<Camera>();

		print (transform.position.x);
		float leftScreen = screenWidth / 2.0f + transform.position.x;
		left = -cam.nearClipPlane / -transform.position.z * leftScreen;

		float rightScreen = screenWidth / 2.0f - transform.position.x;
		right = cam.nearClipPlane / -transform.position.z * rightScreen;
		
		float bottomScreen = - screenHeight / 2.0f - transform.position.y;
		bottom = cam.nearClipPlane / -transform.position.z * bottomScreen;
		
		float topScreen = screenHeight / 2.0f - transform.position.y;
		top = cam.nearClipPlane / -transform.position.z * topScreen;
	}

	void LateUpdate() {
		Camera cam = GetComponent<Camera>();
		Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
		cam.projectionMatrix = m;
	}

	static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far) {
		float x = 2.0F * near / (right - left);
		float y = 2.0F * near / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0F * far * near) / (far - near);
		float e = -1.0F;
		Matrix4x4 m = new Matrix4x4();
		m[0, 0] = x;
		m[0, 1] = 0;
		m[0, 2] = a;
		m[0, 3] = 0;
		m[1, 0] = 0;
		m[1, 1] = y;
		m[1, 2] = b;
		m[1, 3] = 0;
		m[2, 0] = 0;
		m[2, 1] = 0;
		m[2, 2] = c;
		m[2, 3] = d;
		m[3, 0] = 0;
		m[3, 1] = 0;
		m[3, 2] = e;
		m[3, 3] = 0;
		return m;
	}

}
