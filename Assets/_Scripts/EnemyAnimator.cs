using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour {

	//PUBLIC INSTANCE VARIABLES
    public Sprite[] sprites; // sprite array for sprite images 
    public float framesPerSecond;

    private SpriteRenderer _spriteRenderer;
	// Use this for initialization
	void Start () {
        this._spriteRenderer = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
	}
	
	// Update is called once per frame
    void Update()
    {
        int index = (int)(Time.timeSinceLevelLoad * this.framesPerSecond); //recast every frame
        index = index % (this.sprites.Length - 1); //dividing by length of sprites -need remainder - and when remainder = 0, switch frames on tick count
        this._spriteRenderer.sprite = this.sprites[index]; //changes sprite renderer to one sprite image to another
    }
}
