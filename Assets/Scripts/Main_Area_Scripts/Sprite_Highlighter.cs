using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Highlighter : MonoBehaviour
{

    private bool currentlyPulsing = false;
    public double timeBetweenPulses {get; private set;}
    private double prevPulseTime;


    List<SpriteRenderer> pulsingGameObjectRenderers;
    List<Color> pulsingGameObjectsOrigColors;


    void Awake(){
        pulsingGameObjectRenderers = new List<SpriteRenderer>();
        pulsingGameObjectsOrigColors = new List<Color>();
    }


    public void StartPulsing(float pulseTime){
        if (pulsingGameObjectRenderers != null){
            SpriteRenderer curRenderer;
            for(int i=0; i<pulsingGameObjectRenderers.Count; i++){
                curRenderer = pulsingGameObjectRenderers[i];
                curRenderer.color = pulsingGameObjectsOrigColors[i];
            }
        }
        prevPulseTime = Game_Manager.instance.gameTimeUnix;
        timeBetweenPulses = pulseTime;
        currentlyPulsing = true;
    }

    public void EndPulsing(){
        currentlyPulsing = false;
        SpriteRenderer curRenderer;
        for(int i=0; i<pulsingGameObjectRenderers.Count; i++){
            curRenderer = pulsingGameObjectRenderers[i];
            curRenderer.color = pulsingGameObjectsOrigColors[i];
        }
    }

    public void AddHighlightedSprites(List<GameObject> objs){
        AddHighlightedSprites(objs.ToArray());
    }

    public void AddHighlightedSprites(GameObject[] objs){
        foreach(GameObject g in objs){
            AddHighlightedSprites(g);
        }
    }

    public void AddHighlightedSprites(GameObject g){
        SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
        Debug.Assert(sr != null);
        if(!pulsingGameObjectRenderers.Contains(sr)){
            pulsingGameObjectRenderers.Add(sr);
            pulsingGameObjectsOrigColors.Add(sr.color);
        }
    }


    public void RemoveHighlightedSprites(List<GameObject> objs){
        RemoveHighlightedSprites(objs.ToArray());
    }

    public void RemoveHighlightedSprites(GameObject[] objs){
        foreach(GameObject g in objs){
            RemoveHighlightedSprites(g);
        }
    }
    
    public void RemoveHighlightedSprites(GameObject g){
        SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
        Debug.Assert(sr != null);
        if(pulsingGameObjectRenderers.Contains(sr)){
            int index = pulsingGameObjectRenderers.FindIndex(srl => srl == sr);
            pulsingGameObjectRenderers[index].color = pulsingGameObjectsOrigColors[index];
            pulsingGameObjectRenderers.RemoveAt(index);
            pulsingGameObjectsOrigColors.RemoveAt(index);  
        }
    }







    private void Pulse(){
        prevPulseTime = Game_Manager.instance.gameTimeUnix;
        Color newColor = new Color();
        SpriteRenderer curRenderer;
        for(int i=0; i<pulsingGameObjectRenderers.Count; i++){
            curRenderer = pulsingGameObjectRenderers[i];
            if(curRenderer.color != pulsingGameObjectsOrigColors[i]){
                curRenderer.color = pulsingGameObjectsOrigColors[i];
            }
            else{
                newColor.r = .75f * pulsingGameObjectsOrigColors[i].r;
                newColor.g = .75f * pulsingGameObjectsOrigColors[i].g;
                newColor.b = .75f * pulsingGameObjectsOrigColors[i].b;
                newColor.a = pulsingGameObjectsOrigColors[i].a;
                curRenderer.color = newColor;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentlyPulsing && Game_Manager.instance.gameTimeUnix - prevPulseTime >= timeBetweenPulses){
            Pulse();
        }
    }
}
