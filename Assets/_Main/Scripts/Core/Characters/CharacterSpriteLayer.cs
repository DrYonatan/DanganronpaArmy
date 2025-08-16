// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using System.Linq;
//
// namespace CHARACTERS
// {
//     public class CharacterSpriteLayer
//     {
//         private CharacterManager characterManager = CharacterManager.instance;
//
//         private const float DEFAULT_TRANSITION_SPEED = 3f;
//         private float transitionSpeedMultiplier = 1;
//
//         public Image renderer { get; private set; } = null;
//
//         public CanvasGroup rendererCG => renderer ? renderer.GetComponent<CanvasGroup>() : null;
//         
//         private List<CanvasGroup> oldRenderers = new List<CanvasGroup>();
//
//
//         private Coroutine co_transitioningLayer = null;
//         private Coroutine co_levelingAlpha = null;
//         public bool isTransitioningLayer => co_transitioningLayer != null;
//         public bool isLevelingAlpha => co_levelingAlpha != null;
//
//
//         public CharacterSpriteLayer(Image defaultRenderer, int layer)
//         {
//             renderer = defaultRenderer;
//         }
//         public void SetSprite(Sprite sprite)
//         {
//             renderer.sprite = sprite;
//         }
//
//         public Coroutine TransitionSprite(Sprite sprite, float speed = 1)
//         {
//             if(renderer)
//             {
//                 if (sprite == renderer.sprite)
//                     return null;
//
//                 if (isTransitioningLayer)
//                     characterManager.StopCoroutine(co_transitioningLayer);
//
//                 co_transitioningLayer = characterManager.StartCoroutine(TransitioningSprite(sprite, speed));
//                 return co_transitioningLayer;
//
//             }
//             return null;
//         }
//
//         private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier)
//         {
//             if(renderer)
//             {
//                 transitionSpeedMultiplier = speedMultiplier;
//                 Image newRenderer = CreateRenderer(renderer.transform.parent);
//                 newRenderer.sprite = sprite;
//
//                 yield return TryStartLevelingAlphas();
//
//                 co_transitioningLayer = null;
//             }
//             
//
//         }
//
//
//
//         private Image CreateRenderer(Transform parent)
//         {
//             Image newRenderer = Object.Instantiate(renderer, parent);
//             oldRenderers.Add(rendererCG);
//
//             newRenderer.name = renderer.name;
//             renderer = newRenderer;
//             renderer.gameObject.SetActive(true);
//             rendererCG.alpha = 0;
//
//             return newRenderer;
//         }
//
//         private Coroutine TryStartLevelingAlphas()
//         {
//             if (isLevelingAlpha)
//                 return co_levelingAlpha;
//
//             co_levelingAlpha = characterManager.StartCoroutine(RunAlphaLeveling());
//
//             return co_levelingAlpha;
//         }
//
//         private IEnumerator RunAlphaLeveling()
//         {
//             while(rendererCG ? (rendererCG.alpha < 1 || oldRenderers.Any(oldCG => oldCG.alpha > 0)) : false)
//             {
//                 float speed = DEFAULT_TRANSITION_SPEED * transitionSpeedMultiplier * Time.deltaTime;
//                 rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed);
//
//                 for(int i = oldRenderers.Count-1; i >= 0; i--)
//                 {
//                     CanvasGroup oldCG = oldRenderers[i];
//                     oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);
//
//                     if(oldCG.alpha <= 0)
//                     {
//                         oldRenderers.RemoveAt(i);
//                         Object.Destroy(oldCG.gameObject);
//                     }
//                 }
//                 yield return null;
//             }
//
//             co_levelingAlpha = null;
//         }
//     }
// }