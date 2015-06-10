#pragma strict
var mat: Material;
var cleared: boolean;

function Start () {

}

function Update () {

}

function OnRenderImage(src: RenderTexture, dest: RenderTexture) {
	// Copy the source Render Texture to the destination,
	// applying the material along the way.
    if (!cleared) {
        Graphics.Blit(null, dest);
        cleared = false;
    }
}
