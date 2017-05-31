using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour))]
public class PostProcessingEffectsController : MonoBehaviour {
    public int minQualityForImageEffects;

    void Start () {
        int qualityLevel = QualitySettings.GetQualityLevel();

        if (qualityLevel < minQualityForImageEffects) {
            GetComponent<PostProcessingBehaviour>().enabled = false;
        }
    }
}
